using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Collections;

namespace CvGenerator.Logic
{
    public class HtmlRenderer
    {
        private readonly string HtmlTemplate;

        public HtmlRenderer(string htmlTemplate)
        {
            this.HtmlTemplate = htmlTemplate;
        }

        private static readonly Type enumerableType = typeof(IEnumerable);
        private static readonly Type stringType = typeof(string);

        private static bool IsNonStringEnumerable(Type type)
        {
            return !type.IsEquivalentTo(stringType) && enumerableType.IsAssignableFrom(type);
        }

        public string FillData(object data)
        {
            StringBuilder sb = new StringBuilder(HtmlTemplate);

            Type type = data.GetType();
            Type enumerableType = typeof(IEnumerable);
            foreach (var prop in type.GetProperties().Where(p => !IsNonStringEnumerable(p.PropertyType)))
            {
                var value = prop.GetValue(data);
                if(value != null)
                    sb.Replace("{" + prop.Name + "}", value.ToString());
            }
            string result = sb.ToString();

            // Remove unused remaining placeholders:
            result = Regex.Replace(result, @"\{\w+\}", "");

            XElement rootElem = XElement.Parse(result);
            var elemsByClass = rootElem.Descendants()
                .Where(e => e.Attribute("class") != null)
                .GroupBy(e => e.Attribute("class").Value)
                .ToDictionary(k => k.Key);
            foreach (var prop in type.GetProperties())
            {
                var value = prop.GetValue(data);
                if(elemsByClass.ContainsKey(prop.Name))
                {
                    foreach (var elem in elemsByClass[prop.Name])
                    {
                        if (value == null || string.Empty.Equals(value))
                            elem.Remove();
                        else if (IsNonStringEnumerable(prop.PropertyType))
                        {
                            var repeatingElem = elem.Elements().FirstOrDefault(e => e.Attribute("class")?.Value == "Item");
                            if (repeatingElem == null)
                                continue;
                            var repeatingElemStr = repeatingElem.ToString();
                            foreach (var innerValue in (IEnumerable)value)
                            {
                                var newElemStr = new StringBuilder(repeatingElemStr);
                                newElemStr.Replace("{*}", innerValue.ToString());
                                foreach (Match match in Regex.Matches(newElemStr.ToString(), @"\{\*\.(\w+)\}"))
                                {
                                    newElemStr.Replace(match.Value, innerValue.GetType().GetProperty(match.Groups[1].Value).GetValue(innerValue).ToString());
                                }
                                repeatingElem.Parent.Add(XElement.Parse(newElemStr.ToString()));
                            }
                            repeatingElem.Remove();
                        }
                    }    
                }
            }

            result = rootElem.ToString();
            return result;
        }

    }
}
