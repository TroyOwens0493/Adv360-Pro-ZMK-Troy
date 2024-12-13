class KeyTranslator
{
    //Attr
    private readonly List<string> _zmkKeyActions = new();
    private readonly List<string> _zmkKeyPresses = new();
    private readonly List<string> _zmkMacroModifiers = new();
    private readonly List<string> _keyActions = new();
    private readonly List<string> _keyPresses = new();
    private readonly List<string> _macroModifiers = new();
    private FileManager _fileMan = new FileManager("../config/adv360.keymap", "../config/macros.dtsi");

    //Methods
    public KeyTranslator()
    {
        //Items that are commented out are not supported with this program yet
        _keyPresses = new List<string>
        {
            "Momentary layer activation",
            //"Exclusively switch to a layer",
            "Toggle a layer on/off",
            "Layer-tap",
            //"Sticky key",
            "Capsword behavior",
            "Key press",
            //"Mod-tap",
            //"Home row mod",
            "Custom macro definition",
            //"Mute audio",
            //"Increase volume",
            //"Decrease volume",
            //"Bluetooth control and device switching",
            //"RGB lighting control and effects",
            "No action",
            "Transparent",
            "Reset keyboard",
            "Enter bootloader",
        };

        _zmkKeyPresses = new List<string>
        {
            "&mo",
            //"&to",
            "&tog",
            "&lt",
            //"&sk",
            "&caps_word",
            "&kp",
            //"&mt",
            //"&hrm",
            "&macro",
            //"&mute",
            //"&vol_up",
            //"&vol_dn",
            //"&bt",
            //"&rgb",
            "&none",
            "&trans",
            "&reset",
            "&bootloader"
        };

        _keyActions = new List<string>
        {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z",
            "1 and !",
            "2 and @",
            "3 and #",
            "4 and $",
            "5 and %",
            "6 and ^",
            "7 and &",
            "8 and *",
            "9 and (",
            "0 and )",
            "!",
            "@",
            "#",
            "$",
            "%",
            "^",
            "&",
            "*",
            "(",
            ")",
            "= and +",
            "+",
            "-",
            "_",
            "/",
            "?",
            "\\ and |",
            "|",
            "Non-US \\ and |",
            "; and :",
            "' and \"",
            "\"",
            ",",
            "<",
            ">",
            "[ and {",
            "] and }",
            "` and ~",
            "~",
            "Non-US # and ~",
            "Escape",
            "Return",
            "Enter",
            "Space",
            "Tab",
            "Backspace",
            "Delete",
            "Insert",
            "Home",
            "End",
            "Page UP",
            "Page Down",
            "Up Arrow",
            "Down Arrow",
            "Left Arrow",
            "Right Arrow",
            "Application (Context Menu)",
            "Caps Lock",
            "Locking Caps Lock",
            "Scroll Lock",
            "Locking Num",
            "Print Screen",
            "Pause/Break",
            "Alternate Erase",
            "SysReq/Attention",
            "Cancel",
            "Clear",
            "Clear/Again",
            "CrSel/Props",
            "Prior",
            "Separator",
            "Out",
            "Oper",
            "ExSel",
            "Edit Keyboard",
            "Left Shift",
            "Right Shift",
            "Left Control",
            "Right Control",
            "Left Alt",
            "Right Alt",
            "Left Command",
            "Right Command",
            "Numlock and Clear",
            "Keypad Clear",
            "Keypad Enter",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0",
            "F1",
            "F2",
            "F3",
            "F4",
            "F5",
            "F6",
            "F7",
            "F8",
            "F9",
            "F10",
            "F11",
            "F12",
            " +",
            "- ",
            "* ",
            "/ ",
            "=",
            ".",
            ". ",
            ",",
            "Left Parenthesis",
            "Right Parenthesis",
            "Cut",
            "Copy",
            "Paste",
            "Undo",
            "Redo/Repeat",
            "Volume Up",
            "Volume Down",
            "Mute",
            "Alternate Audio Increment",
            "Increase Brightness",
            "Decrease Brightness",
            "Max Brightness",
            "Min Brightness",
            "Auto Brightness",
            "Backlight Toggle",
            "Picture in Picture",
            "Channel Increment",
            "Channel Decrement",
            "Recall Last",
            "VCR Plus",
            "Mode Step",
            "Bluetooth select "
        };

        _zmkKeyActions = new List<string>
        {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z",
            "N1",
            "N2",
            "N3",
            "N4",
            "N5",
            "N6",
            "N7",
            "N8",
            "N9",
            "N0",
            "EXCL",
            "AT",
            "HASH",
            "DOLLAR",
            "PERCENT",
            "CARET",
            "AMPERSAND",
            "STAR",
            "LPAR",
            "RPAR",
            "EQUAL",
            "PLUS",
            "MINUS",
            "UNDERSCORE",
            "FSLH",
            "QUESTION",
            "BSLH",
            "PIPE",
            "NON_US_BACKSLASH",
            "SEMI",
            "SQT",
            "DOUBLE_QUOTES",
            "COMMA",
            "LT",
            "GT",
            "LBKT",
            "RBKT",
            "GRAVE",
            "TILDE",
            "NON_US_HASH",
            "ESC",
            "RETURN",
            "ENTER",
            "SPACE",
            "TAB",
            "BSPC",
            "DEL",
            "INSERT",
            "HOME",
            "END",
            "PAGE_UP",
            "PAGE_DOWN",
            "UP",
            "DOWN",
            "LEFT",
            "RIGHT",
            "K_APPLICATION",
            "CAPS",
            "LOCKING_CAPS",
            "SCROLLLOCK",
            "KP_NUM",
            "PRINTSCREEN",
            "PAUSE_BREAK",
            "ALT_ERASE",
            "SYSREQ",
            "K_CANCEL",
            "CLEAR",
            "CLEAR_AGAIN",
            "CRSEL",
            "PRIOR",
            "SEPARATOR",
            "OUT",
            "OPER",
            "EXSEL",
            "K_EDIT",
            "LSHFT",
            "RSHFT",
            "LEFT_CONTROL",
            "RIGHT_CONTROL",
            "LEFT_ALT",
            "RIGHT_ALT",
            "LEFT_COMMAND",
            "RIGHT_COMMAND",
            "KP_NUMLOCK",
            "KP_CLEAR",
            "KP_ENTER",
            "KP_N1",
            "KP_N2",
            "KP_N3",
            "KP_N4",
            "KP_N5",
            "KP_N6",
            "KP_N7",
            "KP_N8",
            "KP_N9",
            "KP_N0",
            "F1",
            "F2",
            "F3",
            "F4",
            "F5",
            "F6",
            "F7",
            "F8",
            "F9",
            "F10",
            "F11",
            "F12",
            "KP_PLUS",
            "KP_MINUS",
            "KP_MULTIPLY",
            "KP_DIVIDE",
            "KP_EQUAL",
            "DOT",
            "KP_DOT",
            "KP_COMMA",
            "KP_LPAR",
            "KP_RPAR",
            "C_AC_CUT",
            "C_AC_COPY",
            "C_AC_PASTE",
            "C_AC_UNDO",
            "C_AC_REDO",
            "C_VOLUME_UP",
            "C_VOLUME_DOWN",
            "C_MUTE",
            "C_ALTERNATE_AUDIO_INCREMENT",
            "C_BRI_UP",
            "C_BRI_DN",
            "C_BRI_MAX",
            "C_BRI_MIN",
            "C_BRI_AUTO",
            "C_BKLT_TOG",
            "C_PIP",
            "C_CHAN_INC",
            "C_CHAN_DEC",
            "C_CHAN_LAST",
            "C_MEDIA_VCR_PLUS",
            "C_MODE_STEP",
            "BT_SEL"
        };

        _zmkMacroModifiers = new List<string>
        {
            "RC(",
            "LC(",
            "RS",
            "LS(",
            "LCTRL(",
            "RCTRL("
        };

        _macroModifiers = new List<string>
        {
            "Hold_Right_Command",
            "Hold_Left_Command",
            "Hold_Right_Shift",
            "Hold_Left_Shift",
            "Hold_Right_Controll",
            "Hold_Left_Controll",
        };
    }

    public string GetZmkAction(string keyAction, string keyPress)
    {
        int index = _keyActions.IndexOf(keyAction);
        return _zmkKeyActions[index];
    }

    public string GetZmkPress(string keyPress, string keyAction)
    {
        int index = _keyPresses.IndexOf(keyPress);
        return _zmkKeyPresses[index];
    }

    public string GetAction(string zmkAction, string zmkPress)
    {
        var keyAction = "";
        if (zmkPress == "&tog" || zmkPress == "&mo")
        {
            var keymapNames = _fileMan.ParseKeymapNames();
            try
            {
                keyAction = keymapNames[int.Parse(zmkAction)];
            }
            catch
            {
            throw new Exception("It looks like one of the keys toggles a layer that doesn't exist");
            }
        }
        else if (zmkPress.StartsWith("&macro"))
        {
            keyAction = zmkPress;
        }
        else if (zmkPress == "&none")
        {
            keyAction = "";
        }
        else if (zmkPress == "&trans")
        {
            keyAction = "Transparent";
        }
        else
        {
            int index = _zmkKeyActions.IndexOf(zmkAction);
            try
            {
            keyAction = _keyActions[index];
            }
            catch
            {
                throw new Exception("Key action not found in list");
            }
        }
        return keyAction;
    }

    public string GetPress(string zmkPress, string zmkAction)
    {
        var keypress = "";
        if (zmkPress.StartsWith("&macro"))
        {
            keypress = zmkPress;
        }
        else
        {
            int index = _zmkKeyPresses.IndexOf(zmkPress);
            try
            {
            keypress = _keyPresses[index];
            }
            catch
            {
                throw new Exception("Key press not found in list");
            }
        }
        return keypress;
    }

    public List<string> GetAllActions()
    {
        return _keyActions;
    }

    public List<string> GetAllPresses()
    {
        return _keyPresses;
    }
}
