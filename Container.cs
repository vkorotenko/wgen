using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace wgen;

/// <summary>
/// Контейнер описывающий примеры и обрабатывающий логику работы с WB
/// </summary>
public class Container
{
    private const string FileName = "parsed.json";
    /// <summary>
    /// Баскеты и их границы
    /// </summary>

    public List<Node> Nodes { get; set; } = new()
    {
        { 143, 1 },
        { 287, 2 },
        { 431, 3 },
        { 719, 4 },
        { 1007, 5 },
        { 1061, 6 },
        { 1115, 7 },
        { 1169, 8 },
        { 1313, 9 },
        { 1601, 10 },
        { 1655, 11 },
        { 1919, 12 },
        { 2045, 13 },
        { 2189, 14 },
        { 2405, 15 },
        { 2621, 16 },
        { 2837, 17 },
        { 3053, 18 },
        { 3269, 19 },
        { 3484, 20 },
        { 3701, 21 },
        { 3917, 22 },
        { 4133, 23 },
        { 4349, 24 },
        { 4565, 25 },
        { 4877, 26 },
        { 5143, 27 },
        { 5500, 28 },
        { 5813, 29 },
        { 6125, 30 },
        { 6435, 31 },
        { 6749, 32 },
        { 7061, 33 },
        { 7373, 34 },
        { 7685, 35 },
        { 7997, 36 },
        { 8309, 37 },
        { 8740, 38 },
        { 9173, 39 },
        { 9603, 40 },
        { 10373, 41 },
        { 11141, 42 }
    };
    /// <summary>
    /// Генерация ява скрипта
    /// </summary>
    /// <returns></returns>
    public string  GenerateJs()
    {
        var sb = new StringBuilder();
        sb.AppendLine("/**********************");
        sb.AppendLine("* input - nmId товара");
        sb.AppendLine("* Делает картинки для артикулов");
        sb.AppendLine("***********************/");
        sb.AppendLine("function wb_img_url(input) {");
        sb.AppendLine("var nm = parseInt(input, 10)");
        sb.AppendLine("var vol = ~~(nm / 1e5)");
        sb.AppendLine("var part = ~~(nm / 1e3)");
        sb.AppendLine("var host = ''");
        foreach (var node in Nodes) sb.AppendLine(node.ToString());
        sb.AppendLine($"else host = '{Nodes.Max(x=>x.Num) +1}'");
        sb.AppendLine("return `https://basket-${host}.wbbasket.ru/vol${vol}/part${part}/${nm}/images/tm/1.webp`");
        sb.AppendLine("}");
        return sb.ToString();
    }
    
    
    /// <summary>
    /// Проверка существования файла
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public string CheckSingle(string s)
    {
        var nm = long.Parse(s);
        var vol = (int)(Math.Floor(nm / 1e5));
        foreach (var node in Nodes.Where(node => nm <= node.End))
        {
            return $"{node.Num:D2}";
        }

        var host = Nodes.Max(x => x.Num) + 1;
        var url = GetAddress(host, nm);
        var res = CheckExist(url);
        return res ? url : "run wgen g";
        
    }
    /// <summary>
    /// Проверка существования файла без скачивания
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static bool CheckExist(string url)
    {
        try
        {
            var res = HeadOnlyClient.GetWebRequest(new Uri(url)).Result;
            if (res.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine($"OK: {url}");
                return true;
            }
            Console.WriteLine($"ERROR: {url}");
            return false;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {url}");
            Console.WriteLine(ex.Message);
            return false;
        }
        
    }
    /// <summary>
    /// Получение адреса изображения. 
    /// </summary>
    /// <param name="host">Номер хоста</param>
    /// <param name="nmId">Артикул товара в WB</param>
    /// <returns></returns>
    public static string GetAddress(int host, long nmId)
    {
        var vol = (int)(Math.Floor(nmId / 1e5));
        var part = (int)Math.Floor(nmId / 1e3);
        return $"https://basket-{host}.wbbasket.ru/vol{vol}/part{part}/{nmId}/images/tm/1.webp";
    }
    /// <summary>
    /// Загрузка файла parsed.json для восстановления словарей. 
    /// </summary>
    public void Load()
    {
        if (File.Exists(FileName))
        {
            using var stream = new StreamReader(FileName);
            var opt = new JsonSerializerOptions
            {
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            };
            try
            {
                var ctx = JsonSerializer.Deserialize<Container>(stream.BaseStream, opt);
                if(ctx!=null) Nodes = ctx.Nodes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
                ;
            }
        }
    }
    /// <summary>
    /// Создание нового словаря и сканирование хостов. 
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public int Make(string num)
    {

        var nmId = long.Parse(num);
        var host = Nodes.Max(x => x.Num) + 1;
        var max = Nodes.Max(x => x.End);
        Console.WriteLine("Already update. Use wgen g");
        if (nmId <= max) return 0;

        var counter = new ContinuousError();



        var node = new Node{End = max,Num = Nodes.Max(x=>x.Num +1) };
        while (nmId > max * 1e5)
        {
            max++;
            var test = (long)(max * 1e5);
            
            var result = CheckExist(GetAddress(host,test));
            Thread.Sleep(100);
            if (result)
            {
                node.ErrorCount = 0;
                node.End = max;
                counter.Clean();
            }
            else
            {
                if(counter.Increase())
                {
                    return 2;
                }
                if (node.ErrorCount < 60)
                {
                    node.ErrorCount++;
                    continue;
                }
                Nodes.Add(node);
                host = node.Num + 1;
                node = new Node { End = max, Num = host };
                Sync();
            }
        }
        
        return 0;
    }
    /// <summary>
    /// Синхронизируем данные
    /// </summary>
    private void Sync()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver()
        };
        var json = JsonSerializer.Serialize(this, options);
        File.WriteAllText(FileName, json);
    }
}