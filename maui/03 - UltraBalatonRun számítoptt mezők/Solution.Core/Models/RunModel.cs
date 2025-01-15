
using MauiValidationLibrary;
using MauiValidationLibrary.ValidationRules;
using Microsoft.IdentityModel.Tokens;
using Solution.Database.Entities;
using System.Transactions;

namespace Solution.Core.Models;

public partial class RunModel
{
    public string Id { get; set; }
    public ValidatableObject<DateTime> Date { get; set; }
    public ValidatableObject<double?> Distance { get; protected set; }

    public ValidatableObject<double?> Weight { get; protected set; }
    public double AverageSpeed {  get; protected set; }
    public double BurntCalories { get; protected set; }
    public ValidatableObject<uint?> RunningTime { get;  protected set; }

    public RunModel() 
    {
        this.Date = new ValidatableObject<DateTime>();
        this.Distance = new ValidatableObject<double?>();
        this.Weight = new ValidatableObject<double?>();
        this.AverageSpeed = new double();
        this.BurntCalories = new double();
        this.RunningTime = new ValidatableObject<uint?> { };

        AddValidators();
    }

    public RunModel(RunEntity entity) : this()
    {
        Id = entity.PublicId;
        Date.Value = entity.Date;
        Distance.Value = entity.Distance;
        Weight.Value = entity.Weight;
        AverageSpeed = entity.AverageSpeed;
        BurntCalories = entity.BurntCalories;
        RunningTime.Value = entity.RunningTime;
    }

    public RunEntity ToEntity()
    {
        return new RunEntity
        {
            PublicId = Id,
            Date = Date.Value,
            Distance = Distance.Value ?? 0,
            Weight = Weight.Value ?? 0,
            AverageSpeed = CalcAverageSpeed(),
            BurntCalories = CalcBurntCalories(),
            RunningTime = RunningTime.Value ?? 0,
        };
    }

    public void ToEntity(RunEntity entity)
    { 
        entity.PublicId = Id;
        entity.Date = Date.Value;
        entity.Distance = Distance.Value ?? 0;
        entity.Weight = Weight.Value ?? 0;
        entity.AverageSpeed = AverageSpeed;
        entity.BurntCalories = BurntCalories;
        entity.RunningTime = RunningTime.Value ?? 0;
    }
    private void AddValidators() 
    {
        this.Distance.Validations.Add(new NullableDoubleRule<double?> { ValidationMessage = "Distance is a required field." });
        this.Distance.Validations.Add(new MinValueDoubleRule<double?>(0) { ValidationMessage = "Distance can't be less than 0." });

        this.Weight.Validations.Add(new NullableDoubleRule<double?> { ValidationMessage = "Weight is a required field." });
        this.Weight.Validations.Add(new MinValueDoubleRule<double?>(0) { ValidationMessage = "Weight can't be less than 0." });

        this.RunningTime.Validations.Add(new NullableIntegerRule<uint?> { ValidationMessage = "Running time is a required field." });
        this.RunningTime.Validations.Add(new MinValueRule<uint?>(0) { ValidationMessage = "Running time can't be less than 0." });
    }

    private double CalcAverageSpeed() => (double)(Distance.Value ?? 0) / (double)((RunningTime.Value ?? 0) / (double)60);
        
    private double CalcBurntCalories() => (double)(Weight.Value ?? 0) * ((double)(RunningTime.Value ?? 0) / (double)60) * (double)CalcMETVAlue(CalcAverageSpeed());
    private double CalcMETVAlue(double speed)
    {
        double result;
        switch (speed)
        {
            case <= 8:
                { 
                    result = 6;
                    break;
                }
            case <= 9.7:
                {
                    result = 8;
                    break;
                }
            case <=11.3:
                {
                    result = 10;
                    break;
                }
            case <= 12.9:
                {
                    result = 11.5;
                    break;
                }
            case <= 14.5:
                {
                    result = 12.8;
                    break;
                }
            default: 
                {
                    result = 1; 
                    break;
                }
        };

        return result;
    }
}
