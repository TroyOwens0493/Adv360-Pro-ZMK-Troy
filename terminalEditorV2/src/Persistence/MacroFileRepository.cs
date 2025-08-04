
using TerminalEditorV2.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TerminalEditorV2.Persistence
{
    public class MacroFileRepository : IMacroRepository
    {
        private readonly string _macroFilePath;
        private readonly MacroParser _parser = new MacroParser();

        public MacroFileRepository(string macroFilePath)
        {
            _macroFilePath = macroFilePath;
        }

        public List<string> GetMacroNames()
        {
            return _parser.ParseMacroNames(File.ReadAllLines(_macroFilePath));
        }

        public Macro GetMacro(string name)
        {
            return _parser.ParseMacro(File.ReadAllText(_macroFilePath), name);
        }

        public void SaveMacro(Macro macro)
        {
            var fileContent = File.ReadAllText(_macroFilePath);
            var updatedContent = _parser.UpdateMacro(fileContent, macro);
            File.WriteAllText(_macroFilePath, updatedContent);
        }

        public void CreateNewMacro(string name)
        {
            var fileContent = File.ReadAllText(_macroFilePath);
            var updatedContent = _parser.AddNewMacro(fileContent, name);
            File.WriteAllText(_macroFilePath, updatedContent);
        }
    }
}
