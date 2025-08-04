using TerminalEditorV2.Models;
using TerminalEditorV2.Persistence;
using System.Collections.Generic;

namespace TerminalEditorV2.Services
{
    public class KeymapService : IKeymapService
    {
        private readonly IKeymapRepository _keymapRepository;
        private readonly KeyTranslatorService _keyTranslator;

        public KeymapService(IKeymapRepository keymapRepository, KeyTranslatorService keyTranslator)
        {
            _keymapRepository = keymapRepository;
            _keyTranslator = keyTranslator;
        }

        public List<string> GetKeymapNames()
        {
            return _keymapRepository.GetKeymapNames();
        }

        public Keymap GetKeymap(string name)
        {
            var keymap = _keymapRepository.GetKeymap(name);
            var keymapNames = _keymapRepository.GetKeymapNames();
            foreach (var key in keymap.Keys)
            {
                _keyTranslator.TranslateFromZmk(key, keymapNames);
            }
            return keymap;
        }

        public void UpdateKey(Keymap keymap, int keyIndex, string newKeyPress, string newKeyAction)
        {
            var key = keymap.Keys[keyIndex];
            key.KeyPress = newKeyPress;
            key.KeyAction = newKeyAction;
            _keyTranslator.TranslateToZmk(key);
            _keymapRepository.SaveKeymap(keymap);
        }

        public void CreateNewKeymap(string name)
        {
            _keymapRepository.CreateNewKeymap(name);
        }

        public List<string> GetAllPresses()
        {
            return _keyTranslator.GetAllPresses();
        }

        public List<string> GetAllActions()
        {
            return _keyTranslator.GetAllActions();
        }

        public string GetZmkPress(string keyPress, string keyAction)
        {
            return _keyTranslator.GetZmkPress(keyPress, keyAction);
        }

        public string GetZmkAction(string keyAction, string keyPress)
        {
            return _keyTranslator.GetZmkAction(keyAction, keyPress);
        }
    }
}