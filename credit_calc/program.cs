// using System.Runtime.CompilerServices;
using System;
using System.Data;
using System.Runtime.InteropServices;

Main(args);

int GetYearDaysCount(int Year)
{
	return (DateTime.IsLeapYear(Year) ? 366 : 365);
}
int GetPeriodDaysCount(DateTime Date)
{
	return (Date.Subtract(Date.AddMonths(-1)).Days);
}

double GetMonthAnnuityPayment(double Sum, double MonthCount, double Rate)
{
	double MonthRateCount = Math.Pow((1 + Rate), MonthCount);
	return Sum * Rate * MonthRateCount / (MonthRateCount - 1);
}
double GetMonthPaymentPercent(double Sum, double Rate, int PeriodDaysCount, int YearDaysCount)
{
	return Sum * Rate * PeriodDaysCount / YearDaysCount;
}
/*
double GetCurrentMonthCount(double Sum, double PaymentSum, double LoanRate)
{
	return (Math.Log(1 + LoanRate, PaymentSum / (PaymentSum - LoanRate * Sum)));
}
*/

string Replicate(char ch, int count)
{
	string str = new String(ch, count);
	return str;
}

void WriteTableHorizontalLine(int TableCellLength)
{
	Console.WriteLine("{0}{0}{0}{0}{0}{0}|"
		,"|"+Replicate('-',TableCellLength)
	);
}
void WriteTableName(string TableName, int TableLength)
{
	Console.WriteLine("|{0}|",Replicate('-',TableLength));
	Console.WriteLine("|{1}{0}{1}|",TableName,Replicate(' ',(TableLength - TableName.Length) / 2));
}
void WriteTableHead()
{
	Console.WriteLine("| {0} \t| {1} \t| {2} \t| {3}\t| {4} \t| {5} |"
		,"№ платежа"
		,"Дата платежа"
		,"Платеж"
		,"Основной долг"
		,"Проценты"
		,"Остаток долга"
	);
	WriteTableHorizontalLine(15);
}
void WriteTableValue(int PaymentNum, DateTime PaymentDate, double Payment, double MainDebt, double Percentages, double RemainDebt)
{
	Console.WriteLine("|{0, 3}\t\t|{1: MM/dd/yy}\t|{2,11:N2}\t|{3,11:N2}\t|{4,11:N2}\t|{5,11:N2}\t|"
		,PaymentNum
		,PaymentDate
		,Payment
		,MainDebt
		,Percentages
		,RemainDebt
	);
}
void WriteTableOverPayment(string OverPaymentName, double OverPaymentSum, int TableSize)
{
	Console.WriteLine("{0}",Convert.ToString(OverPaymentSum,CultureInfo.CreateSpecificCulture("en-US")));
	Console.WriteLine("| {0}: {1:0.00} р.{2} |"
		,OverPaymentName
		,OverPaymentSum
		,Replicate(' ',TableSize - OverPaymentName.Length - Convert.ToString(OverPaymentSum).Length )
	);
}


double GetSumReduce(double sum, double rate, double term, DateTime PaymentDate, int ExtraPayMonthNum, double ExtraPaySum)
{
	int PaymentMonthNum = 0;
	double RemainDebt = sum;
	double MonthAnnuityPayment = GetMonthAnnuityPayment(sum, term, rate / 12);
	double Percentages;
	double Overpayment = 0;

	WriteTableName("ДОСРОЧНОЕ ПОГАШЕНИЕ С УМЕНЬШЕНИЕМ СУММЫ", 95);
	WriteTableHorizontalLine(15);
	WriteTableHead();
	while (PaymentMonthNum++ < term)
	{
		PaymentDate = PaymentDate.AddMonths(1);
		Percentages = GetMonthPaymentPercent(RemainDebt, rate, GetPeriodDaysCount(PaymentDate), GetYearDaysCount(PaymentDate.Year));
		Overpayment += Percentages;
		RemainDebt -= (MonthAnnuityPayment - Percentages);

		if (Percentages > RemainDebt)
		{
			MonthAnnuityPayment += RemainDebt;
			RemainDebt = 0;
		}
		WriteTableValue(PaymentMonthNum, PaymentDate, MonthAnnuityPayment, MonthAnnuityPayment - Percentages, Percentages, RemainDebt);
		WriteTableHorizontalLine(15);
		if (PaymentMonthNum == ExtraPayMonthNum)
		{
			RemainDebt -= ExtraPaySum;
			sum = RemainDebt;
			MonthAnnuityPayment = GetMonthAnnuityPayment(RemainDebt, term - ExtraPayMonthNum, rate / 12);
		}
	}
	return (Overpayment);
}


bool ReadParams(string[] args, out double sum, out double rate, out int term, ref int selectedMonth, ref double payment)
{
	bool IsCorrect = true;

	IsCorrect &= Double.TryParse(args[0], out sum);
	IsCorrect &= Double.TryParse(args[1], out rate);
	IsCorrect &= Int32.TryParse(args[2], out term);
	IsCorrect &= (sum > 0 && rate > 0 && term > 0);
	if (args.Length == 5)
	{
		IsCorrect &= Int32.TryParse(args[3], out selectedMonth);
		IsCorrect &= Double.TryParse(args[4], out payment);
		IsCorrect &= (selectedMonth > 0 && selectedMonth <= term);
		IsCorrect &= (payment > 0 && payment <= sum);
	}
	return (IsCorrect);
}


void Main(string[] args)
{

	double	sum				=	1000000.0	;	//	Credit total summ
	double	rate			=	12.0		;	//	Annual interest rate
	int		term			=	10			;	//	Number of loan months
	int		selectedMonth	=	5			;	//	Number of month in which early payment was made
	double	payment			=	100000		;	//	Sum of early payment

	DateTime	PaymentDate = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(0);
	double		OverPayment;

	selectedMonth = 0;
	payment = 0;

	// Console.WriteLine(": {0}",sum);
	// Console.WriteLine(": {0}",rate);
	// Console.WriteLine(": {0}",term);
	// Console.WriteLine(": {0}",selectedMonth);
	// Console.WriteLine(": {0}",payment);

	// double		MonthAnnuityPayment	=	GetMonthAnnuityPayment(sum, term, rate / 12);
	// double		Percentages			=	GetMonthPaymentPercent(sum, rate, GetPeriodDaysCount(PaymentDate), GetYearDaysCount(PaymentDate.Year));
	// Console.WriteLine("{0}:d",DateTime.Parse("01/01/2021",enUS));

	// Console.WriteLine("ReadParams: {0}",ReadParams(args, out sum, out rate, out term, ref selectedMonth, ref payment));

	if (
		!((args.Length == 5 || args.Length == 3) && ReadParams(args, out sum, out rate, out term, ref selectedMonth, ref payment))
	)
	{
		Console.WriteLine("Something went wrong. Check your input and retry.");
		Environment.Exit(0);
	}
	OverPayment = GetSumReduce(sum, rate / 100, term, PaymentDate, selectedMonth, payment);
	// Console.WriteLine("| Переплата при уменьшении платежа: {0:0.00} р.",OverPayment);
	WriteTableOverPayment("Переплата при уменьшении платежа",OverPayment,95);
	Console.WriteLine("|{0}|",Replicate('-',95));





	// Console.WriteLine("Percentages: {0}",Percentages);
	// Console.WriteLine("MonthAnnuityPayment = {0:N2}",MonthAnnuityPayment);
	// Console.WriteLine("Переплата при уменьшении платежа: {0:N2}р.",sum - MonthAnnuityPayment);

	// Console.WriteLine("CurrentDate: {0}",PaymentDate);
	// Console.WriteLine("CurrentDate: {0}",PaymentDate.AddMonths(-1));
	// Console.WriteLine("GetPeriodDaysCount: {0}",GetPeriodDaysCount(PaymentDate));
	// Console.WriteLine("GetYearDaysCount: {0}",GetYearDaysCount(PaymentDate.Year));
/*
	int i = 4;
	while (i++ > 0)
	{
		Percentages =
	}
	Console.WriteLine(" {0} ", GetMonthPaymentPercent(sum, MonthRate, PeriodDaysCount));
*/
}
