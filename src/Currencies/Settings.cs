using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using MiniJSON;

namespace Craxy.Parkitect.Currencies
{
  internal struct ValidationResult
  {
    public readonly string Message;
    private ValidationResult(string message)
    {
      Message = message;
    }

    public bool IsOk()
    {
      return Message == _ok;
    }
    public bool IsError()
    {
      return Message != _ok;
    }

    private static string _ok = "";
    public static ValidationResult OK()
    {
      return new ValidationResult(_ok);
    }

    public static ValidationResult Error(string message)
    {
      return new ValidationResult(message);
    }
  }

  internal class Entry<T>
  {
    // for each settings:
    //  validate
    //  default value
    //  name for serialization
    //  Property

    public Entry(string name, Func<T> getValue, Action<T> setValue, Func<T, ValidationResult> validate, Func<T> defaultValue)
    {
      Name = name;
      _getValue = getValue;
      _setValue = setValue;
      _validate = validate;
      _defaultValue = defaultValue;
    }

    public readonly Func<T, ValidationResult> _validate;
    private ValidationResult Validate(T value)
    {
      try
      {
        return _validate(value);
      }
      catch (Exception e)
      {
        return ValidationResult.Error(String.Format("Exception during validation of '{0}' for '{1}': {2}", value, Name, e.Message));
      }
    }
    private readonly Func<T> _defaultValue;
    public T DefaultValue { get { return _defaultValue(); } }
    public string Name { get; private set; }
    private readonly Func<T> _getValue;
    public readonly Action<T> _setValue;

    public T Value
    {
      get
      {
        return _getValue();
      }
      set
      {
        if (Object.Equals(value, Value))
        {
          return;
        }

        var validation = Validate(value);
        if (validation.IsError())
        {
          throw new ArgumentException(validation.Message, "value");
        }
        _setValue(value);
      }
    }

    public void SaveToDictionary(Dictionary<string, Object> dict)
    {
      dict[Name] = Value;
    }
    public ValidationResult LoadFromDictionary(Dictionary<string, Object> dict)
    {
      object value;
      if (!dict.TryGetValue(Name, out value))
      {
        return ValidationResult.Error(String.Format("There's no data for '{0}'.", Name));
      }

      // MiniJson does support only a couple of types
      // and there types are not mapped directly.
      // For example: int gets deserialized as long
      // https://gist.github.com/darktable/1411710

      // null is always wrong
      if (value == null)
      {
        return ValidationResult.Error(String.Format("Data for '{0}' is null.", Name));
      }


      try
      {
        T v = (T)Convert.ChangeType(value, typeof(T));
        var validation = Validate(v);
        if (validation.IsOk())
        {
          Value = v;
        }
        return validation;
      }
      catch (Exception e)
      {
        return ValidationResult.Error(String.Format("Exception during loading of '{0}' with value '{1}': {2}", Name, value, e.Message));
      }
    }
  }

  internal sealed class Settings
  {
    #region Number Format
    private static NumberFormatInfo _defaultNumberFormat = GameController.currencyFormat;
    public static NumberFormatInfo DefaultNumberFormat
    {
      get
      {
        return _defaultNumberFormat;
      }
    }

    private NumberFormatInfo _numberFormat = (NumberFormatInfo)DefaultNumberFormat.Clone();
    public NumberFormatInfo NumberFormat
    {
      get
      {
        return _numberFormat;
      }
    }
    #endregion

    #region helper
    private static Func<T, ValidationResult> Validate<T>(Func<T, bool> validator, string errorMessage)
    {
      return (value) => validator(value) ? ValidationResult.OK() : ValidationResult.Error(errorMessage);
    }
    private static Func<int, bool> Between(int min, int max)
    {
      return (value) => min <= value && value <= max;
    }
    private static Func<string, bool> LengthBetween(int min, int max)
    {
      return (value) => min <= value.Length && value.Length <= max;
    }
    #endregion

    #region Symbol
    private Entry<string> _symbol;
    public Entry<string> Symbol
    {
      get
      {
        if (_symbol == null)
        {
          _symbol = new Entry<string>("Symbol",
                                      () => NumberFormat.CurrencySymbol,
                                      (value) => NumberFormat.CurrencySymbol = value,
                                      Validate(LengthBetween(0, 3), "Symbol must be between 0 and 3 characters."),
                                      () => DefaultNumberFormat.CurrencySymbol
                                    );
        }
        return _symbol;
      }
    }
    #endregion
    #region DecimalSeparator
    private Entry<string> _decimalSeparator;
    public Entry<string> DecimalSeparator
    {
      get
      {
        if (_decimalSeparator == null)
        {
          _decimalSeparator = new Entry<string>("DecimalSeparator",
                                      () => NumberFormat.CurrencyDecimalSeparator,
                                      (value) => NumberFormat.CurrencyDecimalSeparator = value,
                                      Validate(LengthBetween(0, 3), "Decimal separator must be between 1 and 3 characters."),
                                      () => DefaultNumberFormat.CurrencyDecimalSeparator
                                    );
        }
        return _decimalSeparator;
      }
    }
    #endregion
    #region GroupSeparator
    private Entry<string> _groupSeparator;
    public Entry<string> GroupSeparator
    {
      get
      {
        if (_groupSeparator == null)
        {
          _groupSeparator = new Entry<string>("GroupSeparator",
                                      () => NumberFormat.CurrencyGroupSeparator,
                                      (value) => NumberFormat.CurrencyGroupSeparator = value,
                                      Validate(LengthBetween(0, 3), "Group separator must be between 0 and 3 characters."),
                                      () => DefaultNumberFormat.CurrencyGroupSeparator
                                    );
        }
        return _groupSeparator;
      }
    }
    #endregion
    #region PositivePattern
    private Entry<int> _positivePattern;
    public Entry<int> PositivePattern
    {
      get
      {
        if (_positivePattern == null)
        {
          _positivePattern = new Entry<int>("PositivePattern",
                                      () => NumberFormat.CurrencyPositivePattern,
                                      (value) => NumberFormat.CurrencyPositivePattern = value,
                                      Validate(Between(0, 3), "Positive pattern must be between 0 and 3."),
                                      () => DefaultNumberFormat.CurrencyPositivePattern
                                    );
        }
        return _positivePattern;
      }
    }
    #endregion
    #region NegativePattern
    private Entry<int> _negativePattern;
    public Entry<int> NegativePattern
    {
      get
      {
        if (_negativePattern == null)
        {
          _negativePattern = new Entry<int>("NegativePattern",
                                      () => NumberFormat.CurrencyNegativePattern,
                                      (value) => NumberFormat.CurrencyNegativePattern = value,
                                      Validate(Between(0, 15), "Negative pattern must be between 0 and 15."),
                                      () => DefaultNumberFormat.CurrencyNegativePattern
                                    );
        }
        return _negativePattern;
      }
    }
    #endregion

    #region Serialize
    public string SaveToJson()
    {
      var dict = new Dictionary<string, object>();
      Symbol.SaveToDictionary(dict);
      DecimalSeparator.SaveToDictionary(dict);
      GroupSeparator.SaveToDictionary(dict);
      PositivePattern.SaveToDictionary(dict);
      NegativePattern.SaveToDictionary(dict);

      return Json.Serialize(dict);
    }
    public string[] LoadFromJson(string json)
    {
      var dict = (Dictionary<string, object>)Json.Deserialize(json);

      return new[]
        {
          Symbol.LoadFromDictionary(dict),
          DecimalSeparator.LoadFromDictionary(dict),
          GroupSeparator.LoadFromDictionary(dict),
          PositivePattern.LoadFromDictionary(dict),
          NegativePattern.LoadFromDictionary(dict),
        }
        .Where(vr => vr.IsError())
        .Select(vr => vr.Message)
        .ToArray()
      ;
    }
    #endregion
  }
}
