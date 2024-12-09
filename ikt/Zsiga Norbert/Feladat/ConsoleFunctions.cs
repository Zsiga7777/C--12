namespace Feladat
{
    public class ConsoleFunctions
    {
        public static void WriteMenu<T>(List<T> options, int selected)
        {
            Console.Clear();
            for (int i = 0; i < options.Count; i++)
            {
                if (i == selected)
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine(options[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"{options[i]}");
                }
            }
        }
       
    }
}
