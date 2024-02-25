
//var ele= "<div id=\"myId\" class=\"class1 class2\">text<div/>"; 
//var attribute = new Regex("([^\\s]*?=\"(.*?)\"").Matches(ele);
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace project2
{

    class Program
    {
        static async Task Main(string[] args)
        {
            var html = await Load("https://learn.malkabruk.co.il/practicode/projects/pract-2/#_3");
            var cleanHtml = new Regex("\\s").Replace(html, "");
            var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0);
            var arr = HtmlHelper.Instance.AllTags;
            static async Task<string> Load(string url)
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(url);
                var html = await response.Content.ReadAsStringAsync();
                return html;
            }
            var root = new HtmlElement("div");
            var child1 = new HtmlElement("div", root);
            var child2 = new HtmlElement("p class=”class-name”", root);
            var grandchild = new HtmlElement("p class=”class-name”", child1);
            root.Children.Add(child1);
            root.Children.Add(child2);
            child1.Children.Add(grandchild);
            Console.WriteLine("Descendants:");
            foreach (var element in root.Descendants())
            {
                Console.WriteLine(element.Name);
            }
            Console.WriteLine("\nAncestors:");
            foreach (var element in grandchild.Ancestors())
            {
                Console.WriteLine(element.Name);
            }
            string queryString = "div p.class-name";
            Selector selector2 = Selector.ParseSelector(queryString);
            Console.WriteLine("\nFindElements:");
            var elements = root.FindElements(selector2);
            foreach (var element in elements)
            {
                Console.WriteLine(element.Name);
                foreach (var child in element.Children)
                {
                    Console.WriteLine($"<{element.Name} class=\"{string.Join(" ", element.Classes)}\">{element.InnerHtml}</{element.Name}>");
                }
            }
            HtmlHelper htmlHelper = new HtmlHelper();
            List<string> values = new List<string>() { "/div", "div<", "/p", "class-name/>", "rf", "sd/", "ghgb" };
            HtmlElement htmlElement2 = htmlHelper.Build(values);
            var selector = Selector.ParseSelector("p");
            var rootElement = HtmlHelper.Instance.Build(htmlLines.ToList());
            root.FindElements(selector2);
            Console.WriteLine(selector2.TagName);
            Console.WriteLine(selector2.SelectorId);
            Console.WriteLine(string.Join(", ", selector2.Classes));
            Console.WriteLine(selector2.Parent?.TagName);

        }

    }
}



