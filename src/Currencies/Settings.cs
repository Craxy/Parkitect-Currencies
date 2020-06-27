using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using MiniJSON;
using System.IO;
using static UnityEngine.Debug;

namespace Craxy.Parkitect.Currencies
{
  readonly struct ValidationResult
  {
    private readonly bool _isOk;
    public readonly string Message;
    private ValidationResult(bool isOk, string message)
    {
      _isOk = isOk;
      Message = message;
    }

    public bool IsOk() => _isOk;
    public bool IsError() => !_isOk;

    private static string _ok = "";
    public static ValidationResult OK()
    {
      return new ValidationResult(true, _ok);
    }

    public static ValidationResult Error(string message)
    {
      return new ValidationResult(false, message);
    }
  }

  sealed class Entry<T>
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
        if (EqualityComparer<T>.Default.Equals(value, Value))
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
      // and there are types not mapped directly.
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

  sealed class Settings
  {
    #region Number Format
    private static NumberFormatInfo _defaultNumberFormat = GameController.currencyFormat;
    public static NumberFormatInfo DefaultNumberFormat => _defaultNumberFormat;
    private NumberFormatInfo _numberFormat = (NumberFormatInfo)DefaultNumberFormat.Clone();
    public NumberFormatInfo NumberFormat => _numberFormat;

    private NumberFormatInfo _numberFormatWithSeparators = (NumberFormatInfo)DefaultNumberFormat.Clone();
    public NumberFormatInfo NumberFormatWithSeparators => _numberFormatWithSeparators;
    public NumberFormatInfo ActiveNumberFormat => UseCustomSeparators.Value ? NumberFormatWithSeparators : NumberFormat;
    #endregion

    #region helper
    private static Func<T, ValidationResult> AlwaysValid<T>()
      => (value) => ValidationResult.OK();
    private static Func<T, ValidationResult> Validate<T>(Func<T, bool> validator, string errorMessage)
      => (value) => validator(value) ? ValidationResult.OK() : ValidationResult.Error(errorMessage);
    private static Func<int, bool> Between(int min, int max)
      => (value) => min <= value && value <= max;
    private static Func<string, bool> LengthBetween(int min, int max)
      => (value) => min <= value.Length && value.Length <= max;
    #endregion

    #region Symbol
    private string symbol
    {
      get
      {
        Assert(NumberFormat.CurrencySymbol == _numberFormatWithSeparators.CurrencySymbol);
        return NumberFormat.CurrencySymbol;
      }
      set
      {
        Assert(NumberFormat.CurrencySymbol == _numberFormatWithSeparators.CurrencySymbol);
        NumberFormat.CurrencySymbol = value;
        NumberFormatWithSeparators.CurrencySymbol = value;
        Assert(NumberFormat.CurrencySymbol == _numberFormatWithSeparators.CurrencySymbol);
      }
    }
    private Entry<string> _symbol;
    public Entry<string> Symbol
    {
      get
      {
        if (_symbol == null)
        {
          _symbol = new Entry<string>("Symbol",
                                      () => symbol,
                                      (value) => symbol = value,
                                      Validate(LengthBetween(0, 3), "Symbol must be between 0 and 3 characters."),
                                      () => DefaultNumberFormat.CurrencySymbol
                                    );
        }
        return _symbol;
      }
    }
    #endregion
    #region PositivePattern
    private int positivePattern
    {
      get
      {
        Assert(NumberFormat.CurrencyPositivePattern == _numberFormatWithSeparators.CurrencyPositivePattern);
        return NumberFormat.CurrencyPositivePattern;
      }
      set
      {
        Assert(NumberFormat.CurrencyPositivePattern == _numberFormatWithSeparators.CurrencyPositivePattern);
        NumberFormat.CurrencyPositivePattern = value;
        NumberFormatWithSeparators.CurrencyPositivePattern = value;
        Assert(NumberFormat.CurrencyPositivePattern == _numberFormatWithSeparators.CurrencyPositivePattern);
      }
    }
    private Entry<int> _positivePattern;
    public Entry<int> PositivePattern
    {
      get
      {
        if (_positivePattern == null)
        {
          _positivePattern = new Entry<int>("PositivePattern",
                                      () => positivePattern,
                                      (value) => positivePattern = value,
                                      Validate(Between(0, 3), "Positive pattern must be between 0 and 3."),
                                      () => DefaultNumberFormat.CurrencyPositivePattern
                                    );
        }
        return _positivePattern;
      }
    }
    #endregion
    #region NegativePattern
    private int negativePattern
    {
      get
      {
        Assert(NumberFormat.CurrencyNegativePattern == _numberFormatWithSeparators.CurrencyNegativePattern);
        return NumberFormat.CurrencyNegativePattern;
      }
      set
      {
        Assert(NumberFormat.CurrencyNegativePattern == _numberFormatWithSeparators.CurrencyNegativePattern);
        NumberFormat.CurrencyNegativePattern = value;
        NumberFormatWithSeparators.CurrencyNegativePattern = value;
        Assert(NumberFormat.CurrencyNegativePattern == _numberFormatWithSeparators.CurrencyNegativePattern);
      }
    }
    private Entry<int> _negativePattern;
    public Entry<int> NegativePattern
    {
      get
      {
        if (_negativePattern == null)
        {
          _negativePattern = new Entry<int>("NegativePattern",
                                      () => negativePattern,
                                      (value) => negativePattern = value,
                                      Validate(Between(0, 15), "Negative pattern must be between 0 and 15."),
                                      () => DefaultNumberFormat.CurrencyNegativePattern
                                    );
        }
        return _negativePattern;
      }
    }
    #endregion

    #region Custom Separators

    #region UseCustomSeparators
    private bool _customSeparators = false;
    private Entry<bool> _useCustomSeparators;
    public Entry<bool> UseCustomSeparators
    {
      get
      {
        if (_useCustomSeparators == null)
        {
          _useCustomSeparators = new Entry<bool>("UseCustomSeparators",
                                      () => _customSeparators,
                                      (value) => _customSeparators = value,
                                      AlwaysValid<bool>(),
                                      () => false
                                    );
        }
        return _useCustomSeparators;
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
                                      () => NumberFormatWithSeparators.CurrencyDecimalSeparator,
                                      (value) => NumberFormatWithSeparators.CurrencyDecimalSeparator = value,
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
                                      () => NumberFormatWithSeparators.CurrencyGroupSeparator,
                                      (value) => NumberFormatWithSeparators.CurrencyGroupSeparator = value,
                                      Validate(LengthBetween(0, 3), "Group separator must be between 0 and 3 characters."),
                                      () => DefaultNumberFormat.CurrencyGroupSeparator
                                    );
        }
        return _groupSeparator;
      }
    }
    #endregion
    #endregion Custom Separator

    #region Serialize
    public string ToJson()
    {
      var dict = new Dictionary<string, object>();
      Symbol.SaveToDictionary(dict);
      PositivePattern.SaveToDictionary(dict);
      NegativePattern.SaveToDictionary(dict);
      UseCustomSeparators.SaveToDictionary(dict);
      DecimalSeparator.SaveToDictionary(dict);
      GroupSeparator.SaveToDictionary(dict);

      return Json.Serialize(dict);
    }
    public string[] FromJson(string json)
    {
      var dict = (Dictionary<string, object>)Json.Deserialize(json);

      return new[]
        {
          Symbol.LoadFromDictionary(dict),
          PositivePattern.LoadFromDictionary(dict),
          NegativePattern.LoadFromDictionary(dict),
          UseCustomSeparators.LoadFromDictionary(dict),
          DecimalSeparator.LoadFromDictionary(dict),
          GroupSeparator.LoadFromDictionary(dict),
        }
        .Where(vr => vr.IsError())
        .Select(vr => vr.Message)
        .ToArray()
      ;
    }
    #endregion

    #region Load/Save
    public static Settings Load(string path)
    {
      var settings = new Settings();

      if (File.Exists(path))
      {
        try
        {
          var json = File.ReadAllText(path);
          var errors = settings.FromJson(json);

          if (errors.Length > 0)
          {
            Mod.Log("Error(s) loading settings: " + String.Join("; ", errors));
          }
          else
          {
            Mod.Log(String.Format("Settings loaded from \"{0}\"", path));
          }
        }
        catch (Exception ex)
        {
          Mod.Log("Exception while loading settings: " + ex.Message);
        }
      }

      return settings;
    }
    public void Save(string path)
    {
      try
      {
        var json = this.ToJson();
        File.WriteAllText(path, json);
        Mod.Log(String.Format("Settings saved to \"{0}\"", path));
      }
      catch (Exception ex)
      {
        Mod.Log("Exception while saving settings: " + ex.Message);
      }
    }
    #endregion Load/Save
  }
}
