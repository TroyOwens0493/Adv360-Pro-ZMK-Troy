class Key
{
    //Attr
    private int _zmkIndex;
    private string _zmkKeyAction;
    private string _zmkKeyPress;
    private string _keyAction;
    private string _keyPress;
    private string _isModKey;
    private Key _keyToModifiy;
    private FileManager _fileMan = new FileManager("../config/adv360.keymap", "../config/macros.dtsi");

    //Methods
    public Key(int zmkIndex, string zmkKeyAction, string zmkKeyPress)
    {
        _zmkIndex = zmkIndex;
        _zmkKeyAction = zmkKeyAction;
        _zmkKeyPress = zmkKeyPress;
        KeyTranslator translate = new();
        _keyPress = translate.GetPress(zmkKeyPress, zmkKeyAction);
        _keyAction = translate.GetAction(zmkKeyAction, zmkKeyPress);
    }

    public Key(string keyAction, string keyPress)
    {
        _keyAction = keyAction;
        _keyPress = keyPress;
        KeyTranslator translate = new();
        _zmkKeyPress = translate.GetZmkPress(keyPress, keyAction);
        _zmkKeyAction = translate.GetZmkAction(keyAction, keyPress);
    }

    public void SetZmkKeyIndex(int index)
    {
        _zmkIndex = index;
    }

    public void SetZmkAction(string action)
    {
        _zmkKeyAction = action;
    }

    public void SetZmkPress(string press)
    {
        _zmkKeyPress = press;
    }

    public void SetKeyPress(string press)
    {
        _keyPress = press;
    }

    public void SetKeyAction(string action)
    {
        _keyAction = action;
    }

    public int GetZmkIndex()
    {
        return _zmkIndex;
    }

    public string GetZmkAction()
    {
        return _zmkKeyAction;
    }

    public string GetZmkPress()
    {
        return _zmkKeyPress;
    }

    public string GetKeyAction()
    {
        return _keyAction;
    }

    public string GetKeyPress()
    {
        return _keyPress;
    }
}
