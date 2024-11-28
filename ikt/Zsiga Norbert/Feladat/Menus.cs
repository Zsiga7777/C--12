

using ChineseKreta.Database;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
                int input = ReusableMenu(["Tanuló adat kiírása",
                    "Tanulók neveinek kiírása",
                    "Tanulók hozzáadása",
                    "Tanulók módosítása",
                    "Tanulók törlése",
                    "Cím törlése",
                    "Cím módosítása",
                    "Tantárgy törlése",
                    "Tantárgy módosítása",
                    "Jegy hozzáadása",
                    "Jegy módosítása",
                    "Jegy törlése"]);

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
                            await ConsoleFunctions.WriteStudentData(dbContext);
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
                            await DataService.AddNewStudentsAsync(dbContext);
                            break;
                        }
                    case 3:
                        {
                            Console.Clear();
                           
                            await DataService.ModifyStudentsDataAsync(dbContext);
                            break;
                        }
                    case 4:
                        {
                            Console.Clear();
                            await DataService.DeleteStudentsDataAsync(dbContext);
                            break;
                        }
                    case 5:
                        {
                            Console.Clear();
                            await DataService.DeleteAddressAsync(dbContext);
                            break;
                        }
                    case 6:
                        {
                            Console.Clear();
                            await DataService.ModifyAddressAsync(dbContext);
                            break;
                        }
                    case 7:
                        {
                            Console.Clear();
                            await DataService.DeleteSubjectsAsync(dbContext);
                            break;
                        }
                    case 8:
                        {
                            Console.Clear();
                            await DataService.ModifySubjectNameAsync(dbContext);
                            break;
                        }
                    case 9:
                        {
                            Console.Clear();
                            await DataService.AddNewMarkAsync(dbContext);
                            break;
                        }
                    case 10:
                        {
                            Console.Clear();
                            await DataService.ModifyMarkAsync(dbContext);
                            break;
                        }
                    case 11:
                        {
                            Console.Clear();
                            await DataService.DeleteMarkAsync(dbContext);
                            break;
                        }
                }

            }
            while (!endOfWork);
        }
        public static async Task<uint> SelectNewOrExistingAddressCompleteAsync(ApplicationDbContext dbContext)
        {
            Console.WriteLine("Válasston lehetőséget: ");
            int input = ReusableMenu(["Meglévő cím használata", "Új cím hozzáadása"]);
            if (input == -1) { return 0; }
            uint selectedAddressId = 0;
            switch (input)
            { 
                case 0:
                    { 
                       selectedAddressId = await ConsoleFunctions.GetAddressIdCompleteAsync(dbContext);
                        break ;
                    }
                    case 1:
                    {
                        await ConsoleFunctions.AddAddressCompleteAsync(dbContext);
                        selectedAddressId = dbContext.Addresses.OrderBy(x => x.Id).Last().Id;
                        break;
                    }
            }
            return selectedAddressId;
        }
        public static async Task<uint> SelectNewOrExistingCountryAsync(ApplicationDbContext dbContext)
        {
            Console.WriteLine("Válasston lehetőséget: ");
            int input = ReusableMenu(["Meglévő ország használata", "Új ország hozzáadása"]);
            if(input == -1) { return 0; }
            uint selectedCountryId = 0;
            switch (input)
            {
                case 0:
                    {
                        selectedCountryId = await ConsoleFunctions.GetCountryIdAsync(dbContext);
                        break;
                    }
                case 1:
                    {
                        await ConsoleFunctions.AddCountryAsync(dbContext);
                        selectedCountryId = dbContext.Countries.OrderBy(x => x.Id).Last().Id;
                        break;
                    }
            }
            return selectedCountryId;
        }
        public static async Task<uint> SelectNewOrExistingCityAsync(ApplicationDbContext dbContext, uint countryId)
        {
            Console.WriteLine("Válasston lehetőséget: ");
            int input = ReusableMenu(["Meglévő város használata", "Új város hozzáadása"]);
            if(input == -1) {return 0; }
            uint selectedCityId = 0;
            switch (input)
            {
                case 0:
                    {
                        selectedCityId = await ConsoleFunctions.GetCityIdAsync(dbContext, countryId);
                        break;
                    }
                case 1:
                    {
                        await ConsoleFunctions.AddCityAsync(dbContext, countryId);
                        selectedCityId = dbContext.Cities.OrderBy(x => x.Id).Last().Id;
                        break;
                    }
            }
            return selectedCityId;
        }
        public static async Task<uint> SelectNewOrExistingStreetAsync(ApplicationDbContext dbContext, uint postalcode)
        {
            Console.WriteLine("Válasston lehetőséget: ");
            int input = ReusableMenu(["Meglévő utca használata", "Új utca hozzáadása"]);
            if(input == -1) { return 0; }
            uint selectedStreetId = 0;
            switch (input)
            {
                case 0:
                    {
                        selectedStreetId = await ConsoleFunctions.GetStreetIdAsync(dbContext, postalcode);
                        break;
                    }
                case 1:
                    {
                        await ConsoleFunctions.AddStreetAsync(dbContext, postalcode);
                        selectedStreetId = dbContext.Streets.OrderBy(x => x.Id).Last().Id;
                        break;
                    }
            }
            return selectedStreetId;
        }
        public static async Task<uint> SelectNewOrExistingSubjectAsync(ApplicationDbContext dbContext)
        {
            Console.WriteLine("Válasston lehetőséget: ");
            int input = ReusableMenu(["Meglévő tantárgy használata", "Új tantárgy hozzáadása"]);
            if (input == -1) { return 0; }
            uint selectedSubjectId = 0;
            switch (input)
            {
                case 0:
                    {
                        selectedSubjectId = await ConsoleFunctions.GetSubjectId(dbContext);
                        break;
                    }
                case 1:
                    {
                        await DataService.AddNewSubjectAsync(dbContext);
                        selectedSubjectId = dbContext.Subjects.OrderBy(x => x.Id).Last().Id;
                        break;
                    }
            }
            return selectedSubjectId;
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
