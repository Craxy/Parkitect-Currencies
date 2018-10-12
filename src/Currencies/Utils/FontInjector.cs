using TMPro;
using FontAsset = TMPro.TMP_FontAsset;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using System.Globalization;
using System.Collections.Generic;

namespace Craxy.Parkitect.Currencies.Utils
{
  class FontInjector : IDisposable
  {
    class FontData
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
    #region Editor
    // Requires UnityEditor.dll
    // [MenuItem("Assets/ExportFontAsJsonAndPng")]
    // static void ExportFontAsJsonAndPng()
    // {
    //   var font = Selection.activeObject as TMPro.TMP_FontAsset;
    //   if (!font)
    //   {
    //     Debug.Log("No TextMesh pro Font Asset selected");
    //   }
    //   else
    //   {
    //     var startFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
    //     var folder = EditorUtility.SaveFolderPanel("Export TextMesh Pro Font Asset as json and png to folder", startFolder, font.name);

    //     // export json
    //     {
    //       var fe = new FontData
    //       {
    //         Name = font.name,
    //         FontInfo = font.fontInfo,
    //         KerningInfo = font.kerningInfo,
    //         Glyphs = font.characterDictionary.Values.ToArray(),
    //         CreationSettings = font.creationSettings,
    //       };
    //       var json = JsonUtility.ToJson(fe);

    //       var file = $"{font.name}.json";
    //       var path = Path.Combine(folder, file);

    //       File.WriteAllText(path, json);

    //       Debug.Log($"TexthMesh Pro Font Asset saved under {path}");
    //     }

    //     // export texture
    //     {
    //       // source: TMPro_ContextMenu.ExtractAtlas

    //       // Create a Serialized Object of the texture to allow us to make it readable.
    //       var texprop = new SerializedObject(font.material.GetTexture(ShaderUtilities.ID_MainTex));
    //       texprop.FindProperty("m_IsReadable").boolValue = true;
    //       texprop.ApplyModifiedProperties();

    //       // Create a copy of the texture.
    //       Texture2D tex = GameObject.Instantiate(font.material.GetTexture(ShaderUtilities.ID_MainTex)) as Texture2D;

    //       // Set the texture to not readable again.
    //       texprop.FindProperty("m_IsReadable").boolValue = false;
    //       texprop.ApplyModifiedProperties();

    //       // Saving File for Debug
    //       var pngData = tex.EncodeToPNG();

    //       var file = $"{font.name}.png";
    //       var path = Path.Combine(folder, file);

    //       File.WriteAllBytes(path, pngData);

    //       GameObject.DestroyImmediate(tex);

    //       Debug.Log($"TexthMesh Pro Font Asset Atlas saved under {path}");
    //     }
    //   }
    // }
    // [MenuItem("Assets/LoadFontFromJsonAndPng")]
    // static void LoadFontFromJsonAndPng()
    // {
    //   var sourceFont = Selection.activeObject as TMPro.TMP_FontAsset;
    //   if (!sourceFont)
    //   {
    //     Debug.Log("No TextMesh pro Font Asset selected");
    //   }
    //   else
    //   {
    //     var startFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
    //     var jsonPath = EditorUtility.OpenFilePanelWithFilters("Load TextMesh Pro Font Asset from json and png (Required atlas to be in same folder and same name)", startFolder, new[] { "json", "json" });
    //     if (!File.Exists(jsonPath))
    //     {
    //       Debug.LogError($"Json file {jsonPath} doesn't exist!");
    //     }
    //     var pngPath = Path.ChangeExtension(jsonPath, "png");
    //     if (!File.Exists(pngPath))
    //     {
    //       Debug.LogError($"Png file {pngPath} matching json file {jsonPath} doesn't exist!");
    //     }

    //     var font = ScriptableObject.CreateInstance<TMP_FontAsset>();
    //     {
    //       var json = File.ReadAllText(jsonPath);
    //       var fe = JsonUtility.FromJson<FontData>(json);
    //       font.name = fe.Name;
    //       font.creationSettings = fe.CreationSettings;
    //       font.AddFaceInfo(fe.FontInfo);
    //       font.AddKerningInfo(fe.KerningInfo);
    //       font.AddGlyphInfo(fe.Glyphs);
    //     }
    //     // Assets/Imports/Fonts must exist!
    //     var savePath = $"Assets/Imports/Fonts/{font.name}";
    //     {
    //       var pngData = File.ReadAllBytes(pngPath);
    //       var tex = new Texture2D(1, 1, TextureFormat.Alpha8, true, true);
    //       tex.LoadImage(pngData, true);
    //       font.atlas = tex;

    //       AssetDatabase.CreateAsset(tex, savePath + ".texture.asset");
    //     }
    //     {
    //       var sourceMaterial = sourceFont.material;
    //       var material = new Material(sourceMaterial);
    //       material.SetTexture(ShaderUtilities.ID_MainTex, font.atlas);
    //       font.material = material;

    //       AssetDatabase.CreateAsset(material, savePath + ".material.asset");
    //     }

    //     font.ReadFontDefinition();

    //     AssetDatabase.CreateAsset(font, savePath + ".asset");
    //     AssetDatabase.Refresh();

    //     Debug.Log($"Font {font.name} loaded");
    //   }
    // }
    #endregion Editor

    #region Determine Symbols
    internal static char[] GetAllCurrencySymbols()
    {
      return
        CultureInfo
        .GetCultures(CultureTypes.AllCultures)
        .SelectMany(c => c.NumberFormat.CurrencySymbol)
        .Distinct()
        .ToArray();
    }
    internal static char[] GetAllCurrencySymbolsNotIn(FontAsset font, bool searchFallbacks)
    {
      return
        GetAllCurrencySymbols()
        .Where(c => !font.HasCharacter(c, searchFallbacks))
        .ToArray();
    }
    #endregion Determine Symbols

    public static readonly string DefaultGameFontName = "museosans_500 SDF";
    private static FontAsset _defaultGameFont = null;
    public static FontAsset GetDefaultGameFont()
    {
      if(_defaultGameFont == null)
      {
        _defaultGameFont = Resources.FindObjectsOfTypeAll<FontAsset>().FirstOrDefault(f => f.name == DefaultGameFontName);
      }
      return _defaultGameFont;
    }

    private static readonly string FontDataResourcePath = "Assets.Fonts.Roboto.Roboto-Regular SDF.json";
    private static readonly string AtlasResourcePath = "Assets.Fonts.Roboto.Roboto-Regular SDF.png";

    private FontAsset _font = null;
    private IList<FontAsset> _injects = new List<FontAsset>();
    private void LoadFont()
    {
      if (_font != null)
      {
        return;
      }

      var assembly = System.Reflection.Assembly.GetExecutingAssembly();
      FontData LoadFontData()
      {
        using(var aStream = assembly.GetManifestResourceStream(typeof(Mod), FontDataResourcePath))
        using(var stream = new StreamReader(aStream))
        {
          var json = stream.ReadToEnd();
          return JsonUtility.FromJson<FontData>(json);
        }
      }
      Texture2D LoadAtlas()
      {
        using(var stream = assembly.GetManifestResourceStream(typeof(Mod), AtlasResourcePath))
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
      var sourceFont = GetDefaultGameFont();
      var material = new Material(sourceFont.material);
      material.SetTexture(ShaderUtilities.ID_MainTex, font.atlas);
      font.material = material;

      font.ReadFontDefinition();

      _font = font;

      Mod.Log($"Font {font.name} loaded");
    }
    public void AssertFontLoaded()
    {
      if (_font == null)
      {
        LoadFont();
      }
    }

    public void Inject() => Inject(GetDefaultGameFont());
    public void Inject(FontAsset root)
    {
      AssertFontLoaded();

      var font = _font;
      if (root.fallbackFontAssets.Contains(font))
      {
        return;
      }
      root.fallbackFontAssets.Add(font);
      _injects.Add(root);
      Mod.Log($"Font {font.name} injected as fallback into {root.name}");
    }
    public void Eject() => Eject(GetDefaultGameFont());
    public void Eject(FontAsset root)
    {
      if (_font == null)
      {
        return;
      }

      var font = _font;
      root.fallbackFontAssets.Remove(font);
      _injects.Remove(root);
      Mod.Log($"Font {font.name} removed as fallback from {root.name}");
    }

    public void Dispose()
    {
      if (_font == null)
      {
        return;
      }

      var font = _font;

      foreach (var inject in _injects)
      {
        Eject(inject);
      }
      _injects.Clear();

      GameObject.Destroy(font.material);
      GameObject.Destroy(font.atlas);
      GameObject.Destroy(font);

      _font = null;
    }
  }
}
