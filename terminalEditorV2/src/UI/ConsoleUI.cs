using TerminalEditorV2.Models;
using TerminalEditorV2.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TerminalEditorV2.UI
{
    public class ConsoleUI
    {
        private readonly IKeymapService _keymapService;
        private readonly IMacroService _macroService;

        public ConsoleUI(IKeymapService keymapService, IMacroService macroService)
        {
            _keymapService = keymapService;
            _macroService = macroService;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Kinesis Keyboard Terminal Editor V2");
                Console.WriteLine("1. Edit a layer");
                Console.WriteLine("2. Create a new layer");
                Console.WriteLine("3. Edit a macro");
                Console.WriteLine("4. Create a new macro");
                Console.WriteLine("5. Exit");

                switch (GetIntegerInput(1, 5))
                {
                    case 1: EditLayer(); break;
                    case 2: CreateLayer(); break;
                    case 3: EditMacro(); break;
                    case 4: CreateMacro(); break;
                    case 5: return;
                }
            }
        }

        private void EditLayer()
        {
            var keymapNames = _keymapService.GetKeymapNames();
            Console.Clear();
            Console.WriteLine("Choose a layer to edit:");
            PrintList(keymapNames);
            int selection = GetIntegerInput(1, keymapNames.Count) - 1;
            string selectedKeymap = keymapNames[selection];

            var keymap = _keymapService.GetKeymap(selectedKeymap);
            
            string side = KeyBoardSideSelector();

            if(side == "left")
            {
                DisplayKeymap(keymap, side);
            }
            else
            {
                DisplayKeymap(keymap, side);
            }

            Console.Write("\nEnter the number of the key to edit: ");
            int keyIndex = GetIntegerInput(1, keymap.Keys.Count) - 1;

            var keyToEdit = keymap.Keys[keyIndex];
            Console.WriteLine($"\nEditing Key {keyIndex + 1}: {keyToEdit.KeyPress} - {keyToEdit.KeyAction}");

            var newKeyPress = EditKeyPressType(keyToEdit);
            var newKeyAction = EditKeyAction(newKeyPress);

            if(newKeyPress.KeyPress != null && newKeyAction.KeyAction != null)
            {
                _keymapService.UpdateKey(keymap, keyIndex, newKeyPress.KeyPress, newKeyAction.KeyAction);
            }
            
            Console.WriteLine("\nKey updated. Press any key to continue...");
            Console.ReadKey();
        }

        private void CreateLayer()
        {
            Console.Clear();
            Console.Write("Enter the name for the new layer: ");
            string? name = Console.ReadLine();
            if(!string.IsNullOrEmpty(name))
            {
                _keymapService.CreateNewKeymap(name);
            }
            Console.WriteLine($"\nLayer '{name}' created. Press any key to continue...");
            Console.ReadKey();
        }

        private void EditMacro()
        {
            Console.Clear();
            var macroNames = _macroService.GetMacroNames();
            PrintList(macroNames);
            Console.WriteLine("Which macro would you like to edit?");
            int macroIndex = GetIntegerInput(1, macroNames.Count) - 1;
            var selectedMacro = _macroService.GetMacro(macroNames[macroIndex]);

            string addOrDelete = AddOrDeleteFromMacro();

            if (addOrDelete == "add")
            {
                var newMacro = AddToMacro(selectedMacro);
                if (newMacro.Actions.Any())
                {
                    var lastAction = newMacro.Actions.Last();
                    if (lastAction.Modifier != null && lastAction.Action != null)
                    {
                        _macroService.AddActionToMacro(newMacro, lastAction.Modifier, lastAction.Action);
                    }
                }
            }
            else
            {
                var newMacro = DeleteFromMacro(selectedMacro);
                _macroService.RemoveActionFromMacro(newMacro, newMacro.Actions.Count);
            }

            Console.WriteLine("\nMacro updated. Press any key to continue...");
            Console.ReadKey();
        }

        private void CreateMacro()
        {
            Console.Clear();
            Console.WriteLine("What would you like your new macro to be called");
            string? macroName = Console.ReadLine();
            if (!string.IsNullOrEmpty(macroName))
            {
                if (!macroName.StartsWith("macro"))
                {
                    macroName = $"macro_{macroName}";
                }
                _macroService.CreateNewMacro(macroName);
                var newMacro = _macroService.GetMacro(macroName);
                do
                {
                    AddToMacro(newMacro);
                    Console.Clear();
                    Console.WriteLine("Would you like to continue adding to this macro? (y/n)");
                    var res = Console.ReadLine();
                    if (res == "y")
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                } while (true);
            }
        }

        private string AddOrDeleteFromMacro()
        {
            Console.Clear();
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1. Add something 2. Delete something");

            int res = GetIntegerInput(1, 2);
            if (res == 1)
            {
                return "add";
            }
            else
            {
                return "delete";
            }
        }

        private Macro AddToMacro(Macro macroToEdit)
        {
            Console.Clear();
            Console.WriteLine("Will this have a modifier key? (y/n)");
            var res = Console.ReadLine();
            var macroModKeys = _keymapService.GetAllActions();
            var keyActions = _keymapService.GetAllActions();

            if (res == "y")
            {
                Console.Clear();
                PrintList(macroModKeys);
                Console.WriteLine("Which mod key would you like to add?");
                int modKeyIndex = GetIntegerInput(1, macroModKeys.Count) - 1;
                var mod = _keymapService.GetZmkAction(macroModKeys[modKeyIndex], "&kp");
                Console.Clear();
                PrintList(keyActions);
                Console.WriteLine("What action would you like to perform with this mod key?");
                int actionIndex = GetIntegerInput(1, keyActions.Count) - 1;
                var action = _keymapService.GetZmkAction(keyActions[actionIndex], "&kp");
                MacroAction macroKey = new MacroAction { Modifier = mod, Action = action };
                macroToEdit.Actions.Add(macroKey);
            }
            else
            {
                Console.Clear();
                PrintList(keyActions);
                Console.WriteLine("What action would you like to add?");
                int actionIndex = GetIntegerInput(1, keyActions.Count) - 1;
                var action = _keymapService.GetZmkAction(keyActions[actionIndex], "&kp");
                MacroAction keyToAdd = new MacroAction { Modifier = "", Action = action };
                macroToEdit.Actions.Add(keyToAdd);
            }

            return macroToEdit;
        }

        private Macro DeleteFromMacro(Macro macroToEdit)
        {
            Console.Clear();
            var macroKeys = macroToEdit.Actions;
            List<string> macroActions = new();
            foreach (MacroAction action in macroKeys)
            {
                if (action.Modifier != "")
                {
                    macroActions.Add($"{action.Modifier}({action.Action})");
                }
                else
                {
                    if(action.Action != null)
                    {
                        macroActions.Add(action.Action);
                    }
                }
            }
            PrintList(macroActions);
            Console.WriteLine("What action would you like to delete?");
            int actionToDeleteIndex = GetIntegerInput(1, macroActions.Count) - 1;
            macroToEdit.Actions.RemoveAt(actionToDeleteIndex);
            return macroToEdit;
        }

        private void DisplayKeymap(Keymap keymap)
        {
            Console.Clear();
            Console.WriteLine($"--- {keymap.Name} ---");
            for (int i = 0; i < keymap.Keys.Count; i++)
            {
                var key = keymap.Keys[i];
                Console.WriteLine($"{i + 1,2}. {key.KeyPress, -25} {key.KeyAction}");
                if ((i + 1) % 7 == 0) Console.WriteLine(); // Simple row breaks
            }
        }

        private void DisplayKeymap(Keymap keymap, string side)
        {
            Console.Clear();
            Console.WriteLine($"--- {keymap.Name} ---");
            if(side == "left")
            {
                for (int i = 0; i < keymap.Keys.Count / 2; i++)
                {
                    var key = keymap.Keys[i];
                    Console.WriteLine($"{i + 1,2}. {key.KeyPress, -25} {key.KeyAction}");
                    if ((i + 1) % 7 == 0) Console.WriteLine(); // Simple row breaks
                }
            }
            else
            {
                for (int i = keymap.Keys.Count / 2; i < keymap.Keys.Count; i++)
                {
                    var key = keymap.Keys[i];
                    Console.WriteLine($"{i + 1,2}. {key.KeyPress, -25} {key.KeyAction}");
                    if ((i + 1) % 7 == 0) Console.WriteLine(); // Simple row breaks
                }
            }
        }

        private void PrintList(List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {list[i]}");
            }
        }

        private int GetIntegerInput(int min, int max)
        {
            int value;
            while (true)
            {
                Console.Write($"> ");
                if (int.TryParse(Console.ReadLine(), out value) && value >= min && value <= max)
                {
                    return value;
                }
                Console.WriteLine($"Invalid input. Please enter a number between {min} and {max}.");
            }
        }

        private string KeyBoardSideSelector()
        {
            Console.Clear();
            Console.WriteLine("Please choose which side you would like to edit.");
            Console.WriteLine("1. Left 2. Right");
            int side = GetIntegerInput(1, 2);
            if (side == 1)
            {
                return "left";
            }
            else
            {
                return "right";
            }
        }

        private Key EditKeyPressType(Key keyToEdit)
        {
            Console.Clear();

            var presses = _keymapService.GetAllPresses();
            PrintList(presses);
            Console.WriteLine($"The currennt keypress for this key is {keyToEdit.KeyPress}");
            Console.WriteLine("What would you like the new keypress to be? ");

            int pressIndex = GetIntegerInput(1, presses.Count) - 1;
            var newKeyPress = presses[pressIndex];

            keyToEdit.KeyPress = newKeyPress;
            var newZmkPress = _keymapService.GetZmkPress(newKeyPress, keyToEdit.KeyAction);

            keyToEdit.ZmkPress = newZmkPress;
            return keyToEdit;
        }

        private Key EditKeyAction(Key keyToEdit)
        {
            Console.Clear();
            var actions = _keymapService.GetAllActions();
            var keyAction = keyToEdit.KeyAction;
            var zmkPress = keyToEdit.ZmkKeyPress;
            if (zmkPress == "&trans" || zmkPress == "&none")
            {
                keyToEdit.ZmkKeyAction = "";
                return keyToEdit;
            }
            else if (zmkPress == "&mo" || zmkPress == "&tog")
            {
                var keyMapNames = _keymapService.GetKeymapNames();
                PrintList(keyMapNames);
                Console.WriteLine("What layer would you like this to toggle?");
                int res = GetIntegerInput(1, keyMapNames.Count) - 1;
                keyToEdit.ZmkKeyAction = res.ToString();
                return keyToEdit;
            }
            else if (zmkPress.StartsWith("&macro"))
            {
                Console.Clear();
                var macroNames = _macroService.GetMacroNames();
                PrintList(macroNames);
                Console.WriteLine("What macro would you like this to run?");
                int res = GetIntegerInput(1, macroNames.Count) - 1;
                keyToEdit.ZmkKeyPress = $"&{macroNames[res]}";
                keyToEdit.ZmkKeyAction = "";
                return keyToEdit;
            }
            else
            {
                PrintList(actions);
                Console.WriteLine($"The currennt key action for this key is {keyAction}");
                Console.WriteLine("What would you like the new action to be? ");

                int actionIndex = GetIntegerInput(1, actions.Count) - 1;
                var newAction = actions[actionIndex];

                keyToEdit.KeyAction = newAction;
                var newZmkAction = _keymapService.GetZmkAction(newAction, keyToEdit.KeyPress);

                keyToEdit.ZmkKeyAction = newZmkAction;
                return keyToEdit;
            }
        }
    }
}