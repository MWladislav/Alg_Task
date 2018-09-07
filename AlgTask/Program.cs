using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlgTask
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var inputPath = "input.txt";

                Console.WriteLine("Введите название выходного файла:");
                var outputPath = Console.ReadLine();
                var stream = File.Create(outputPath);
                stream.Close();
                //Чтение с файла и добавление слов текста в коллекцию
                var sr = new StreamReader(inputPath);
                var text = sr.ReadToEnd().ToLower();
                var list = Regex.Split(text.Remove(text.Length - 2), @"[^\w-]+").ToList();
                list.Sort();
                sr.Close();

                //Подсчет количества вхождений слов в тексте с помощью словаря
                var dict = new Dictionary<string, int>();
                foreach (var str in list)
                {
                    if (!dict.ContainsKey(str))
                    {
                        dict.Add(str, 1);
                    }
                    else dict[str]++;
                }
                //Группирование пар словаря в первому символу, сортировка групп по убыванию количества вхождений и запись результата в файл
                var firstSymbols = new SortedSet<char>(from s in dict.Keys select s.First());
                var sw = new StreamWriter(outputPath);
                foreach (var s in firstSymbols)
                {
                    sw.WriteLine(s);
                    var sortedGroups = dict.Where(d => d.Key.StartsWith(s.ToString())).OrderByDescending(pair => pair.Value);
                    foreach (var pair in sortedGroups)
                    {
                        sw.WriteLine("{0} {1}", pair.Key, pair.Value);
                    }
                }
                sw.Close();

                Console.WriteLine("Проверьте результат в выходном файле:\n{0}", outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Что то пошло не по плану, прочтите описание ниже\n" + ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}
