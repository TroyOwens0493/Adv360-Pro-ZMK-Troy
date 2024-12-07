class Macro
{
    //Attr
    private List<Key> _actions;

    //Methods
    public Macro(List<Key> actions)
    {
        _actions = actions;
    }

    public void AddAction(Key newAction)
    {
        _actions.Add(newAction);
    }

    public List<Key> GetMacro()
    {
        return _actions;
    }
}
