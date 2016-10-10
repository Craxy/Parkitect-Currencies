using System.Linq;
using System.Reflection;
using Parkitect.UI;
using UnityEngine;

namespace Craxy.Parkitect.Currencies
{
  internal class MoneyLabeler : MonoBehaviour
  {
    public Settings Settings { get; set; }

    private void OnDisable()
    {
      _count = 0;
      _lastForemostWindow = null;
    }

    private void Update()
    {
      TrackNewWindow();
    }


    //reflection is expensive -> limit reflection
    // 3 is hardly noticable but reduces the reflection calls
    private int _updateEvery = 3;
    private int _count = 0;
    private UIWindowFrame _lastForemostWindow = null;
    private void TrackNewWindow()
    {
      // 40 to 70 ticks per loop
      // "A single tick represents one hundred nanoseconds or one ten-millionth of a second. There are 10,000 ticks in a millisecond, or 10 million ticks in a second."
      // https://msdn.microsoft.com/en-us/library/system.datetime.ticks(v=vs.110).aspx

      if (_count++ >= _updateEvery)
      {
        _count = 0;

        var window = GetForemostWindow();
        if (!ReferenceEquals(window, _lastForemostWindow))
        {
          ChangeMoneySymbol(window);

          _lastForemostWindow = window;
        }
      }
    }

    private void ChangeMoneySymbol(UIWindowFrame window)
    {
      if(window != null)
      {
        // search for unit symbol
        foreach (var input in window.GetComponentsInChildren<UIUnitInputField>(true).Where(i => !(i is UIVelocityInputField)))
        {
          // unitText = null: some input fields don't have a label
          // unitText.text = Default: not all input fields are for money: Duration/Time, Velocity (which is handled by where)
          if (input != null && input.unitText != null && input.unitText.text == Settings.Symbol.DefaultValue)
          {
            input.unitText.text = Settings.Symbol.Value;
          }
        }
      }
    }


    private FieldInfo _foremostWindow;
    private UIWindowFrame GetForemostWindow()
    {
      if (_foremostWindow == null)
      {
        _foremostWindow = typeof(UIWindowsController).GetField("foremostWindow", BindingFlags.Instance | BindingFlags.NonPublic);
      }

      return (UIWindowFrame)_foremostWindow.GetValue(UIWindowsController.Instance);
    }
  }
}
