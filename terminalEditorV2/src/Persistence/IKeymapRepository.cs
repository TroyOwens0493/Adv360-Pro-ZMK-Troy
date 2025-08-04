
using TerminalEditorV2.Models;
using System.Collections.Generic;

namespace TerminalEditorV2.Persistence
{
    public interface IKeymapRepository
    {
        List<string> GetKeymapNames();
        Keymap GetKeymap(string name);
        void SaveKeymap(Keymap keymap);
        void CreateNewKeymap(string name);
    }
}
