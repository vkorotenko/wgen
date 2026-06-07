namespace wgen;
/// <summary>
/// Простое добавление в список год
/// </summary>
public static class NodesExtension
{
    /// <summary>
    /// Добавление новой ноды в словарь
    /// </summary>
    /// <param name="list">Список нод</param>
    /// <param name="end">Последний номер</param>
    /// <param name="num">Номер хоста</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void Add(this List<Node> list, int end, int num)
    {
        if (list == null) throw new ArgumentNullException(nameof(list));
        list.Add(new Node{End =end, Num = num});
        
    }
}