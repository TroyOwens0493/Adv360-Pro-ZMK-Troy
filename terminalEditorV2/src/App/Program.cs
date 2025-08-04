using TerminalEditorV2.Models;
using TerminalEditorV2.Persistence;
using TerminalEditorV2.Services;
using TerminalEditorV2.UI;

namespace TerminalEditorV2.App
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configuration
            var config = new AppConfig
            {
                KeymapFilePath = "../config/adv360.keymap",
                MacroFilePath = "../config/macros.dtsi"
            };

            // Dependency Injection Setup
            var keymapRepo = new KeymapFileRepository(config.KeymapFilePath);
            var macroRepo = new MacroFileRepository(config.MacroFilePath);
            var keyTranslator = new KeyTranslatorService();
            var keymapService = new KeymapService(keymapRepo, keyTranslator);
            var macroService = new MacroService(macroRepo, keyTranslator);
            var consoleUI = new ConsoleUI(keymapService, macroService);

            // Run the application
            consoleUI.Run();
        }
    }
}