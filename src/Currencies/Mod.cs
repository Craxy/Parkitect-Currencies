using System;
using UnityEngine;

namespace Craxy.Parkitect.Currencies
{
  public class Mod : IMod, IModSettings
  {
    public String Name
    {
      get
      {
        return "Currencies";
      }
    }
    public String Description
    {
      get
      {
        return "Change the currency symbol.";
      }
    }

    public String Identifier { get; set; }
    public String Path { get; set; }

    private Settings _settings;
    internal Settings Settings
    {
      get
      {
        if (_settings == null)
        {
          //todo: load from file
          _settings = new Settings();
          _settings.Symbol.Value = "€";
          _settings.NegativePattern.Value = 8;
          _settings.PositivePattern.Value = 3;
        }
        return _settings;
      }
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

      Debug.Log("[Currencies] enabled");
    }
    public void onDisabled()
    {
      // reset currency format to default
      GameController.currencyFormat = Settings.DefaultNumberFormat;

      // delete money labeler
      GameObject.Destroy(_go);
      _go = null;

      Debug.Log("[Currencies] disabled");
    }

    public void onSettingsOpened()
    {
      //todo: implement
    }
    public void onSettingsClosed()
    {
      //todo: implement
    }
    public void onDrawSettingsUI()
    {
      //todo: implement
    }
  }
}
