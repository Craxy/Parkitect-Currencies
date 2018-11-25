using UnityEngine;
using UnityEditor;
using TMPro;
using System.IO;
using System.Linq;

public class FontExporter
{
  sealed class FontData
  {
    public string Name;
    public TMP_FontAsset.FontAssetTypes FontAssetTypes;
    public FaceInfo FontInfo;
    public KerningTable KerningInfo;
    public TMP_Glyph[] Glyphs;
    public FontAssetCreationSettings CreationSettings;
  }

  [MenuItem("Assets/ExportSelectedFontAsJsonAndPng")]
  static void ExportSelectedFontAsJsonAndPng()
  {
    var font = Selection.activeObject as TMPro.TMP_FontAsset;
    if (!font)
    {
      Debug.Log("No TextMesh pro Font Asset selected");
    }
    else
    {
      var startFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
      var folder = EditorUtility.SaveFolderPanel("Export TextMesh Pro Font Asset as json and png to folder", startFolder, font.name);

      // export json
      {
        var fe = new FontData
        {
          Name = font.name,
          FontInfo = font.fontInfo,
          KerningInfo = font.kerningInfo,
          Glyphs = font.characterDictionary.Values.ToArray(),
          CreationSettings = font.creationSettings,
        };
        var json = JsonUtility.ToJson(fe);

        var file = $"{font.name}.json";
        var path = Path.Combine(folder, file);

        File.WriteAllText(path, json);

        Debug.Log($"TexthMesh Pro Font Asset saved under {path}");
      }

      // export name and characters
      {
        var name = font.name;
        var chars = string.Join("", font.characterDictionary.Keys.Select(c => (char)c));

        var file = $"{font.name}.txt";
        var path = Path.Combine(folder, file);

        File.WriteAllText(path, name + "\n" + chars);

        Debug.Log($"TexthMesh Pro Name and Characters saved under {path}");
      }

      // export texture
      {
        // source: TMPro_ContextMenu.ExtractAtlas

        // Create a Serialized Object of the texture to allow us to make it readable.
        var texprop = new SerializedObject(font.material.GetTexture(ShaderUtilities.ID_MainTex));
        texprop.FindProperty("m_IsReadable").boolValue = true;
        texprop.ApplyModifiedProperties();

        // Create a copy of the texture.
        Texture2D tex = GameObject.Instantiate(font.material.GetTexture(ShaderUtilities.ID_MainTex)) as Texture2D;

        // Set the texture to not readable again.
        texprop.FindProperty("m_IsReadable").boolValue = false;
        texprop.ApplyModifiedProperties();

        // Saving File for Debug
        var pngData = tex.EncodeToPNG();

        var file = $"{font.name}.png";
        var path = Path.Combine(folder, file);

        File.WriteAllBytes(path, pngData);

        GameObject.DestroyImmediate(tex);

        Debug.Log($"TexthMesh Pro Font Asset Atlas saved under {path}");
      }
    }
  }

  [MenuItem("Assets/LoadFontFromJsonAndPngWithMaterialBasedOnSelectedFont")]
  static void LoadFontFromJsonAndPng()
  {
    var sourceFont = Selection.activeObject as TMPro.TMP_FontAsset;
    if (!sourceFont)
    {
      Debug.Log("No TextMesh pro Font Asset selected");
    }
    else
    {
      var startFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
      var jsonPath = EditorUtility.OpenFilePanelWithFilters("Load TextMesh Pro Font Asset from json and png (Required atlas to be in same folder and same name)", startFolder, new[] { "json", "json" });
      if (!File.Exists(jsonPath))
      {
        Debug.LogError($"Json file {jsonPath} doesn't exist!");
      }
      var pngPath = Path.ChangeExtension(jsonPath, "png");
      if (!File.Exists(pngPath))
      {
        Debug.LogError($"Png file {pngPath} matching json file {jsonPath} doesn't exist!");
      }

      var font = ScriptableObject.CreateInstance<TMP_FontAsset>();
      {
        var json = File.ReadAllText(jsonPath);
        var fe = JsonUtility.FromJson<FontData>(json);
        font.name = fe.Name;
        font.creationSettings = fe.CreationSettings;
        font.AddFaceInfo(fe.FontInfo);
        font.AddKerningInfo(fe.KerningInfo);
        font.AddGlyphInfo(fe.Glyphs);
      }
      // Assets/Imports/Fonts must exist!
      var savePath = $"Assets/Imports/Fonts/{font.name}";
      {
        var pngData = File.ReadAllBytes(pngPath);
        var tex = new Texture2D(1, 1, TextureFormat.Alpha8, true, true);
        tex.LoadImage(pngData, true);
        font.atlas = tex;

        AssetDatabase.CreateAsset(tex, savePath + ".texture.asset");
      }
      {
        var sourceMaterial = sourceFont.material;
        var material = new Material(sourceMaterial);
        material.SetTexture(ShaderUtilities.ID_MainTex, font.atlas);
        font.material = material;

        AssetDatabase.CreateAsset(material, savePath + ".material.asset");
      }

      font.ReadFontDefinition();

      AssetDatabase.CreateAsset(font, savePath + ".asset");
      AssetDatabase.Refresh();

      Debug.Log($"Font {font.name} loaded");
    }
  }
}
