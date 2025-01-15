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

        if (run.Date.Value == null ||
            run.Distance.Value == null ||
            run.Weight.Value == null ||
            run.RunningTime.Value == null)
        {
            return Error.Conflict(description: $"All of the fields must be filled.");
        }

        var isRunExists = await dbContext.Runs.AnyAsync(x => x.Date == run.Date.Value &&
        x.Distance == run.Distance.Value &&
        x.Weight == run.Weight.Value &&
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
