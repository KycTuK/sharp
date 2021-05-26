using System;
using System.Data;
using System.Runtime.InteropServices;

Main(args);

int GetYearDaysCount(int Year)
{
	return (DateTime.IsLeapYear(Year) ? 366 : 365);
}
int GetPeriodDaysCount(DateTime CurrentDate)
{
	return (CurrentDate.Subtract(CurrentDate.AddMonths(-1)).Days + 1);
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
double GetCurrentMonthCount(double TotalSum, double PaymentSum, double LoanRate)
{
	return (Math.Log(1 + LoanRate, PaymentSum / (PaymentSum - LoanRate * TotalSum)));
}
*/

double GetSumReduce(double sum, double rate, double term, DateTime PaymentDate)
{
	Console.WriteLine("\t\t|{0:N2}\t\t|{0:N2}\t\t|{0:N2}",PaymentDate,);
}

void Main(string[] args)
{

	double	sum				=	1000000.0	;	//	Credit total summ
	double	rate			=	12.0		;	//	Annual interest rate
	int		term			=	10			;	//	Number of loan months
	// int		selectedMonth	=	5			;	//	Number of month in which early payment was made
	// double	payment			=	100000		;	//	Sum of early payment

	rate /= 100;
	DateTime	PaymentDate			=	DateTime.Now.AddDays(1 - DateTime.Now.Day);
	double		MonthAnnuityPayment	=	GetMonthAnnuityPayment(sum, term, rate / 12);
	double		Percentages			=	GetMonthPaymentPercent(sum, rate, GetPeriodDaysCount(PaymentDate), GetYearDaysCount(PaymentDate.Year));

	Console.WriteLine("\t\t|Date\t\t|Payment\t\t|Percentages\t\t");

	Console.WriteLine("Percentages: {0}",Percentages);
	Console.WriteLine("MonthAnnuityPayment = {0:N2}",MonthAnnuityPayment);
	Console.WriteLine("Переплата при уменьшении платежа: {0:N2}р.",sum - MonthAnnuityPayment);

	Console.WriteLine("CurrentDate: {0}",PaymentDate);
	Console.WriteLine("CurrentDate: {0}",PaymentDate.AddMonths(-1));
	Console.WriteLine("GetPeriodDaysCount: {0}",GetPeriodDaysCount(PaymentDate));
	Console.WriteLine("GetYearDaysCount: {0}",GetYearDaysCount(PaymentDate.Year));
/*
	int i = 4;
	while (i++ > 0)
	{
		Percentages =
	}
	Console.WriteLine(" {0} ", GetMonthPaymentPercent(sum, MonthRate, PeriodDaysCount));
*/
}
