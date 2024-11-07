using ChineseKreta.Database;
using Feladat;
using Microsoft.EntityFrameworkCore;

using var dbContext = new ApplicationDbContext();
var studentsData = await dbContext.Students.Include(x => x.Class)
                                            .Include(x => x.Address)
                                            .ThenInclude(x => x.City)
                                            .Include(x => x.Marks)
                                            .ThenInclude(x => x.Subject)
                                            .ToListAsync();

await Menus.MainMenu(studentsData);

await dbContext.SaveChangesAsync(); 