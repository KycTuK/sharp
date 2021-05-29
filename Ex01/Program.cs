
using System;


int MinNumber(int a, int b, int c)
{
	return ((a = a > b ? b : a) > c ? c : a);
}

int LevenshteinDis(string str1, string str2)
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

bool searchExactName(string name, string[] allName)
{
	foreach (string nameBase in allName)
	{
		if (name == nameBase)
			return (true);
	}
	return (false);
}


bool NameClarification(string name)
{
	string answer;

	Console.WriteLine($">Did you mean “{name}”? Y/N");
	while ((answer = Console.ReadLine()) != null)
	{
		if (answer == "y" || answer == "Y")
			return (true);
		if (answer == "N" || answer == "n")
			return (false);
		Console.WriteLine("The answer can be either Y (yes) or N (no)");
	}
	return (false);
}

bool CheckedValidSimvols(string name)
{
	if (name == "")
		return (false);
	foreach (char c in name)
	{
		if (!((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ' || c == '-'))
			return (false);
	}
	return (true);
}

string NameVerification(string name)
{
	string[] allNames;

	if (!CheckedValidSimvols(name))
		return (null);
	allNames = File.ReadAllLines("names.txt");
	if (searchExactName(name, allNames))
		return (name);
	foreach (string possibleName in allNames)
	{
		if (LevenshteinDis(name, possibleName) < 3 && NameClarification(possibleName))
			return (possibleName);
	}
	return (null);
}


string name;

Console.WriteLine(">Enter name:");
if ((name = NameVerification(Console.ReadLine())) == null)
	Console.WriteLine("Your name was not found");
else
	Console.WriteLine($"Hello, {name}!");

