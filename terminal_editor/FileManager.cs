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

        //Make the skeleton for the new keymap
        //Vars
        string[] lines = File.ReadAllLines(_keymapFilePath);
        var fileLength = lines.Count();
        var indexToStartNewKeymap = fileLength - 2;
        string[] fileData = new string[fileLength + 8];
        Array.Copy(lines, 0, fileData, 0, indexToStartNewKeymap);
        //Write skeleton
        fileData[indexToStartNewKeymap] = keymapName + "{";
        fileData[indexToStartNewKeymap + 1] = "bindings = <";
        for (int i = 1; i < 6; i++)
        {
            fileData[indexToStartNewKeymap + i] = " ";
        }
        fileData[indexToStartNewKeymap + 6] = ">;";
        fileData[indexToStartNewKeymap + 7] = "};";
        WriteDataToFile(_keymapFilePath, fileData);

        //Write the raw data
        RightSide newLayout = new(defaultLayout);
        WriteKeymap(keymapName, newLayout);
        FixLayoutFile(keymapName);
    }

    private void FixLayoutFile(string newKeymapName)
    {
    }

    public void WriteKeymap(string keymapName, Keymap sideKeymap)
    {
        //Vars for writing to file
        var layout = sideKeymap.GetLayout();
        RightSide keyMap = new RightSide(layout);
        string[] lines = File.ReadAllLines(_keymapFilePath);
        bool isCorrectKeymapLine = false;
        int currentRow = 0;
        int currentKeyIndex = 0;

        //Loop through the file
        for (int lineNum = 0; lineNum < lines.Count(); lineNum++)
        {
            var currentLine = lines[lineNum];
            //Find the right keymap
            if (currentLine.Contains(keymapName))
            {
                isCorrectKeymapLine = true;
                currentRow = 0;
                continue;
            }

            // Don't overwrite the bindings marker
            if (isCorrectKeymapLine && currentLine.Contains("bindings"))
            {
                currentRow++;
                continue;
            }

            //Stop editing once we have passed the keymap
            if (currentRow > 5)
            {
                isCorrectKeymapLine = false;
            }

            //Set the length of the row
            if (isCorrectKeymapLine)
            {
                // Write the new key data
                currentLine = " ";
                do
                {
                    if (currentKeyIndex == 75)
                    {
                        break;
                    }
                    Console.WriteLine(currentKeyIndex);
                    var currentKey = layout[currentKeyIndex];
                    currentLine += $"{currentKey.GetZmkPress()} {currentKey.GetZmkAction()} ";
                    currentKeyIndex++;
                    //Stop writing to row when reaching the end of the row
                    if (keyMap.GetRowEnds().Contains(currentKeyIndex - 1))
                    {
                        lines[lineNum] = currentLine;
                        break;
                    }

                } while (true);

                currentRow++;
            }
        }
        //Write to file
        WriteDataToFile(_keymapFilePath, lines);
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

    //Macro parser written by chatgpt because of time constraints
    public Macro ParseZmkMacro(string zmkMacro)
    {
        var macro = new Macro();

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

                    macro.AddAction(action);
                }
            }
        }

        return macro;
    }

    //Macro loader wirtten by chatgpt because of time constraints
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

    //Macro writer written by chatgpt because of time constraints
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

    //Generate macro def written by chatgpt because of time constraints
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
            macroData.Append($"&kp {action.GetAction()}({action.GetAction()})");
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
