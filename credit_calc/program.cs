using System;

namespace credit_calc
{
	class Program
	{
		double GetMonthPaymentPercent(double TotalDept, double InterestRate, uint PeriodDaysCount, ushort Year)
		{
			return ((TotalDept * InterestRate * PeriodDaysCount) / (100));
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
		{/*
			double sum;
			double rate;
			int term;
			int selectedMonth;
			double payment;
		*/
			Console.WriteLine("> {0}",DateTime.IsLeapYear(DateTime.Now.Year));
  			Console.WriteLine("Переплата при уменьшении платежа: {сумма переплаты}р.?");
		}
	}
}
