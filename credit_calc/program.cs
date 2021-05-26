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
double GetCurrentMonthCount(double TotalSum, double PaymentSum, double LoanRate)
{
	return (Math.Log(1 + LoanRate, PaymentSum / (PaymentSum - LoanRate * TotalSum)));
}
*/

void GetSumReduce(double sum, double rate, double term, DateTime PaymentDate, double MonthAnnuityPayment, double Percentages)
{
	int i = 0;
	while (i++ < 10)
	{
		Console.WriteLine("\t\t|{0:dd.MM.yy}\t|{1:N2}\t\t|{2:N2}\t\t{3}\t\t{4:N2}"
			,PaymentDate
			,MonthAnnuityPayment
			,Percentages
			,GetPeriodDaysCount(PaymentDate)
			,MonthAnnuityPayment - Percentages
		);
		PaymentDate = PaymentDate.AddMonths(1);
		MonthAnnuityPayment = GetMonthAnnuityPayment(sum, term, rate / 12);
		Percentages = GetMonthPaymentPercent(sum, rate, GetPeriodDaysCount(PaymentDate), GetYearDaysCount(PaymentDate.Year));

	}
	// return ();
}

/*
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
*/
void Main(string[] args)
{

	double	sum				=	1000000.0	;	//	Credit total summ
	double	rate			=	12.0		;	//	Annual interest rate
	int		term			=	10			;	//	Number of loan months
	// int		selectedMonth	=	5			;	//	Number of month in which early payment was made
	// double	payment			=	100000		;	//	Sum of early payment

	rate /= 100;
	DateTime	PaymentDate = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(1);
	double		MonthAnnuityPayment	=	GetMonthAnnuityPayment(sum, term, rate / 12);
	double		Percentages			=	GetMonthPaymentPercent(sum, rate, GetPeriodDaysCount(PaymentDate), GetYearDaysCount(PaymentDate.Year));

	Console.WriteLine("\t\t|Date\t\t|Payment\t\t|Percentages\t\t");
	GetSumReduce(sum,rate,term,PaymentDate,MonthAnnuityPayment,Percentages);

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
