
using ChineseKreta.Database;
using ChineseKreta.Database.Entities;
using Custom.Library.ConsoleExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Feladat
{
    public class ConsoleFunctions
    {
        public static async Task WriteStudentData(ApplicationDbContext dbContext)
        {
            var studentsData = await dbContext.Students.Include(x => x.Address).ThenInclude(x => x.Street).ThenInclude(x => x.City).ToListAsync();
            uint studentIndex =await GetStudentIdAsync(dbContext);
            if (studentIndex == 0) return;
            StudentEntity student = studentsData.First(x => x.EducationalID == studentIndex);
            Console.WriteLine($"neve: {student.Name}\nanyja neve: {student.MothersName}\nszületési ideje: {student.BirthDay}\nlakhelye: {student.Address.Street.City.Name} {student.Address.Street.Name} {student.Address.Address} ");
            await Task.Delay(10000);
        }

        public static async Task<uint> GetStudentIdAsync(ApplicationDbContext dbContext)
        {
            Console.WriteLine("\nA tanulók nevei: ");
            int temp = Menus.ReusableMenu(await dbContext.Students.Select(x => x.Name).ToListAsync());
            if (temp == -1) return 0;
            return dbContext.Students.ElementAt(temp).EducationalID;
        }

        public static async Task<uint> GetSubjectId(ApplicationDbContext dbContext)
        {
            Console.WriteLine("\nA tantárgyak: ");
            List<SubjectEntity> subjects =await dbContext.Subjects.ToListAsync();
            List<string> subjectNames = await dbContext.Subjects.Select(x => x.Name).ToListAsync();
            int indexOfSubjectName = Menus.ReusableMenu(subjectNames);

            if (indexOfSubjectName == -1)
            {
                return 0;
            }
            
            return subjects.First(x => x.Name == subjectNames[indexOfSubjectName]).Id;
        }

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
        public static async Task AddCountryAsync(ApplicationDbContext dbContext)
        {
            string temp = "";
            List<CountryEntity> countries =await dbContext.Countries.ToListAsync();

            do
            {
                Console.Clear();
                temp = ExtendentConsole.ReadString("Kérem az új ország nevét: ");

            } while (countries.Any(x => x.Name.ToLower() == temp.ToLower()));

            CountryEntity country = new CountryEntity() {Name = temp };
            await dbContext.Countries.AddAsync(country);
            await dbContext.SaveChangesAsync();
        }
        public static async Task AddCityAsync(ApplicationDbContext dbContext, uint counryId)
        {
            int temp = 0;
            List<CityEntity> cities = await dbContext.Cities.ToListAsync();
           
            do
            {
                Console.Clear();
                temp = ExtendentConsole.ReadInteger(0,"Kérem az új város irányító számát: ");

            } while (cities.Any(x => x.PostalCode == temp));
            CityEntity city = new CityEntity() { Name =ExtendentConsole.ReadString("Kérem az új város nevét: "), CountryId = counryId, PostalCode = (uint)temp};
            await dbContext.Cities.AddAsync(city);
            await dbContext.SaveChangesAsync();
        }

        public static async Task AddStreetAsync(ApplicationDbContext dbContext, uint postalCode)
        {
            Console.Clear();
            StreetEntity street = new StreetEntity() { Name = ExtendentConsole.ReadString("Kérem az új utca nevét: "), PostalCode = postalCode };
            await dbContext.Streets.AddAsync(street);
            await dbContext.SaveChangesAsync();
        }

        public static async Task AddAddressAsync(ApplicationDbContext dbContext, uint streetId)
        {
            Console.Clear();
            AddressEntity address = new AddressEntity() { Address = ExtendentConsole.ReadString("Kérem az új házszám nevét: "), StreetId = streetId };
            await dbContext.Addresses.AddAsync(address);
            await dbContext.SaveChangesAsync();
        }

        public static async Task<string> ReadStudentNameAsync(ApplicationDbContext dbContext)
        {
            string input = ExtendentConsole.ReadString("Kérem a tanuló nevét vagy a feladat végeztével a 'e' gomb lenyomását: ");
            if (input.ToLower() == "e") { return input; }

            int counter = 0;
            while (dbContext.Students.Any(x => x.Name == input))
            {
                if (counter == 0)
                {
                    counter++;
                    input = input + $"{counter}";
                }
                else
                {
                    input = input.Remove(counter);
                    counter++;
                    input = input + $"{counter}";
                }
            }
            return input;
        }

        public static async Task<uint> GetCountryIdAsync(ApplicationDbContext dbContext)
        {
            List<CountryEntity> countries = await dbContext.Countries.ToListAsync();
            List<string> countryNames = countries.Select(x => x.Name).ToList();
            int selectedCountry = Menus.ReusableMenu(countryNames);
            uint countryId = countries.First(x => x.Name == countryNames[selectedCountry]).Id; ;
          
            return countryId;
        }

        public static async Task<uint> GetCityIdAsync(ApplicationDbContext dbContext, uint countryId)
        {
            List<CityEntity> cities = await dbContext.Cities.Where(x => x.CountryId == countryId).ToListAsync();
            List<string> cityNames = cities.Select(x => x.Name).ToList();
            int selectedCity = Menus.ReusableMenu(cityNames);
            uint postalCode = cities.First(x => x.Name == cityNames[selectedCity]).PostalCode;
         
            return postalCode;
        }

        public static async Task<uint> GetStreetIdAsync(ApplicationDbContext dbContext, uint postalCode)
        {
            List<StreetEntity> streets = await dbContext.Streets.Where(x => x.PostalCode == postalCode).ToListAsync();
            List<string> streetNames = streets.Select(x => x.Name).ToList();
            int selectedStreet = Menus.ReusableMenu(streetNames);
            uint streetId = streets.First(x => x.Name == streetNames[selectedStreet]).Id;
           
            return streetId;
        }

        public static async Task<uint> GetAddressIdAsync(ApplicationDbContext dbContext, uint streetId)
        {
            List<AddressEntity> addresses = await dbContext.Addresses.Where(x => x.StreetId == streetId).ToListAsync();
            List<string> addressNames = addresses.Select(x => x.Address).ToList();
            int selectedAddress = Menus.ReusableMenu(addressNames);
            uint AddressId = addresses.First(x => x.Address == addressNames[selectedAddress]).Id;
           
            return AddressId;
        }

        public static async Task<uint> GetAddressIdCompleteAsync(ApplicationDbContext dbContext)
        {
            uint countryId =await GetCountryIdAsync(dbContext);
            uint postalCode = await GetCityIdAsync(dbContext, countryId);
            uint streetId = await GetStreetIdAsync(dbContext, postalCode);
            uint addressId = await GetAddressIdAsync(dbContext,streetId);
            return addressId;
        }
        public static async Task AddAddressCompleteAsync(ApplicationDbContext dbContext)
        {
            uint countryId = await Menus.SelectNewOrExistingCountryAsync(dbContext);

            uint postalCode = await Menus.SelectNewOrExistingCityAsync(dbContext, countryId);

            uint streetId = await Menus.SelectNewOrExistingStreetAsync(dbContext, postalCode);

            await AddAddressAsync(dbContext, streetId);
            
        }

        public static async Task<uint> GetMarkIdAsync(ApplicationDbContext dbContext)
        { 
            List<MarkEntity> marks =await dbContext.Marks.Include(x => x.Student).Include(x => x.Subject).ToListAsync();
            List<string> fullmarks = new List<string>();
            string temp = "";
            foreach (MarkEntity mark in marks)
            {
                temp = $"{mark.Mark}, {mark.Date}, {mark.Subject.Name}, {mark.Student.Name}";
                fullmarks.Add(temp);
            }
            int selectedIndex = Menus.ReusableMenu(fullmarks);
            if (selectedIndex == -1) return 0;
            return marks[selectedIndex].MarkId;
        }
    }
}
