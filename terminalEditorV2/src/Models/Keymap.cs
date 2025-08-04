using System.Collections.Generic;

namespace TerminalEditorV2.Models
{
    public class Keymap
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Key> Keys { get; set; } = new List<Key>();
    }
}