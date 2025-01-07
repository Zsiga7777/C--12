namespace Kreta.ConsoleApp;

public static class CountryFunctions
{
    public static async Task AddCountryAsync(ApplicationDbContext dbContext)
    {
        
        List<CountryEntity> countries = await dbContext.Countries.ToListAsync();

        string countryName =await ReadCountryNameAsync(countries);

        CountryEntity country = new CountryEntity() { Name = countryName };
        await dbContext.Countries.AddAsync(country);
        await dbContext.SaveChangesAsync();
    }

    public static async Task<string> ReadCountryNameAsync(List<CountryEntity> countries)
    {
        string countryName = "";
        do
        {
            Console.Clear();
            countryName = ExtendentConsole.ReadString("Kérem az új ország nevét: ");
            if (countries.Any(x => x.Name.ToLower() == countryName.ToLower()))
            {
                Console.WriteLine("Ilyen ország már létezik.");
                await Task.Delay(2000);
            }

        } while (countries.Any(x => x.Name.ToLower() == countryName.ToLower()));

        return countryName;
    }
    public static async Task<uint> GetCountryIdAsync(ApplicationDbContext dbContext)
    {
        List<CountryEntity> countries = await dbContext.Countries.ToListAsync();
        List<string> countryNames = countries.Select(x => x.Name).ToList();

        Console.Clear();

        int selectedCountry = Menus.ReusableMenu(countryNames);

        if (selectedCountry == -1)
        {
            return 0;
        }
        uint countryId = countries[selectedCountry].Id; ;

        return countryId;
    }
    public static async Task ModifyCountryAsync(ApplicationDbContext dbContext)
    {
        List<CountryEntity> countries = await dbContext.Countries.ToListAsync();

        Console.Clear();

        uint selectedCountryId =await GetCountryIdAsync(dbContext);

        if (selectedCountryId == 0) 
        { 
            return; 
        }

        countries.First(x => x.Id == selectedCountryId).Name = ExtendentConsole.ReadString("Kérem a módosított ország nevet: ");
        await dbContext.SaveChangesAsync();
    }

    public static async Task<uint> UseNewOrExistingCountryAsync(ApplicationDbContext dbContext)
    {
        int countryOption = Menus.ReusableMenu(["Meglévő ország használata", "Új ország hozzáadása"]);
        uint countryId;
        uint cityId;
        uint streetId;

        if (countryOption == -1)
        {
            return 0;
        }
        else if (countryOption == 0)
        {
            countryId = await GetCountryIdAsync(dbContext);

            if (countryId == 0) 
            {
                return 0; 
            }

            streetId = await CityFunctions.UseNewOrExistingCityAsync(dbContext, countryId);
        }
        else
        {
            await AddCountryAsync(dbContext);
            countryId = dbContext.Countries.OrderBy(x => x.Id).Last().Id;

            await CityFunctions.AddCityAsync(dbContext, countryId);
            cityId = dbContext.Cities.OrderBy(x => x.Id).Last().Id;

            await StreetFunctions.AddStreetAsync(dbContext, cityId);
            streetId = dbContext.Streets.OrderBy(x => x.Id).Last().Id;
        }

        return streetId;
    }
}
