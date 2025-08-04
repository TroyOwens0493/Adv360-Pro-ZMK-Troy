
using TerminalEditorV2.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TerminalEditorV2.Persistence
{
    public class KeymapFileRepository : IKeymapRepository
    {
        private readonly string _keymapFilePath;
        private readonly KeymapParser _parser = new KeymapParser();

        public KeymapFileRepository(string keymapFilePath)
        {
            _keymapFilePath = keymapFilePath;
        }

        public List<string> GetKeymapNames()
        {
            return _parser.ParseKeymapNames(File.ReadAllLines(_keymapFilePath));
        }

        public Keymap GetKeymap(string name)
        {
            return _parser.ParseKeymap(File.ReadAllLines(_keymapFilePath), name);
        }

        public void SaveKeymap(Keymap keymap)
        {
            var lines = File.ReadAllLines(_keymapFilePath).ToList();
            var updatedLines = _parser.UpdateKeymap(lines, keymap);
            File.WriteAllLines(_keymapFilePath, updatedLines);
        }

        public void CreateNewKeymap(string name)
        {
            var lines = File.ReadAllLines(_keymapFilePath).ToList();
            var updatedLines = _parser.AddNewKeymap(lines, name);
            File.WriteAllLines(_keymapFilePath, updatedLines);
        }
    }
}
