using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Craxy.Parkitect.Currencies
{
  public class Mod : IMod, IModSettings
  {
    public string Name => name;
    public string Description => description;
    public string Identifier => identifier;

    private static string name, description, identifier;

    static Mod()
    {
      var assembly = Assembly.GetExecutingAssembly();

      var meta =
          assembly.GetCustomAttributes(typeof(AssemblyMetadataAttribute), false)
          .Cast<AssemblyMetadataAttribute>()
          .Single(a => a.Key == "Identifier");
      identifier = meta.Value;

      T GetAssemblyAttribute<T>() where T : System.Attribute => (T)assembly.GetCustomAttribute(typeof(T));

      name = GetAssemblyAttribute<AssemblyTitleAttribute>().Title;
      description = GetAssemblyAttribute<AssemblyDescriptionAttribute>().Description;
    }

    private GameObject _go;
    public void onEnabled()
    {
      // change currency format to own one
      GameController.currencyFormat = Settings.NumberFormat;

      // money labeler:
      //   tracks windows for input fields with currencies
      //   unfortunately UIInputFields don't use the currency from GameControl.currencyFormat
      _go = new GameObject();
      _go.AddComponent<MoneyLabeler>().Settings = Settings;
    }
    public void onDisabled()
    {
      // reset currency format to default
      GameController.currencyFormat = Settings.DefaultNumberFormat;

      // delete money labeler
      GameObject.Destroy(_go);
      _go = null;
    }

    #region Settings
    private Settings _settings;
    internal Settings Settings
    {
      get
      {
        if (_settings == null)
        {
          _settings = InitializeSettings();
        }
        return _settings;
      }
    }

    #region Load/Save
    private const string FileName = "Currencies.json";
    private string FilePath
    {
      get
      {
        return FilePaths.getFolderPath(FileName);
      }
    }

    private Settings InitializeSettings()
    {
      var settings = new Settings();

      if (File.Exists(FilePath))
      {
        Log(String.Format("Loading settings from \"{0}\"", FilePath));
        //try to load
        try
        {
          var json = File.ReadAllText(FilePath);
          var errors = settings.LoadFromJson(json);

          if (errors.Length > 0)
          {
            Log("Error(s) loading settings: " + String.Join("; ", errors));
          }
          else
          {
            Log(String.Format("Settings loaded from \"{0}\"", FilePath));
          }
        }
        catch (Exception ex)
        {
          Log("Exception while loading settings: " + ex.Message);
        }
      }

      return settings;
    }
    private void SaveSettings()
    {
      try
      {
        Log(String.Format("Saving settings to \"{0}\"", FilePath));
        var json = Settings.SaveToJson();
        File.WriteAllText(FilePath, json);
        Log(String.Format("Settings saved to \"{0}\"", FilePath));
      }
      catch (Exception ex)
      {
        Log("Exception while saving settings: " + ex.Message);
      }
    }
    #endregion

    #endregion


    private SettingsWindow _settingsWindow = null;
    public void onSettingsOpened()
    {
      _settingsWindow = new SettingsWindow(Settings);
    }
    public void onSettingsClosed()
    {
      SaveSettings();

      _settingsWindow = null;
    }
    public void onDrawSettingsUI()
    {
      _settingsWindow.Draw();
    }


    public void Log(string msg)
    {
      Debug.Log("[" + Name + "] " + msg);
    }
  }
}
