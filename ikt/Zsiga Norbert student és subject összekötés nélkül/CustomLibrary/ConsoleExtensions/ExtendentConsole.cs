namespace Custom.Library.ConsoleExtensions;

    public static class ExtendentConsole
    {
        public static int ReadInteger(string promt)
        {
            bool isNumber = false;
            int number = 0;
            do
            {
            Console.Write($"{promt}");
            string text = Console.ReadLine();
                isNumber = int.TryParse( text, out number );
            }
            while (!isNumber);

            return number;
        }

        public static int ReadInteger(int minimum, string promt)
            {
                bool isNumber = false;
                int number = 0;
                do
                {
            Console.Write($"{promt}");
                    string text = Console.ReadLine();
                    isNumber = int.TryParse(text, out number);
                    if(number < minimum)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"A megadott szám nem lehet kisebb, mint {minimum}");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"A folytatáshoz nyomja meg bármely gombot!");

                Console.ReadKey();
                Console.Clear();

                Console.ResetColor();
            }
                }
                while (!isNumber || number < minimum);

                return number;
            }

    public static int ReadInteger(int minimum, int maximum, string promt)
    {
        bool isNumber = false;
        int number = 0;
        do
        {
            Console.Write($"{promt}");
            string text = Console.ReadLine();
            isNumber = int.TryParse(text, out number);
            if (number < minimum || number > maximum)
            { 
                Console.WriteLine($"Hibás számot adott meg, {minimum} és {maximum} közötti számot kell megadnia");
            }
        }
        while (!isNumber || number < minimum || number > maximum);

        return number;
    }

    public static ulong ReadUlong(string promt, ulong min, ulong max)
    {
        bool isNumber = false;
        ulong number = 0;
        do
        {
            Console.Write($"{promt}");
            string text = Console.ReadLine();
            isNumber = ulong.TryParse(text, out number);
        }
        while (!isNumber || min > number || max < number);

        return number;
    }

    public static double ReadDouble(string promt)
        {
            bool isNumber = false;
            double number = 0;
            do
            {
            Console.Write($"{promt}");
            string text = Console.ReadLine();
                isNumber = double.TryParse(text, new CultureInfo("en-EN"), out number);
            }
            while (!isNumber);

            return number;
        }

    public static double ReadDouble(double min,string promt)
    {
        bool isNumber = false;
        double number = 0;
        do
        {
            Console.Write($"{promt}");
            string text = Console.ReadLine();
            isNumber = double.TryParse(text, new CultureInfo("en-EN"), out number);
        }
        while (!isNumber || number < min);

        return number;
    }

    public static double ReadDouble(double min,double max, string promt)
    {
        bool isNumber = false;
        double number = 0;
        do
        {
            Console.Write($"{promt}");
            string text = Console.ReadLine();
            isNumber = double.TryParse(text, new CultureInfo("en-EN"), out number);
        }
        while (!isNumber || number < min || number > max);

        return number;
    }

    public static string ReadString(string promt)
    {
        string text = string.Empty;
        do
        {
            Console.Write($"{promt}");
            text = Console.ReadLine().Trim();
           
        }
        while (string.IsNullOrWhiteSpace(text));

        return text;
    }
    public static DateTime ReadDateTime(string prompt)
    {
        bool isDateTime = false;
        DateTime dateTime;
        do
        {
            Console.WriteLine($"{prompt}");
            string text = Console.ReadLine();
            isDateTime = DateTime.TryParse(text, out dateTime);
        } while (!isDateTime);
        return dateTime;

    }

    public static DateTime ReadDateTime(string prompt, DateTime maximumDate)
    {
        bool isDateTime = false;
        DateTime dateTime;
        do
        {
            Console.WriteLine($"{prompt}");
            string text = Console.ReadLine();
            isDateTime = DateTime.TryParse(text, out dateTime);
        } while (!isDateTime || dateTime > maximumDate );
        return dateTime;

    }
    public static DateTime ReadDateTime(string prompt,DateTime minimumDate, DateTime maximumDate)
    {
        bool isDateTime = false;
        DateTime dateTime;
        do
        {
            Console.WriteLine($"{prompt}");
            string text = Console.ReadLine();
            isDateTime = DateTime.TryParse(text, out dateTime);
        } while (!isDateTime || dateTime > maximumDate || dateTime < minimumDate);
        return dateTime;

    }

}

