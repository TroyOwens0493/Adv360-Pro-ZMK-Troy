using TerminalEditorV2.Models;
using TerminalEditorV2.Persistence;
using System.Collections.Generic;

namespace TerminalEditorV2.Services
{
    public class MacroService : IMacroService
    {
        private readonly IMacroRepository _macroRepository;
        private readonly KeyTranslatorService _keyTranslator;

        public MacroService(IMacroRepository macroRepository, KeyTranslatorService keyTranslator)
        {
            _macroRepository = macroRepository;
            _keyTranslator = keyTranslator;
        }

        public List<string> GetMacroNames()
        {
            return _macroRepository.GetMacroNames();
        }

        public Macro GetMacro(string name)
        {
            var macro = _macroRepository.GetMacro(name);
            foreach (var action in macro.Actions)
            {
                // Placeholder for ZMK translation
            }
            return macro;
        }

        public void AddActionToMacro(Macro macro, string modifier, string action)
        {
            var newAction = new MacroAction { Modifier = modifier, Action = action };
            // Placeholder for ZMK translation
            macro.Actions.Add(newAction);
            _macroRepository.SaveMacro(macro);
        }

        public void RemoveActionFromMacro(Macro macro, int actionIndex)
        {
            macro.Actions.RemoveAt(actionIndex);
            _macroRepository.SaveMacro(macro);
        }

        public void CreateNewMacro(string name)
        {
            _macroRepository.CreateNewMacro(name);
        }
    }
}