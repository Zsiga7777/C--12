namespace Feladat
{
    public class Menus
    {
        public static async Task MainMenuAsync(ApplicationDbContext dbContext)
        {
            bool endOfWork = false;
            do
            {

                Console.WriteLine("A rendszer lehetőségei:");
                int input = ReusableMenu(["Tanuló kezelése",
                "jegyek kezelése",
                "tantárgyak kezelése",
                "lakcímek kezelése"
                ]);

                switch (input)
                {
                    case -1:
                        {
                            endOfWork = true;
                            break;
                        }
                    case 0:
                        {
                            Console.Clear();
                            await StudentMenuAsync(dbContext);
                            break;
                        }
                    case 1:
                        {

                            Console.Clear();
                            await MarkMenuAsync(dbContext);
                            break;
                        }

                    case 2:
                        {
                            Console.Clear();
                            await SubjectMenuAsync(dbContext);
                            break;
                        }
                    case 3:
                        {
                            Console.Clear();
                            await LocationMenuAsync(dbContext);
                            break;
                        }

                }

            }
            while (!endOfWork);
        }
        public static async Task StudentMenuAsync(ApplicationDbContext dbContext)
        {
            bool endOfWork = false;
            do
            {

                Console.WriteLine("A rendszer lehetőségei:");
                int input = ReusableMenu(["Tanuló adat kiírása",
                "Tanulók neveinek kiírása",
                "Tanuló hozzáadása",
                "Tanuló módosítása",
                "Tanuló törlése",
            ]);

                switch (input)
                {
                    case -1:
                        {
                            endOfWork = true;
                            break;
                        }
                    case 0:
                        {
                            Console.Clear();
                            await StudentFunctions.WriteStudentData(dbContext);
                            break;
                        }
                    case 1:
                        {

                            Console.Clear();
                            ReusableMenu(await dbContext.Students.Select(x => x.Name).ToListAsync());
                            break;
                        }

                    case 2:
                        {
                            Console.Clear();
                            await StudentFunctions.AddNewStudentsAsync(dbContext);
                            break;
                        }
                    case 3:
                        {
                            Console.Clear();

                            await StudentFunctions.ModifyStudentsDataAsync(dbContext);
                            break;
                        }
                    case 4:
                        {
                            Console.Clear();
                            await StudentFunctions.DeleteStudentsDataAsync(dbContext);
                            break;
                        }
                }
            }
            while (!endOfWork);
        }

        public static async Task MarkMenuAsync(ApplicationDbContext dbContext)
        {
            bool endOfWork = false;
            do
            {

                Console.WriteLine("A rendszer lehetőségei:");
                int input = ReusableMenu(["Jegy hozzáadása",
                "Jegy módosítása",
                "Jegy törlése",
            ]);

                switch (input)
                {
                    case -1:
                        {
                            endOfWork = true;
                            break;
                        }

                    case 0:
                        {
                            Console.Clear();
                            await MarkFunctions.AddNewMarkAsync(dbContext);
                            break;
                        }
                    case 1:
                        {
                            Console.Clear();
                            await MarkFunctions.ModifyMarkAsync(dbContext);
                            break;
                        }
                    case 2:
                        {
                            Console.Clear();
                            await MarkFunctions.DeleteMarkAsync(dbContext);
                            break;
                        }
                }

            }
            while (!endOfWork);
        }

        public static async Task SubjectMenuAsync(ApplicationDbContext dbContext)
        {
            bool endOfWork = false;
            do
            {

                Console.WriteLine("A rendszer lehetőségei:");
                int input = ReusableMenu([
                "Tantárgy törlése",
                "Tantárgy módosítása",
            ]);

                switch (input)
                {
                    case -1:
                        {
                            endOfWork = true;
                            break;
                        }
                    case 0:
                        {
                            Console.Clear();
                            await SubjectFunctions.DeleteSubjectAsync(dbContext);
                            break;
                        }
                    case 1:
                        {
                            Console.Clear();
                            await SubjectFunctions.ModifySubjectNameAsync(dbContext);
                            break;
                        }
                }

            }
            while (!endOfWork);
        }

        public static async Task LocationMenuAsync(ApplicationDbContext dbContext)
        {
            bool endOfWork = false;
            do
            {

                Console.WriteLine("A rendszer lehetőségei:");
                int input = ReusableMenu([
                    "Cím törlése",
                "Cím módosítása",
                "Utca törlése",
                "Utca módosítása",
                "Város módosítása",
                "Ország módosítása",
            ]);

                switch (input)
                {
                    case -1:
                        {
                            endOfWork = true;
                            break;
                        }

                    case 0:
                        {
                            Console.Clear();
                            await AddressFunctions.DeleteAddressAsync(dbContext);
                            break;
                        }
                    case 1:
                        {
                            Console.Clear();
                            await AddressFunctions.ModifyAddressAsync(dbContext);
                            break;
                        }
                    case 2:
                        {
                            Console.Clear();
                            await StreetFunctions.DeleteStreetAsync(dbContext);
                            break;
                        }
                    case 3:
                        {
                            Console.Clear();
                            await StreetFunctions.ModifyStreetAsync(dbContext);
                            break;
                        }
                    case 4:
                        {
                            Console.Clear();
                            await CityFunctions.ModifyCityAsync(dbContext);
                            break;
                        }
                    case 5:
                        {
                            Console.Clear();
                            await CountryFunctions.ModifyCountryAsync(dbContext);
                            break;
                        }
                }

            }
            while (!endOfWork);
        }


        public static int ReusableMenu<T>(List<T> options)
        {
            int index = 0;
            int pageNumber = 0;
            ConsoleKeyInfo keyinfo;
            do
            {
                List<T> currentPage = options.Skip((pageNumber) * 10).Take(10).ToList();
                ConsoleFunctions.WriteMenu(currentPage, index);
                Console.WriteLine("\nfelfele lépéshez felfelenyíl, lefele lépéhez lefelenyíl, kiválasztáshoz enter, kilépéshez e");
                Console.WriteLine("előző oldal balranyíl, követkető oldal jobbranyíl");
                keyinfo = Console.ReadKey();
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 < 0)
                    {
                        index = 0;
                    }
                    else
                    {
                        index--;
                    }
                }
                else if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 + pageNumber * 10 >= options.Count)
                    {
                        index = 0;
                    }
                    else if (index == 9)
                    {
                        index = 0;
                    }
                    else
                    {
                        index++;
                    }
                }
                else if (keyinfo.Key == ConsoleKey.LeftArrow)
                {
                    if (pageNumber - 1 < 0 && options.Count > 10)
                    {
                        pageNumber = options.Count / 10;
                        index = 0;
                    }
                    else if (pageNumber - 1 < 0)
                    {
                        pageNumber = 0;
                        index = 0;
                    }
                    else
                    {
                        pageNumber--;
                        index = 0;
                    }
                }
                else if (keyinfo.Key == ConsoleKey.RightArrow)
                {
                    if (pageNumber + 1 >= options.Count / (double)10)
                    {
                        pageNumber = 0;
                        index = 0;
                    }
                    else
                    {
                        pageNumber++;
                        index = 0;
                    }
                }
                else if (keyinfo.Key == ConsoleKey.Enter)
                {
                    return index + pageNumber * 10;
                }
                else if (keyinfo.Key == ConsoleKey.E)
                {
                    return -1;
                }
            } while (true);
        }
       
    }
}
