using System.Text.Json.Serialization;

namespace wgen;
/// <summary>
/// Элемент коллекции для сохранения бакетов
/// </summary>
public class Node
{
    /// <summary>
    /// Идентификатор в WB
    /// </summary>
    public int  Num { get; set; }
    /// <summary>
    /// Номер в бакете.
    /// </summary>
    public int End { get; set; }
    /// <summary>
    /// Преобразование к строке используется для построения JS
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Num == 1 ? "if      (vol <= 143) host = '01'" : $"else if (vol <= {End}) host = '{Num:D2}'";
    }
    /// <summary>
    /// Счетчик ошибок
    /// </summary>
    [JsonIgnore]
    public int ErrorCount { get; set; } = 0;
}