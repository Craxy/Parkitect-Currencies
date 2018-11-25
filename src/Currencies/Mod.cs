using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Craxy.Parkitect.Currencies.Utils;
using UnityEngine;

namespace Craxy.Parkitect.Currencies
{
  public sealed class Mod : IMod, IModSettings
  {
    public string Name => name;
    public string Description => description;
    public string Identifier => identifier;

    private static readonly string name, description, identifier;

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

    public static void Log(string msg)
    {
      Debug.Log("[" + name + "] " + msg);
    }

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

    private readonly CurrencyHandler _currencyHandler = new CurrencyHandler();
    public void onEnabled() => _currencyHandler.Enable();
    public void onDisabled() => _currencyHandler.Disable();

    public void onSettingsOpened()
    {
#if LogAllCurrencySymbolsNotInDefaultFont
      LogAllCurrencySymbolsNotInDefaultFont();
#endif

      _currencyHandler.OpenSettings();
    }
    public void onDrawSettingsUI() => _currencyHandler.DrawSettings();
    public void onSettingsClosed() => _currencyHandler.CloseSettings();



  }
}
