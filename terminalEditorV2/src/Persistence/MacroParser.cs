using TerminalEditorV2.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace TerminalEditorV2.Persistence
{
    public class MacroParser
    {
        public List<string> ParseMacroNames(string[] lines)
        {
            var macroNames = new List<string>();
            foreach (string line in lines)
            {
                if (line.Trim().EndsWith("{"))
                {
                    var parts = line.Trim().Split(' ');
                    if (parts.Length > 1 && !parts[0].StartsWith("//"))
                    {
                        macroNames.Add(parts[0].Replace(":", ""));
                    }
                }
            }
            return macroNames;
        }

        public Macro ParseMacro(string fileContent, string name)
        {
            var macro = new Macro { Name = name };
            var macroMatch = Regex.Match(fileContent, $@"{name}:\s*{name}\s*{{([\s\S]*?)}}", RegexOptions.Singleline);
            if (macroMatch.Success)
            {
                string macroContent = macroMatch.Groups[1].Value;
                var bindingsMatch = Regex.Match(macroContent, @"bindings\s*=\s*<([\s\S]*?)>;", RegexOptions.Singleline);
                if (bindingsMatch.Success)
                {
                    string bindingsContent = bindingsMatch.Groups[1].Value.Replace("\n", " ").Replace("\r", " ");
                    var individualBindings = Regex.Matches(bindingsContent, @"&kp\s+([A-Z0-9_]+)");

                    foreach (Match bindingMatch in individualBindings)
                    {
                        if (bindingMatch.Success)
                        {
                            macro.Actions.Add(new MacroAction { ZmkAction = bindingMatch.Groups[1].Value });
                        }
                    }
                }
            }
            return macro;
        }

        public string UpdateMacro(string fileContent, Macro macro)
        {
            string newMacroString = GenerateMacroDefinition(macro);
            string pattern = $@"{macro.Name}:\s*{macro.Name}\s*{{[\s\S]*?}};";
            return Regex.Replace(fileContent, pattern, newMacroString);
        }

        public string AddNewMacro(string fileContent, string name)
        {
            var newMacro = new Macro { Name = name };
            return fileContent + "\n" + GenerateMacroDefinition(newMacro);
        }

        private string GenerateMacroDefinition(Macro macro)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{macro.Name}: {macro.Name} {{");
            sb.AppendLine("    compatible = \"zmk,behavior-macro\";");
            sb.AppendLine($"    label = \"{macro.Name.ToUpper()}_MACRO\";");
            sb.AppendLine("    #binding-cells = <0>;");
            sb.AppendLine("    bindings = <");

            for (int i = 0; i < macro.Actions.Count; i++)
            {
                var action = macro.Actions[i];
                string binding = !string.IsNullOrEmpty(action.ZmkModifier)
                    ? $"&kp {action.ZmkModifier}({action.ZmkAction})"
                    : $"&kp {action.ZmkAction}";

                sb.Append($"        {binding}");
                if (i < macro.Actions.Count - 1)
                {
                    sb.Append(",");
                }
                sb.AppendLine();
            }

            sb.AppendLine("    >;");
            sb.AppendLine("};");
            return sb.ToString();
        }
    }
}