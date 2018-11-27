### 1.1.0 - *unreleased*
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

#### 1.0.1 - 2016-10-16
* Increase speed for labeling money input fields

#### 1.0.0 - 2016-10-14
* First release
