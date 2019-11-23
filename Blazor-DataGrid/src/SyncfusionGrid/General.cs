using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeraSystems.CodeNanite.Expansion;

namespace ZeraSystems.Syncfusion.Grid
{
    public class General
    {
        public static string CreateRow(string tag, string properties)
        {
            var result = string.Empty;
            if (!properties.IsBlank())
                result = properties.OpenTag(tag) + properties.CloseTag(tag);
            return result;
        }

        public static string CreateRow(string tag, string properties, int indent)
        {
            return CreateRow(tag, properties).Indent(indent);
        }

        public static string GenerateRow(string text, int indent, string tagParent)
        {
            //var text = BuildSnippet();
            return text.TrimEnd('\r', '\n'); //.OpenCloseTagWithCr(tagParent, indent - 4);
        }

        /// <summary>
        /// Creates a 2-Level Html tag on the passed text
        /// </summary>
        /// <param name="text">Passed Text</param>
        /// <param name="tag1">Highest level of the tags</param>
        /// <param name="tag2">2nd/next level of the tags</param>
        /// <param name="indent"></param>
        /// <returns>Html tagged content</returns>
        public static string Wrap2TagLevels(string text, string tag1, string tag2, int indent)
        {
            var sortTag = Wrap1TagLevel(text, tag2, indent);
            return Wrap1TagLevel(sortTag, tag1, indent - 4);
        }
        /// <summary>
        /// Creates a 1-Level Html tag on passed text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="tag"></param>
        /// <param name="indent"></param>
        /// <returns>Html tagged content</returns>
        public static string Wrap1TagLevel(string text, string tag, int indent)
        {
            var result =
                ("<" + tag + ">").Indent(indent).AddCarriage() +
                text.AddCarriage() +
                ("</" + tag + ">").Indent(indent);
            return result;
        }

        public static string SetValue(string property, string value = "true")
        {
            return " " + property.Trim() + "=" + value.AddQuotes();
        }

    }
}
