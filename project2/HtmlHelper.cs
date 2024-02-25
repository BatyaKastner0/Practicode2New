using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace project2
{
    public class HtmlHelper
    {
        public string[] AllTags { get; set; }
        public string[] SelfClosingTags { get; set; }
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public HtmlHelper()
        {
            try
            {
                var allTagsJson = File.ReadAllText("allTags.json");
                var selfClosingTagsJson = File.ReadAllText("selfClosingTags.json");

                AllTags = JsonSerializer.Deserialize<string[]>(allTagsJson);
                SelfClosingTags = JsonSerializer.Deserialize<string[]>(selfClosingTagsJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading HTML tags: {ex.Message}");
          
            }


        }
       


        public HtmlElement Build(List<string> htmlLines)
        {

            HtmlElement rootElement = null;
            HtmlElement currentElement = null;

            foreach (var line in htmlLines)
            {
                if (line == "html/")
                {
                    break;
                }
                if (line.StartsWith("/"))
                {
                    currentElement = currentElement?.Parent;
                }
                else if (HtmlHelper.Instance.AllTags != null && HtmlHelper.Instance.AllTags.Contains(line))
                {

                    var isSelfClosing = line.EndsWith("/") || HtmlHelper.Instance.SelfClosingTags.Contains(line);
                    var newElement = new HtmlElement(line);
                    var attributes = Regex.Matches(line, @"\w+\s*=\s*""([^""]*)""");
                    foreach (Match attribute in attributes)
                    {
                        var attributeParts = attribute.Value.Split('=');
                        var attributeName = attributeParts[0].Trim();
                        var attributeValue = attributeParts[1].Trim('"');

                        if (attributeName.ToLower() == "class")
                        {

                            var classes = attributeValue.Split(' ');
                            foreach (var className in classes)
                            {
                                Console.WriteLine("class" + className);
                                newElement.Classes.Add(className);
                            }
                        }
                        else if (attributeName.ToLower() == "id")
                        {

                            newElement.Id = attributeValue;
                        }
                    }

                    newElement.Parent = currentElement;

                    if (currentElement == null)
                    {
                        rootElement = newElement;
                    }
                    else
                    {
                        currentElement.Children.Add(newElement);
                    }

                    if (!isSelfClosing)
                    {
                        currentElement = newElement;
                    }
                }
                else
                {
                    if (currentElement != null)
                        currentElement.InnerHtml = line;
                }
            }

            return rootElement;
        }


    }




















}

