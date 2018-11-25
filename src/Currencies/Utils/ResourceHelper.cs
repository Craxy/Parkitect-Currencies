using System.IO;
using UnityEngine;

namespace Craxy.Parkitect.Currencies.Utils
{
  static class ResourceHelper
  {
    public static Stream LoadResource(string path)
    {
      var assembly = System.Reflection.Assembly.GetExecutingAssembly();
      return assembly.GetManifestResourceStream(typeof(Mod), path);
    }
    public static string LoadString(string path)
    {
      using (var s = LoadResource(path))
      using (var stream = new StreamReader(s))
      {
        return stream.ReadToEnd();
      }
    }
    public static Texture2D LoadTexture(string path)
    {
      using (var stream = LoadResource(path))
      {
        var buf = new byte[stream.Length];
        stream.Read(buf, 0, buf.Length);

        var tex = new Texture2D(1, 1, TextureFormat.Alpha8, false, true);
        tex.LoadImage(buf, true);

        return tex;
      }
    }
  }
}
