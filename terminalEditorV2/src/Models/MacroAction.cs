namespace TerminalEditorV2.Models
{
    public class MacroAction
    {
        public int Id { get; set; }
        public int MacroId { get; set; }
        public string? Modifier { get; set; }
        public string? Action { get; set; }
        public string? ZmkModifier { get; set; }
        public string? ZmkAction { get; set; }
    }
}
