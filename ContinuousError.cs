namespace wgen;

/// <summary>
/// Счетчик для выхода из цикла
/// </summary>
public class ContinuousError
{
    private const int Count = 90;
    private int _runСount = 0;
    /// <summary>
    /// Увеличиваем счетчик
    /// </summary>
    /// <returns></returns>
    public bool Increase()
    {
        _runСount++;
        return _runСount > Count;
    }
    /// <summary>
    /// Зачищаем счетчик
    /// </summary>
    public void Clean() => _runСount = 0;
}