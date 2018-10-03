using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Craxy.Parkitect.Currencies.Utils
{
  // the unity CultureInfos are quite outdated
  // creating own culture isn't trivial
  // -> custom class autocreated from current .net with current values
  public class CustomRegionInfo
  {
    public string Name { get; set; }
    public string EnglishName { get; set; }
    public string DisplayName { get; set; }
    public string NativeName { get; set; }
    public string TwoLetterISORegionName { get; set; }
    public string ThreeLetterISORegionName { get; set; }
    public string CurrencyEnglishName { get; set; }
    public string CurrencyNativeName { get; set; }
    public string CurrencySymbol { get; set; }
    public string ISOCurrencySymbol { get; set; }
    public bool IsMetric { get; internal set; }
    public int GeoId { get; internal set; }
  }
  public class CustomCultureInfo
  {
    public int LCID { get; set; }
    public string IetfLanguageTag { get; set; }
    public string TwoLetterISOLanguageName { get; set; }
    public string ThreeLetterISOLanguageName { get; set; }
    public bool IsNeutralCulture { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string EnglishName { get; set; }
    public string NativeName { get; set; }
    public NumberFormatInfo NumberFormat { get; set; }

    public CustomRegionInfo RegionInfo { get; set; }
  }

  public static partial class CultureInfoHelper
  {

  }
}
