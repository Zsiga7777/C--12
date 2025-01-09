using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Solution.Core.Interfaces;
using Solution.Core.Models;
using Solution.Database.Entities;
using Solution.DataBase;

namespace Solution.Services;

public class RunService(AppDbContext dbContext) : IRunService
{
    public async Task<ErrorOr<RunModel>> CreateAsync(RunModel run)
    {
        var isRunExists = await dbContext.Runs.AnyAsync(x => x.Date == run.Date.Value &&
        x.Distance == run.Distance.Value &&
        x.BurntCalories == run.BurntCalories.Value &&
        x.AverageSpeed == run.AverageSpeed.Value &&
        x.RunningTime == run.RunningTime.Value);

        if (isRunExists)
        {
            return Error.Conflict(description: $"Running record with the same data already exists.");
        }

        run.Id = Guid.NewGuid().ToString();
        RunEntity entity = run.ToEntity();

        await dbContext.Runs.AddAsync(entity);
        await dbContext.SaveChangesAsync();

        return new RunModel(entity);
    }
}
