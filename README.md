Currencies
===============
Mod for [Parkitect](http://themeparkitect.com/).  
Available on the [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=1576312321).

Change the currency symbol.  


Parkitect displays money as Dollars marked by *$*. With this mod you can change the symbol to other representations like *€* or *£*.  

Besides just exchanging the symbol, you can also control where to put it to match a regional preference: While the default US-dollar style is *$123.12*, the German preference (with Euro) is *123.12 €*.  
![settings](./docs/files/img/ExampleEuro.png)

## Installation
* Steam: Just subscribe to [the mod on the Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=1576312321).
* Other:
  * Download the newest version of this mod from the [release page on GitHub](https://github.com/Craxy/Parkitect-Currencies/releases).
  * Extract the zip file into `{MyDocuments}/Parkitect/Mods/`.

This mod doesn't alter savegames. That means you can disable and remove this mod at any time without any impact on savegames, scenarios, etc.. The only effect is that the currency symbol is reverted back to its original *$* sign.

## Settings 
![Settings](./docs/files/img/SettingsMenu.png)
### Symbols
The symbol can be any sequence of characters between a length of 0 and 3. Ergo no symbol at all up to ISO 4217 codes (like EUR or USD). However it is not recommended to use a symbol longer than 1 character: Longer symbols might not fit in their fixed position and might stick out or even change the layout of a window.  

The list of available characters in game is rather limited.  
Unfortunately the settings menu uses a different font display than ingame. Therefore some symbols might be displayed in the settings, but not in the game. In that case a warning is shown:
![Missing character](./docs/files/img/SettingsMenu_Missing.png)  
If you keep a missing symbol it will show up as a box in game:  
![Missing character](./docs/files/img/Ingame_Missing.png)  

The default Parkitect font supports just the most common currency symbols like $, € and £. For more options this mod injects a couple additional currency symbols like ₽ (Ruble) or ₩ (Won). If there's a symbol missing you would like to use, best request it as a [issue on github](https://github.com/Craxy/Parkitect-Currencies/issues).

The injected symbols use a different font ([*Roboto*](https://fonts.google.com/specimen/Roboto)) than Parkitect (*Museo*). Therefore the injected symbols look slightly different in style -- but they are strange symbols, so it shouldn't be too noticeable.

### Symbol position
The Symbol can either be positioned before or after the number. Additional there might be an additional space between the currency symbol and the value. The possible representations are: € 123.12, €123.12, 123.12€, 123.12 €  
For negative values there are even more possible positions because of the negative sign: 
-€ 123.12, € -123.12, €-123.12, -€123.12, -123.12€, -123.12 €  

### Separators
Different countries use different decimal and group separators. For example: US: `12,345.67`, DE: `12.345,67`.  Parkitect uses the US style for all numbers.  
This mod can change the representation for currencies -- ONLY currencies, other types (like speed or height) aren't changed. Neither are price input fields affected.  
These limitations result in a quite quirky experience: some numbers are in one style while others use another style. And for input fields you have to remember to use the original (US) style. Because of these issues no custom separators are used unless you enable them separately.

<br></br>
**Note**: The change is purely cosmetic: $123.12 = 123.12 € = £123.12 = ¥123.12 = R 123.12 -- there's no currency conversion.

**Note**: The updated currency applies once a game is loaded. It doesn't show up in the main menu.

**Note**: If you change the currency symbol while in game (via the pause/*ESC* menu): The displays with a currency symbol only update to the new one when their text gets updated by Parkitect. That might not be immediately but after certain actions like a new month (-> statistics get updated) or not at all during the current session because the text were cached (this happens in the Finances window with past monthly reports).  
**It is therefore recommended to save your current park and reload it to force all texts to update after you changed the currency symbol!**

<br></br>

### Release notes
Listed in [RELEASE_NOTES.md](./RELEASE_NOTES.md) and on the [release page](https://github.com/Craxy/Parkitect-Currencies/releases).  
A simplified changelog can also be found [on the Change Notes page on the Steam Workshop page](https://steamcommunity.com/sharedfiles/filedetails/changelog/1576312321) for this mod.

### Issues
Please report issues via the [issue tracker on GitHub](https://github.com/Craxy/Parkitect-Currencies/issues).  
If this mod is responsible for a crash or an error/exception please include your *output_log.txt*. Copy this log file immediately after the crash -- its content is cleared for each Parkitect start.  

*output_log.txt* is Parkitects log file, located at 
* Windows: `%USERPROFILE%\AppData\LocalLow\Texel Raptor\Parkitect\output_log.txt`
* Linux: `~/.config/unity3d/Texel Raptor/Parkitect/Player.log`
* Mac: `~/Library/Logs/Unity/Player.log`

### Source code
[Craxy/Parkitect-Currencies on GitHub](https://github.com/Craxy/Parkitect-Currencies)
