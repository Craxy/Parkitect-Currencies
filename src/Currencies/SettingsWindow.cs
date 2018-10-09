using System;
using System.Linq;
using Craxy.Parkitect.Currencies.Utils;
using UnityEngine;
using System.Globalization;
using System.Text;
using System.Collections.Generic;

namespace Craxy.Parkitect.Currencies
{
  internal class SettingsWindow
  {
    public Settings Settings { get; private set; }

    public SettingsWindow(Settings settings)
    {
      Settings = settings;

      // style for TextField isn't in correct position
      // -> shift down
      _textField = new GUIStyle(Skin.textField);
      _textField.margin.top = 17;

      _smallText = new GUIStyle(Skin.label);
      _smallText.fontSize = 12;
      _smallText.margin.top = 10;
      _smallText.margin.bottom = 5;

      UpdateNotIncludedDisplay();
    }

    public void Draw()
    {
      using (Layout.Vertical())
      {
        DrawSettings();
        GUILayout.Space(20.0f);
        DrawPresets();
        GUILayout.Space(20.0f);
        DrawExamples();
        GUILayout.Space(20.0f);
      }
    }

    private static GUISkin Skin
    {
      get
      {
        return ScriptableSingleton<UIAssetManager>.Instance.guiSkin;
      }
    }
    private GUIStyle _textField, _smallText;
    private void DrawSettings()
    {
      DrawSymbol();
      DrawSymbolPosition();
    }

    private string _notIncluded = "";
    private void UpdateSymbol(string symbol)
    {
      if(symbol != Settings.Symbol.Value)
      {
        Settings.Symbol.Value = symbol;
        UpdateNotIncludedDisplay();
      }
    }
    private void UpdateNotIncludedDisplay()
    {
      var font = FontInjector.GetDefaultGameFont();
      _notIncluded = string.Join(", ", Settings.Symbol.Value.Distinct().Where(c => !font.HasCharacter(c, true)));
    }
    private void DrawSymbol()
    {
      using (Layout.Horizontal())
      {
        GUILayout.Label("Symbol:");
        GUILayout.Space(5.0f);
        var symbol = GUILayout.TextField(Settings.Symbol.Value, 3, _textField, GUILayout.Width(60.0f));
        UpdateSymbol(symbol);
        GUILayout.FlexibleSpace();
      }
      if(_notIncluded.Length > 0)
      {
        using(Layout.Horizontal())
        {
          GUILayout.Space(50.0f);
          GUILayout.Label($"Character(s) {_notIncluded} are not available ingame and will be displayed as ☐.", _smallText);
          GUILayout.FlexibleSpace();
        }
      }
    }
    private void DrawSymbolPosition()
    {
      using (Layout.Horizontal())
      {
        GUILayout.Label("Symbol Position:");
        GUILayout.FlexibleSpace();
      }
      GUILayout.Space(-10.0f);
      DrawPositiveNumberFormat();
      GUILayout.Space(-10.0f);
      DrawNegativeNumberFormat();
    }
    private void DrawPositiveNumberFormat()
    {
      using (Layout.Horizontal())
      {
        GUILayout.Space(30.0f);
        GUILayout.Label("positive numbers:");
        GUILayout.FlexibleSpace();
      }

      GUILayout.Space(-10.0f);

      var current = Settings.PositivePattern.Value;
      var possibleValues = new[] { 2, 0, 1, 3, };

      var value = 32.75f;
      var selected = current;
      using (Layout.Horizontal())
      {
        GUILayout.Space(60.0f);
        foreach (var v in possibleValues)
        {
          Settings.PositivePattern.Value = v;
          if (GUILayout.Toggle(v == current, " " + value.ToString("C", Settings.NumberFormat)))
          {
            if (v != current)
            {
              selected = v;
            }
          }
        }
        GUILayout.FlexibleSpace();
      }
      Settings.PositivePattern.Value = selected;
    }
    private void DrawNegativeNumberFormat()
    {
      using (Layout.Horizontal())
      {
        GUILayout.Space(30.0f);
        GUILayout.Label("negative numbers:");
        GUILayout.FlexibleSpace();
      }

      GUILayout.Space(-10.0f);

      var current = Settings.NegativePattern.Value;
      var possibleValues = new[] { 9, 12, 1, 2, 5, 8, };

      var value = -32.75f;
      var selected = current;
      using (Layout.Horizontal())
      {
        GUILayout.Space(60.0f);
        foreach (var v in possibleValues)
        {
          Settings.NegativePattern.Value = v;
          if (GUILayout.Toggle(v == current, " " + value.ToString("C", Settings.NumberFormat)))
          {
            if (v != current)
            {
              selected = v;
            }
          }
        }

        if (!possibleValues.Contains(current))
        {
          Settings.NegativePattern.Value = current;
          GUILayout.Toggle(true, " " + value.ToString("C", Settings.NumberFormat));
        }

        GUILayout.FlexibleSpace();
      }
      Settings.NegativePattern.Value = selected;
    }


    private NumberFormatInfo FromNumberFormat(NumberFormatInfo value)
    {
      var nf = (NumberFormatInfo)Settings.NumberFormat.Clone();

      nf.CurrencySymbol = value.CurrencySymbol;
      nf.CurrencyPositivePattern = value.CurrencyPositivePattern;
      nf.CurrencyNegativePattern = value.CurrencyNegativePattern;

      return nf;
    }
    //issue: sometimes a currency has multiple symbols -- and one is used in roboto (font) and the other in CultureInfo
    //         for example yen has U+00A5 ¥ YEN SIGN, U+FFE5 ￥ FULLWIDTH YEN SIGN
    //          roboto uses YEN SIGN, but CultureInfo returns FULLWIDTH YEN SIGN
    private static Dictionary<char, char> ReplacementChars = new Dictionary<char, char>() {
      { (char)0xFFE5, (char)0x00A5 } // U+FFE5 ￥ Fullwidth Yen sign -> U+00A5 ¥ Yen sign
    };
    private static char ReplaceCharIfNecessary(char c)
    {
      return ReplacementChars.TryGetValue(c, out var replacement) ? replacement : c;
    }
    private static string ReplaceCharsIfNecessary(string str)
    {
      return string.Join("", str.Select(ReplaceCharIfNecessary));
    }
    private void ApplyNumberFormatInfo(NumberFormatInfo value)
    {
      var nf = Settings.NumberFormat;

      nf.CurrencySymbol = ReplaceCharsIfNecessary(value.CurrencySymbol);
      nf.CurrencyPositivePattern = value.CurrencyPositivePattern;
      nf.CurrencyNegativePattern = value.CurrencyNegativePattern;

      UpdateNotIncludedDisplay();
    }

    private class Preset
    {
      public CultureInfo Culture { get; internal set; }
      public RegionInfo Region { get; internal set; }
      public string Remarks { get; internal set; } = "";
    }
    private Preset CreatePreset(string cultureName, string remarks)
    {
      var culture = new CultureInfo(cultureName);
      var region = new RegionInfo(cultureName);
      // we don't want all NumberFormat properties, but just some
      culture.NumberFormat = FromNumberFormat(culture.NumberFormat);

      return new Preset
      {
        Culture = culture,
        Region = region,
        Remarks = remarks ?? "",
      };
    }
    private Preset CreatePreset(string cultureName)
    {
      return CreatePreset(cultureName, "");
    }
    private Preset[] _presets = null;
    private void InitalizePresets()
    {
      var usDefault = CreatePreset("en-US", "default");
      usDefault.Culture.NumberFormat.CurrencyNegativePattern = 1;

      _presets = new Preset[]
        {
          //http://trigeminal.fmsinc.com/samples/setlocalesample2.asp
          //https://msdn.microsoft.com/en-us/goglobal/bb896001.aspx
          usDefault,
          CreatePreset("en-GB"),
          CreatePreset("de-DE"),
          CreatePreset("ru-RU"),
          CreatePreset("zh-CN"),
          CreatePreset("ja-JP"),
          CreatePreset("tr-TR"),
          CreatePreset("hi-IN"),
        };
    }
    private void DrawPresets()
    {
      if (_presets == null)
      {
        InitalizePresets();
      }

      GUILayout.Label("Presets:");

      foreach (var preset in _presets)
      {
        GUILayout.Space(-10.0f);
        DrawPreset(preset);
      }
    }
    private void DrawPreset(Preset preset)
    {

      var text = string.Format("{0} - {1}", preset.Culture.NumberFormat.CurrencySymbol, preset.Region.CurrencyEnglishName);
      if (!string.IsNullOrEmpty(preset.Remarks))
      {
        text = string.Format("{0} ({1})", text, preset.Remarks);
      }

      var positiveNumber = 32.75f;
      var negativeNumber = -32.75f;

      using (Layout.Horizontal())
      {
        GUILayout.Space(60.0f);
        if (GUILayout.Button(text, GUILayout.Width(200.0f)))
        {
          ApplyNumberFormatInfo(preset.Culture.NumberFormat);
        }
        GUILayout.Space(40.0f);
        GUILayout.Label(string.Format("<size=13>{0}</size>", positiveNumber.ToString("C", preset.Culture.NumberFormat)), GUILayout.Width(65.0f));
        GUILayout.Space(15.0f);
        GUILayout.Label(string.Format("<size=13>{0}</size>", negativeNumber.ToString("C", preset.Culture.NumberFormat)));
        GUILayout.FlexibleSpace();
      }
    }

    private static float[] _examples = new float[] { 0.00f, 2.50f, 19.99f, 123.45f, 2500.85f, 50000.00f, 12345567890.1233456789f, };
    private void DrawExamples()
    {
      GUILayout.Label("Examples:");
      GUILayout.Space(-10.0f);

      foreach (var example in _examples)
      {
        using (Layout.Horizontal())
        {
          GUILayout.Space(60.0f);
          GUILayout.Label(string.Format("<size=13>{0}</size>", example.ToString("C", Settings.NumberFormat)), GUILayout.Width(200.0f));
          GUILayout.Space(30.0f);
          GUILayout.Label(string.Format("<size=13>{0}</size>", (-example).ToString("C", Settings.NumberFormat)));
          GUILayout.FlexibleSpace();
        }
        GUILayout.Space(-15.0f);
      }
    }
  }
}
