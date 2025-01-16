using ErrorOr;
using Solution.Core.Models;

namespace Solution.Core.Interfaces;

public interface IManufacturerService
{
    Task<ErrorOr<ManufacturerModel>> CreateAsync(ManufacturerModel manufacturer);
}
