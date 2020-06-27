### 1.2.1 - 2020-06-27
Tested with Parkitect *1.5i*
* Fix: Custom font doesn't work with current TMPro version
* Update: Use AssetBundle instead of extra files to import font

### 1.2.0 - 2018-12-29
Tested with Parkitect *1.2a*
* Compile with new Parkitect version
* Add decimal & group separator (only readonly currencies, neither input fields nor other numbers like speed or height)

### 1.1.1 - 2018-11-29
Tested with Parkitect *1.0*
* Compile with new Parkitect version
* Copy preview.png after build

### 1.1.0 - 2018-11-28
Tested with Parkitect *Beta 12*
* Fix [#1](https://github.com/Craxy/Parkitect-Currencies/pull/1)
* Remove Parkitect Nexus dependency and use Parkitect directly
* Update to .Net 4.5
  * Use .Net CultureInfo instead of custom one
* Inject a couple of currency symbols if necessary (using [Robot](https://fonts.google.com/specimen/Roboto) font -- Parkitect uses [Museo](https://www.exljbris.com/museo.html))
* Simplify symbol replacement
  * Change symbols in Resources instead of listening to newly opened windows
  * Remove Update method in MonoBehaviour
* Code Cleanup
* Update build system
  * Use new csproj file with metadata
  * Add post compile action to autocopy mod and start game
* Upload to [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=1576312321)

#### 1.0.1 - 2016-10-16
* Increase speed for labeling money input fields

#### 1.0.0 - 2016-10-14
* First release
