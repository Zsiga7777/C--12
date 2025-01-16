using MauiValidationLibrary.ValidationRules;
using Solution.Database.Entities;
using Solution.ValidationLibrary;
using System.ComponentModel.DataAnnotations;

namespace Solution.Core.Models;

public partial class MotorcycleModel
{
    public string Id { get; set; }

    public ValidatableObject<uint> ManufacturerId { get; protected set; }

    public ValidatableObject<string> Model { get; protected set; }
    public ValidatableObject<uint?> Cubic { get; protected set; }
    public ValidatableObject<uint?> ReleaseYear { get; protected set; }
    public ValidatableObject<uint?> Cylinders { get; protected set; }

    public MotorcycleModel() 
    {
        this.ManufacturerId = new ValidatableObject<uint>();
        this.Model = new ValidatableObject<string>();
        this.Cubic = new ValidatableObject<uint?>();
        this.ReleaseYear = new ValidatableObject<uint?>();
        this.Cylinders = new ValidatableObject<uint?>();

        AddValidators();
    }

    public MotorcycleModel(MotorcycleEntity entity) : this()
    {
        this.Id = entity.PublicId;
        this.ManufacturerId.Value = entity.ManufacturerId;
        this.Model.Value = entity.Model;
        this.Cubic.Value = entity.Cubic;
        this.ReleaseYear.Value = entity.ReleaseYear;
        this.Cylinders.Value = entity.Cylinders;
    }

    public MotorcycleEntity ToEntity()
    {
        return new MotorcycleEntity
        {
            PublicId = Id,
            ManufacturerId = ManufacturerId.Value,
            Model = Model.Value,
            Cubic = Cubic.Value ?? 0,
            ReleaseYear = ReleaseYear.Value ?? 0,
            Cylinders = Cylinders.Value ?? 0,
        };
    }

    public void ToEntity(MotorcycleEntity entity)
    {

        entity.PublicId = Id;
        entity.ManufacturerId = ManufacturerId.Value;
        entity.Model = Model.Value;
        entity.Cubic = Cubic.Value ?? 0;
        entity.ReleaseYear = ReleaseYear.Value ?? 0;
        entity.Cylinders = Cylinders.Value ?? 0;
      
    }



    private void AddValidators()
    {
        this.ManufacturerId.Validations.Add(new MinValueRule<uint>(1) { ValidationMessage = "Manufacturer id must be greater than 0" });

        this.Model.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Model field is required" });

        this.Cubic.Validations.Add(new IsNotNullOrEmptyRule<uint?> { ValidationMessage = "Cubic field is required" });
        this.Cubic.Validations.Add(new MinValueRule<uint?>(1) { ValidationMessage = "Cubic must be greater than 0" });

        this.ReleaseYear.Validations.Add(new IsNotNullOrEmptyRule<uint?> { ValidationMessage = "Release year field is required" });
        this.ReleaseYear.Validations.Add(new MinValueRule<uint?>(1) { ValidationMessage = "Release year must be greater than 0" });
        this.ReleaseYear.Validations.Add(new MaxValueRule<uint?>(DateTime.Now.Year) { ValidationMessage = $"Release year must be less than {DateTime.Now.Year}" });

        this.Cylinders.Validations.Add(new IsNotNullOrEmptyRule<uint?> { ValidationMessage = "Cylinder field is required" });

    }
}
