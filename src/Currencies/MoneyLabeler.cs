using System.Linq;
using System.Reflection;
using Parkitect.UI;
using UnityEngine;
using System.Collections.Generic;

namespace Craxy.Parkitect.Currencies
{
  internal class MoneyLabeler : MonoBehaviour
  {
    public Settings Settings { get; set; }

    private void OnDisable()
    {
      _lastForemostWindow = null;
      _windows = null;
    }

    private void Update()
    {
      TrackNewWindow();
    }


    private IList<UIWindowFrame> _windows = null;
    private UIWindowFrame _lastForemostWindow = null;
    private void TrackNewWindow()
    {
      // 3 to 4 ticks per loop when there's no change
      // "A single tick represents one hundred nanoseconds or one ten-millionth of a second. There are 10,000 ticks in a millisecond, or 10 million ticks in a second."
      // https://msdn.microsoft.com/en-us/library/system.datetime.ticks(v=vs.110).aspx


      // a previous check if the currency symbol was changed is quite expensive:
      // it adds 4 to 8 ticks (but prevents checks for new windows if symbol is default (which are around 100 to 300 ticks))
      //if (Settings.NumberFormat.CurrencySymbol != Settings.DefaultNumberFormat.CurrencySymbol)

      // the window on top is the last item in UIWindowsController.uiWindows
      if (_windows == null)
      {
        var field = typeof(UIWindowsController).GetField("uiWindows", BindingFlags.Instance | BindingFlags.NonPublic);
        _windows = (IList<UIWindowFrame>)field.GetValue(UIWindowsController.Instance);
      }

      UIWindowFrame window = null;
      if (_windows.Count > 0)
      {
        window = _windows[_windows.Count - 1];
      }

      if (!ReferenceEquals(window, _lastForemostWindow))
      {
        ChangeMoneySymbol(window);

        _lastForemostWindow = window;
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
  }
}
