using System;
using System.IO;
using Craxy.Parkitect.Currencies.Utils;
using Parkitect.UI;
using UnityEngine;

namespace Craxy.Parkitect.Currencies
{
  sealed class CurrencyHandler
  {
    private const string SettingsFileName = "Currencies.json";
    private static string SettingsFilePath => FilePaths.getFolderPath(SettingsFileName);
    private Settings _settings = null;
    internal Settings Settings
    {
      get
      {
        if (_settings == null)
        {
          _settings = Settings.Load(SettingsFilePath);
        }
        return _settings;
      }
    }
    private void SaveSettings() => Settings.Save(SettingsFilePath);

    private const string FontName = "Roboto-Regular SDF";
    private const string FontBaseResourcePath = "Assets.Fonts.Roboto." + FontName;

    private ResourceFontInfo _customFontInfo = null;
    internal ResourceFontInfo CustomFontInfo
    {
      get
      {
        if(_customFontInfo == null)
        {
          _customFontInfo = ResourceFontInfo.Create(FontBaseResourcePath);
        }
        return _customFontInfo;
      }
    }

    private SettingsWindow _settingsWindow = null;
    public void OpenSettings()
    {
      _settingsWindow = new SettingsWindow(Settings, CustomFontInfo);
    }
    public void DrawSettings()
    {
      _settingsWindow.Draw();
    }
    public void CloseSettings()
    {
      OnSettingsChanged();
      _settingsWindow = null;
    }

    private bool inGame = false;
    private TMPro.TMP_FontAsset _font = null;
    private InputFieldInjector _inputFieldInjector = null;
    private void OnSettingsChanged()
    {
      SaveSettings();

      // only update while running
      if(inGame)
      {
        ApplySettings();
      }
    }
    private void ApplySettings()
    {
      //todo: check if settings changed

      // inject custom font only if needed
      var symbol = Settings.Symbol.Value;
      // var injected = CustomFontInfo.IsFallbackForDefaultGameFont();
      var injected = _font != null;
      var needsInjection = CustomFontInfo.IsInButNotInDefaultGameFont(symbol);
      if(injected && !needsInjection)
      {
        // remove font
        _font.EjectFromDefaultGameFont();
        //todo: enough to destroy just font?
        GameObject.Destroy(_font.atlas);
        GameObject.Destroy(_font.material);
        GameObject.Destroy(_font);
        _font = null;
      }
      else if(!injected && needsInjection)
      {
        // add font
        _font = CustomFontInfo.LoadFont();
        _font.InjectIntoDefaultGameFont();
      }
      else
      {
        // already in correct place
        // -> no need to inject or eject font
      }

      // change string.Format
      GameController.currencyFormat = Settings.NumberFormat;
      // change input fields
      if(_inputFieldInjector == null)
      {
        _inputFieldInjector =
          new GameObject(nameof(InputFieldInjector))
          .AddComponent<InputFieldInjector>()
          .Initialize(Settings.Symbol.DefaultValue, Settings.Symbol.Value);
      }
      else
      {
        _inputFieldInjector.UpdateSymbol(Settings.Symbol.Value);
      }

      Mod.Log($"Settings applied (Symbol: {Settings.Symbol.Value}, font was previously injected: {injected}; font is now injected: {needsInjection})");
    }
    private void UndoSettings()
    {
      if(_font != null)
      {
        _font.EjectFromDefaultGameFont();
        _font = null;
      }
      GameController.currencyFormat = Settings.DefaultNumberFormat;
      if(_inputFieldInjector != null)
      {
        GameObject.Destroy(_inputFieldInjector.gameObject);
      }

      Mod.Log($"Settings undone (Symbol: {Settings.Symbol.Value} -> {Settings.Symbol.DefaultValue})");
    }

    public void Enable()
    {
      inGame = true;
      ApplySettings();
    }
    public void Disable()
    {
      inGame = false;
      UndoSettings();
    }

    private class InputFieldInjector : MonoBehaviour
    {
      public InputFieldInjector Initialize(string defaultSymbol, string currentSymbol)
      {
        this.defaultSymbol = defaultSymbol;
        this.currentSymbol = currentSymbol;

        return this;
      }

      private string defaultSymbol, currentSymbol;
      void Start()
      {
        var initSymbol = currentSymbol;
        currentSymbol = defaultSymbol;
        UpdateSymbol(initSymbol);
      }
      void OnDestroy()
      {
        UpdateSymbol(defaultSymbol);
      }

      public void UpdateSymbol(string newSymbol)
      {
        if (currentSymbol == newSymbol)
        {
          return;
        }

        var inputs = Resources.FindObjectsOfTypeAll<UIUnitInputField>();
        foreach (var input in inputs)
        {
          if (
            input == null
            || input is UIVelocityInputField
            || input.unitText == null
            )
          {
            continue;
          }

          try
          {
            if (input.unitText.text == currentSymbol)
            {
              // Mod.Log($"Looking at {input} with unitText={input.unitText.text}");
              input.unitText.text = newSymbol;
            }
          }
          catch (Exception ex)
          {
            Mod.Log($"Exception while trying to change the symbol on input {input}: {ex.ToString()}");
          }
        }

        currentSymbol = newSymbol;
      }
    }
  }
}
