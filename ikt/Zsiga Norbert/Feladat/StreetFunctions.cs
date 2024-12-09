namespace Feladat;

public static class StreetFunctions
{
    public static async Task<uint> SelectNewOrExistingStreetAsync(ApplicationDbContext dbContext, uint postalcode)
    {
        Console.WriteLine("Válasston lehetőséget: ");
        int input = Menus.ReusableMenu(["Meglévő utca használata", "Új utca hozzáadása"]);

        if (input == -1) { return 0; }

        uint selectedStreetId = 0;

        switch (input)
        {
            case 0:
                {
                    selectedStreetId = await GetStreetIdAsync(dbContext, postalcode);
                    break;
                }
            case 1:
                {
                    await AddStreetAsync(dbContext, postalcode);
                    selectedStreetId = dbContext.Streets.OrderBy(x => x.Id).Last().Id;
                    break;
                }
        }
        return selectedStreetId;
    }
    public static async Task<uint> GetStreetIdAsync(ApplicationDbContext dbContext, uint cityId)
    {
        List<StreetEntity> streets = await dbContext.Streets.Where(x => x.CityId == cityId).ToListAsync();
        List<string> streetNames = streets.Select(x => x.Name).ToList();
        int selectedStreet = Menus.ReusableMenu(streetNames);

        if (selectedStreet == -1) { return 0; }

        uint streetId = streets.First(x => x.Name == streetNames[selectedStreet]).Id;

        return streetId;
    }

    public static async Task AddStreetAsync(ApplicationDbContext dbContext, uint cityId)
    {
        Console.Clear();
        StreetEntity street = new StreetEntity() { Name = ExtendentConsole.ReadString("Kérem az új utca nevét: "), CityId = cityId };
        await dbContext.Streets.AddAsync(street);
        await dbContext.SaveChangesAsync();
    }
    public static async Task DeleteStreetAsync(ApplicationDbContext dbContext)
    {
        List<StreetEntity> streets = await dbContext.Streets.Include(a => a.City).ToListAsync();
        List<string> streetNames = new List<string>();

        string temp = "";
        foreach (StreetEntity street in streets)
        {
            temp = $"{street.City.Name} {street.Name}.";
            streetNames.Add(temp);
        }
        int selectedStreetNumber = Menus.ReusableMenu(streetNames);

        if (selectedStreetNumber == -1) { return; }

        dbContext.Remove(streets[selectedStreetNumber]);
        dbContext.SaveChanges();
    }
    public static async Task ModifyStreetAsync(ApplicationDbContext dbContext)
    {
        List<StreetEntity> streets = await dbContext.Streets.Include(a => a.City).ToListAsync();
        List<string> streetNames = new List<string>();

        string temp = "";
        foreach (StreetEntity street in streets)
        {
            temp = $"{street.City.Name} {street.Name}.";
            streetNames.Add(temp);
        }
        int selectedStreetNumber = Menus.ReusableMenu(streetNames);

        if (selectedStreetNumber == -1) { return; }

        streets[selectedStreetNumber].Name = ExtendentConsole.ReadString("Kérem a módosított utca nevet: ");
        dbContext.SaveChanges();
    }
}
