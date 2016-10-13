open System
open System.Globalization
open System.Reflection
open System.IO

// the unity CultureInfos are quite outdated
// -> autocreate updated CultureInfos from current .net

let inQuotes str = sprintf "\"%s\"" str
let indent i str = (String.replicate i "  ") + str
let indents i strs = strs |> List.map (indent i)

let valueToString (value: obj) =
  match value with
  | :? (int array) as arr -> 
      sprintf "new int[] { %s }" (arr |> Seq.map string |> String.concat ", ")
  | :? string as str ->
      str |> inQuotes
  | :? int as i ->
      i |> string
  | :? bool as b ->
      if b then "true" else "false"
  | _ -> value |> string

let createNumberFormatInfo (nf: NumberFormatInfo) =
  // of course the unity NUmberFormatInfo is special:
  // there are some properties missing
  // for example DigitSubstitution:
  //        There's a private field digitSubstitution, that's nowhere used
  //        and of course no public DigitSubstitution property
  let inUnity = [
      "CurrencyDecimalDigits"
      "CurrencyDecimalSeparator"
      "CurrencyGroupSeparator"
      "CurrencyGroupSizes"
      "CurrencyNegativePattern"
      "CurrencyPositivePattern"
      "CurrencySymbol"
      //"IsReadOnly" IsReadOnly is readonly (set in ctor)
      "NaNSymbol"
      "NegativeInfinitySymbol"
      "NegativeSign"
      "NumberDecimalDigits"
      "NumberDecimalSeparator"
      "NumberGroupSeparator"
      "NumberGroupSizes"
      "NumberNegativePattern"
      "PercentDecimalDigits"
      "PercentDecimalSeparator"
      "PercentGroupSeparator"
      "PercentGroupSizes"
      "PercentNegativePattern"
      "PercentPositivePattern"
      "PercentSymbol"
      "PerMilleSymbol"
      "PositiveInfinitySymbol"
      "PositiveSign"
    ]

  [
    yield sprintf "new NumberFormatInfo"
    yield sprintf "{"
    yield! typeof<NumberFormatInfo>.GetProperties(BindingFlags.Instance ||| BindingFlags.Public)
           |> Seq.filter (fun p -> inUnity |> List.contains (p.Name))
           |> Seq.map (fun p -> sprintf "  %s = %s," p.Name (p.GetValue(nf) |> valueToString))
    yield sprintf "}"
  ]

let createCustomRegionInfo (culture: CultureInfo) =
  let usedProperties = [
    "TwoLetterISORegionName"
    "ThreeLetterISORegionName"
    "Name"
    "DisplayName"
    "EnglishName"
    "NativeName"
    "IsMetric"
    "GeoId"
    "CurrencyEnglishName"
    "CurrencyNativeName"
    "CurrencySymbol"
    "ISOCurrencySymbol"
  ]

  try
    let region = RegionInfo(culture.LCID)
    [
      yield sprintf "new CustomRegionInfo"
      yield sprintf "{"
      yield! typeof<RegionInfo>.GetProperties(BindingFlags.Instance ||| BindingFlags.Public)
              |> Seq.filter (fun p -> usedProperties |> List.contains (p.Name))
              |> Seq.map (fun p -> sprintf "  %s = %s," p.Name (p.GetValue(region) |> valueToString))
      yield sprintf "}"
    ]
  with
  | ex ->
      ["null"]

let createCustomCultureInfo (culture: CultureInfo) =
  [
    yield sprintf "new CustomCultureInfo"
    yield sprintf "{"
    yield sprintf "  LCID = %i," culture.LCID
    yield sprintf "  IetfLanguageTag = %s," (culture.IetfLanguageTag |> inQuotes)
    yield sprintf "  TwoLetterISOLanguageName = %s," (culture.TwoLetterISOLanguageName |> inQuotes)
    yield sprintf "  ThreeLetterISOLanguageName = %s," (culture.ThreeLetterISOLanguageName |> inQuotes)
    yield sprintf "  IsNeutralCulture = %b," culture.IsNeutralCulture
    yield sprintf "  Name = %s," (culture.Name |> inQuotes)
    yield sprintf "  DisplayName = %s," (culture.DisplayName |> inQuotes)
    yield sprintf "  EnglishName = %s," (culture.EnglishName |> inQuotes)
    yield sprintf "  NativeName = %s," (culture.NativeName |> inQuotes)
    yield sprintf "  NumberFormat ="
    yield! createNumberFormatInfo culture.NumberFormat |> indents 2
    yield sprintf "  ,"
    yield sprintf "  RegionInfo ="
    yield! createCustomRegionInfo culture |> indents 2
    yield sprintf "  ,"
    yield sprintf "}"
  ]

let createGetCultureInfoMethod methodName (cultures: CultureInfo seq) =
  [
    yield sprintf "public static CustomCultureInfo %s(string name)" methodName
    yield sprintf "{"
    yield sprintf "  switch(name)"
    yield sprintf "  {"
    for c in cultures do
      yield sprintf "    case \"%s\":" c.IetfLanguageTag
      yield sprintf "      return"
      yield! c |> createCustomCultureInfo |> indents 4
      yield sprintf "        ;"
    yield sprintf "    default:"
    yield sprintf "      throw new ArgumentException(\"Culture name \" + name + \" is not supported.\", \"name\");"
    yield sprintf "  }"
    yield sprintf "}"
  ]

let createPartialClass className (content: string list) =
  [
    yield sprintf "public static partial class %s" className
    yield sprintf "{"
    yield! content |> indents 1
    yield sprintf "}"
  ]
let createUsingsAndNamespace (usings: string list) namespaceName (content: string list) =
  [
    for using in usings do
      yield sprintf "using %s;" using
    yield sprintf ""
    yield sprintf "namespace %s" namespaceName
    yield sprintf "{"
    yield! content |> indents 1
    yield sprintf "}"
  ]

let toFile path (lines: string seq) =
  System.IO.File.WriteAllLines(path, lines)

// CultureInfo.GetCultures(CultureTypes.AllCultures)
[
  CultureInfo.GetCultureInfo("en-US")
  CultureInfo.GetCultureInfo("en-GB")
  CultureInfo.GetCultureInfo("de-DE")
  CultureInfo.GetCultureInfo("fr-FR")
  CultureInfo.GetCultureInfo("es-ES")
  CultureInfo.GetCultureInfo("ru-RU")
  CultureInfo.GetCultureInfo("zh-CN")
  CultureInfo.GetCultureInfo("ja-JP")
  CultureInfo.GetCultureInfo("tr-TR")
  CultureInfo.GetCultureInfo("hi-IN")
  CultureInfo.GetCultureInfo("pt-BR")
]
|> createGetCultureInfoMethod "GetCulture"
|> createPartialClass "CultureInfoHelper"
|> createUsingsAndNamespace ["System"; "System.Globalization"] "Craxy.Parkitect.Currencies.Utils"
|> toFile (Path.Combine(__SOURCE_DIRECTORY__, "./CultureInfoHelper.generated.cs"))
