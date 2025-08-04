
using TerminalEditorV2.Models;
using System.Collections.Generic;

namespace TerminalEditorV2.Persistence
{
    public interface IMacroRepository
    {
        List<string> GetMacroNames();
        Macro GetMacro(string name);
        void SaveMacro(Macro macro);
        void CreateNewMacro(string name);
    }
}
