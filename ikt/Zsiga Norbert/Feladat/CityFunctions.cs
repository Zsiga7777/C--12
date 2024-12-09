namespace Feladat;

public static class CityFunctions
{
    public static async Task<uint> SelectNewOrExistingCityAsync(ApplicationDbContext dbContext, uint countryId)
    {
        Console.WriteLine("Válasston lehetőséget: ");
        int input = Menus.ReusableMenu(["Meglévő város használata", "Új város hozzáadása"]);

        if (input == -1) { return 0; }

        uint selectedCityId = 0;

        switch (input)
        {
            case 0:
                {
                    selectedCityId = await GetCityIdAsync(dbContext, countryId);
                    break;
                }
            case 1:
                {
                    await AddCityAsync(dbContext, countryId);
                    selectedCityId = dbContext.Cities.OrderBy(x => x.Id).Last().Id;
                    break;
                }
        }
        return selectedCityId;
    }
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
        int selectedCity = Menus.ReusableMenu(cityNames);

        if (selectedCity == -1) { return 0; }

        uint cityId = cities.First(x => x.Name == cityNames[selectedCity]).Id;

        return cityId;
    }
    public static async Task ModifyCityAsync(ApplicationDbContext dbContext)
    {
        List<CityEntity> cities = await dbContext.Cities.ToListAsync();
        List<string> cityNames = cities.Select(x => x.Name).ToList();

        int selectedCityNumber = Menus.ReusableMenu(cityNames);

        if (selectedCityNumber == -1) { return; }

        cities[selectedCityNumber].Name = ExtendentConsole.ReadString("Kérem a módosított város nevet: ");
        dbContext.SaveChanges();
    }
}
