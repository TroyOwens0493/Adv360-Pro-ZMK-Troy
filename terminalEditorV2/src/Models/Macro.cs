using System.Collections.Generic;

namespace TerminalEditorV2.Models
{
    public class Macro
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<MacroAction> Actions { get; set; } = new List<MacroAction>();
    }
}
