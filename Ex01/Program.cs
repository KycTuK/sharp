using System.Globalization;
using System.Linq;

using System;
using System.IO;


int MinNumber(int a, int b, int c)
{
	return ((a = a > b ? b : a) > c ? c : a);
}

int GetLevenshteinDistance(string str1, string str2)
{
	int diffChar;
	int len1 = str1.Length + 1;
	int len2 = str2.Length + 1;
	int [,]mas = new int [len1, len2];

	for (int i = 0; i < len1; i++)
		mas[i, 0] = i;
	for (int i = 0; i < len2; i++)
		mas[0, i] = i;
	for (int i = 1; i < len1; i++)
	{
		for (int j = 1; j < len2; j++)
		{
			diffChar = str1[i - 1] == str2[j - 1] ? 0 : 1;
			mas[i, j] = MinNumber(mas[i, j - 1] + 1, mas[i - 1, j] + 1, mas[i - 1, j - 1] + diffChar);
		}
	}
	return (mas[len1 - 1, len2 - 1]);
}

bool SearchTheName(string SearchedName, string[] NameList)
{
	foreach (string Name in NameList)
	{
		if (SearchedName == Name)
			return (true);
	}
	return (false);
}


bool NameClarification(string Name)
{
	ConsoleKey PressedKey;

	Console.Write($"$ Did you mean ”{Name}”? (Y/N): ");
	while ((PressedKey = Console.ReadKey().Key) != ConsoleKey.Escape)	// Just for fun!
	{
		Console.WriteLine();
		if (PressedKey == ConsoleKey.Y)
			return (true);
		else
		if (PressedKey == ConsoleKey.N)
			return (false);
		else
			Console.Write("$ The answer can be either Y (yes) or N (no): ");
	}
	Console.WriteLine();
	return (true);
}

bool IsValid(string Name)
{
	if (Name.Length == 0)
		return (false);
	foreach (char c in Name)
		if (!(Enumerable.Range('A','Z').Contains(Char.ToUpper(c)) || " -".Contains(c)))
			return (false);
	return (true);
}

string NameVerification(string Name, string[] NamesList)
{
	Name.Trim();
	if (!IsValid(Name))
		return (null);

	if (SearchTheName(Name, NamesList))
		return (Name);
	foreach (string PredictName in NamesList)
	{
		if (GetLevenshteinDistance(Name, PredictName) < 2 && NameClarification(PredictName))
			return (PredictName);
	}
	return (null);
}

string Name;
string FileName;
string[] NamesList;

FileName = "names.txt";

if (!File.Exists(FileName))
{
	Console.WriteLine($"File ”{FileName}” Read Error! Check is it exists and avalible.");
	Environment.Exit(0);
}

NamesList = File.ReadAllLines(FileName);
Console.WriteLine("$ Enter Name: ");
if ((Name = NameVerification(Console.ReadLine(), NamesList)) == null)
	Console.WriteLine("$ Your Name was not found!");
else
	Console.WriteLine($"$ Hello, {Name}!");

