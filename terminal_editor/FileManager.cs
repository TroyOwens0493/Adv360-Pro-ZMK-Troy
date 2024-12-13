using System.Text.RegularExpressions;
class FileManager
{
    //Attr
    private string _keymapFilePath = "";
    private string _macroFilePath = "";

    //Methods
    public FileManager(string keymapPath, string macroPath)
    {
        _keymapFilePath = keymapPath;
        _macroFilePath = macroPath;
    }

    public List<string> ParseKeymapNames()
    {
        var keymap = new List<List<List<string>>>();
        bool isKeymapLine = false;
        string prevLine = "";
        List<string> keymapNames = new();

        using (var reader = new StreamReader(_keymapFilePath))
        {
            // Set up vars for iterating
            string? line;
            while ((line = reader.ReadLine()) != null)
            {

                string cleanLine = line.Trim();

                //Find the start of the keymap
                if (cleanLine == "bindings = <")
                {
                    isKeymapLine = true;
                    continue;
                }

                //Store the line before the map because that is the name
                if (isKeymapLine)
                {
                    //Delete the space an curly brace
                    var seperatedLine = prevLine.Split(" ");
                    keymapNames.Add(seperatedLine[0]);
                    isKeymapLine = false;
                }
                //Store the line for later use
                prevLine = cleanLine;
            }
        }

        return keymapNames;
    }

    public List<Key> ParseKeysInMap(string keyMapName)
    {
        var keymap = new List<Key>();
        bool isKeymapLine = false;
        int i = 0;
        int zmkIndex = 0;
        bool isCorrectKeymap = false;

        using (var reader = new StreamReader(_keymapFilePath))
        {
            // Set up vars for iterating
            string? line;
            while ((line = reader.ReadLine()) != null)
            {

                string cleanLine = line.Trim();

                //Start parsing if we are on a keymap line
                if (cleanLine.StartsWith(keyMapName))
                {
                    isCorrectKeymap = true;
                }

                if (cleanLine == "bindings = <" && isCorrectKeymap)
                {
                    isKeymapLine = true;
                    continue;
                }

                if (isKeymapLine)
                {
                    if (i < 5) //Add the keymap lines
                    {
                        var keys = Regex.Split(cleanLine, @"(?=&)")
                                        .Where(part => !string.IsNullOrWhiteSpace(part))
                                        .ToList();
                        foreach (string key in keys)
                        {
                            //Get the key info and make the key obj.
                            string[] keyInfo = key.Split(' ');
                            string keyPress = keyInfo[0];
                            string keyAction;
                            if (keyInfo.Count() >= 2)
                            {
                                keyAction = keyInfo[1];
                            }
                            else
                            {
                                keyAction = "";
                            }
                            Key newKey = new(zmkIndex, keyAction, keyPress);
                            keymap.Add(newKey);
                        }
                        i++;
                    }
                    else
                    {
                        i = 0;
                        isKeymapLine = false;
                        isCorrectKeymap = false;
                    }
                }
            }
        }
        return keymap;
    }

    public void MakeNewKeymap(string keymapName)
    {
        //Default vals for the keymap
        List<Key> defaultLayout = new();
        // List of keypresses
        List<string> actions = new List<string>
        {
            "EQUAL", "N1", "N2", "N3", "N4", "N5", "1", "2", "N6", "N7", "N8", "N9", "N0", "MINUS",
            "TAB", "Q", "W", "E", "R", "T", "NONE", "NONE", "Y", "U", "I", "O", "P", "BSLH",
            "ESC", "A", "S", "D", "F", "G", "NONE", "LEFT_CONTROL", "LEFT_ALT", "LEFT_COMMAND", "RIGHT_CONTROL", "NONE", "H", "J", "K", "L", "SEMI", "SQT",
            "LSHFT", "Z", "X", "C", "V", "B", "HOME", "PAGE_UP", "N", "M", "COMMA", "DOT", "FSLH", "RSHFT",
            "2", "GRAVE", "CAPS", "LEFT", "RIGHT", "BSPC", "DEL", "END", "PAGE_DOWN", "ENTER", "SPACE", "UP", "DOWN", "LBKT", "RBKT", "2"
        };

        // List of actions
        List<string> presses = new List<string>
        {
            "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&tog", "&mo", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp",
            "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&none", "&none", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp",
            "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&none", "&kp", "&kp", "&kp", "&kp", "&none", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp",
            "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp",
            "&mo", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&kp", "&mo"
        };

        //Build the default layout from the raw data.
        for (int itr = 0; itr < presses.Count(); itr++)
        {
            var keyPress = presses[itr];
            var keyAction = actions[itr];
            Key newKey = new Key(itr, keyAction, keyPress);
            defaultLayout.Add(newKey);
        }

        //Write the raw data
        RightSide newLayout = new(defaultLayout);
        WriteKeymap(keymapName, newLayout);
    }

    public void WriteKeymap(string keymapName, Keymap sideKeymap)
    {
        // Extract layout and initialize the helper object
        var layout = sideKeymap.GetLayout();
        RightSide keyMap = new RightSide(layout);

        // Read existing file lines
        string[] lines = File.ReadAllLines(_keymapFilePath);

        // Replace the keymap or append a new one if it doesn't exist
        var updatedLines = FindAndReplaceOrAddKeymap(lines, keymapName, keyMap, layout);

        // Write the updated lines back to the file
        WriteDataToFile(_keymapFilePath, updatedLines);
    }

    private string BuildKeymapRow(RightSide keyMap, List<Key> layout, ref int currentKeyIndex)
    {
        string row = " ";
        do
        {
            // Stop if we reach the maximum number of keys
            if (currentKeyIndex >= layout.Count())
            {
                break;
            }

            var currentKey = layout[currentKeyIndex];
            row += $"{currentKey.GetZmkPress()} {currentKey.GetZmkAction()} ";
            currentKeyIndex++;

            // Stop writing to the row when reaching the end of a row
            if (keyMap.GetRowEnds().Contains(currentKeyIndex - 1))
            {
                break;
            }

        } while (true);

        return row;
    }

    // Function to find the target keymap in the file, replace its rows, or add a new keymap
private string[] FindAndReplaceOrAddKeymap(string[] lines, string keymapName, RightSide keyMap, List<Key> layout)
{
    List<string> updatedLines = new List<string>();
    bool isInTargetKeymap = false;
    int currentKeyIndex = 0;
    int rowCount = 0;

    for (int i = 0; i < lines.Length; i++)
    {
        string currentLine = lines[i];

        // Detect start of the target keymap
        if (currentLine.Contains(keymapName) && currentLine.Contains("{"))
        {
            isInTargetKeymap = true;
            updatedLines.Add(currentLine);
            continue;
        }

        // If we're in the target keymap and haven't completed replacing rows
        if (isInTargetKeymap)
        {
            // Look for the bindings section
            if (currentLine.Contains("bindings = <"))
            {
                updatedLines.Add(currentLine);
                
                // Replace the next 6 lines with new keymap rows
                for (int row = 0; row < 6; row++)
                {
                    if (i + row + 1 < lines.Length)
                    {
                        updatedLines.Add(BuildKeymapRow(keyMap, layout, ref currentKeyIndex));
                    }
                }

                // Skip the original keymap rows
                i += 6;
                rowCount = 6;
                continue;
            }

            // End of the keymap block
            if (currentLine.Contains(">;") || currentLine.Contains("};"))
            {
                updatedLines.Add(currentLine);
                isInTargetKeymap = false;
                currentKeyIndex = 0;
                rowCount = 0;
                continue;
            }
        }

        // If not in target keymap or not replacing, add the original line
        if (!isInTargetKeymap)
        {
            updatedLines.Add(currentLine);
        }
    }

    // If keymap was not found, append a new keymap
    if (!updatedLines.Any(line => line.Contains(keymapName)))
    {
        updatedLines.AddRange(AddNewKeymap(lines, keymapName, keyMap, layout));
    }

    return updatedLines.ToArray();
}
    // Function to append a new keymap at the end of the file
    private string[] AddNewKeymap(string[] lines, string keymapName, RightSide keyMap, List<Key> layout)
    {
        var newLines = new List<string>(lines);

        // Find the last keymap closing brace "};"
        int lastKeymapIndex = Array.FindLastIndex(lines, line => line.Trim() == "};");

        // Add the new keymap at the end
        var newKeymap = new List<string>
    {
        $"        {keymapName} {{",
        $"            bindings = <"
    };

        int currentKeyIndex = 0;
        for (int row = 0; row < 6; row++)
        {
            newKeymap.Add($" {BuildKeymapRow(keyMap, layout, ref currentKeyIndex)}");
        }

        newKeymap.Add("            >;");
        newKeymap.Add("        };");

        // Insert the new keymap after the last keymap
        if (lastKeymapIndex >= 0)
        {
            newLines.InsertRange(lastKeymapIndex, newKeymap);
        }
        else
        {
            // If no keymap is found, append at the end
            newLines.AddRange(newKeymap);
        }

        return newLines.ToArray();
    }

    public List<string> ParseMacroNames()
    {
        string[] lines = File.ReadAllLines(_macroFilePath);
        List<string> macroNames = new();
        foreach (string line in lines)
        {
            if (line.EndsWith("{"))
            {
                var rawName = line.Split(" ");
                string cleanName = rawName[1].Remove(rawName[1].Length - 1);
                macroNames.Add(cleanName);
            }
        }

        return macroNames;
    }

    //Macro parser written partially by chatgpt because of time constraints
    public Macro ParseZmkMacro(string zmkMacro)
    {
        List<MacroAction> newActions = new();

        // Regex to extract bindings
        var bindingsMatch = Regex.Match(zmkMacro, @"bindings\s*=\s*<(.*?)>;", RegexOptions.Singleline);
        if (bindingsMatch.Success)
        {
            string bindingsContent = bindingsMatch.Groups[1].Value;

            // Regex to parse individual bindings
            var individualBindings = Regex.Matches(bindingsContent, @"&kp\s+(\w+)\((\w+)\)");

            foreach (Match bindingMatch in individualBindings)
            {
                if (bindingMatch.Success)
                {
                    string modifier = bindingMatch.Groups[1].Value;
                    string baseAction = bindingMatch.Groups[2].Value;

                    MacroAction action;
                    if (!string.IsNullOrEmpty(modifier))
                    {
                        action = new MacroAction(modifier, baseAction);
                    }
                    else
                    {
                        action = new MacroAction("", baseAction);
                    }

                    newActions.Add(action);
                }
            }
        }

        Macro macro = new Macro(newActions);

        return macro;
    }

    //Macro loader wirtten partially by chatgpt because of time constraints
    public Macro LoadMacroFromFile(string macroName)
    {
        string fileContent = File.ReadAllText(_macroFilePath);

        // Find the macro block by name
        var macroMatch = Regex.Match(fileContent, @$"{macroName}:.*?{{(.*?)}}", RegexOptions.Singleline);
        if (macroMatch.Success)
        {
            string macroContent = macroMatch.Groups[0].Value;
            return ParseZmkMacro(macroContent);
        }

        throw new Exception($"Macro '{macroName}' not found in {_macroFilePath}");
    }
    public void MakeNewMacro(string macroName)
    {
    }

    //Macro writer written partially by chatgpt because of time constraints
    public void WriteMacroToFile(Macro macro, string macroName)
    {
        string macroDefinition = GenerateMacroDefinition(macro, macroName);
        string fileContent = File.ReadAllText(_macroFilePath);

        string pattern = $@"{macroName}: {macroName}\{{.*?}};";
        Regex regex = new Regex(pattern, RegexOptions.Singleline);

        if (regex.IsMatch(fileContent))
        {
            fileContent = regex.Replace(fileContent, macroDefinition);
        }
        else
        {
            fileContent += Environment.NewLine + macroDefinition;
        }

        File.WriteAllText(_macroFilePath, fileContent);
    }

    //Generate macro def written partially by chatgpt because of time constraints
    private string GenerateMacroDefinition(Macro macro, string macroName)
    {
        var macroData = new System.Text.StringBuilder();
        macroData.AppendLine($"{macroName}: {macroName}{{");
        macroData.AppendLine("compatible = \"zmk,behavior-macro\";");
        macroData.AppendLine($"label = \"{macroName}\";");
        macroData.AppendLine("#binding-cells = <0>;");
        macroData.Append("bindings = <");

        List<MacroAction> actions = macro.GetMacro();
        for (int i = 0; i < actions.Count; i++)
        {
            MacroAction action = actions[i];
            if (action.GetModifier() != "")
            {
                macroData.Append($"&kp {action.GetZmkModifier()}({action.GetZmkAction()})");
            }
            else
            {
                macroData.Append($"&kp {action.GetAction()}");
            }

            if (i < actions.Count - 1)
            {
                macroData.Append(">, <");
            }
        }

        macroData.AppendLine(">;");
        macroData.AppendLine("};");

        return macroData.ToString();
    }

    private void WriteDataToFile(string filePath, string[] data)
    {
        //Write to file
        using (var writer = new StreamWriter(filePath))
        {
            foreach (string line in data)
            {
                writer.WriteLine(line);
            }
        }
    }
}
