using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Solution.Core.Interfaces;
using Solution.Core.Models;
using Solution.Database.Entities;
using Solution.DataBase;

namespace Solution.Services;

public class MotorcycleService(AppDbContext dbContext) : IMotorcycleService
{
    private const int ROW_COUNT = 25;
    public async Task<ErrorOr<MotorcycleModel>> CreateAsync(MotorcycleModel motorcycle)
    {

        bool isMotorcycleExists = await dbContext.Motorcycles.AnyAsync(x => x.ManufacturerId == motorcycle.Manufacturer.Value.Id &&
        x.Model.ToLower() == motorcycle.Model.Value.ToLower().Trim() &&
        x.ReleaseYear == motorcycle.ReleaseYear.Value);

        if (isMotorcycleExists)
        {
            return Error.Conflict(description: $"Motorcycle with the same data already exists.");
        }

        MotorcycleEntity entity = motorcycle.ToEntity();
        entity.PublicId = Guid.NewGuid().ToString();

        await dbContext.Motorcycles.AddAsync(entity);
        await dbContext.SaveChangesAsync();

        return new MotorcycleModel(entity)
        {
            Manufacturer = motorcycle.Manufacturer
        };
    }

    public async Task<ErrorOr<Success>> UpdateAsync(MotorcycleModel model)
    {
        var result = await dbContext.Motorcycles.Include(x => x.Manufacturer).Where(x => x.PublicId == model.Id)
            .ExecuteUpdateAsync(x => x.SetProperty(p =>p.PublicId, model.Id)
            .SetProperty(p => p.ManufacturerId, model.Manufacturer.Value.Id)
            .SetProperty(p => p.Model, model.Model.Value)
            .SetProperty(p => p.Cubic, model.Cubic.Value)
            .SetProperty(p => p.ReleaseYear, model.ReleaseYear.Value)
            .SetProperty(p => p.Cylinders, model.NumberOfCylinders.Value));


        return result > 0 ? Result.Success : Error.NotFound();
    }
    public async Task<ErrorOr<Success>> DeleteAsync(string motorcycleId)
    {

        var result = await dbContext.Motorcycles.AsNoTracking()
            .Include(x => x.Manufacturer)
            .Where(x => x.PublicId == motorcycleId)
            .ExecuteDeleteAsync();

        return result > 0 ? Result.Success : Error.NotFound();
    }

    public async Task<ErrorOr<MotorcycleModel>> GetByIdAsync(string motorcycleId)
    {

        var motorcycle = await dbContext.Motorcycles.Include(x => x.Manufacturer).FirstOrDefaultAsync(x => x.PublicId == motorcycleId);

        if (motorcycle is null)
        {
            return Error.NotFound(description: $"Motorcycle not found");
        }

        return new MotorcycleModel(motorcycle);
    }

    public async Task<ErrorOr<List<MotorcycleModel>>> GetAllAsync() =>await dbContext.Motorcycles.AsNoTracking()
        .Include(x => x.Manufacturer)
        .Select(x => new MotorcycleModel(x))
        .ToListAsync();

    public async Task<ErrorOr<List<MotorcycleModel>>> GetPageAsync(int page = 0)
    {
        page = page < 0? 0 : page -1;

       return await dbContext.Motorcycles.AsNoTracking()
                                        .Include(x => x.Manufacturer)
                                        .Skip(page*ROW_COUNT)
                                        .Take(ROW_COUNT)
                                        .Select(x => new MotorcycleModel(x))
                                        .ToListAsync();
    }
}
