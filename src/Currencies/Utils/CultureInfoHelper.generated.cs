using System;
using System.Globalization;

namespace Craxy.Parkitect.Currencies.Utils
{
  public static partial class CultureInfoHelper
  {
    public static CustomCultureInfo GetCulture(string name)
    {
      switch(name)
      {
        case "en-US":
          return
            new CustomCultureInfo
            {
              LCID = 1033,
              IetfLanguageTag = "en-US",
              TwoLetterISOLanguageName = "en",
              ThreeLetterISOLanguageName = "eng",
              IsNeutralCulture = false,
              Name = "en-US",
              DisplayName = "English (United States)",
              EnglishName = "English (United States)",
              NativeName = "English (United States)",
              NumberFormat =
                new NumberFormatInfo
                {
                  CurrencyDecimalDigits = 2,
                  CurrencyDecimalSeparator = ".",
                  CurrencyGroupSizes = new int[] { 3 },
                  NumberGroupSizes = new int[] { 3 },
                  PercentGroupSizes = new int[] { 3 },
                  CurrencyGroupSeparator = ",",
                  CurrencySymbol = "$",
                  NaNSymbol = "NaN",
                  CurrencyNegativePattern = 0,
                  NumberNegativePattern = 1,
                  PercentPositivePattern = 0,
                  PercentNegativePattern = 0,
                  NegativeInfinitySymbol = "-∞",
                  NegativeSign = "-",
                  NumberDecimalDigits = 2,
                  NumberDecimalSeparator = ".",
                  NumberGroupSeparator = ",",
                  CurrencyPositivePattern = 0,
                  PositiveInfinitySymbol = "∞",
                  PositiveSign = "+",
                  PercentDecimalDigits = 2,
                  PercentDecimalSeparator = ".",
                  PercentGroupSeparator = ",",
                  PercentSymbol = "%",
                  PerMilleSymbol = "‰",
                }
              ,
              RegionInfo =
                new CustomRegionInfo
                {
                  Name = "US",
                  EnglishName = "United States",
                  DisplayName = "United States",
                  NativeName = "United States",
                  TwoLetterISORegionName = "US",
                  ThreeLetterISORegionName = "USA",
                  IsMetric = false,
                  GeoId = 244,
                  CurrencyEnglishName = "US Dollar",
                  CurrencyNativeName = "US Dollar",
                  CurrencySymbol = "$",
                  ISOCurrencySymbol = "USD",
                }
              ,
            }
            ;
        case "en-GB":
          return
            new CustomCultureInfo
            {
              LCID = 2057,
              IetfLanguageTag = "en-GB",
              TwoLetterISOLanguageName = "en",
              ThreeLetterISOLanguageName = "eng",
              IsNeutralCulture = false,
              Name = "en-GB",
              DisplayName = "English (United Kingdom)",
              EnglishName = "English (United Kingdom)",
              NativeName = "English (United Kingdom)",
              NumberFormat =
                new NumberFormatInfo
                {
                  CurrencyDecimalDigits = 2,
                  CurrencyDecimalSeparator = ".",
                  CurrencyGroupSizes = new int[] { 3 },
                  NumberGroupSizes = new int[] { 3 },
                  PercentGroupSizes = new int[] { 3 },
                  CurrencyGroupSeparator = ",",
                  CurrencySymbol = "£",
                  NaNSymbol = "NaN",
                  CurrencyNegativePattern = 1,
                  NumberNegativePattern = 1,
                  PercentPositivePattern = 1,
                  PercentNegativePattern = 1,
                  NegativeInfinitySymbol = "-∞",
                  NegativeSign = "-",
                  NumberDecimalDigits = 2,
                  NumberDecimalSeparator = ".",
                  NumberGroupSeparator = ",",
                  CurrencyPositivePattern = 0,
                  PositiveInfinitySymbol = "∞",
                  PositiveSign = "+",
                  PercentDecimalDigits = 2,
                  PercentDecimalSeparator = ".",
                  PercentGroupSeparator = ",",
                  PercentSymbol = "%",
                  PerMilleSymbol = "‰",
                }
              ,
              RegionInfo =
                new CustomRegionInfo
                {
                  Name = "GB",
                  EnglishName = "United Kingdom",
                  DisplayName = "United Kingdom",
                  NativeName = "United Kingdom",
                  TwoLetterISORegionName = "GB",
                  ThreeLetterISORegionName = "GBR",
                  IsMetric = true,
                  GeoId = 242,
                  CurrencyEnglishName = "British Pound",
                  CurrencyNativeName = "Pound Sterling",
                  CurrencySymbol = "£",
                  ISOCurrencySymbol = "GBP",
                }
              ,
            }
            ;
        case "de-DE":
          return
            new CustomCultureInfo
            {
              LCID = 1031,
              IetfLanguageTag = "de-DE",
              TwoLetterISOLanguageName = "de",
              ThreeLetterISOLanguageName = "deu",
              IsNeutralCulture = false,
              Name = "de-DE",
              DisplayName = "German (Germany)",
              EnglishName = "German (Germany)",
              NativeName = "Deutsch (Deutschland)",
              NumberFormat =
                new NumberFormatInfo
                {
                  CurrencyDecimalDigits = 2,
                  CurrencyDecimalSeparator = ",",
                  CurrencyGroupSizes = new int[] { 3 },
                  NumberGroupSizes = new int[] { 3 },
                  PercentGroupSizes = new int[] { 3 },
                  CurrencyGroupSeparator = ".",
                  CurrencySymbol = "€",
                  NaNSymbol = "NaN",
                  CurrencyNegativePattern = 8,
                  NumberNegativePattern = 1,
                  PercentPositivePattern = 0,
                  PercentNegativePattern = 0,
                  NegativeInfinitySymbol = "-∞",
                  NegativeSign = "-",
                  NumberDecimalDigits = 2,
                  NumberDecimalSeparator = ",",
                  NumberGroupSeparator = ".",
                  CurrencyPositivePattern = 3,
                  PositiveInfinitySymbol = "∞",
                  PositiveSign = "+",
                  PercentDecimalDigits = 2,
                  PercentDecimalSeparator = ",",
                  PercentGroupSeparator = ".",
                  PercentSymbol = "%",
                  PerMilleSymbol = "‰",
                }
              ,
              RegionInfo =
                new CustomRegionInfo
                {
                  Name = "DE",
                  EnglishName = "Germany",
                  DisplayName = "Germany",
                  NativeName = "Deutschland",
                  TwoLetterISORegionName = "DE",
                  ThreeLetterISORegionName = "DEU",
                  IsMetric = true,
                  GeoId = 94,
                  CurrencyEnglishName = "Euro",
                  CurrencyNativeName = "Euro",
                  CurrencySymbol = "€",
                  ISOCurrencySymbol = "EUR",
                }
              ,
            }
            ;
        case "fr-FR":
          return
            new CustomCultureInfo
            {
              LCID = 1036,
              IetfLanguageTag = "fr-FR",
              TwoLetterISOLanguageName = "fr",
              ThreeLetterISOLanguageName = "fra",
              IsNeutralCulture = false,
              Name = "fr-FR",
              DisplayName = "French (France)",
              EnglishName = "French (France)",
              NativeName = "français (France)",
              NumberFormat =
                new NumberFormatInfo
                {
                  CurrencyDecimalDigits = 2,
                  CurrencyDecimalSeparator = ",",
                  CurrencyGroupSizes = new int[] { 3 },
                  NumberGroupSizes = new int[] { 3 },
                  PercentGroupSizes = new int[] { 3 },
                  CurrencyGroupSeparator = " ",
                  CurrencySymbol = "€",
                  NaNSymbol = "NaN",
                  CurrencyNegativePattern = 8,
                  NumberNegativePattern = 1,
                  PercentPositivePattern = 0,
                  PercentNegativePattern = 0,
                  NegativeInfinitySymbol = "-∞",
                  NegativeSign = "-",
                  NumberDecimalDigits = 2,
                  NumberDecimalSeparator = ",",
                  NumberGroupSeparator = " ",
                  CurrencyPositivePattern = 3,
                  PositiveInfinitySymbol = "∞",
                  PositiveSign = "+",
                  PercentDecimalDigits = 2,
                  PercentDecimalSeparator = ",",
                  PercentGroupSeparator = " ",
                  PercentSymbol = "%",
                  PerMilleSymbol = "‰",
                }
              ,
              RegionInfo =
                new CustomRegionInfo
                {
                  Name = "FR",
                  EnglishName = "France",
                  DisplayName = "France",
                  NativeName = "France",
                  TwoLetterISORegionName = "FR",
                  ThreeLetterISORegionName = "FRA",
                  IsMetric = true,
                  GeoId = 84,
                  CurrencyEnglishName = "Euro",
                  CurrencyNativeName = "euro",
                  CurrencySymbol = "€",
                  ISOCurrencySymbol = "EUR",
                }
              ,
            }
            ;
        case "es-ES":
          return
            new CustomCultureInfo
            {
              LCID = 3082,
              IetfLanguageTag = "es-ES",
              TwoLetterISOLanguageName = "es",
              ThreeLetterISOLanguageName = "spa",
              IsNeutralCulture = false,
              Name = "es-ES",
              DisplayName = "Spanish (Spain)",
              EnglishName = "Spanish (Spain, International Sort)",
              NativeName = "español (España, alfabetización internacional)",
              NumberFormat =
                new NumberFormatInfo
                {
                  CurrencyDecimalDigits = 2,
                  CurrencyDecimalSeparator = ",",
                  CurrencyGroupSizes = new int[] { 3 },
                  NumberGroupSizes = new int[] { 3 },
                  PercentGroupSizes = new int[] { 3 },
                  CurrencyGroupSeparator = ".",
                  CurrencySymbol = "€",
                  NaNSymbol = "NaN",
                  CurrencyNegativePattern = 8,
                  NumberNegativePattern = 1,
                  PercentPositivePattern = 0,
                  PercentNegativePattern = 0,
                  NegativeInfinitySymbol = "-∞",
                  NegativeSign = "-",
                  NumberDecimalDigits = 2,
                  NumberDecimalSeparator = ",",
                  NumberGroupSeparator = ".",
                  CurrencyPositivePattern = 3,
                  PositiveInfinitySymbol = "∞",
                  PositiveSign = "+",
                  PercentDecimalDigits = 2,
                  PercentDecimalSeparator = ",",
                  PercentGroupSeparator = ".",
                  PercentSymbol = "%",
                  PerMilleSymbol = "‰",
                }
              ,
              RegionInfo =
                new CustomRegionInfo
                {
                  Name = "ES",
                  EnglishName = "Spain",
                  DisplayName = "Spain",
                  NativeName = "España",
                  TwoLetterISORegionName = "ES",
                  ThreeLetterISORegionName = "ESP",
                  IsMetric = true,
                  GeoId = 217,
                  CurrencyEnglishName = "Euro",
                  CurrencyNativeName = "euro",
                  CurrencySymbol = "€",
                  ISOCurrencySymbol = "EUR",
                }
              ,
            }
            ;
        case "ru-RU":
          return
            new CustomCultureInfo
            {
              LCID = 1049,
              IetfLanguageTag = "ru-RU",
              TwoLetterISOLanguageName = "ru",
              ThreeLetterISOLanguageName = "rus",
              IsNeutralCulture = false,
              Name = "ru-RU",
              DisplayName = "Russian (Russia)",
              EnglishName = "Russian (Russia)",
              NativeName = "русский (Россия)",
              NumberFormat =
                new NumberFormatInfo
                {
                  CurrencyDecimalDigits = 2,
                  CurrencyDecimalSeparator = ",",
                  CurrencyGroupSizes = new int[] { 3 },
                  NumberGroupSizes = new int[] { 3 },
                  PercentGroupSizes = new int[] { 3 },
                  CurrencyGroupSeparator = " ",
                  CurrencySymbol = "₽",
                  NaNSymbol = "не число",
                  CurrencyNegativePattern = 8,
                  NumberNegativePattern = 1,
                  PercentPositivePattern = 1,
                  PercentNegativePattern = 1,
                  NegativeInfinitySymbol = "-∞",
                  NegativeSign = "-",
                  NumberDecimalDigits = 2,
                  NumberDecimalSeparator = ",",
                  NumberGroupSeparator = " ",
                  CurrencyPositivePattern = 3,
                  PositiveInfinitySymbol = "∞",
                  PositiveSign = "+",
                  PercentDecimalDigits = 2,
                  PercentDecimalSeparator = ",",
                  PercentGroupSeparator = " ",
                  PercentSymbol = "%",
                  PerMilleSymbol = "‰",
                }
              ,
              RegionInfo =
                new CustomRegionInfo
                {
                  Name = "RU",
                  EnglishName = "Russia",
                  DisplayName = "Russia",
                  NativeName = "Россия",
                  TwoLetterISORegionName = "RU",
                  ThreeLetterISORegionName = "RUS",
                  IsMetric = true,
                  GeoId = 203,
                  CurrencyEnglishName = "Russian Ruble",
                  CurrencyNativeName = "рубль",
                  CurrencySymbol = "₽",
                  ISOCurrencySymbol = "RUB",
                }
              ,
            }
            ;
        case "zh-CN":
          return
            new CustomCultureInfo
            {
              LCID = 2052,
              IetfLanguageTag = "zh-CN",
              TwoLetterISOLanguageName = "zh",
              ThreeLetterISOLanguageName = "zho",
              IsNeutralCulture = false,
              Name = "zh-CN",
              DisplayName = "Chinese (Simplified, PRC)",
              EnglishName = "Chinese (Simplified, China)",
              NativeName = "中文(中国)",
              NumberFormat =
                new NumberFormatInfo
                {
                  CurrencyDecimalDigits = 2,
                  CurrencyDecimalSeparator = ".",
                  CurrencyGroupSizes = new int[] { 3 },
                  NumberGroupSizes = new int[] { 3 },
                  PercentGroupSizes = new int[] { 3 },
                  CurrencyGroupSeparator = ",",
                  CurrencySymbol = "¥",
                  NaNSymbol = "NaN",
                  CurrencyNegativePattern = 2,
                  NumberNegativePattern = 1,
                  PercentPositivePattern = 1,
                  PercentNegativePattern = 1,
                  NegativeInfinitySymbol = "-∞",
                  NegativeSign = "-",
                  NumberDecimalDigits = 2,
                  NumberDecimalSeparator = ".",
                  NumberGroupSeparator = ",",
                  CurrencyPositivePattern = 0,
                  PositiveInfinitySymbol = "∞",
                  PositiveSign = "+",
                  PercentDecimalDigits = 2,
                  PercentDecimalSeparator = ".",
                  PercentGroupSeparator = ",",
                  PercentSymbol = "%",
                  PerMilleSymbol = "‰",
                }
              ,
              RegionInfo =
                new CustomRegionInfo
                {
                  Name = "CN",
                  EnglishName = "China",
                  DisplayName = "People's Republic of China",
                  NativeName = "中国",
                  TwoLetterISORegionName = "CN",
                  ThreeLetterISORegionName = "CHN",
                  IsMetric = true,
                  GeoId = 45,
                  CurrencyEnglishName = "Chinese Yuan",
                  CurrencyNativeName = "人民币",
                  CurrencySymbol = "¥",
                  ISOCurrencySymbol = "CNY",
                }
              ,
            }
            ;
        case "ja-JP":
          return
            new CustomCultureInfo
            {
              LCID = 1041,
              IetfLanguageTag = "ja-JP",
              TwoLetterISOLanguageName = "ja",
              ThreeLetterISOLanguageName = "jpn",
              IsNeutralCulture = false,
              Name = "ja-JP",
              DisplayName = "Japanese (Japan)",
              EnglishName = "Japanese (Japan)",
              NativeName = "日本語 (日本)",
              NumberFormat =
                new NumberFormatInfo
                {
                  CurrencyDecimalDigits = 0,
                  CurrencyDecimalSeparator = ".",
                  CurrencyGroupSizes = new int[] { 3 },
                  NumberGroupSizes = new int[] { 3 },
                  PercentGroupSizes = new int[] { 3 },
                  CurrencyGroupSeparator = ",",
                  CurrencySymbol = "¥",
                  NaNSymbol = "NaN",
                  CurrencyNegativePattern = 1,
                  NumberNegativePattern = 1,
                  PercentPositivePattern = 1,
                  PercentNegativePattern = 1,
                  NegativeInfinitySymbol = "-∞",
                  NegativeSign = "-",
                  NumberDecimalDigits = 2,
                  NumberDecimalSeparator = ".",
                  NumberGroupSeparator = ",",
                  CurrencyPositivePattern = 0,
                  PositiveInfinitySymbol = "∞",
                  PositiveSign = "+",
                  PercentDecimalDigits = 2,
                  PercentDecimalSeparator = ".",
                  PercentGroupSeparator = ",",
                  PercentSymbol = "%",
                  PerMilleSymbol = "‰",
                }
              ,
              RegionInfo =
                new CustomRegionInfo
                {
                  Name = "JP",
                  EnglishName = "Japan",
                  DisplayName = "Japan",
                  NativeName = "日本",
                  TwoLetterISORegionName = "JP",
                  ThreeLetterISORegionName = "JPN",
                  IsMetric = true,
                  GeoId = 122,
                  CurrencyEnglishName = "Japanese Yen",
                  CurrencyNativeName = "円",
                  CurrencySymbol = "¥",
                  ISOCurrencySymbol = "JPY",
                }
              ,
            }
            ;
        case "tr-TR":
          return
            new CustomCultureInfo
            {
              LCID = 1055,
              IetfLanguageTag = "tr-TR",
              TwoLetterISOLanguageName = "tr",
              ThreeLetterISOLanguageName = "tur",
              IsNeutralCulture = false,
              Name = "tr-TR",
              DisplayName = "Turkish (Turkey)",
              EnglishName = "Turkish (Turkey)",
              NativeName = "Türkçe (Türkiye)",
              NumberFormat =
                new NumberFormatInfo
                {
                  CurrencyDecimalDigits = 2,
                  CurrencyDecimalSeparator = ",",
                  CurrencyGroupSizes = new int[] { 3 },
                  NumberGroupSizes = new int[] { 3 },
                  PercentGroupSizes = new int[] { 3 },
                  CurrencyGroupSeparator = ".",
                  CurrencySymbol = "₺",
                  NaNSymbol = "NaN",
                  CurrencyNegativePattern = 8,
                  NumberNegativePattern = 1,
                  PercentPositivePattern = 2,
                  PercentNegativePattern = 2,
                  NegativeInfinitySymbol = "-∞",
                  NegativeSign = "-",
                  NumberDecimalDigits = 2,
                  NumberDecimalSeparator = ",",
                  NumberGroupSeparator = ".",
                  CurrencyPositivePattern = 3,
                  PositiveInfinitySymbol = "∞",
                  PositiveSign = "+",
                  PercentDecimalDigits = 2,
                  PercentDecimalSeparator = ",",
                  PercentGroupSeparator = ".",
                  PercentSymbol = "%",
                  PerMilleSymbol = "‰",
                }
              ,
              RegionInfo =
                new CustomRegionInfo
                {
                  Name = "TR",
                  EnglishName = "Turkey",
                  DisplayName = "Turkey",
                  NativeName = "Türkiye",
                  TwoLetterISORegionName = "TR",
                  ThreeLetterISORegionName = "TUR",
                  IsMetric = true,
                  GeoId = 235,
                  CurrencyEnglishName = "Turkish Lira",
                  CurrencyNativeName = "Türk Lirası",
                  CurrencySymbol = "₺",
                  ISOCurrencySymbol = "TRY",
                }
              ,
            }
            ;
        case "hi-IN":
          return
            new CustomCultureInfo
            {
              LCID = 1081,
              IetfLanguageTag = "hi-IN",
              TwoLetterISOLanguageName = "hi",
              ThreeLetterISOLanguageName = "hin",
              IsNeutralCulture = false,
              Name = "hi-IN",
              DisplayName = "Hindi (India)",
              EnglishName = "Hindi (India)",
              NativeName = "हिंदी (भारत)",
              NumberFormat =
                new NumberFormatInfo
                {
                  CurrencyDecimalDigits = 2,
                  CurrencyDecimalSeparator = ".",
                  CurrencyGroupSizes = new int[] { 3, 2 },
                  NumberGroupSizes = new int[] { 3, 2 },
                  PercentGroupSizes = new int[] { 3, 2 },
                  CurrencyGroupSeparator = ",",
                  CurrencySymbol = "₹",
                  NaNSymbol = "NaN",
                  CurrencyNegativePattern = 12,
                  NumberNegativePattern = 1,
                  PercentPositivePattern = 1,
                  PercentNegativePattern = 1,
                  NegativeInfinitySymbol = "-∞",
                  NegativeSign = "-",
                  NumberDecimalDigits = 2,
                  NumberDecimalSeparator = ".",
                  NumberGroupSeparator = ",",
                  CurrencyPositivePattern = 0,
                  PositiveInfinitySymbol = "∞",
                  PositiveSign = "+",
                  PercentDecimalDigits = 2,
                  PercentDecimalSeparator = ".",
                  PercentGroupSeparator = ",",
                  PercentSymbol = "%",
                  PerMilleSymbol = "‰",
                }
              ,
              RegionInfo =
                new CustomRegionInfo
                {
                  Name = "IN",
                  EnglishName = "India",
                  DisplayName = "India",
                  NativeName = "भारत",
                  TwoLetterISORegionName = "IN",
                  ThreeLetterISORegionName = "IND",
                  IsMetric = true,
                  GeoId = 113,
                  CurrencyEnglishName = "Indian Rupee",
                  CurrencyNativeName = "रुपया",
                  CurrencySymbol = "₹",
                  ISOCurrencySymbol = "INR",
                }
              ,
            }
            ;
        case "pt-BR":
          return
            new CustomCultureInfo
            {
              LCID = 1046,
              IetfLanguageTag = "pt-BR",
              TwoLetterISOLanguageName = "pt",
              ThreeLetterISOLanguageName = "por",
              IsNeutralCulture = false,
              Name = "pt-BR",
              DisplayName = "Portuguese (Brazil)",
              EnglishName = "Portuguese (Brazil)",
              NativeName = "português (Brasil)",
              NumberFormat =
                new NumberFormatInfo
                {
                  CurrencyDecimalDigits = 2,
                  CurrencyDecimalSeparator = ",",
                  CurrencyGroupSizes = new int[] { 3 },
                  NumberGroupSizes = new int[] { 3 },
                  PercentGroupSizes = new int[] { 3 },
                  CurrencyGroupSeparator = ".",
                  CurrencySymbol = "R$",
                  NaNSymbol = "NaN",
                  CurrencyNegativePattern = 1,
                  NumberNegativePattern = 1,
                  PercentPositivePattern = 1,
                  PercentNegativePattern = 1,
                  NegativeInfinitySymbol = "-∞",
                  NegativeSign = "-",
                  NumberDecimalDigits = 2,
                  NumberDecimalSeparator = ",",
                  NumberGroupSeparator = ".",
                  CurrencyPositivePattern = 0,
                  PositiveInfinitySymbol = "∞",
                  PositiveSign = "+",
                  PercentDecimalDigits = 2,
                  PercentDecimalSeparator = ",",
                  PercentGroupSeparator = ".",
                  PercentSymbol = "%",
                  PerMilleSymbol = "‰",
                }
              ,
              RegionInfo =
                new CustomRegionInfo
                {
                  Name = "BR",
                  EnglishName = "Brazil",
                  DisplayName = "Brazil",
                  NativeName = "Brasil",
                  TwoLetterISORegionName = "BR",
                  ThreeLetterISORegionName = "BRA",
                  IsMetric = true,
                  GeoId = 32,
                  CurrencyEnglishName = "Brazilian Real",
                  CurrencyNativeName = "Real",
                  CurrencySymbol = "R$",
                  ISOCurrencySymbol = "BRL",
                }
              ,
            }
            ;
        default:
          throw new ArgumentException("Culture name " + name + " is not supported.", "name");
      }
    }
  }
}
