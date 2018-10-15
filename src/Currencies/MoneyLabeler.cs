using System.Linq;
using System.Reflection;
using Parkitect.UI;
using UnityEngine;
using System.Collections.Generic;
using Craxy.Parkitect.Currencies.Utils;

namespace Craxy.Parkitect.Currencies
{
  internal class MoneyLabeler : MonoBehaviour
  {
    public Settings Settings { get; set; }

    void Start()
    {
      currentSymbol = Settings.Symbol.DefaultValue;
    }

    void Update()
    {
      if(currentSymbol != Settings.Symbol.Value)
      {
        TryInject();
      }
    }
    private string currentSymbol;
    private void TryInject()
    {
      var inputs = Resources.FindObjectsOfTypeAll<UIUnitInputField>();
      // Mod.Log($"MoneyLabeler.TryInject. Inputs={inputs.Length}");
      foreach (var input in inputs)
      {
        if(input == null || input is UIVelocityInputField)
        {
          continue;
        }

        try
        {
          if (input.unitText != null && (input.unitText.text == currentSymbol))
          {
            // Mod.Log($"Looking at {input} with unitText={input.unitText.text}");
            input.unitText.text = Settings.Symbol.Value;
          }

        }
        catch (System.Exception ex)
        {
          Mod.Log($"Exception while trying injecting into {input}: {ex.ToString()} -- input.unitText={input.unitText}");
        }
      }

      currentSymbol = Settings.Symbol.Value;
    }
  }
}
