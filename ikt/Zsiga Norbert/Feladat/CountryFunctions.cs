namespace Feladat;

public static class CountryFunctions
{
    public static async Task<uint> SelectNewOrExistingCountryAsync(ApplicationDbContext dbContext)
    {
        Console.WriteLine("Válasston lehetőséget: ");
        int input = Menus.ReusableMenu(["Meglévő ország használata", "Új ország hozzáadása"]);

        if (input == -1) { return 0; }

        uint selectedCountryId = 0;

        switch (input)
        {
            case 0:
                {
                    selectedCountryId = await GetCountryIdAsync(dbContext);
                    break;
                }
            case 1:
                {
                    await AddCountryAsync(dbContext);
                    selectedCountryId = dbContext.Countries.OrderBy(x => x.Id).Last().Id;
                    break;
                }
        }
        return selectedCountryId;
    }

    public static async Task AddCountryAsync(ApplicationDbContext dbContext)
    {
        string temp = "";
        List<CountryEntity> countries = await dbContext.Countries.ToListAsync();

        do
        {
            Console.Clear();
            temp = ExtendentConsole.ReadString("Kérem az új ország nevét: ");

        } while (countries.Any(x => x.Name.ToLower() == temp.ToLower()));

        CountryEntity country = new CountryEntity() { Name = temp };
        await dbContext.Countries.AddAsync(country);
        await dbContext.SaveChangesAsync();
    }

    public static async Task<uint> GetCountryIdAsync(ApplicationDbContext dbContext)
    {
        List<CountryEntity> countries = await dbContext.Countries.ToListAsync();
        List<string> countryNames = countries.Select(x => x.Name).ToList();
        int selectedCountry = Menus.ReusableMenu(countryNames);

        if (selectedCountry == -1)
        {
            return 0;
        }
        uint countryId = countries.First(x => x.Name == countryNames[selectedCountry]).Id; ;

        return countryId;
    }
    public static async Task ModifyCountryAsync(ApplicationDbContext dbContext)
    {
        List<CountryEntity> countries = await dbContext.Countries.ToListAsync();
        List<string> countryNames = countries.Select(x => x.Name).ToList();

        int selectedCountryNumber = Menus.ReusableMenu(countryNames);

        if (selectedCountryNumber == -1) { return; }

        countries[selectedCountryNumber].Name = ExtendentConsole.ReadString("Kérem a módosított ország nevet: ");
        dbContext.SaveChanges();
    }
}
