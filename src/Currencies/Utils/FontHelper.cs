using System;
using System.Linq;
using UnityEngine;
using FontAsset = TMPro.TMP_FontAsset;

namespace Craxy.Parkitect.Currencies.Utils
{
  sealed class AssetBundleInfo
  {
    public AssetBundleInfo(LoadFrom type, string path)
    {
      Type = type;
      Path = path;
    }

    public readonly LoadFrom Type;
    public readonly string Path;

    public AssetBundle Load() => Type switch
    {
      LoadFrom.URL => LoadWWW(Path),
      LoadFrom.File => LoadFile(Path),// or LoadWWW("file://" + Path);
      LoadFrom.EmbeddedResource => LoadEmbeddedResource(Path),
      _ => throw new InvalidOperationException($"Invalid LoadFrom.Type: {Type}"),
    };

    private static AssetBundle LoadWWW(string path)
    {
      using var www = new WWW(path);
      return www.assetBundle;
    }
    private static AssetBundle LoadFile(string path)
    {
      return AssetBundle.LoadFromFile(path);
    }
    private static AssetBundle LoadEmbeddedResource(string path)
    {
      var assembly = System.Reflection.Assembly.GetExecutingAssembly();
      using var stream = assembly.GetManifestResourceStream(typeof(Mod), path);
      return AssetBundle.LoadFromStream(stream);
    }

    public enum LoadFrom
    {
      URL,
      File,
      EmbeddedResource,
    }
  }

  class SimpleFontInfo
  {
    public SimpleFontInfo(string name, string characters)
    {
      Name = name;
      Characters = characters;
    }
    public string Name { get; }
    public string Characters { get; }

    public override string ToString()
      => $"{nameof(SimpleFontInfo)}(Name={Name}; Characters={Characters})";
  }
  sealed class FontInfoFromAssetBundle : SimpleFontInfo
  {
    public static FontInfoFromAssetBundle Load(AssetBundleInfo assetBundle, string nameInAssetBundle)
    {
      var bundle = assetBundle.Load();
      try
      {
        var font = bundle.LoadAsset<FontAsset>(nameInAssetBundle);
        var name = font.name;
        var chars = font.characterTable.Select(c => (char)c.unicode).ToArray();
        var characters = new String(chars);

        return new FontInfoFromAssetBundle(name, characters, nameInAssetBundle);
      }
      finally
      {
        bundle.Unload(unloadAllLoadedObjects: true);
      }
    }

    public FontInfoFromAssetBundle(string name, string characters, string nameInAssetBundle)
      : base(name, characters)
    {
      NameInAssetBundle = nameInAssetBundle;
    }
    public string NameInAssetBundle { get; }

    public FontAsset LoadFrom(AssetBundle assetBundle)
      => assetBundle.LoadAsset<FontAsset>(NameInAssetBundle);

    public FontAsset LoadFrom(AssetBundleInfo assetBundleInfo)
    {
      var assetBundle = assetBundleInfo.Load();
      try
      {
        return LoadFrom(assetBundle);
      }
      finally
      {
        assetBundle.Unload(unloadAllLoadedObjects: false);
      }
    }
  }

  static class SimpleFontInfoExtensions
  {
    public static bool Contains<T>(this T info, char c) where T : SimpleFontInfo
      => info.Characters.Contains(c);
    public static bool IsFallbackFor<T>(this T info, FontAsset font) where T : SimpleFontInfo
      => font.fallbackFontAssetTable.Any(f => f.name == info.Name);
    public static bool IsFallbackForDefaultGameFont<T>(this T info) where T : SimpleFontInfo
      => info.IsFallbackFor(FontHelper.DefaultGameFont);

    public static bool IsInInfoButNotInDefaultGameFont<T>(this T info, char c) where T : SimpleFontInfo
    {
      var font = FontHelper.DefaultGameFont;
      if (font.HasCharacter(c, searchFallbacks: false))
      {
        return false;
      }
      else
      {
        foreach (var fallback in font.fallbackFontAssetTable)
        {
          if (fallback.name == info.Name)
          {
            continue;
          }
          if (fallback.HasCharacter(c, searchFallbacks: true))
          {
            return true;
          }
        }
      }

      return info.Contains(c);
    }
    public static bool IsInInfoButNotInDefaultGameFont<T>(this T info, string s) where T : SimpleFontInfo
      => s.All(info.IsInInfoButNotInDefaultGameFont);

    public static bool IsFallbackFor(this FontAsset info, FontAsset font)
      => font.fallbackFontAssetTable.Contains(info);
    public static void InjectInto(this FontAsset font, FontAsset into)
    {
      if (font.IsFallbackFor(into))
      {
        // already injected
        return;
      }
      into.fallbackFontAssetTable.Add(font);
    }
    public static void InjectIntoDefaultGameFont(this FontAsset font)
      => font.InjectInto(FontHelper.DefaultGameFont);

    public static void EjectFrom<T>(this T font, FontAsset from) where T : SimpleFontInfo
    {
      var idx = from.fallbackFontAssetTable.FindIndex(f => f.name == font.Name);
      if (idx < 0)
      {
        // not injected
        return;
      }
      from.fallbackFontAssetTable.RemoveAt(idx);
    }
    public static void EjectFromDefaultGameFont<T>(this T font) where T : SimpleFontInfo
      => font.EjectFrom(FontHelper.DefaultGameFont);

    public static void EjectFrom(this FontAsset font, FontAsset from)
      => from.fallbackFontAssetTable.Remove(font);
    public static void EjectFromDefaultGameFont(this FontAsset font)
      => font.EjectFrom(FontHelper.DefaultGameFont);
  }
  static class FontHelper
  {
    public static readonly string DefaultGameFontName = "museosans_500 SDF";
    private static FontAsset _defaultGameFont = null;
    public static FontAsset DefaultGameFont
    {
      get
      {
        if (_defaultGameFont == null)
        {
          _defaultGameFont = Resources.FindObjectsOfTypeAll<FontAsset>().First(f => f.name == DefaultGameFontName);
        }
        return _defaultGameFont;
      }
    }

    public static bool IsInFont(char c, FontAsset font, bool includeFallbacks)
      => font.HasCharacter(c, includeFallbacks);
    public static bool IsInGameFont(char c)
      => IsInFont(c, DefaultGameFont, includeFallbacks: true);
    public static bool IsInCustomFont(char c, string customFontChars)
      => customFontChars.Contains(c);
    public static bool IsInCustomFont<T>(char c, T customFont) where T : SimpleFontInfo
      => customFont.Characters.Contains(c);
    public static bool IsInGameOrCustomFont(char c, string customFontChars)
      => IsInGameFont(c) || IsInCustomFont(c, customFontChars);
    public static bool IsInGameOrCustomFont<T>(char c, T customFont) where T : SimpleFontInfo
      => IsInGameOrCustomFont(c, customFont.Characters);

  }
}
