using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Craxy.Parkitect.Currencies.Utils;
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

      T GetAssemblyAttribute<T>() where T : Attribute => (T)assembly.GetCustomAttribute(typeof(T));

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

      _fontInjector.Rent();
    }
    public void onDisabled()
    {
      // reset currency format to default
      GameController.currencyFormat = Settings.DefaultNumberFormat;

      // delete money labeler
      GameObject.Destroy(_go);
      _go = null;

      _fontInjector.Return();
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

#if LogAllCurrencySymbolsNotInDefaultFont
    private void LogAllCurrencySymbolsNotInDefaultFont()
    {
      var font = FontInjector.GetDefaultGameFont();
      var cultures = FontInjector.GetAllCurrencySymbols();
      // source: https://en.wikipedia.org/wiki/Currency_symbol, https://en.wikipedia.org/wiki/Currency_Symbols_(Unicode_block)
      var wiki =
        "¤؋Ar฿B/.BrBs.Bs.F.GH₵¢cCh.₡C$Dденدج.د.بد.عJDد.كل.دдинد.تد.م.د.إDb$₫Esc€ƒFtFBuFCFACFAFrFRwGgr₲h₴₭KčkrknKzKლLLeлв.ElpMKMMT₥Nfk₦Nu.UMT$MOP$₱pt£LLLSPQqRR$ر.ع.ر.قر.سر.ي៛RMp₽Rf.₹₨SReRp₪TshKshSh.So.UShS/SDR৳WS$₮VT₩¥zł¢HK$元圓NT$元圓₫֏₣₭N ₾₺₼КМ圓元₱₤ج.م.﷼  ریال  ₽₹₸円元¥￥௹૱ರරු꠸₳₢ Cr$₰DMDM₻₯₠ƒFrKčs₤LmLsLtMℳMDNmkPF₧ℛℳSk₷₶৲৹৻₠₡₢₣₤₥₦₧₨₩₪₫€₭₮₯₰₱₲₳₴₵₶₷₸₹₺₻₼₽₾₿";
      var cs =
        cultures.Concat(wiki)
        .Distinct()
        .Where(c => !Char.IsWhiteSpace(c) && !font.HasCharacter(c, true));
      var ss = String.Join("", cs);

      var fallbacks = String.Join(", ", font.fallbackFontAssets.Select(f => f.name));
      Log($"Currency symbols not in {font.name} (incl. fallbacks {fallbacks}) (count={ss.Length}): {ss}");
      // ¤رس‏лв₪￥₩ł₽₺₴یال₫֏ден₾₹₸сомটা៛₭රුብርरु؋₱₦₼ʻ₮नेूدعў৳جم₡تКМيأكإبقДи฿₵ƒ₲ლ₥₨元圓₣₤﷼円௹૱ರ꠸₳₢₰₻₯₠ℳ₧ℛ₷₶৲৹৻₿
      //issue: sometimes a currency has multiple symbols -- and one is used in roboto (font) and the other in CultureInfo
      //         for example yen has U+00A5 ¥ YEN SIGN, U+FFE5 ￥ FULLWIDTH YEN SIGN
      //          roboto uses YEN SIGN, but CultureInfo returns FULLWIDTH YEN SIGN
    }
#endif
    private SettingsWindow _settingsWindow = null;
    public void onSettingsOpened()
    {
#if LogAllCurrencySymbolsNotInDefaultFont
      LogAllCurrencySymbolsNotInDefaultFont();
#endif
      _fontInjector.Rent();
      _settingsWindow = new SettingsWindow(Settings);
    }
    public void onSettingsClosed()
    {
      SaveSettings();

      _settingsWindow = null;
      _fontInjector.Return();
    }
    public void onDrawSettingsUI()
    {
      _settingsWindow.Draw();
    }

    /// <summary>
    /// FontInjector is needed in Game and in Settings.
    /// Settings can be accessed in Game -> can't create for each extra but only one in total
    /// </summary>
    private readonly StackSingleton<FontInjector> _fontInjector = new StackSingleton<FontInjector>(
        create: () => {
          var fi = new FontInjector();
          fi.Inject();
          return fi;
        },
        dispose: fontInjector => {
          fontInjector.Eject();
          fontInjector.Dispose();
        }
      );

    public static void Log(string msg)
    {
      Debug.Log("[" + name + "] " + msg);
    }
  }
}
