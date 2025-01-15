using ErrorOr;
using Solution.Core.Models;

namespace Solution.Core.Interfaces;

public interface IRunService
{
    Task<ErrorOr<RunModel>> CreateAsync (RunModel run);
}
