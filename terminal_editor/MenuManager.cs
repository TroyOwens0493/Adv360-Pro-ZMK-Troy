class MenuManager
{
    InputValidationHandler _userInput = new();

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

    public void keyBoardSideSelector()
    {
    }

    public void EditLeftSide()
    {
    }

    public void EditRightSide()
    {
    }

    public void MakeLayout()
    {
    }

    public void EditMacro()
    {
    }

    public void MakeNewMacro()
    {
    }
}
