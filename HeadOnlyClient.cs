using System.Net;

namespace wgen;
/// <summary>
/// Посылает запрос только заголовка. Не скачивает реальных данных
/// </summary>
internal class HeadOnlyClient : WebClient
{
    /// <summary>
    /// Переопределяем отправку заголовка
    /// </summary>
    public bool HeadOnly { get; set; }
    protected override WebRequest GetWebRequest(Uri address)
    {
        var req = base.GetWebRequest(address);
        if (HeadOnly && req.Method == "GET") req.Method = "HEAD";
        return req;
    }
}