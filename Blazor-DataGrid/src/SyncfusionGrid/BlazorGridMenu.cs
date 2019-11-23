using System.Collections.Generic;
using System.ComponentModel.Composition;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;
using ZeraSystems.Syncfusion.Grid;

namespace ZeraSystems.Syncfusion.Grid
{
    /// <summary>
    /// There are 6 elements in the String Array used by the
    /// 0 - Publisher : This is the name of the publisher
    /// 1 - Title : This is the title of the Code Nanite
    /// 2 - Details : This is the
    /// 3 - Version Number
    /// 4 - Label : Label of the Code Nanite
    /// 5 - Namespace
    /// 6 - Release Date
    /// 7 - Name to use for Expander Label
    /// 8 - Indicates that the Nanite is Schema Dependent
    /// 9 - RESERVED
    /// 10 - RESERVED
    /// </summary>
    [Export(typeof(ICodeStencilCodeNanite))]
    [CodeStencilCodeNanite(new[]
    {
        "ZERA Systems Inc.",                    // 0
        "Generates sidebar menu for table(s)",    // 1
        "This Code Nanite will generate the code for side bar menu that links to the created grids.",                                  // 2
        "1.0",                                 // 3
        "BlazorGridMenu",                         // 4
        "ZeraSystems.Syncfusion.Grid",  // 5
        "11/21/2019",                          // 6
        "CS_BLAZOR_GRIDMENU",                     // 7
        "1",                                   // 8
        "",                                    // 9
        ""                                     // 10
    })]
    public partial class BlazorGridMenu: ExpansionBase, ICodeStencilCodeNanite
    {
        public string Input { get; set; }
        public string Output { get; set; }
        public int Counter { get; set; }
        public List<string> OutputList { get; set; }
        public List<ISchemaItem> SchemaItem { get; set; }
        public List<IExpander> Expander { get; set; }
        public List<string> InputList { get ; set; }

        public void ExecutePlugin()
        {
            Initializer(SchemaItem, Expander);
            MainFunction();
            Output = ExpandedText.ToString();
        }

    }
}
