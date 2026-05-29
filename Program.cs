using System;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace wgen
{
    internal class Program
    {
        /// <summary>
        /// Точка входа
        /// </summary>
        /// <param name="args">Параметры запуска</param>
        /// <returns>Возвращает статусы подробности в помощи</returns>
        private static int Main(string[] args)
        {


            var retcode = 0;
            const string fileName = "parsed.json";
            Container? container = null;
            if (File.Exists(fileName))
            {
                using var stream = new StreamReader(fileName);
                var opt = new JsonSerializerOptions
                {
                    TypeInfoResolver = new DefaultJsonTypeInfoResolver()
                };
                container = JsonSerializer.Deserialize<Container>(stream.BaseStream,opt);
            }

            container ??= new Container();
            try
            {
                switch (args[0])
                {
                    case "g":
                        Console.WriteLine("");
                        Console.Write(container.GenerateJs());
                        break;
                    case "s":
                        Console.WriteLine("");
                        var res = container.CheckSingle(args[1]);
                        Console.Write(res);
                        if (res.Contains("run wgen")) retcode=1;
                        break;
                    case "m":
                        Console.WriteLine("");
                        Console.Write(container.Make());
                        break;
                    default:
                        Console.WriteLine(ShowHelp());
                        break;
                }
            }
            catch
            {
               Console.WriteLine( ShowHelp());
            }
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            };
            var json = JsonSerializer.Serialize(container, options);

            File.WriteAllText("parsed.json",json);
            return retcode;
        }
        /// <summary>
        /// показываем справку
        /// </summary>
        /// <returns></returns>
        private static string ShowHelp()
        {
            return @"wgen simple program to generate image path from nmId for https://www.wildberries.ru/ 
wgen g - generate a javascript for create a full url to image based on nmId. Attention check nmId before generate by command wgen s 998883843
wgen s nmId - check exist or no image. If not exist rum  wgen m to create new parsed.json file. This command send request to server https://www.wildberries.ru/ and create new parsed.json file contain ranges of basket 
wgen m - scan https://www.wildberries.ru/ for new basket and write to parsed.json file

WARNING: Don't delete parsed.json file! This file contain all parsed data. This cached values improve generation speed.
Retcodes: 
 * 0 - normal
 * 1 - single check nmId not passed, need recalculate base wgen m
";
        }
    }
}
