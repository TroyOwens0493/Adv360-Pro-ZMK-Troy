class MacroAction 
{
    //Attributes
    private string _modifier;
    private string _action;
    private string _zmkModifier;
    private string _zmkAction;
    private KeyTranslator _trans = new();

    //Methods
    public MacroAction(string zmkModifier, string zmkAction)
    {
        _zmkModifier = zmkModifier;
        _zmkAction = zmkAction;
        if (_zmkModifier != "")
        {
        _modifier = _trans.GetModifier(_zmkModifier);
        }
        else
        {
            _modifier = "";
        }
        _action = _trans.GetAction(zmkAction, "&kp");
    }

    public string GetZmkModifier()
    {
        return _zmkModifier;
    }

    public string GetZmkAction()
    {
        return _zmkAction;
    }

    public string GetModifier()
    {
        return _modifier;
    }

    public string GetAction()
    {
        return _action;
    }

    public string TranslateZmkModifier()
    {
        return _trans.GetModifier(_modifier);
    }

    public string TranslateZmkAction()
    {
         return _trans.GetAction(_action, "&kp");
    }
}
