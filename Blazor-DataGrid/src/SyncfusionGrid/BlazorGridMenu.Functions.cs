using System;
using System.Collections.Generic;
using System.Linq;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.Syncfusion.Grid
{
    public partial class BlazorGridMenu : ExpansionBase
    {
        private List<ISchemaItem> _tables;
        private Dictionary<string, string> _gridConfiguration;

        private void MainFunction()
        {
            var settings = GetExpansionString("GRID_SETTINGS");

            if (!settings.IsBlank())
            {
                var items = settings.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                _gridConfiguration = items.Select(item => item.Split('='))
                    .ToDictionary(keyValue => keyValue[0], keyValue => keyValue[1], StringComparer.InvariantCultureIgnoreCase);
            }
            _tables = GetTables();

            #region Razor tags

            AppendText();
            AppendText(GenerateMenus(8));

            #endregion Razor tags
        }

        private string GenerateMenus(int indent)
        {
            var topLi = "<li" + General.SetValue("class", "nav-item px-3") + ">";
            const string topNav = "<NavLink";
            const string bottomNav = "</NavLink>";
            const string bottomLi = "</li>";

            BuildSnippet(null);
            var x = 0;
            foreach (var table in _tables)
            {
                x++;
                var navLink = General.SetValue("class", "nav-link") + General.SetValue("href", table.TableName.Pluralize().ToLower());
                var span = General.SetValue("class", "oi oi-list-rich") + General.SetValue("aria-hidden");
                span = span.OpenTag("span") + span.CloseTag("span");
                BuildSnippet(topLi, indent);
                BuildSnippet(topNav + navLink +">", indent + 4);
                BuildSnippet(span + " " + table.TableName.Pluralize(), indent + 8);
                BuildSnippet(bottomNav, indent + 4);
                BuildSnippet(bottomLi, indent);
            }
            return BuildSnippet();
            //var row = General.CreateRow("span", span);
            //properties.OpenTag(tag) + properties.CloseTag(tag)
        }

    }
}