using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.IO;

namespace FindNulls
{
    public class Program
    {
        static void Main(string[] args)
        {
            Manager man = new Manager(@"E:\Sky\Programming\Personal\C#\Proj_win8_first\App3\App3.Shared\Data\2");

            foreach (var q in man.questions)
            {
                Console.WriteLine(q.Key);
                Console.WriteLine(q.Value.ToString());
            }

            Console.Read();
        }

    }

    class Manager
    {
        public Dictionary<string, XElement> questions = new Dictionary<string, XElement>();


        public Manager(string path)
        {
            var files = Directory.GetFiles(path);

            foreach (var file in files)
            {
                XDocument document = XDocument.Load(file);

                var elements = document.Root.Elements("question");

                foreach (var elem in elements)
                    try
                    {
                        var trol = elem.Elements().Where(el => el.Attribute("value").Value == "1").First();
                    }
                    catch
                    {
                        questions.Add(file, elem);
                        break;
                    }

            }
        }
    }

}
