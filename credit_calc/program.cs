using System;

namespace credit_calc
{
	class Program
	{
		static int GetYearDaysCount(int Year)
		{
			return (DateTime.IsLeapYear(Year) ? 366 : 365);
		}
		static int GetPeriodDaysCount(DateTime CurrentDate)
		{
			return (CurrentDate.Subtract(CurrentDate.AddMonths(-1)).Days);
		}
		static double GetLoanRatePerMonth(double YearRate)
		{
			return (YearRate / 12 / 100);
		}
		static double GetMonthAnnuityCoefficient(double MonthCount, double MonthRate)
		{
			double MonthRateCount = Math.Pow((1 + MonthRate), MonthCount);
			return (MonthRate * MonthRateCount / (MonthRateCount - 1));
		}
		static double GetMonthAnnuityPayment(double TotalSum, double AnnuityCoefficient)
		{
			return (TotalSum * AnnuityCoefficient);
		}
		static double GetCurrentMonthPaymentPercent(double TotalSum, double MonthRate, uint PeriodDaysCount, ushort DaysInYear)
		{
			return ((TotalSum * MonthRate * PeriodDaysCount) / (100));
		}
		static double GetCurrentMonthCount(double TotalSum, double PaymentSum, double LoanRate)
		{
			return (Math.Log(1 + LoanRate, PaymentSum / (PaymentSum - LoanRate * TotalSum)));
		}

		static void Main(string[] args)
		{

			double	sum				=	1000000.0	;	//	Credit total summ
			double	rate			=	12.0		;	//	Annual interest rate
			int		term			=	10			;	//	Number of loan months
			// int		selectedMonth	=	5			;	//	Number of month in which early payment was made
			// double	payment			=	100000		;	//	Sum of early payment

			int		DaysInYear		=	GetYearDaysCount(DateTime.Now.Year);
			double	MonthRate		=	GetLoanRatePerMonth(rate);
			double	MonthAnnuityCoefficient	=	GetMonthAnnuityCoefficient(Convert.ToDouble(term), MonthRate);
			double	MonthAnnuityPayment	=	GetMonthAnnuityPayment(sum, MonthAnnuityCoefficient);
			// double	Percentages	=	GetCurrentMonthPaymentPercent(sum, MonthRate, 0, DaysInYear);

			Console.WriteLine("MonthRate = {0}",MonthRate);
			Console.WriteLine("MonthAnnuityCoefficient = {0}",MonthAnnuityCoefficient);
			Console.WriteLine("MonthAnnuityPayment = {0}",MonthAnnuityPayment);
			// Console.WriteLine("Percentages: {0}",Percentages);
			Console.WriteLine("Переплата при уменьшении платежа: {0}р.",sum - MonthAnnuityPayment);

			Console.WriteLine("CurrentDate: {0}",DateTime.Now);
			Console.WriteLine("AddMonths(-1): {0}",DateTime.Now.AddMonths(-1));
			Console.WriteLine("Subtract: {0}",GetPeriodDaysCount(DateTime.Parse("1 March 2007")));

			// int i = 4;
			// while (i++ > 0)
			// {
			// 	Percentages =
			// }
			// Console.WriteLine(" {0} ", GetMonthPaymentPercent(sum, MonthRate, PeriodDaysCount));
		}
	}
}
