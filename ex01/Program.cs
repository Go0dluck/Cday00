using System;
using System.IO;

Console.Write(">Enter name: ");
var name = Console.ReadLine();
string path; // file name
string[] str; // string array from file
string[] str_name2;
string[] str_name3;
int dist;
int a = 0, s = 0;

if (name.Length == 0)
{
    Console.WriteLine("Your name was not found.");
    return;   
}
if (CheckName(name) == 1)
{
    Console.WriteLine("Error name");
    return;
}
path = Path.GetFullPath("names.txt");
if (!File.Exists(path))
{
    Console.WriteLine("Error file");
    return;
}
str = File.ReadAllLines(path);
str_name2 = new string[str.Length];
str_name3 = new string[str.Length];
for (int i = 0; i < str.Length; i++)
{
    if (name == str[i])
    {
        Console.WriteLine("Hello, " + str[i] + "!");
        return;
    }
}
for (int i = 0; i < str.Length; i++)
{
    dist = DamerauLevenshteinDistance(name, str[i]);
    if (dist == 1)
    {
        Console.Write(">Did you mean " + str[i] + "? Y/N ");
        if (Console.ReadKey().KeyChar == 'y')
        {
            Console.WriteLine("\nHello, " + str[i] + "!");
            return;  
        }
        Console.WriteLine();
    }

    if (dist == 2)
        str_name2[a++] = str[i];
    if (dist == 3)
        str_name3[s++] = str[i];
}
for (int i = 0; i < str_name2.Length; i++)
{
    if (str_name2[i] == null)
        break;
    Console.Write(">2Did you mean " + str_name2[i] + "? Y/N ");
    if (Console.ReadKey().KeyChar == 'y')
    {
        Console.WriteLine("\nHello, " + str_name2[i] + "!");
        return;
    }
    Console.WriteLine();
}
for (int i = 0; i < str_name3.Length; i++)
{
    if (str_name3[i] == null)
        break;
    Console.Write(">3Did you mean " + str_name3[i] + "? Y/N ");
    if (Console.ReadKey().KeyChar == 'y')
    {
        Console.WriteLine("\nHello, " + str_name3[i] + "!");
        return;
    }
    Console.WriteLine();   
}
Console.WriteLine("Your name was not found.");

static int MinimumDwo(int a, int b)
{
    return a < b ? a : b;
}

static int Minimum(int a, int b, int c)
{
    return (a = a < b ? a : b) < c ? a : c;
}

static int DamerauLevenshteinDistance(string nameTmp, string strTmp)
{
    var n = nameTmp.Length + 1;
    var m = strTmp.Length + 1;
    var arrayD = new int[n, m];

    for (var i = 0; i < n; i++)
    {
        arrayD[i, 0] = i;
    }
    for (var j = 0; j < m; j++)
    {
        arrayD[0, j] = j;
    }
    for (var i = 1; i < n; i++)
    {
        for (var j = 1; j < m; j++)
        {
            var cost = nameTmp[i - 1] == strTmp[j - 1] ? 0 : 1;

            arrayD[i, j] = Minimum(arrayD[i - 1, j] + 1,          // удаление
                arrayD[i, j - 1] + 1,         // вставка
                arrayD[i - 1, j - 1] + cost); // замена

            if (i > 1 && j > 1 
                      && nameTmp[i - 1] == strTmp[j - 2]
                      && nameTmp[i - 2] == strTmp[j - 1])
            {
                arrayD[i, j] = MinimumDwo(arrayD[i, j],
                    arrayD[i - 2, j - 2] + cost); // перестановка
            }
        }
    }
    return arrayD[n - 1, m - 1];
}

static int CheckName(string nameTmp)
{
    var spec = "!@#$%^&*()+-={}[];:*\\\"\'";
    
    for (int i = 0; i < nameTmp.Length; i++)
    {
        if (Char.IsDigit(nameTmp[i]) || spec.IndexOf(nameTmp[i]) != -1)
            return 1;
    }
    return 0;
}