class Macro
{
    //Attr
    private List<MacroAction> _actions;

    //Methods
    public Macro(List<MacroAction> actions)
    {
        _actions = actions;
    }

    public void AddAction(MacroAction newAction)
    {
        Console.WriteLine("Writing new action");
        _actions.Add(newAction);

        foreach (MacroAction Action in _actions)
        {
            Console.WriteLine($"{Action.GetModifier()}, {Action.GetAction()}");
        }
    }

    public void SetActions(List<MacroAction> actions)
    {
        _actions = actions;
    }

    public List<MacroAction> GetMacro()
    {
        return _actions;
    }
}
