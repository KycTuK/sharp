// using System.Runtime.CompilerServices;
using System;
using System.Data;
using System.Runtime.InteropServices;

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
	Console.WriteLine("| {0} \t| {1} \t| {2}\t| {3}\t| {4} \t| {5} |"
		,"№ платежа"
		,"Дата платежа"
		,"Платеж"	//"Сумма платежа"
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
	string OverPayMessage = String.Format("| {0}{1:0.00} р.",OverPaymentName, OverPaymentSum);
	Console.WriteLine("{0}{1}|", OverPayMessage, Replicate(' ',TableSize - OverPayMessage.Length + 1));
	Console.WriteLine("|{0}|",Replicate('-',TableSize));
}

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

int GetCurrentMonthCount(double MonthAnnuityPayment, double Payment, double i)
{
	//return (Math.Log(1 + LoanRate, PaymentSum / (PaymentSum - LoanRate * Sum)));
	return (int)Math.Ceiling(Math.Log(MonthAnnuityPayment / (MonthAnnuityPayment - i * Payment), 1 + i));
}

double GetPaymentCalc(double sum, double rate, double term, DateTime PaymentDate, int ExtraPayMonthNum, double ExtraPaySum, bool IsSumReduce)
{
	int PaymentNumber = 0;
	double RemainDebt = sum;
	double i = rate / 12;
	double MonthAnnuityPayment = GetMonthAnnuityPayment(sum, term, i);
	double Overpayment = 0;
	double Percentages;
	double Payment;

	WriteTableName("ДОСРОЧНОЕ ПОГАШЕНИЕ С УМЕНЬШЕНИЕМ " + (IsSumReduce? "СУММЫ" : "СРОКА"), 95);
	WriteTableHorizontalLine(15);
	WriteTableHead();
	while (PaymentNumber++ < term)
	{
		PaymentDate = PaymentDate.AddMonths(1);
		Percentages = GetMonthPaymentPercent(RemainDebt, rate, GetPeriodDaysCount(PaymentDate), GetYearDaysCount(PaymentDate.Year));
		Payment = MonthAnnuityPayment - Percentages;
		RemainDebt -= Payment;

		if (PaymentNumber == term)	// Percentages > RemainDebt ?
		{
			MonthAnnuityPayment += RemainDebt;
			MonthAnnuityPayment += GetMonthPaymentPercent(RemainDebt, rate, GetPeriodDaysCount(PaymentDate), GetYearDaysCount(PaymentDate.Year));
			RemainDebt = 0;
		}
		Overpayment += MonthAnnuityPayment;

		WriteTableValue(PaymentNumber, PaymentDate, MonthAnnuityPayment, Payment, Percentages, RemainDebt);
		if (PaymentNumber == ExtraPayMonthNum)
		{
			RemainDebt -= ExtraPaySum;
			Overpayment += ExtraPaySum;
			if (IsSumReduce)
				MonthAnnuityPayment = GetMonthAnnuityPayment(RemainDebt, term - ExtraPayMonthNum, i);
			else
				term -= GetCurrentMonthCount(MonthAnnuityPayment, Payment, i);
		}
	}
	WriteTableHorizontalLine(15);
	return (Overpayment - sum);
}


bool GetOuterParams(string[] args, out double sum, out double rate, out int term, ref int selectedMonth, ref double payment)
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

double	sum				=	1000000.0	;	//	Credit total summ
double	rate			=	12.0		;	//	Annual interest rate
int		term			=	10			;	//	Number of loan months
int		selectedMonth	=	5			;	//	Number of month in which early payment was made
double	payment			=	100000		;	//	Sum of early payment

DateTime	PaymentDate = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(0); //"01.01.2021"

sum				= 0;
rate			= 0;
term			= 0;
selectedMonth	= 0;
payment			= 0;
selectedMonth	= 0;
payment			= 0;

// Check input params
if (
	!((args.Length == 5 || args.Length == 3) && GetOuterParams(args, out sum, out rate, out term, ref selectedMonth, ref payment))
)
{
	Console.WriteLine("Something went wrong. Check your input and retry.");
	Environment.Exit(0);
}
double		OverPaymentSum;
double		OverPaymentDate;
OverPaymentSum = GetPaymentCalc(sum, rate / 100, term, PaymentDate, selectedMonth, payment, true);
OverPaymentDate = GetPaymentCalc(sum, rate / 100, term, PaymentDate, selectedMonth, payment, false);
WriteTableOverPayment("Переплата при уменьшении платежа: ",OverPaymentSum, 95);
WriteTableOverPayment("Переплата при уменьшении срока: ",OverPaymentDate, 95);

if (OverPaymentSum > OverPaymentDate)
	WriteTableOverPayment("Уменьшение срока выгоднее уменьшения платежа на: ", OverPaymentSum - OverPaymentDate, 95);
else
if (OverPaymentSum > OverPaymentDate)
	WriteTableOverPayment("Уменьшение платежа выгоднее уменьшения срока на: ", OverPaymentDate - OverPaymentSum, 95);
else
	WriteTableOverPayment("Переплата одинакова в обоих вариантах, разница: ", 0, 95);
