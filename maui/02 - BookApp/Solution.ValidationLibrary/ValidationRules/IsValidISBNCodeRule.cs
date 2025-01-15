using MauiValidationLibrary;

namespace Solution.ValidationLibrary.ValidationRules;

public class IsValidISBNCodeRule<T> : IValidationRule<T>
{
    public string ValidationMessage { get; set; } = $"ISBN code must be 10 or 13 characters long.";

    public bool Check(T value)
    {
        if (ulong.TryParse(value?.ToString(), out ulong data))
         { 
            string number = data.ToString();
            if (number.Length == 10 || number.Length == 13)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        { 
            return false;
        }
    }
}
