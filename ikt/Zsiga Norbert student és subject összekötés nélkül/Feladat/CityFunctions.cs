namespace Feladat;

public static class CityFunctions
{
    public static async Task AddCityAsync(ApplicationDbContext dbContext, uint counryId)
    {
        List<CityEntity> cities = await dbContext.Cities.ToListAsync();

        uint postalCode = await ReadPostalCodeAsync(cities);

        CityEntity city = new CityEntity() 
        { 
            Name = ExtendentConsole.ReadString("Kérem az új város nevét: "), 
            CountryId = counryId, 
            Id = postalCode
        };

        await dbContext.Cities.AddAsync(city);
        await dbContext.SaveChangesAsync();
    }

    public static async Task<uint> ReadPostalCodeAsync(List<CityEntity> cities)
    {
        int postalCode = 0;
        do
        {
            Console.Clear();
            postalCode = ExtendentConsole.ReadInteger(0, "Kérem az új város irányító számát: ");
            if (cities.Any(x => x.Id == postalCode))
            {
                Console.WriteLine("Ilyen postakód már létezik.");
                await Task.Delay(2000);
            }

        } while (cities.Any(x => x.Id == postalCode));

        return (uint)postalCode;
    }

    public static async Task<uint> GetCityIdAsync(ApplicationDbContext dbContext, uint countryId)
    {
        List<CityEntity> cities = await dbContext.Cities.Where(x => x.CountryId == countryId).ToListAsync();
        List<string> cityNames = cities.Select(x => x.Name).ToList();

        Console.Clear(); 

        int selectedCity = Menus.ReusableMenu(cityNames);

        if (selectedCity == -1) 
        { 
            return 0; 
        }

        uint cityId = cities[selectedCity].Id;

        return cityId;
    }
    public static async Task ModifyCityAsync(ApplicationDbContext dbContext)
    {
        List<CityEntity> cities = await dbContext.Cities.ToListAsync();
        List<string> cityNames = cities.Select(x => x.Name).ToList();

        Console.Clear();

        int selectedCityNumber = Menus.ReusableMenu(cityNames);

        if (selectedCityNumber == -1) 
        {
            return; 
        }

        cities[selectedCityNumber].Name = ExtendentConsole.ReadString("Kérem a módosított város nevet: ");
        await dbContext.SaveChangesAsync();
    }

    public static async Task<uint> UseNewOrExistingCityAsync(ApplicationDbContext dbContext, uint countryId)
    {
        int cityOption;
        uint cityId;
        uint streetId; 

            cityOption = Menus.ReusableMenu(["Meglévő város használata", "Új város hozzáadása"]);

            if (cityOption == -1)
            {
                return 0;
            }

            else if (cityOption == 0)
            {

                cityId = await GetCityIdAsync(dbContext, countryId);

                if (cityId == 0) 
                { 
                    return 0; 
                }

                streetId = await StreetFunctions.UseNewOrExistingStreetAsync(dbContext, cityId);

            }

            else
            {
                await AddCityAsync(dbContext, countryId);
                cityId = dbContext.Cities.OrderBy(x => x.Id).Last().Id;

                await StreetFunctions.AddStreetAsync(dbContext, cityId);
                streetId = dbContext.Streets.OrderBy(x => x.Id).Last().Id;
            }
            return streetId;
        }
       
    }
