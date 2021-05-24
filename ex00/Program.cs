using System;

/*
sum  double  Сумма кредита, р 1000000
rate  double Годовая процентная ставка, % 12
term  int Количество месяцев кредита 10 
selectedMonth int Номер месяца кредита, в котором вносится досрочный платёж 5
payment double Сумма досрочного платежа, р 100000 */

double sum;
double rate;
int    term;
int    selectedMonth;
double payment;

double i; // процентная ставка по займу в месяц
double anum; // анументный платеж
double proc = 0; // проценты ежемесячного платежа
double proc_minussum = 0; // переплата за уменьшение суммы платежа
double proc_minsdate = 0; // переплата за уменьшение месяцев
int month = 0; // месяц в период
int year; // текущий год
int year_day; // дней в году

if (args.Length != 5)
{
    Console.WriteLine("Ошибка ввода. Проверьте входные данные и повторите запрос.");
    return;
}
if (!double.TryParse(args[0], out sum) || !double.TryParse(args[1], out rate) ||
    !int.TryParse(args[2], out term) || !int.TryParse(args[3], out selectedMonth) ||
    !double.TryParse(args[4], out payment))
{
    Console.WriteLine("Ошибка ввода. Проверьте входные данные и повторите запрос.");
    return;  
}
i = rate / 12 / 100;
anum = Math.Round(sum * i * Math.Pow(1 + i, term) / ((Math.Pow(1 + i, term) - 1)), 2);
year = DateTime.Now.Year;
Console.WriteLine("ПЛАТЕЖ         ОД       ПРОЦЕНТЫ    ОСТАТОК ДОЛГА");

MinusSum(term, sum, payment, selectedMonth, rate, year, month, proc, anum);
Console.WriteLine("ПЛАТЕЖ         ОД       ПРОЦЕНТЫ    ОСТАТОК ДОЛГА");
MinusDate(term, sum, payment, selectedMonth, rate, year, month, proc);
WriteMess();


void MinusSum(int term, double sum, double payment, int selectedMonth, double rate, int year, int month, double proc, double anum)
{
    for (int m = 1; m <= term; m++)
    {
        month = m - 1 + DateTime.Now.Month;
        while (month > 12)
            month -= 12;
        if (m != 1 && month == 1)
            year++;
        if (DateTime.IsLeapYear(year))
            year_day = 366;
        else
            year_day = 365;
        proc = Math.Round(sum * rate * DateTime.DaysInMonth(year, month) / (100 * year_day), 2);
        sum -= anum - proc;
        sum = Math.Round(sum, 2);
        proc_minussum += Math.Round(proc, 2);
        if (m == selectedMonth)
        {
            sum -= payment;
            anum = Math.Round(sum * i * Math.Pow(1 + i, term - m) / ((Math.Pow(1 + i, term - m) - 1)), 2);
        }
        Console.WriteLine(anum + "    " + Math.Round((anum - proc), 2) + "    " + proc + "    " + sum);
    }
    Console.WriteLine("\tОбщая переплата " + Math.Round(proc_minussum, 2)); 
}


void MinusDate(int term, double sum, double payment, int selectedMonth, double rate, int year, int month, double proc)
{
    for (int m = 1; m <= term; m++)
    {
        month = m - 1 + DateTime.Now.Month;
        while (month > 12)
            month -= 12;
        if (m != 1 && month == 1)
            year++;
        if (DateTime.IsLeapYear(year))
            year_day = 366;
        else
            year_day = 365;
        proc = Math.Round(sum * rate * DateTime.DaysInMonth(year, month) / (100 * year_day), 2);
        if (sum + proc < anum)
            break;
        sum -= anum - proc;
        sum = Math.Round(sum, 2);
        proc_minsdate += Math.Round(proc, 2);
        if (m == selectedMonth)
            sum -= payment;
        Console.WriteLine(anum + "    " + Math.Round((anum - proc), 2) + "    " + proc + "    " + sum);
    }
    proc_minsdate += proc;
    Console.WriteLine("\tОбщая переплата " + Math.Round(proc_minsdate, 2)); 
}

void WriteMess()
{
    if (proc_minussum > proc_minsdate)
    {
        Console.WriteLine("Переплата при уменьшении платежа: " + Math.Round(proc_minussum, 2) + "p.");
        Console.WriteLine("Переплата при уменьшении срока: " + Math.Round(proc_minsdate, 2) + "p.");
        Console.WriteLine("Уменьшение срока выгоднее уменьшения платежа на: " + Math.Round(proc_minussum - proc_minsdate, 2) + "p.");
    }
    else if (proc_minussum < proc_minsdate)
    {
        Console.WriteLine("Переплата при уменьшении платежа: " + Math.Round(proc_minussum, 2) + "p.");
        Console.WriteLine("Переплата при уменьшении срока: " + Math.Round(proc_minsdate, 2) + "p.");
        Console.WriteLine("Уменьшение платежа выгоднее уменьшения срока на: " + Math.Round(proc_minussum - proc_minsdate, 2) + "p.");     
    }
    else
    {
        Console.WriteLine("Переплата при уменьшении платежа: " + Math.Round(proc_minussum, 2) + "p.");
        Console.WriteLine("Переплата при уменьшении срока: " + Math.Round(proc_minsdate, 2) + "p.");  
    }
}