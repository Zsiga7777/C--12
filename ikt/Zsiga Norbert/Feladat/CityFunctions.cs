namespace Feladat;

public static class CityFunctions
{
    public static async Task AddCityAsync(ApplicationDbContext dbContext, uint counryId)
    {
        int temp = 0;
        List<CityEntity> cities = await dbContext.Cities.ToListAsync();

        do
        {
            Console.Clear();
            temp = ExtendentConsole.ReadInteger(0, "Kérem az új város irányító számát: ");

        } while (cities.Any(x => x.Id == temp));

        CityEntity city = new CityEntity() { Name = ExtendentConsole.ReadString("Kérem az új város nevét: "), CountryId = counryId, Id = (uint)temp };

        await dbContext.Cities.AddAsync(city);
        await dbContext.SaveChangesAsync();
    }

    public static async Task<uint> GetCityIdAsync(ApplicationDbContext dbContext, uint countryId)
    {
        List<CityEntity> cities = await dbContext.Cities.Where(x => x.CountryId == countryId).ToListAsync();
        List<string> cityNames = cities.Select(x => x.Name).ToList();

        Console.Clear(); 

        int selectedCity = Menus.ReusableMenu(cityNames);

        if (selectedCity == -1) { return 0; }

        uint cityId = cities[selectedCity].Id;

        return cityId;
    }
    public static async Task ModifyCityAsync(ApplicationDbContext dbContext)
    {
        List<CityEntity> cities = await dbContext.Cities.ToListAsync();
        List<string> cityNames = cities.Select(x => x.Name).ToList();

        Console.Clear();

        int selectedCityNumber = Menus.ReusableMenu(cityNames);

        if (selectedCityNumber == -1) { return; }

        cities[selectedCityNumber].Name = ExtendentConsole.ReadString("Kérem a módosított város nevet: ");
        await dbContext.SaveChangesAsync();
    }
}
