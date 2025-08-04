
using TerminalEditorV2.Models;
using System.Collections.Generic;

namespace TerminalEditorV2.Services
{
    public interface IMacroService
    {
        List<string> GetMacroNames();
        Macro GetMacro(string name);
        void AddActionToMacro(Macro macro, string modifier, string action);
        void RemoveActionFromMacro(Macro macro, int actionIndex);
        void CreateNewMacro(string name);
    }
}
