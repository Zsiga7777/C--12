using var dbContext = new ApplicationDbContext();

await Menus.MainMenuAsync( dbContext);
