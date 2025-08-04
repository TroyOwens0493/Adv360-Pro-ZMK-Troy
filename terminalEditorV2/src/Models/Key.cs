namespace TerminalEditorV2.Models
{
    public class Key
    {
        public int Id { get; set; }
        public int KeymapId { get; set; }
        public int Position { get; set; }
        public string? ZmkKeyAction { get; set; }
        public string? ZmkKeyPress { get; set; }
        public string? KeyAction { get; set; }
        public string? KeyPress { get; set; }
    }
}
