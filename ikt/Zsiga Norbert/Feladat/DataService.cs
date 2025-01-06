using System.Reflection.Metadata.Ecma335;

namespace Kreta.ConsoleApp;

public static class DataService
{
    public static DateTime CreateCustomDate(int year, string monthAndDay) => DateTime.Parse($"{year}-{monthAndDay}");
    public static DateTime CreateCustomDate(int year, int month, int day) => DateTime.Parse($"{year}-{month}-{day}");
}
