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
	// return (Date.AddMonths(1).Subtract(Date).Days);
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
void GetSumReduce(double sum, double rate, double term, DateTime PaymentDate, int ExtraPayMonthNum, double ExtraPaySum)
{
	int PaymentMonthNum = 0;
	double Remainder = sum;
	double MonthAnnuityPayment = GetMonthAnnuityPayment(sum, term, rate / 12);
	double Percentages;
	Console.WriteLine("| PayNum \t| Date  \t| Payment \t| TotalDebt \t| Percentages \t| RemainDebt ");
	Console.WriteLine("|--------\t|-------\t|---------\t|-----------\t|-------------\t|------------");
	while (PaymentMonthNum++ < term)
	{

		PaymentDate = PaymentDate.AddMonths(1);
		Percentages = GetMonthPaymentPercent(Remainder, rate, GetPeriodDaysCount(PaymentDate), GetYearDaysCount(PaymentDate.Year));
		Remainder -= (MonthAnnuityPayment - Percentages);
		if (PaymentMonthNum == ExtraPayMonthNum)
		{
			Remainder -= ExtraPaySum;
			sum = Remainder;
			term -= ExtraPayMonthNum;
			MonthAnnuityPayment = GetMonthAnnuityPayment(sum, term, rate / 12);
		}
		if (Percentages > Remainder)
		{
			MonthAnnuityPayment += Remainder;
			Remainder = 0;
		}
		Console.WriteLine("|{0,3}\t\t|{1:dd/MM/yy}\t|{2,10:N2}\t|{3,11:N2}\t|{4,10:N2}\t|{5,11:N2}"
			,PaymentMonthNum
			,PaymentDate
			,MonthAnnuityPayment
			,MonthAnnuityPayment - Percentages
			,Percentages
			,Remainder
		);

	}
	// return ();
}


bool ReadParams(string[] args, out double sum, out double rate, out int term,
            out int selectedMonth, out double payment)
{
    bool ret;

    ret = true;
    ret &= Double.TryParse(args[0], out sum);
    ret &= Double.TryParse(args[1], out rate);
    ret &= Int32.TryParse(args[2], out term);
    ret &= Int32.TryParse(args[3], out selectedMonth);
    ret &= Double.TryParse(args[4], out payment);
    if (sum <= 0 || rate <= 0 || term <= 0 || selectedMonth <= 0 || selectedMonth > term || payment <= 0 ||
        payment > sum)
        ret &= false;
    return (ret);
}

void Main(string[] args)
{

	double	sum				=	1000000.0	;	//	Credit total summ
	double	rate			=	12.0		;	//	Annual interest rate
	int		term			=	10			;	//	Number of loan months
	// int		selectedMonth	=	5			;	//	Number of month in which early payment was made
	// double	payment			=	100000		;	//	Sum of early payment

	rate /= 100;
	DateTime	PaymentDate = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(0);
	// double		MonthAnnuityPayment	=	GetMonthAnnuityPayment(sum, term, rate / 12);
	// double		Percentages			=	GetMonthPaymentPercent(sum, rate, GetPeriodDaysCount(PaymentDate), GetYearDaysCount(PaymentDate.Year));

	// Console.WriteLine("{0}:d",DateTime.Parse("01/01/2021",enUS));

	GetSumReduce(sum,rate,term,PaymentDate, 5, 100000);

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
