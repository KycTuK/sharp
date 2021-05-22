using System;

namespace credit_calc
{
	class Program
	{
		uint GetYearDaysCount(ushort year)
		{
			if ((year % 4) != 0 && (year % 100) >= 0)
				return (366);
			else if ((year % 400) != 0)
				return (366);
			return (365);
		}
		double GetMonthPaymentPercent(double TotalDept, double InterestRate, uint PeriodDaysCount, ushort Year)
		{
			return ((TotalDept * InterestRate * PeriodDaysCount) / (100 * GetYearDaysCount(Year)));
		}
		double GetMonthsCount(double PaymentSum, double i, double TotalDept)
		{
			i /= 1200;
			return (Math.Log(i, PaymentSum / (PaymentSum - i * TotalDept)));
		}
		double GetAnnuityPayment(double csum, double i, ushort n)
		{
			i /= 1200;
			return ((csum * i * Math.Pow((i + 1), n)) / (Math.Pow((i + 1), n) - 1));
		}

		static void Main(string[] args)
		{
			Console.WriteLine("WTF?");
		}
	}
}
