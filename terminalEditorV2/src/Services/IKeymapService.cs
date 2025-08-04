

using TerminalEditorV2.Models;
using System.Collections.Generic;

namespace TerminalEditorV2.Services
{
    public interface IKeymapService
    {
        List<string> GetKeymapNames();
        Keymap GetKeymap(string name);
        void UpdateKey(Keymap keymap, int keyIndex, string newKeyPress, string newKeyAction);
        void CreateNewKeymap(string name);
        List<string> GetAllPresses();
        List<string> GetAllActions();
        string GetZmkPress(string keyPress, string keyAction);
        string GetZmkAction(string keyAction, string keyPress);
    }
}

