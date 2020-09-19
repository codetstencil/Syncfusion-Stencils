using System;
using System.Collections.Generic;
using System.Linq;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.Syncfusion.Grid
{
    public partial class BlazorGrid : ExpansionBase
    {
        private string _table;
        private List<ISchemaItem> _columns;
        private List<ISchemaItem> _sortColumns;

        /// <summary>
        /// Dictionary containing configuration values
        /// </summary>
        //private Dictionary<string, string> _gridConfiguration;
        private readonly string _true = "true".AddQuotes() + " ";

        private void MainFunction()
        {
            var settings = GetExpansionString("GRID_SETTINGS");
            if (!settings.IsBlank())
            {
                var items = settings.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                General.GridConfiguration = items.Select(item => item.Split('='))
                    .ToDictionary(keyValue => keyValue[0], keyValue => keyValue[1], StringComparer.InvariantCultureIgnoreCase);
            }

            _table = GetTable(Input);
            _columns = GetColumns(_table);
            _sortColumns = GetSortColumns(_table);

            #region Razor tags

            const int indent = 4;
            AppendText();
            AppendText(("<EjsGrid " + DataSourceSettings() + ">"));

            if (_sortColumns.Any() && General.ConfigValue("AllowSorting"))
                AppendText(General.Wrap2TagLevels(GridSortColumn(indent * 3), "GridSortSettings", "GridSortColumns", indent + 4));

            AppendText(GridEditSettings(indent));
            AppendText(EjsDataManager(indent));
            AppendText(General.Wrap1TagLevel(GridColumn(indent * 2), "GridColumns", indent));
            AppendText("</EjsGrid>");

            #endregion Razor tags
        }

        private string ContextMenuString()
        {
            var result = "AllowExcelExport = " + _true +
                         "AllowPdfExport= " + _true +
                         "ContextMenuItems=" +
                         ("@(new List<object>() { " +
                         "AutoFit".AddQuotes() + "," +
                         "AutoFitAll".AddQuotes() + "," +
                         "SortAscending".AddQuotes() + "," +
                         "SortDescending".AddQuotes() + "," +
                         "Copy".AddQuotes() + "," +
                         "Edit".AddQuotes() + "," +
                         "Delete".AddQuotes() + "," +
                         "Save".AddQuotes() + "," +
                         "Cancel".AddQuotes() + "," +
                         "PdfExport".AddQuotes() + "," +
                         "ExcelExport".AddQuotes() + "," +
                         "CsvExport".AddQuotes() + "," +
                         "FirstPage".AddQuotes() + "," +
                         "PrevPage".AddQuotes() + "," +
                         "LastPage".AddQuotes() + "," +
                         "NextPage".AddQuotes() + "})").AddQuotes();
            return result;
        }

        private string GridColumn(int indent)
        {
            BuildSnippet(null);
            foreach (var item in _columns)
            {
                var properties = item.ColumnField() + item.ColumnPrimary() + item.ColumnHeader() + item.ColumnWidth(150) + item.Alignment();
                var row = General.CreateRow("GridColumn", properties);
                BuildSnippet(row.AddCarriage(), indent, true);
            }
            return General.GenerateRow(BuildSnippet(), indent, "GridColumns");
        }

        private string GridSortColumn(int indent)
        {
            BuildSnippet(null);
            foreach (var item in _sortColumns)
            {
                var properties = item.ColumnSortField() + item.ColumnSortDirection();
                var row = General.CreateRow("GridSortColumn", properties);
                BuildSnippet(row.AddCarriage(), indent, true);
            }
            return General.GenerateRow(BuildSnippet(), indent, "GridSortColumns");
        }

        private string GridEditSettings(int indent, bool setEditSettings = true)
        {
            var result = string.Empty;
            if (setEditSettings)
            {
                BuildSnippet(null);
                result = General.SetValue("AllowAdding") + General.SetValue("AllowDeleting") + General.SetValue("AllowEditing");
                result = General.CreateRow("GridEditSettings", result, indent);
            }
            return result;
        }

        private string EjsDataManager(int indent, bool setEditSettings = true)
        {
            BuildSnippet(null);
            var result = General.SetValue("Url", "/api/" + _table) + General.SetValue("Adaptor", "Adaptors.WebApiAdaptor");
            result = General.CreateRow("EjsDataManager", result, indent);
            return result;
        }

        private string DataSourceSettings(bool setSettings = true)
        {
            var result  = General.SetValue("@ref", "@Grid");
                result += General.SetValue("ID", _table+"Grid");
                result += General.SetValue("TValue", _table);
                result += General.SetValue("Toolbar", "@(new List<string> {" +
                                                      "Add".AddQuotes() + "," +
                                                      "Edit".AddQuotes() + "," +
                                                      "Delete".AddQuotes() + "," +
                                                      "Update".AddQuotes() + "," +
                                                      "Cancel".AddQuotes() + "," +
                                                      "Print".AddQuotes() + " })");
            if (setSettings)
            {
                BuildSnippet(null);

                //Use Context Menu
                if (General.ConfigValue("UseContextMenu"))
                    result += ContextMenuString();

                //Use GridLines
                if (General.ConfigValue("UseGridLines"))
                    result += General.SetValue("GridLines", "GridLine.Both");

                //Use Grouping
                if (General.ConfigValue("AllowGrouping"))
                    result += General.SetValue("AllowGrouping");

                //Use EnablePersistence
                if (General.ConfigValue("EnablePersistence"))
                    result += General.SetValue("EnablePersistence");

                result = (result + General.SetValue("AllowPaging"));
            }
            return result;
        }

    }
}