using System;

namespace credit_calc
{
	class Program
	{
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
		static double GetMonthPaymentPercent(double TotalSum, double InterestRate, uint PeriodDaysCount, ushort Year)
		{
			return ((TotalSum * InterestRate * PeriodDaysCount) / (100));
		}

		static void Main(string[] args)
		{

			double	sum				=	1000000.0	;	//	Credit total summ
			double	rate			=	12.0		;	//	Annual interest rate
			int		term			=	10		;	//	Number of loan months
			// int		selectedMonth	=	0		;	//	Number of month in which early payment was made
			// double	payment			=	0		;	//	Sum of early payment
			int		DaysInYear		=	365 + (DateTime.IsLeapYear(DateTime.Now.Year)? 1 : 0);

Console.WriteLine("");
			Console.WriteLine("Переплата при уменьшении платежа: {0}р."
				,sum - GetMonthAnnuityPayment(sum
				,GetMonthAnnuityCoefficient(Convert.ToDouble(term)
				,GetLoanRatePerMonth(rate)
				)
				)
				);

		}
	}
}
