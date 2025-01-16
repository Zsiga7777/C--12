using ErrorOr;
using Solution.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Core.Interfaces;

public interface IMotorcycleService
{
    Task<ErrorOr<MotorcycleModel>> CreateAsync(MotorcycleModel motorcycle);
}
