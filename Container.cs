using System.Text;

namespace wgen;

public class Container
{
    public Container()
    {
        Nodes = new List<Node>();
      
        Nodes.AddRange([
            new Node{Start = 0,    End = 143,Num = 1},
            new Node{Start = 144,  End = 287,Num = 2},
            new Node{Start = 288,  End = 431,Num = 3},
            new Node{Start = 432,  End = 719,Num = 4},
            new Node{Start = 720,  End = 1007,Num = 5},
            new Node{Start = 1008, End = 1061,Num = 6},
            new Node{Start = 1062, End = 1115,Num = 7},
            new Node{Start = 1116, End = 1169,Num = 8},
            new Node{Start = 1170, End = 1313,Num = 9},
            new Node{Start = 1314, End = 1601,Num = 10},
            new Node{Start = 1602, End = 1655,Num = 11},
            new Node{Start = 1656, End = 1919,Num = 12},
            new Node{Start = 1920, End = 2045,Num = 13},
            new Node{Start = 1920, End = 2189,Num = 14},
            new Node{Start = 1920, End = 2405,Num = 15},
            new Node{Start = 1920, End = 2621,Num = 16},
            new Node{Start = 1920, End = 2837,Num = 17},
        ]);
        Last = 17;
    }
    public int  Last {
        get;
        set;
    }

    public List<Node> Nodes { get; set; }

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
        
        foreach (var node in Nodes)
        {
            sb.AppendLine(node.ToString());
        }

        sb.AppendLine($"else host = '{Last+1}'");
        sb.AppendLine("return `https://basket-${host}.wbbasket.ru/vol${vol}/part${part}/${nm}/images/c246x328/1.webp`");
        sb.AppendLine("}");
        return sb.ToString();
    }
    /*
     ***
        * input - nmId товара
        * Делает картинки для артикулов
        *
        *
       export function wb_img_url(input: string) {
         const nm = parseInt(input, 10),
           vol = ~~(nm / 1e5),
           part = ~~(nm / 1e3)

         let host = ''
         if (vol >= 0 && vol <= 143) host = '01'
         else if (vol >= 144 && vol <= 287) host = '02'
         else if (vol >= 288 && vol <= 431) host = '03'
         else if (vol >= 432 && vol <= 719) host = '04'
         else if (vol >= 720 && vol <= 1007) host = '05'
         else if (vol >= 1008 && vol <= 1061) host = '06'
         else if (vol >= 1062 && vol <= 1115) host = '07'
         else if (vol >= 1116 && vol <= 1169) host = '08'
         else if (vol >= 1170 && vol <= 1313) host = '09'
         else if (vol >= 1314 && vol <= 1601) host = '10'
         else if (vol >= 1602 && vol <= 1655) host = '11'
         else if (vol >= 1656 && vol <= 1919) host = '12'
         else if (vol >= 1920 && vol <= 2045) host = '13'
         else if (vol >= 1920 && vol <= 2189) host = '14'
         else if (vol >= 1920 && vol <= 2405) host = '15'
         else if (vol >= 1920 && vol <= 2621) host = '16'
         else if (vol >= 1920 && vol <= 2837) host = '17'
         else host = '18'
         // https://basket-41.wbbasket.ru/vol9988/part998883/998883843/images/c246x328/1.webp
         //https://basket-22.wbbasket.ru/vol3710/part371001/371001179/images/c246x328/1.webp
         // https://basket-18.wbbasket.ru/vol3710/part371001/371001179/images/c246x328/1.webp
         //https://basket-{N}.wb.ru/vol{V}/part{P}/{nmID}/images/big/1.webp

         // https://basket-16.wbbasket.ru/vol2441/part244170/244170715/images/c246x328/1.webp  real small
         // https://basket-16.wbbasket.ru/vol2441/part244170/244170715/images/big/1.webp real big

         return `https://basket-${host}.wbbasket.ru/vol${vol}/part${part}/${nm}/images/c246x328/1.webp`
       }
     *
     *
     */
    /// <summary>
    /// Получаем адрес на основе параметров
    /// </summary>
    /// <param name="host"></param>
    /// <param name="vol"></param>
    /// <param name="part"></param>
    /// <param name="nmId"></param>
    /// <returns></returns>
    public static string GetAddress(string host,int vol, int part, int nmId)
    => $"https://basket-{host}.wbbasket.ru/vol{vol}/part{part}/{nmId}/images/c246x328/1.webp";
    
    public string CheckSingle(string s)
    {
        var nm = int.Parse(s);
        var vol = (int)(Math.Floor(nm / 1e5));
        var part = (int)Math.Floor(nm / 1e3);
        var host = vol switch
        {
            >= 0 and <= 143 => "01",
            <= 287   => "02",
            <= 431   => "03",
            <= 719   => "04",
            <= 1007  => "05",
            <= 1061 => "06",
            <= 1115 => "07",
            <= 1169 => "08",
            <= 1313 => "09",
            <= 1601 => "10",
            <= 1655 => "11",
            <= 1919 => "12",
            <= 2045 => "13",
            <= 2189 => "14",
            <= 2405 => "15",
            <= 2621 => "16",
            <= 2837 => "17",
            _ => "18"
        };

        var url = GetAddress(host, vol, part, nm);
        var res = CheckExist(url);
        return res ? url : "run wgen g";
        
    }

    bool CheckExist(string url)
    {
        // using HeadOnlyClient from linked post
        using var client = new HeadOnlyClient();
        client.HeadOnly = true;
        // fine, no content downloaded
        try
        {
            var s1 = client.DownloadString(url);
            return true;
        }
        catch
        {
            return false;
        }
        
    }
    public bool Make()
    {
        throw new NotImplementedException();
    }
}