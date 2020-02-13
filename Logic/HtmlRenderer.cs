using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Web;

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

        public string FillData(object data, bool htmlEncodeStrings = true)
        {
            StringBuilder sb = new StringBuilder(HtmlTemplate);

            Type type = data.GetType();
            Type enumerableType = typeof(IEnumerable);
            foreach (var prop in type.GetProperties().Where(p => !IsNonStringEnumerable(p.PropertyType)))
            {
                var value = prop.GetValue(data);
                if (value != null)
                {
                    string valueStr = value.ToString();
                    if (htmlEncodeStrings)
                        valueStr = HttpUtility.HtmlEncode(valueStr);
                    sb.Replace("{" + prop.Name + "}", valueStr);
                }
            }
            string result = sb.ToString();

            // Remove unused remaining placeholders:
            result = Regex.Replace(result, @"\{\w+\}", "");

            XElement bodyElem = XElement.Parse(result).Element("body");
            var elemsByDataBind = bodyElem.Descendants()
                .Where(e => e.Attribute("data-bind") != null)
                .GroupBy(e => e.Attribute("data-bind").Value)
                .ToDictionary(k => k.Key);
            foreach (var prop in type.GetProperties())
            {
                var value = prop.GetValue(data);
                if(elemsByDataBind.ContainsKey(prop.Name))
                {
                    foreach (var elem in elemsByDataBind[prop.Name])
                    {
                        if (value == null || string.Empty.Equals(value))
                            elem.Remove();
                        else if (IsNonStringEnumerable(prop.PropertyType))
                        {
                            var repeatingElem = elem.Elements().FirstOrDefault(e => e.Attribute("data-bind")?.Value == "Item");
                            if (repeatingElem == null)
                                continue;
                            var repeatingElemStr = repeatingElem.ToString();
                            foreach (var innerValue in (IEnumerable)value)
                            {
                                var newElemStr = new StringBuilder(repeatingElemStr);
                                if (innerValue is IToHtml)
                                    newElemStr.Replace("{*}", (innerValue as IToHtml).ToHtml());
                                else
                                {
                                    string innerValueStr = innerValue.ToString();
                                    if(htmlEncodeStrings)
                                        innerValueStr = HttpUtility.HtmlEncode(innerValueStr);
                                    newElemStr.Replace("{*}", innerValue.ToString());
                                }

                                List<string> dataBindsToDelete = new List<string>();
                                foreach (Match match in Regex.Matches(newElemStr.ToString(), @"\{\*\.(\w+)\}"))
                                {
                                    var itemName = match.Groups[1].Value;
                                    var innerValueItem = innerValue.GetType().GetProperty(itemName).GetValue(innerValue);
                                    if (innerValueItem == null || string.Empty.Equals(innerValueItem))
                                    {
                                        newElemStr.Replace(match.Value, "");
                                        dataBindsToDelete.Add("*." + itemName);
                                    }
                                    else
                                    {
                                        string innerValueItemStr = innerValueItem.ToString();
                                        if (htmlEncodeStrings)
                                            innerValueItemStr = HttpUtility.HtmlEncode(innerValueItemStr);
                                        newElemStr.Replace(match.Value, innerValueItemStr);
                                    }
                                }
                                var newElem = XElement.Parse(newElemStr.ToString());
                                foreach (var innerElem in newElem.Elements().Where(e => dataBindsToDelete.Contains(e.Attribute("data-bind")?.Value)).ToList())
                                    innerElem.Remove();
                                repeatingElem.Parent.Add(newElem);
                            }
                            if (repeatingElem.Parent.Elements().Count() == 1)
                            {
                                foreach (var elemToDelete in elemsByDataBind[prop.Name])
                                    elemToDelete.Remove();
                                break;
                            }
                            else
                                repeatingElem.Remove();
                        }
                    }    
                }
            }

            result = Regex.Replace(result, @"<body>.*<\/body>", bodyElem.ToString(), RegexOptions.Singleline);
            return result;
        }

    }
}
