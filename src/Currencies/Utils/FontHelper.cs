using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using FontAsset = TMPro.TMP_FontAsset;

namespace Craxy.Parkitect.Currencies.Utils
{
  sealed class FontData
  {
    // fields are set by JsonUtility.FromJson and not set manually
    // -> "warning CS0649: Field '...' is never assigned to, and will always have its default value null"
#pragma warning disable 0649
    public string Name;
    public TMP_FontAsset.FontAssetTypes FontAssetTypes;
    public FaceInfo FontInfo;
    public KerningTable KerningInfo;
    public TMP_Glyph[] Glyphs;
    public FontCreationSetting CreationSettings;
#pragma warning restore 0649
  }

  class SimpleFontInfo
  {
    public SimpleFontInfo(string name, string characters)
    {
      Name = name;
      Characters = characters;
    }
    public readonly string Name;
    public readonly string Characters;

    public override string ToString()
      => $"{nameof(SimpleFontInfo)}(Name={Name}; Characters={Characters})";
  }
  sealed class ResourceFontInfo : SimpleFontInfo
  {
    private ResourceFontInfo(string resourcePath, string name, string characters)
      : base(name, characters)
    {
      ResourcePath = resourcePath;
    }

    public static ResourceFontInfo Create(string resourcePath)
    {
      var infoPath = GetFontInfoResourcePath(resourcePath);
      var info = ResourceHelper.LoadString(infoPath).Split('\n');

      return new ResourceFontInfo(resourcePath, info[0], info[1]);
    }

    public readonly string ResourcePath;
    public override string ToString()
      => $"{nameof(ResourceFontInfo)}(Name={Name}; Characters={Characters}; ResourcePath={ResourcePath})";

    private static string CreateResourcePath(string resourcePath, string fileType)
      => $"{resourcePath}.{fileType}";
    private static string GetFontInfoResourcePath(string resourcePath)
      => CreateResourcePath(resourcePath, "txt");
    private string GetResourcePath(string fileType) => CreateResourcePath(ResourcePath, fileType);
    private string FontInfoResourcePath => GetFontInfoResourcePath(ResourcePath);
    private string FontDataResourcePath => GetResourcePath("json");
    private string FontAtlasResourcePath => GetResourcePath("png");

    public FontAsset LoadFont()
    {
      var assembly = System.Reflection.Assembly.GetExecutingAssembly();
      FontData LoadFontData()
      {
        using (var aStream = assembly.GetManifestResourceStream(typeof(Mod), FontDataResourcePath))
        using (var stream = new StreamReader(aStream))
        {
          var json = stream.ReadToEnd();
          return JsonUtility.FromJson<FontData>(json);
        }
      }
      Texture2D LoadAtlas()
      {
        using (var stream = assembly.GetManifestResourceStream(typeof(Mod), FontAtlasResourcePath))
        {
          var buf = new byte[stream.Length];
          stream.Read(buf, 0, buf.Length);

          var atlas = new Texture2D(1, 1, TextureFormat.Alpha8, false, true);
          atlas.LoadImage(buf, true);

          return atlas;
        }
      }

      var data = LoadFontData();
      var font = ScriptableObject.CreateInstance<FontAsset>();
      font.name = data.Name;
      font.fontCreationSettings = data.CreationSettings;
      font.AddFaceInfo(data.FontInfo);
      font.AddKerningInfo(data.KerningInfo);
      font.AddGlyphInfo(data.Glyphs);
      font.atlas = LoadAtlas();

      // use material from default font
      var sourceFont = FontHelper.DefaultGameFont;
      var material = new Material(sourceFont.material);
      material.SetTexture(ShaderUtilities.ID_MainTex, font.atlas);
      font.material = material;

      font.ReadFontDefinition();

      return font;
    }
  }

  static class SimpleFontInfoExtensions
  {
    public static bool Contains(this SimpleFontInfo info, char c)
      => info.Characters.Contains(c);
    public static bool IsFallbackFor(this SimpleFontInfo info, FontAsset font)
      => font.fallbackFontAssets.Any(f => f.name == info.Name);
    public static bool IsFallbackForDefaultGameFont(this SimpleFontInfo info)
      => info.IsFallbackFor(FontHelper.DefaultGameFont);

    public static bool IsInButNotInDefaultGameFont(this SimpleFontInfo info, char c)
    {
      var font = FontHelper.DefaultGameFont;
      if (font.HasCharacter(c, searchFallbacks: false))
      {
        return false;
      }
      else
      {
        foreach (var fallback in font.fallbackFontAssets)
        {
          if (fallback.name == info.Name)
          {
            continue;
          }

          if (fallback.HasCharacter(c, searchFallbacks: true))
          {
            return false;
          }
        }

        return info.Contains(c);
      }
    }

    public static bool IsInButNotInDefaultGameFont(this SimpleFontInfo info, string s)
      => s.All(info.IsInButNotInDefaultGameFont);

    public static bool IsFallbackFor(this FontAsset info, FontAsset font)
      => font.fallbackFontAssets.Contains(info);

    public static void InjectInto(this FontAsset font, FontAsset into)
    {
      if(font.IsFallbackFor(into))
      {
        // already injected
        return;
      }
      into.fallbackFontAssets.Add(font);
    }
    public static void InjectIntoDefaultGameFont(this FontAsset font)
      => font.InjectInto(FontHelper.DefaultGameFont);
    public static void EjectFrom(this SimpleFontInfo font, FontAsset from)
    {
      var idx = from.fallbackFontAssets.FindIndex(f => f.name == font.Name);
      if(idx < 0)
      {
        // not injected
        return;
      }
      from.fallbackFontAssets.RemoveAt(idx);
    }
    public static void EjectFromDefaultGameFont(this SimpleFontInfo font)
      => font.EjectFrom(FontHelper.DefaultGameFont);
    public static void EjectFrom(this FontAsset font, FontAsset from)
      => from.fallbackFontAssets.Remove(font);
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
    public static bool IsInCustomFont(char c, SimpleFontInfo customFont)
      => customFont.Characters.Contains(c);
    public static bool IsInGameOrCustomFont(char c, string customFontChars)
      => IsInGameFont(c) || IsInCustomFont(c, customFontChars);
    public static bool IsInGameOrCustomFont(char c, SimpleFontInfo customFont)
      => IsInGameOrCustomFont(c, customFont.Characters);
  }
}
