class MenuManager
{
    InputValidationHandler _userInput = new();
    FileManager _fileMan = new FileManager("../config/adv360.keymap", "../config/macros.dtsi");
    KeyTranslator _keyTans = new();

    public int MainMenu()
    {
        Console.Clear();
        Console.WriteLine("Welcome to the kinesis keyboard terminal editor");
        Console.WriteLine("Please choose what you would like to do:");
        Console.WriteLine("1. Edit a layer");
        Console.WriteLine("2. Make a new layer");
        Console.WriteLine("3. Edit a macro");
        Console.WriteLine("4. Make a new macro");
        Console.WriteLine("5. Exit");

        return _userInput.GetIntFromUser(1, 5);
    }

    public int ChooseLayout()
    {
        var keymapNames = _fileMan.ParseKeymapNames();
        Console.Clear();
        Console.WriteLine("Choose what layout you would like to edit: ");
        PrintList(keymapNames);

        return _userInput.GetIntFromUser(1, keymapNames.Count());
    }

    public string KeyBoardSideSelector()
    {
        Console.Clear();
        Console.WriteLine("Please choose which side you would like to edit.");
        Console.WriteLine("1. Left 2. Right");
        int side = _userInput.GetIntFromUser(1, 2);
        if (side == 1)
        {
            return "left";
        }
        else
        {
            return "right";
        }
    }

    public int EditLeftSide(Keymap leftSide)
    {
        Console.Clear();
        leftSide.Disp();
        Console.Write("What key would you like to edit? ");
        return _userInput.GetIntFromUser(1, 75) - 1;
    }

    public int EditRightSide(Keymap rightSide)
    {
        Console.Clear();
        rightSide.Disp();
        Console.Write("What key would you like to edit? ");
        return _userInput.GetIntFromUser(1, 75) - 1;
    }

    public Key EditKeyPressType(Key keyToEdit)
    {
        Console.Clear();

        var presses = _keyTans.GetAllPresses();
        PrintList(presses);
        Console.WriteLine($"The currennt keypress for this key is {keyToEdit.GetKeyPress()}");
        Console.WriteLine("What would you like the new keypress to be? ");

        int pressIndex = _userInput.GetIntFromUser(1, presses.Count() - 1);
        var newKeyPress = presses[pressIndex - 1];

        keyToEdit.SetKeyPress(newKeyPress);
        var newZmkPress = _keyTans.GetZmkPress(newKeyPress, keyToEdit.GetKeyAction());

        keyToEdit.SetZmkPress(newZmkPress);
        return keyToEdit;
    }

    public Key EditKeyAction(Key keyToEdit)
    {
        Console.Clear();
        var actions = _keyTans.GetAllActions();
        var keyAction = keyToEdit.GetKeyAction();
        var zmkPress = keyToEdit.GetZmkPress();
        if (zmkPress == "&trans" || zmkPress == "&none")
        {
            keyToEdit.SetZmkAction("");
            return keyToEdit;
        }
        else if (zmkPress == "&mo" || zmkPress == "&tog")
        {
            var keyMapNames = _fileMan.ParseKeymapNames();
            PrintList(keyMapNames);
            Console.WriteLine("What layer would you like this to toggle?");
            int res = _userInput.GetIntFromUser(1, keyMapNames.Count) - 1;
            keyToEdit.SetZmkAction(res.ToString());
            return keyToEdit;
        }
        else
        {
            PrintList(actions);
            Console.WriteLine($"The currennt key action for this key is {keyAction}");
            Console.WriteLine("What would you like the new action to be? ");

            int actionIndex = _userInput.GetIntFromUser(1, actions.Count() - 1);
            var newAction = actions[actionIndex - 1];

            keyToEdit.SetKeyAction(newAction);
            var newZmkAction = _keyTans.GetZmkAction(newAction, keyToEdit.GetKeyPress());

            keyToEdit.SetZmkAction(newZmkAction);
            return keyToEdit;
        }
    }

    public void MakeLayout()
    {
        Console.Clear();
        Console.WriteLine("What would you like your new layout to be called? ");
        string layoutName = _userInput.GetStringFromUser();
        _fileMan.MakeNewKeymap(layoutName);
    }

    public int ChooseMacro()
    {
        Console.Clear();
        Console.WriteLine("Which macro would you like to edit?");
        var macroNames = _fileMan.ParseMacroNames();
        PrintList(macroNames);

        return _userInput.GetIntFromUser(1, macroNames.Count());
    }

    public int EditMacro()
    {
        Console.Clear();
        Console.WriteLine("What would you like to do?");
        Console.WriteLine("1. Add something 2. Delete something");

        return _userInput.GetIntFromUser(1, 2);
    }

    public void AddToMacro()
    {
        Console.Clear();
    }

    public void DeleteFromMacro()
    {
    }

    public void MakeNewMacro()
    {
    }

    public void DisplayAllKeyActions()
    {
        KeyTranslator keyInfo = new();
        var keyActions = keyInfo.GetAllActions();
        PrintList(keyActions);
    }

    //Print lists w/index and makes columns when needed.
    private void PrintList(List<string> list)
    {
        // Get the current cursor position at the bottom of existing content
        var x = 0;
        var y = Console.CursorTop;
        var yStart = y;
        var maxHeight = (Console.WindowHeight / 4) * 3;
        bool hasMultipleColumns = false;

        // Calculate a fixed column width based on the longest string in the list
        int columnWidth = list.Max(item => $"{list.IndexOf(item) + 1}. {item}".Length) + 2;

        for (int i = 0; i < list.Count; i++)
        {
            // Print the option in the current position
            Console.SetCursorPosition(x, y);
            Console.Write($"{i + 1}. {list[i]}");

            y++; // Move down one line

            // Check if we need to move to the next column (halfway down the terminal)
            if (y == maxHeight)
            {
                hasMultipleColumns = true;
                y = yStart;
                x += columnWidth; // Move x to the next column
            }
        }
        if (hasMultipleColumns)
        {
            Console.SetCursorPosition(0, maxHeight - 1);
        }
        Console.WriteLine();
        Console.WriteLine();
    }
}
