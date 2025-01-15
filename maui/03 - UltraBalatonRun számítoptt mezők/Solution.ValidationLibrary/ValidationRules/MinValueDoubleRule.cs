namespace MauiValidationLibrary.ValidationRules;

public class MinValueDoubleRule<T>(double minValue) : IValidationRule<T>
{
  public string ValidationMessage { get; set; } = $"Length can't bee less then {minValue}.";

  public bool Check(T value)
  {
      if(!double.TryParse(value?.ToString(), out double data))
      { 
          return false;
      }
        
      return data >= minValue;
  }
}
