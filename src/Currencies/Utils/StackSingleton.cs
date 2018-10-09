using System;

namespace Craxy.Parkitect.Currencies.Utils
{
  public sealed class StackSingleton<T>
  {
    public StackSingleton(Func<T> create, Action<T> dispose)
    {
      this.create = create;
      this.dispose = dispose;
    }

    private readonly Func<T> create;
    private readonly Action<T> dispose;

    private uint _rents = 0;
    public uint Rents => _rents;

    private T _instance = default;

    public T Rent()
    {
      if(_rents == 0)
      {
        _instance = create();
      }
      _rents++;
      return _instance;
    }
    public void Return()
    {
      if(_rents <= 0)
      {
        throw new InvalidOperationException($"There's nothing rented.");
      }

      _rents--;
      if(_rents == 0)
      {
        dispose?.Invoke(_instance);
        _instance = default;
      }
    }
    internal T ForceGet()
    {
      if(_rents <= 0)
      {
        throw new InvalidOperationException($"There's nothing rented and therefore nothing to get.");
      }
      return _instance;
    }
  }
}
