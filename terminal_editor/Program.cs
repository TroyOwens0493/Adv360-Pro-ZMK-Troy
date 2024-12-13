class Program
{
    static void Main()
    {
        MenuManager _menus = new();
        FileManager _fileMan = new("../config/adv360.keymap", "../config/macros.dtsi");
        InputValidationHandler _input = new();
        int userSelection = _menus.MainMenu();

        switch (userSelection)
        {
            case 1:
                int layoutIndex = _menus.ChooseLayout() - 1;
                List<string> keymapNames = _fileMan.ParseKeymapNames();
                var layoutName = keymapNames[layoutIndex];

                do
                {
                    string side = _menus.KeyBoardSideSelector();
                    var layout = _fileMan.ParseKeysInMap(layoutName);

                    if (side == "left")
                    {
                        var leftSide = new LeftSide(layout);
                        int keyIndex = _menus.EditLeftSide(leftSide);
                        var newKeyPress = _menus.EditKeyPressType(layout[keyIndex]);
                        var finishedKey = _menus.EditKeyAction(newKeyPress);
                        layout[keyIndex] = finishedKey;
                        _fileMan.WriteKeymap(layoutName, leftSide);
                    }
                    else
                    {
                        var rightSide = new RightSide(layout);
                        int keyIndex = _menus.EditRightSide(rightSide);
                        var newKeyPress = _menus.EditKeyPressType(layout[keyIndex]);
                        var finishedKey = _menus.EditKeyAction(newKeyPress);
                        layout[keyIndex] = finishedKey;
                        _fileMan.WriteKeymap(layoutName, rightSide);
                    }
                    //Console.Clear();
                    Console.WriteLine("Would you like to edit another key in this keymap? (y/n)");
                    string res = _input.GetStringFromUser();
                    if (res != "y")
                    {
                        break;
                    }
                    Console.Clear();

                } while (true);
                break;

            case 2:
                do
                {
                    Console.Clear();
                    Console.WriteLine("What would you like your new keymap to be called?");
                    Console.WriteLine("Do not use special characters or spaces.");
                    string newLayerName = _input.GetStringFromUser();
                    _fileMan.MakeNewKeymap(newLayerName);
                    Console.Clear();
                    Console.WriteLine("Would you like to make another new keymap? (y/n)");
                    var res = _input.GetStringFromUser();
                    if (res != "y")
                    {
                        break;
                    }
                } while (true);
                break;

            case 3:
                do
                {
                    Console.Clear();
                    var macroToEdit = _menus.ChooseMacro();
                    string addOrDelete = _menus.AddOrDeleteFromMacro();
                    List<MacroAction> tempActions = new();
                    Macro tempMacro = new Macro(tempActions);
                    if (addOrDelete == "add")
                    {
                        var newMacro = _menus.AddToMacro(_fileMan.LoadMacroFromFile(macroToEdit));
                        tempMacro = newMacro;
                    }
                    else
                    {
                        var newMacro = _menus.DeleteFromMacro(_fileMan.LoadMacroFromFile(macroToEdit));
                        tempMacro = newMacro;
                    }
                    //foreach (MacroAction Action in newMacro.GetMacro())
                    //{
                    //    Console.WriteLine($"{Action.GetModifier()}, {Action.GetAction()}");
                    //}
                    _fileMan.WriteMacroToFile(tempMacro, macroToEdit);

                    Console.WriteLine("Would you like to edit this macro more? (y/n);");
                    var res =_input.GetStringFromUser();
                    if (res == "y")
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }

                } while (true);
                break;
        }
    }
}
