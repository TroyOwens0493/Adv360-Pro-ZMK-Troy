using TerminalEditorV2.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TerminalEditorV2.Persistence
{
    public class KeymapParser
    {
        public List<string> ParseKeymapNames(string[] lines)
        {
            var keymapNames = new List<string>();
            string prevLine = "";

            for (int i = 0; i < lines.Length; i++)
            {
                string cleanLine = lines[i].Trim();
                if (cleanLine == "bindings = <")
                {
                    // Look at the previous line for the keymap name
                    string potentialNameLine = lines[i - 1].Trim();
                    if (potentialNameLine.EndsWith("{"))
                    {
                        var parts = potentialNameLine.Split(' ');
                        keymapNames.Add(parts[0]);
                    }
                }
            }
            return keymapNames;
        }

        public Keymap ParseKeymap(string[] lines, string name)
        {
            var keymap = new Keymap { Name = name };
            bool isCorrectKeymap = false;
            bool isBindingsSection = false;
            int zmkIndex = 0;

            foreach (var line in lines)
            {
                string cleanLine = line.Trim();

                if (cleanLine.Contains(name) && cleanLine.EndsWith("{"))
                {
                    isCorrectKeymap = true;
                    continue;
                }

                if (isCorrectKeymap && cleanLine == "bindings = <")
                {
                    isBindingsSection = true;
                    continue;
                }

                if (isBindingsSection)
                {
                    if (cleanLine.Contains(">;"))
                    {
                        break; // End of bindings
                    }

                    var keys = Regex.Split(cleanLine, @"(?=&)")
                                    .Where(part => !string.IsNullOrWhiteSpace(part))
                                    .ToList();

                    foreach (string keyString in keys)
                    {
                        var keyParts = keyString.Trim().Split(' ');
                        string keyPress = keyParts[0];
                        string keyAction = keyParts.Length > 1 ? keyParts[1] : "";
                                                keymap.Keys.Add(new Key { Position = zmkIndex, ZmkKeyPress = keyPress, ZmkKeyAction = keyAction });
                        zmkIndex++;
                    }
                }
            }
            return keymap;
        }

        public List<string> UpdateKeymap(List<string> lines, Keymap keymap)
        {
            var updatedLines = new List<string>();
            bool inTargetKeymap = false;
            int keyIndex = 0;

            for (int i = 0; i < lines.Count; i++)
            {
                string cleanLine = lines[i].Trim();

                if (cleanLine.Contains(keymap.Name) && cleanLine.EndsWith("{"))
                {
                    inTargetKeymap = true;
                    updatedLines.Add(lines[i]);
                    continue;
                }

                if (inTargetKeymap)
                {
                    if (cleanLine.StartsWith("bindings = <"))
                    {
                        updatedLines.Add(lines[i]);
                        // Skip existing binding lines
                        while (!lines[i + 1].Trim().StartsWith(">;"))
                        {
                            i++;
                        }

                        // Add new binding lines
                        for (int row = 0; row < 6; row++)
                        {
                            updatedLines.Add(BuildKeymapRow(keymap.Keys, ref keyIndex));
                        }
                    }
                    else if (cleanLine.StartsWith(">;"))
                    {
                        updatedLines.Add(lines[i]);
                        inTargetKeymap = false;
                    }
                    else if (!inTargetKeymap)
                    {
                        updatedLines.Add(lines[i]);
                    }
                }
                else
                {
                    updatedLines.Add(lines[i]);
                }
            }
            return updatedLines;
        }

        public List<string> AddNewKeymap(List<string> lines, string name)
        {
            var newKeymap = new Keymap { Name = name, Keys = GetDefaultLayout() };
            var newLines = new List<string>(lines);

            int lastKeymapIndex = newLines.FindLastIndex(line => line.Trim() == "};");

            var keymapLines = new List<string>
            {
                $"        {name} {{",
                $"            bindings = <"
            };

            int keyIndex = 0;
            for (int row = 0; row < 6; row++)
            {
                keymapLines.Add(BuildKeymapRow(newKeymap.Keys, ref keyIndex));
            }

            keymapLines.Add("            >;");
            keymapLines.Add("        };");

            if (lastKeymapIndex != -1)
            {
                newLines.InsertRange(lastKeymapIndex + 1, keymapLines);
            }
            else
            {
                newLines.AddRange(keymapLines);
            }

            return newLines;
        }

        private string BuildKeymapRow(List<Key> keys, ref int keyIndex)
        {
            // This is a simplified version. The original had complex row end logic.
            // For a robust solution, this would need to be more intelligent.
            var rowKeys = new List<string>();
            int keysInRow = (keyIndex < 42) ? 7 : (keyIndex < 60 ? 6 : 5);
            if (keyIndex > 65) keysInRow = 3; // Thumb cluster approximation

            for (int i = 0; i < keysInRow && keyIndex < keys.Count; i++)
            {
                var key = keys[keyIndex++];
                rowKeys.Add($"{key.ZmkKeyPress} {key.ZmkKeyAction}");
            }
            return "            " + string.Join(" ", rowKeys);
        }

        private List<Key> GetDefaultLayout()
        {
            var defaultLayout = new List<Key>();
            var actions = new List<string> { "EQUAL", "N1", "N2", "N3", "N4", "N5", "Y", "U", "I", "O", "P", "BSLH", "H", "J", "K", "L", "SEMI", "SQT", "N", "M", "COMMA", "DOT", "FSLH", "RSHFT", "UP", "DOWN", "LBKT", "RBKT" };
            var presses = new List<string> { "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp" };
            for (int i = 0; i < 76; i++)
            {
                if (i < actions.Count)
                {
                    defaultLayout.Add(new Key { Position = i, ZmkKeyPress = presses[i], ZmkKeyAction = actions[i] });
                }
                else
                {
                    defaultLayout.Add(new Key { Position = i, ZmkKeyPress = "&none", ZmkKeyAction = "" }); // Pad with &none
                }
            }
            return defaultLayout;
        }
    }
}