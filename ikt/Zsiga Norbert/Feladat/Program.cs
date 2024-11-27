using ChineseKreta.Database;
using Feladat;
using Microsoft.EntityFrameworkCore;

using var dbContext = new ApplicationDbContext();

await Menus.MainMenuAsync( dbContext);
