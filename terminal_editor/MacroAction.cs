class MacroAction 
{
    //Attributes
    private string _modifier;
    private string _action;
    private string _zmkModifier;
    private string _zmkAction;
    private KeyTranslator _trans = new();

    //Methods
    public MacroAction(string modifier, string action)
    {
        _modifier = modifier;
        _action = action;
        if (_zmkModifier != "")
        {
        _zmkModifier = GetZmkModifier();
        }
        else
        {
            _zmkModifier = "";
        }
        _zmkAction = GetZmkAction();
    }

    public string GetModifier()
    {
        return _modifier;
    }

    public string GetAction()
    {
        return _action;
    }

    public string GetZmkModifier()
    {
        return _trans.GetZmkModifier(_modifier);
    }

    public string GetZmkAction()
    {
         return _trans.GetZmkAction(_action, "&kp");
    }
}
