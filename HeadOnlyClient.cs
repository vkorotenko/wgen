using System.Net;
using System.Threading;

namespace wgen;
/// <summary>
/// Посылает запрос только заголовка. Не скачивает реальных данных
/// </summary>
internal class HeadOnlyClient 
{
    
    /// <summary>
    /// Делаем зпрос для проверки наличия файла
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    public static async Task<HttpResponseMessage>  GetWebRequest(Uri address)
    {
        using var client = new HttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Head, address);
        using var response = await client.SendAsync(request);
        
        return response;
    }

    
}