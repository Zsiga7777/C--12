﻿using Solution.Database.Entities;

namespace Solution.Core.Models;

public class ManufacturerModel
{
    public uint Id { get; set; }

    public string Name { get; set; }

    public ManufacturerModel()
    {
    }

    public ManufacturerModel(uint id, string name)
    {
        Id = id;
        Name = name;
    }

    public ManufacturerModel(ManufacturerEntity entity)
    { 
        Id = entity.Id;
        Name = entity.Name;
    }
}
