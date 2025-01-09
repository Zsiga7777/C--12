
using MauiValidationLibrary;
using MauiValidationLibrary.ValidationRules;
using Microsoft.IdentityModel.Tokens;
using Solution.Database.Entities;

namespace Solution.Core.Models;

public partial class RunModel
{
    public string Id { get; set; }
    public ValidatableObject<DateTime> Date { get; set; }
    public ValidatableObject<double?> Distance { get; protected set; }
    public ValidatableObject<double?> AverageSpeed { get; protected set; }
    public ValidatableObject<double?> BurntCalories { get; protected set; }
    public ValidatableObject<uint?> RunningTime { get;  protected set; }

    public RunModel() 
    {
        this.Date = new ValidatableObject<DateTime>();
        this.Distance = new ValidatableObject<double?>();
        this.AverageSpeed = new ValidatableObject<double?>();
        this.BurntCalories = new ValidatableObject<double?>();
        this.RunningTime = new ValidatableObject<uint?> { };

        AddValidators();
    }

    public RunModel(RunEntity entity) : this()
    {
        Id = entity.PublicId;
        Date.Value = entity.Date;
        Distance.Value = entity.Distance;
        AverageSpeed.Value = entity.AverageSpeed;
        BurntCalories.Value = entity.BurntCalories;
        RunningTime.Value = entity.RunningTime;
    }

    public RunEntity ToEntity()
    {
        return new RunEntity
        {
            PublicId = Id,
            Date = Date.Value,
            Distance = Distance.Value ?? 0,
            AverageSpeed = AverageSpeed.Value ?? 0,
            BurntCalories = BurntCalories.Value ?? 0,
            RunningTime = RunningTime.Value ?? 0,
        };
    }

    public void ToEntity(RunEntity entity)
    { 
        entity.PublicId = Id;
        entity.Date = Date.Value;
        entity.Distance = Distance.Value ?? 0;
        entity.AverageSpeed = AverageSpeed.Value ?? 0;
        entity.BurntCalories = BurntCalories.Value ?? 0;
        entity.RunningTime = RunningTime.Value ?? 0;
    }
    private void AddValidators() 
    {
        this.Distance.Validations.Add(new NullableIntegerRule<double?> { ValidationMessage = "Distance is a required field." });
        this.Distance.Validations.Add(new MinValueRule<double?>(0) { ValidationMessage = "Distance can't be less than 0." });

        this.AverageSpeed.Validations.Add(new NullableIntegerRule<double?> { ValidationMessage = "Average speed is a required field." });
        this.AverageSpeed.Validations.Add(new MinValueRule<double?>(0) { ValidationMessage = "Average speed can't be less than 0." });

        this.BurntCalories.Validations.Add(new NullableIntegerRule<double?> { ValidationMessage = "Burnt calories is a required field." });
        this.BurntCalories.Validations.Add(new MinValueRule<double?>(0) { ValidationMessage = "Burnt calories can't be less than 0." });

        this.RunningTime.Validations.Add(new NullableIntegerRule<uint?> { ValidationMessage = "Running time is a required field." });
        this.RunningTime.Validations.Add(new MinValueRule<uint?>(0) { ValidationMessage = "Running time can't be less than 0." });
    }
}
