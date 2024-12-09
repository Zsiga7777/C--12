namespace Feladat;

public static class AddressFunctions
{
    public static async Task<uint> SelectNewOrExistingAddressCompleteAsync(ApplicationDbContext dbContext)
    {
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
        int selectedAddress = Menus.ReusableMenu(addressNames);

        if (selectedAddress == -1) { return 0; }

        uint AddressId = addresses.First(x => x.Address == addressNames[selectedAddress]).Id;

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
        uint countryId = await CountryFunctions.SelectNewOrExistingCountryAsync(dbContext);
        if (countryId == 0) { return false; }

        uint postalCode = await CityFunctions.SelectNewOrExistingCityAsync(dbContext, countryId);
        if (postalCode == 0) { return false; }

        uint streetId = await StreetFunctions.SelectNewOrExistingStreetAsync(dbContext, postalCode);
        if (streetId == 0) { return false; }

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
        List<AddressEntity> addresses = await dbContext.Addresses.Include(a => a.Street).ThenInclude(a => a.City).ToListAsync();
        List<string> addressNames = new List<string>();

        string temp = "";
        foreach (AddressEntity address in addresses)
        {
            temp = $"{address.Street.City.Name} {address.Street.Name} {address.Address}.";
            addressNames.Add(temp);
        }
        int selectedAddressNumber = Menus.ReusableMenu(addressNames);

        if (selectedAddressNumber == -1) { return; }

        dbContext.Remove(addresses[selectedAddressNumber]);
        dbContext.SaveChanges();
    }
    public static async Task ModifyAddressAsync(ApplicationDbContext dbContext)
    {
        List<AddressEntity> addresses = await dbContext.Addresses.Include(a => a.Street).ThenInclude(a => a.City).ToListAsync();
        List<string> addressNames = new List<string>();

        string temp = "";
        foreach (AddressEntity address in addresses)
        {
            temp = $"{address.Street.City.Name} {address.Street.Name} {address.Address}.";
            addressNames.Add(temp);
        }

        int selectedAddressNumber = Menus.ReusableMenu(addressNames);

        if (selectedAddressNumber == -1) { return; }

        addresses[selectedAddressNumber].Address = ExtendentConsole.ReadString("Kérem az módosított címet: ");
        dbContext.SaveChanges();
    }
}
