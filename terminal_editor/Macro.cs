class Macro
{
    //Attr
    private List<MacroAction> _actions;

    //Methods
    public Macro()
    {
        List<MacroAction> blank = new();
        _actions = blank;
    }

    public Macro(List<MacroAction> actions)
    {
        _actions = actions;
    }

    public void AddAction(MacroAction newAction)
    {
        _actions.Add(newAction);
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
