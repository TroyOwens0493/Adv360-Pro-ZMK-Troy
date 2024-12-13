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

    public Macro ParseMacro(string macroName)
    {
        string[] lines = File.ReadAllLines(_macroFilePath);
        bool isCorrectMacroLine = false;
        List<Key> macroActions = new();

        for (int i = 0; i < lines.Length; i++)
        {
            var currentLine = lines[i];
            if (currentLine.StartsWith(macroName))
            {
                isCorrectMacroLine = true;
            }

            if (isCorrectMacroLine && currentLine.StartsWith("bindings ="))
            {
                var splitLine = currentLine.Split(",");
                var splitLineList = splitLine.ToList();
                splitLineList.RemoveAt(0);
            }
            else
            {
                continue;
            }
        }

        //All this is just to remove errs. Please delete later
        Key newKey = new Key("", "");
        Macro newMac = new Macro([newKey, newKey]);
        return newMac;
    }

    public void MakeNewMacro(string macroName)
    {
    }

    public void WriteMacro()
    {
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
