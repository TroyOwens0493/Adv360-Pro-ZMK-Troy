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


                //Store the line before the map to know what the map is called
                if (!isKeymapLine)
                {
                    keymapNames.Add(prevLine);
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

        using (var reader = new StreamReader(_keymapFilePath))
        {
            // Set up vars for iterating
            string? line;
            while ((line = reader.ReadLine()) != null)
            {

                string cleanLine = line.Trim();

                //Start parsing if we are on a keymap line
                if (cleanLine == "bindings = <")
                {
                    isKeymapLine = true;
                    continue;
                }

                if (isKeymapLine)
                {
                    if (i < 6) //Add the keymap lines
                    {
                        var keys = Regex.Split(cleanLine, @"(?=&)")
                                        .Where(part => !string.IsNullOrWhiteSpace(part))
                                        .ToList();
                        foreach (string key in keys)
                        {
                            //Get the key info and make the key obj.
                            string[] keyInfo = key.Split(' ');
                            string keyPress = keyInfo[0];
                            string keyAction = keyInfo[0];
                            Key newKey = new(zmkIndex, keyAction, keyPress);
                            keymap.Add(newKey);
                        }
                        i++;
                    }
                    else
                    {
                        i = 0;
                        isKeymapLine = false;
                    }
                }
            }
        }
        return keymap;
    }

    public void MakeNewKeymap(string keymapName)
    {
    }

    public void WriteKeymap(string keymapName)
    {
        //Write contents of file into array
        string[] lines = File.ReadAllLines(_keymapFilePath);

        bool isCorrectKeymapLine = false;
        int itr = 0;

        foreach (string line in lines)
        {
            if (line.StartsWith(keymapName))
            {
                isCorrectKeymapLine = true;
                itr = 0;
            }

            if (itr == 0 & isCorrectKeymapLine) //Don't overwrite the bindings marker.
            {
                continue;
            }

            if (isCorrectKeymapLine)
            {
                // Finish this function
            }
        }
    }

    public List<string> ParseMacroNames()
    {
        List<string> res = new();
        return res;
    }

    public Macro ParseMacro(string macroName)
    {
        List<Key> ls = new();
        Macro res = new(ls);
        return res;
    }

    public void MakeNewMacro(string macroName)
    {
    }

    public void WriteMacro()
    {
    }
}
