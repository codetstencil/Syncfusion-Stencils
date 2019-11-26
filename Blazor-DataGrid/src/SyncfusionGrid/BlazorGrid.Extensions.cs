using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.Syncfusion.Grid
{
    public static class BlazorGridExtensions
    {
        private static string _space = " ";

        public static string Indent(this string text, int indent=4)
        {
            return string.Empty.PadLeft(indent)+text;
        }

        public static string ColumnField(this ISchemaItem schemaItem)
        {
            //return  _space + "Field=@nameof(" + schemaItem.TableName + "." + schemaItem.ColumnName + ")";
            return _space +
                   General.SetValue("Field", schemaItem.ColumnName); //+ 
                   //General.SetValue("TValue", schemaItem.TableName) + ")";
        }

        public static string ColumnPrimary(this ISchemaItem schemaItem)
        {
            if (schemaItem.IsPrimaryKey)
                return _space + "IsPrimary="+"true".AddQuotes() + " AllowEditing="+"false".AddQuotes();
            else
                return "";
        }

        public static string ColumnHeader(this ISchemaItem schemaItem)
        {
            return _space + "HeaderText=" + schemaItem.ColumnLabel.AddQuotes();
        }

        public static string ColumnWidth(this ISchemaItem schemaItem, int width=120)
        {
            return _space + "Width=" + width.ToString().AddQuotes();
        }

        public static string Alignment(this ISchemaItem schemaItem)
        {
            //return _space + "Width=" + width.ToString().AddQuotes();
            return "";
        }

        public static string ColumnSortField(this ISchemaItem schemaItem)
        {
            return _space + "Field="+ schemaItem.ColumnName.AddQuotes();
        }
        public static string ColumnSortDirection(this ISchemaItem schemaItem)
        {
            return _space + "Direction=" + "SortDirection.Ascending".AddQuotes();
        }


        public static string OpenTag(this string text, string tag) => "<" + tag + text+">" ;
        public static string CloseTag(this string text, string tag) => "</" + tag + ">";

        public static string OpenCloseTag(this string text, string tag) => "<" + tag + text + "></"+tag+">";
        public static string OpenCloseTag(this string text, string tag, int indent)
        {
            return (text.OpenCloseTag(tag)).Indent(indent);
        }

        public static string OpenCloseTagWithCr(this string text, string tag, int indent=4)
        {
            //var result = (GetOpenTag(tag, 0) + text + GetCloseTag(tag,0)).Indent(indent) ;
            //return result;

            return ("<" + tag + ">").Indent(indent).AddCarriage() + 
                   text.Indent().AddCarriage() + 
                   ("</"+tag + ">").Indent(indent);
        }

        static string GetOpenTag(string tag, int indent)
        {
            return ("<" + tag + ">").Indent(indent).AddCarriage();
        }

        static string GetCloseTag(string tag, int indent)
        {
            return "".AddCarriage() + ("</" + tag + ">").Indent(indent);
        }


        //public static string Tag(this string text, string tag)
        //{
        //    return "<"+tag + text +"</"+tag+">";
        //}
        //public static string Tag(this string text, string tag, int indent)
        //{
        //    return ("<" + tag + ">").AddCarriage().Indent(indent) + text.AddCarriage() + "</" + tag + ">";
        //}
        //public static string Tag(this string text, string tag, string tag2, int indent = 4)
        //{
        //    return ("<" + tag + " "+ tag2 + ">").AddCarriage().Indent(indent) + text.AddCarriage().Indent(indent) + "</" + tag + ">";
        //}

        //public static string TrimCarriage(this string str) => str.TrimEnd(Convert.ToChar(Environment.NewLine));

    }
}
