Currencies
===============

Enables you to change the currency.  
Parkitect displays money as Dollars marked by *$*. With this mod you can change the symbol to other representations like *€* or *£*.  

Besides just exchanging the symbol, you can also control where to put it to match a regional preference: While the default US-dollar style is *$123.12*, the German preference (with Euro) is *123.12 €* [*](#star1).  

<br/>
<br/>
### settings
##### symbols
The symbol can be any sequence of characters between a length of 0 and 3. Ergo no symbol at all up ISO 4217 codes (like EUR or USD). However it is not recommended to use a symbol longer than 1 character: Longer symbols might not fit in their fixed position and might stick out or even change the layout of a window.

#### symbol position
The Symbol can either be positioned before or after the number. Additional there might be an additional space between the currency symbol and the value. The possible representations are: € 123.12, €123.12, 123.12€, 123.12 €  
For negative values there are even more possible positions because of the negative sign: 
-€ 123.12, € -123.12, €-123.12, -€123.12, -123.12€, -123.12 €  

<br/>
**Note**: The change is solely cosmetic: $123.12 = 123.12 € = £123.12 = ¥123.12 = R 123.12 -- there's no currency conversion.
**Note**: The updated currency applies once a game is loaded. It doesn't show up in the main menu.  

<br/>
<br/>
<a name="star1"></a>*: There are more differences between the US and DE currency presentation like the decimal separator (*123.12* vs *123,12*). This is reflected in this mod: The US version is always used. Although it could easily be changed for most money displays, there are locations where this isn't possible (in money input fields like changing the fee of a ride). Additionally all other (not currency) number displays would still be in US style.  