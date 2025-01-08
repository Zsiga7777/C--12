using MauiValidationLibrary;

namespace Solution.ValidationLibrary.ValidationRules;

public class MaxValueRule<T>(int maxValue) : IValidationRule<T>
{
    public string ValidationMessage { get; set; } = $"Release year can't be more then {maxValue}.";

    public bool Check(T value)
    {
        if (!int.TryParse(value?.ToString(), out int data))
        {
            return false;
        }

        return data <= maxValue;
    }
}
