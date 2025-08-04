using TerminalEditorV2.Models;
using System.Collections.Generic;

namespace TerminalEditorV2.Services
{
    public class KeyTranslatorService
    {
        private readonly List<string> _zmkKeyActions = new();
        private readonly List<string> _zmkKeyPresses = new();
        private readonly List<string> _zmkMacroModifiers = new();
        private readonly List<string> _keyActions = new();
        private readonly List<string> _keyPresses = new();
        private readonly List<string> _macroModifiers = new();

        public KeyTranslatorService()
        {
            _keyPresses = new List<string>
            {
                "Momentary layer activation",
                "Toggle a layer on/off",
                "Layer-tap",
                "Capsword behavior",
                "Key press",
                "Custom macro definition",
                "No action",
                "Transparent",
                "Reset keyboard",
                "Enter bootloader",
            };

            _zmkKeyPresses = new List<string>
            {
                "&mo",
                "&tog",
                "&lt",
                "&caps_word",
                "&kp",
                "&macro",
                "&none",
                "&trans",
                "&reset",
                "&bootloader"
            };

            _keyActions = new List<string>
            {
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                "1 and !", "2 and @", "3 and #", "4 and $", "5 and %", "6 and ^", "7 and &", "8 and *", "9 and (", "0 and )",
                "!", "@", "#", "$", "%", "^", "&", "*", "(", ")",
                "= and +", "+", "-", "_", "/", "?", "\\ and |", "|", "Non-US \\ and |", "; and :", "' and \"", "\"", ",", "<", ">", "[ and {", "] and }", "` and ~", "~", "Non-US # and ~",
                "Escape", "Return", "Enter", "Space", "Tab", "Backspace", "Delete", "Insert", "Home", "End", "Page UP", "Page Down",
                "Up Arrow", "Down Arrow", "Left Arrow", "Right Arrow", "Application (Context Menu)", "Caps Lock", "Locking Caps Lock", "Scroll Lock", "Locking Num",
                "Print Screen", "Pause/Break", "Alternate Erase", "SysReq/Attention", "Cancel", "Clear", "Clear/Again", "CrSel/Props", "Prior", "Separator", "Out", "Oper", "ExSel",
                "Edit Keyboard", "Left Shift", "Right Shift", "Left Control", "Right Control", "Left Alt", "Right Alt", "Left Command", "Right Command",
                "Numlock and Clear", "Keypad Clear", "Keypad Enter", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0",
                "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12",
                " +", "- ", "* ", "/ ", "=", ".", ". ", ",", "Left Parenthesis", "Right Parenthesis",
                "Cut", "Copy", "Paste", "Undo", "Redo/Repeat",
                "Volume Up", "Volume Down", "Mute", "Alternate Audio Increment",
                "Increase Brightness", "Decrease Brightness", "Max Brightness", "Min Brightness", "Auto Brightness", "Backlight Toggle",
                "Picture in Picture", "Channel Increment", "Channel Decrement", "Recall Last", "VCR Plus", "Mode Step", "Bluetooth select "
            };

            _zmkKeyActions = new List<string>
            {
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                "N1", "N2", "N3", "N4", "N5", "N6", "N7", "N8", "N9", "N0",
                "EXCL", "AT", "HASH", "DOLLAR", "PERCENT", "CARET", "AMPERSAND", "STAR", "LPAR", "RPAR",
                "EQUAL", "PLUS", "MINUS", "UNDERSCORE", "FSLH", "QUESTION", "BSLH", "PIPE", "NON_US_BACKSLASH", "SEMI", "SQT", "DOUBLE_QUOTES", "COMMA", "LT", "GT", "LBKT", "RBKT", "GRAVE", "TILDE", "NON_US_HASH",
                "ESC", "RETURN", "ENTER", "SPACE", "TAB", "BSPC", "DEL", "INSERT", "HOME", "END", "PAGE_UP", "PAGE_DOWN",
                "UP", "DOWN", "LEFT", "RIGHT", "K_APPLICATION", "CAPS", "LOCKING_CAPS", "SCROLLLOCK", "KP_NUM",
                "PRINTSCREEN", "PAUSE_BREAK", "ALT_ERASE", "SYSREQ", "K_CANCEL", "CLEAR", "CLEAR_AGAIN", "CRSEL", "PRIOR", "SEPARATOR", "OUT", "OPER", "EXSEL",
                "K_EDIT", "LSHFT", "RSHFT", "LEFT_CONTROL", "RIGHT_CONTROL", "LEFT_ALT", "RIGHT_ALT", "LEFT_COMMAND", "RIGHT_COMMAND",
                "KP_NUMLOCK", "KP_CLEAR", "KP_ENTER", "KP_N1", "KP_N2", "KP_N3", "KP_N4", "KP_N5", "KP_N6", "KP_N7", "KP_N8", "KP_N9", "KP_N0",
                "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12",
                "KP_PLUS", "KP_MINUS", "KP_MULTIPLY", "KP_DIVIDE", "KP_EQUAL", "DOT", "KP_DOT", "KP_COMMA", "KP_LPAR", "KP_RPAR",
                "C_AC_CUT", "C_AC_COPY", "C_AC_PASTE", "C_AC_UNDO", "C_AC_REDO",
                "C_VOLUME_UP", "C_VOLUME_DOWN", "C_MUTE", "C_ALTERNATE_AUDIO_INCREMENT",
                "C_BRI_UP", "C_BRI_DN", "C_BRI_MAX", "C_BRI_MIN", "C_BRI_AUTO", "C_BKLT_TOG",
                "C_PIP", "C_CHAN_INC", "C_CHAN_DEC", "C_CHAN_LAST", "C_MEDIA_VCR_PLUS", "C_MODE_STEP", "BT_SEL"
            };

            _zmkMacroModifiers = new List<string> { "RC", "LC", "RS", "LS", "LCTRL", "RCTRL" };
            _macroModifiers = new List<string> { "Hold_Right_Command", "Hold_Left_Command", "Hold_Right_Shift", "Hold_Left_Shift", "Hold_Right_Controll", "Hold_Left_Controll" };
        }

        public void TranslateFromZmk(Key key, List<string> keymapNames = null)
        {
            if (key.ZmkKeyPress == "&tog" || key.ZmkKeyPress == "&mo")
            {
                if (keymapNames != null && int.TryParse(key.ZmkKeyAction, out int layerIndex) && layerIndex < keymapNames.Count)
                {
                    key.KeyAction = keymapNames[layerIndex];
                }
                else
                {
                    key.KeyAction = "Invalid Layer";
                }
            }
            else if (key.ZmkKeyPress.StartsWith("&macro"))
            {
                key.KeyAction = key.ZmkKeyPress;
            }
            else if (key.ZmkKeyPress == "&none")
            {
                key.KeyAction = "";
            }
            else if (key.ZmkKeyPress == "&trans")
            {
                key.KeyAction = "Transparent";
            }
            else
            {
                int index = _zmkKeyActions.IndexOf(key.ZmkKeyAction);
                key.KeyAction = index != -1 ? _keyActions[index] : "Unknown Action";
            }

            int pressIndex = _zmkKeyPresses.IndexOf(key.ZmkKeyPress);
            key.KeyPress = pressIndex != -1 ? _keyPresses[pressIndex] : "Unknown Press";
        }

        public void TranslateToZmk(Key key)
        {
            int actionIndex = _keyActions.IndexOf(key.KeyAction);
            if (actionIndex != -1)
            {
                key.ZmkKeyAction = _zmkKeyActions[actionIndex];
            }

            int pressIndex = _keyPresses.IndexOf(key.KeyPress);
            if (pressIndex != -1)
            {
                key.ZmkKeyPress = _zmkKeyPresses[pressIndex];
            }
        }

        public List<string> GetAllActions() => _keyActions;
        public List<string> GetAllPresses() => _keyPresses;
        public List<string> GetMacroModifiers() => _macroModifiers;
        public string GetZmkModifier(string modifier) => _zmkMacroModifiers[_macroModifiers.IndexOf(modifier)];

        public string GetZmkPress(string keyPress, string keyAction)
        {
            int pressIndex = _keyPresses.IndexOf(keyPress);
            if (pressIndex != -1)
            {
                return _zmkKeyPresses[pressIndex];
            }
            return "";
        }

        public string GetZmkAction(string keyAction, string keyPress)
        {
            int actionIndex = _keyActions.IndexOf(keyAction);
            if (actionIndex != -1)
            {
                return _zmkKeyActions[actionIndex];
            }
            return "";
        }
    }
}