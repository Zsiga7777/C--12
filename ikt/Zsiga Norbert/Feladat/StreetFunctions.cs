namespace Feladat;

public static class StreetFunctions
{
    public static async Task<uint> GetStreetIdAsync(ApplicationDbContext dbContext, uint cityId)
    {
        List<StreetEntity> streets = await dbContext.Streets.Where(x => x.CityId == cityId).ToListAsync();
        List<string> streetNames = streets.Select(x => x.Name).ToList();

        Console.Clear();

        int selectedStreet = Menus.ReusableMenu(streetNames);

        if (selectedStreet == -1) 
        { 
            return 0; 
        }

        uint streetId = streets[selectedStreet].Id;

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
        List<string> streetNames = GetStreetsWithCityNames(streets);

        Console.Clear();

        int selectedStreetNumber = Menus.ReusableMenu(streetNames);

        if (selectedStreetNumber == -1) 
        { 
            return; 
        }

        dbContext.Remove(streets[selectedStreetNumber]);
        await dbContext.SaveChangesAsync();
    }
    public static async Task ModifyStreetAsync(ApplicationDbContext dbContext)
    {
        List<StreetEntity> streets = await dbContext.Streets.Include(a => a.City).ToListAsync();
        List<string> streetNames = GetStreetsWithCityNames(streets);

        Console.Clear();

        int selectedStreetNumber = Menus.ReusableMenu(streetNames);

        if (selectedStreetNumber == -1) 
        { 
            return; 
        }

        streets[selectedStreetNumber].Name = ExtendentConsole.ReadString("Kérem a módosított utca nevet: ");
        await dbContext.SaveChangesAsync();
    }

    public static async Task<uint> UseNewOrExistingStreetAsync(ApplicationDbContext dbContext, uint cityId)
    {
        int streetOption;
        uint streetId;

                streetOption = Menus.ReusableMenu(["Meglévő utca használata", "Új utca hozzáadása"]);

                if (streetOption == -1)
                {
                    return 0;
                }

                else if (streetOption == 0)
                {
                    streetId = await GetStreetIdAsync(dbContext, cityId);
                }

                else
                {
                    await AddStreetAsync(dbContext, cityId);
                    streetId = dbContext.Streets.OrderBy(x => x.Id).Last().Id;
                }
            
        return streetId;
    }

    public static List<string> GetStreetsWithCityNames(List<StreetEntity> streets)
    {
        string temp = "";
        List<string> result = new List<string>();

        foreach (StreetEntity street in streets)
        {
            temp = $"{street.City.Name} {street.Name}.";
            result.Add(temp);
        }

        return result;
    }
}
