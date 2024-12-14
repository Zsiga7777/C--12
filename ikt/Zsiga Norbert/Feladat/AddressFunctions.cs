namespace Feladat;

public static class AddressFunctions
{
    public static async Task<uint> SelectNewOrExistingAddressCompleteAsync(ApplicationDbContext dbContext)
    {
        Console.Clear();
        Console.WriteLine("Válasston lehetőséget: ");
        int input = Menus.ReusableMenu(["Meglévő cím használata", "Új cím hozzáadása"]);

        if (input == -1) { return 0; }

        uint selectedAddressId = 0;

        switch (input)
        {
            case 0:
                {
                    selectedAddressId = await GetAddressIdCompleteAsync(dbContext);
                    break;
                }
            case 1:
                {
                    if (await AddAddressCompleteAsync(dbContext))
                    {
                        selectedAddressId = dbContext.Addresses.OrderBy(x => x.Id).Last().Id;
                    }
                    else
                    {
                        selectedAddressId = 0;
                    }
                    break;
                }
        }
        return selectedAddressId;
    }
    public static async Task<uint> GetAddressIdAsync(ApplicationDbContext dbContext, uint streetId)
    {
        List<AddressEntity> addresses = await dbContext.Addresses.Where(x => x.StreetId == streetId).ToListAsync();
        List<string> addressNames = addresses.Select(x => x.Address).ToList();

        Console.Clear();
        int selectedAddress = Menus.ReusableMenu(addressNames);

        if (selectedAddress == -1) { return 0; }

        uint AddressId = addresses[selectedAddress].Id;

        return AddressId;
    }

    public static async Task<uint> GetAddressIdCompleteAsync(ApplicationDbContext dbContext)
    {
        uint countryId = await CountryFunctions.GetCountryIdAsync(dbContext);
        if (countryId == 0) return 0;

        uint cityId = await CityFunctions.GetCityIdAsync(dbContext, countryId);
        if (cityId == 0) return 0;

        uint streetId = await StreetFunctions.GetStreetIdAsync(dbContext, cityId);
        if (streetId == 0) return 0;

        uint addressId = await GetAddressIdAsync(dbContext, streetId);
        return addressId;
    }
    public static async Task<bool> AddAddressCompleteAsync(ApplicationDbContext dbContext)
    {
        int countryOption = Menus.ReusableMenu(["Meglévő ország használata", "Új ország hozzáadása"]);
        int cityOption;
        int streetOption;
        uint countryId;
        uint cityId;
        uint streetId;

        if (countryOption == -1)
        {
            return false;
        }
        else if (countryOption == 0)
        { 
            countryId = await CountryFunctions.GetCountryIdAsync(dbContext);

            if(countryId == 0) { return false; }

            cityOption = Menus.ReusableMenu(["Meglévő város használata", "Új város hozzáadása"]);

            if (cityOption == -1)
            {
                return false;
            }

            else if (cityOption == 0)
            {
                cityId = await CityFunctions.GetCityIdAsync(dbContext, countryId);

                if (cityId == 0) { return false; }

                streetOption = Menus.ReusableMenu(["Meglévő utca használata", "Új utca hozzáadása"]);

                if (streetOption == -1)
                {
                    return false;
                }

                else if (streetOption == 0)
                {
                    streetId = await StreetFunctions.GetStreetIdAsync(dbContext, cityId);
                    if (streetId == 0) { return false; }
                }

                else
                {
                    await StreetFunctions.AddStreetAsync(dbContext, cityId);
                    streetId = dbContext.Streets.OrderBy(x => x.Id).Last().Id;
                }
            }

            else
            {
                await CityFunctions.AddCityAsync(dbContext, countryId);
                cityId = dbContext.Cities.OrderBy(x => x.Id).Last().Id;

                await StreetFunctions.AddStreetAsync(dbContext, cityId);
                streetId = dbContext.Streets.OrderBy(x => x.Id).Last().Id;
            }

        }
        else
        {
            await CountryFunctions.AddCountryAsync(dbContext);
            countryId = dbContext.Countries.OrderBy(x => x.Id).Last().Id;

            await CityFunctions.AddCityAsync(dbContext, countryId);
            cityId = dbContext.Cities.OrderBy(x => x.Id).Last().Id;

            await StreetFunctions.AddStreetAsync(dbContext, cityId);
            streetId = dbContext.Streets.OrderBy(x => x.Id).Last().Id;
        }

        await AddAddressAsync(dbContext, streetId);
        return true;
    }
    public static async Task AddAddressAsync(ApplicationDbContext dbContext, uint streetId)
    {
        Console.Clear();
        AddressEntity address = new AddressEntity() { Address = ExtendentConsole.ReadString("Kérem az új házszám nevét: "), StreetId = streetId };

        await dbContext.Addresses.AddAsync(address);
        await dbContext.SaveChangesAsync();
    }
    public static async Task DeleteAddressAsync(ApplicationDbContext dbContext)
    {
        Console.Clear();
        uint selectedAddressId =await GetAddressIdCompleteAsync(dbContext);

        if (selectedAddressId == 0) { return; }

        AddressEntity address = await dbContext.Addresses.FirstAsync(x => x.Id == selectedAddressId);

        dbContext.Addresses.Remove(address);
        await dbContext.SaveChangesAsync();
    }
    public static async Task ModifyAddressAsync(ApplicationDbContext dbContext)
    {
        List<AddressEntity> addresses = await dbContext.Addresses.ToListAsync();
       
        Console.Clear();
        uint selectedAddressId =await GetAddressIdCompleteAsync(dbContext);

        if (selectedAddressId == 0) { return; }

        addresses.First(x => x.Id == selectedAddressId).Address = ExtendentConsole.ReadString("Kérem az módosított címet: ");
        await dbContext.SaveChangesAsync();
    }
}
