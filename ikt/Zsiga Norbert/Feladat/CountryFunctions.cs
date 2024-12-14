namespace Feladat;

public static class CountryFunctions
{
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

        if (selectedCountryId == 0) { return; }

        countries.First(x => x.Id == selectedCountryId).Name = ExtendentConsole.ReadString("Kérem a módosított ország nevet: ");
        await dbContext.SaveChangesAsync();
    }
}
