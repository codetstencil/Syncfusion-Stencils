using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.Syncfusion.Grid
{
    public partial class BlazorGrid : ExpansionBase
    {
        private string _table;
        private List<ISchemaItem> _columns;
        private List<ISchemaItem> _sortColumns;
        private Dictionary<string, string> _gridConfiguration;
        private readonly string _true = "true".AddQuotes() + " ";

        #region Settings

        public bool AllowEditing { get; set; }
        public bool UseContextMenu { get; set; }

        #endregion Settings

        private void MainFunction()
        {
            var settings = GetExpansionString("GRID_SETTINGS");
            
            if (!settings.IsBlank())
            {
                var items = settings.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                _gridConfiguration = items.Select(item => item.Split('='))
                    .ToDictionary(keyValue => keyValue[0], keyValue => keyValue[1], StringComparer.InvariantCultureIgnoreCase);
            }

            _table = GetTable(Input);
            _columns = GetColumns(_table);
            _sortColumns = GetSortColumns(_table);

            #region Razor tags
            const int indent = 4;
            AppendText();
            AppendText(("<EjsGrid " + DataSourceSettings() + ">"));

            if (_sortColumns.Any() && ConfigValue("AllowSorting"))
                AppendText(General.Wrap2TagLevels(GridSortColumn(indent * 3), "GridSortSettings", "GridSortColumns", indent + 4));

            AppendText(GridEditSettings(indent ));
            AppendText(EjsDataManager(indent));
            AppendText(General.Wrap1TagLevel(GridColumn(indent*2), "GridColumns", indent ));
            AppendText("</EjsGrid>");
            #endregion

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
            var x = 0;
            foreach (var item in _columns)
            {
                x++;
                var properties = item.ColumnField() + item.ColumnHeader() + item.ColumnWidth(150) + item.Alignment();
                var row = General.CreateRow("GridColumn", properties);
                BuildSnippet(row.AddCarriage(), indent, true);
            }
            return General.GenerateRow(BuildSnippet(), indent, "GridColumns");
        }

        private string GridSortColumn(int indent)
        {
            BuildSnippet(null);
            var x = 0;
            foreach (var item in _sortColumns)
            {
                x++;
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
            var result = General.SetValue("Url", "/api/"+_table) + General.SetValue("Adaptor", "Adaptors.WebApiAdaptor");
            result = General.CreateRow("EjsDataManager", result, indent);
            return result;
        }


        private string DataSourceSettings(bool setSettings = true)
        {
            //var result = SetValue("DataSource","@GridData");
            var result = General.SetValue("@ref", "@Grid");
            result += General.SetValue("TValue",_table);

            if (setSettings)
            {
                BuildSnippet(null);
                
                //Use Context Menu
                if (ConfigValue("UseContextMenu"))
                    result += ContextMenuString();

                //Use GridLines
                if (ConfigValue("UseGridLines"))
                    result += General.SetValue("GridLines","GridLine.Both");

                //Use Grouping
                if (ConfigValue("AllowGrouping"))
                    result += General.SetValue("AllowGrouping");

                result = (result + General.SetValue("AllowPaging")); 
            }
            return result;
        }

        private string GetConfigValue(string setting)
        {
            return _gridConfiguration.ContainsKey(setting) ? _gridConfiguration[setting] : null;
        }


        private bool ConfigValue(string setting)
        {
            var result = false;
            var value = GetConfigValue(setting);

            if (!value.IsBlank())
            {
                value = value.ToLower();
                if (value == "yes" || value == "true" || value == "1")
                    result = true;
            }
            return result;
        }
    }
}