using System;
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

    private const string FontInAssetBundle = "Roboto-Regular SDF";

    private AssetBundleInfo _fontAssetBundleInfo = null;
    internal AssetBundleInfo FontAssetBundleInfo
    {
      get
      {
        if(_fontAssetBundleInfo == null)
        {
          _fontAssetBundleInfo = new AssetBundleInfo(AssetBundleInfo.LoadFrom.EmbeddedResource, "Assets.currencies");
        }
        return _fontAssetBundleInfo;
      }
    }

    private FontInfoFromAssetBundle _customFontInfo = null;
    internal FontInfoFromAssetBundle CustomFontInfo
    {
      get
      {
        if(_customFontInfo == null)
        {
          _customFontInfo = FontInfoFromAssetBundle.Load(FontAssetBundleInfo, FontInAssetBundle);
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
      var needsInjection = CustomFontInfo.IsInInfoButNotInDefaultGameFont(symbol);
      if(injected && !needsInjection)
      {
        // remove font
        _font.EjectFromDefaultGameFont();
        GameObject.Destroy(_font);
        _font = null;
      }
      else if(!injected && needsInjection)
      {
        // add font
        _font = CustomFontInfo.LoadFrom(FontAssetBundleInfo);
        _font.InjectIntoDefaultGameFont();
      }
      else
      {
        // already in correct place
        // -> no need to inject or eject font
      }

      // change string.Format
      GameController.currencyFormat = Settings.ActiveNumberFormat;
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
