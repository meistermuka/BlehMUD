using BlehMUD.Interfaces;
using BlehMUD.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace BlehMUD
{
    public class CommandParser
    {
        private readonly Dictionary<string, ICommand> _commands;
        public CommandParser() 
        {
            Console.WriteLine("Registering commands");
            _commands = new Dictionary<string, ICommand>
            {
                {"look", new LookCommand() },
                {"l", new LookCommand() },
                {"quit", new QuitCommand() }
            };
        }

        public string ParseAndExecute(string input)
        {
            if(!string.IsNullOrWhiteSpace(input))
            {
                string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string commandName = parts[0].ToLower();

                if (_commands.TryGetValue(commandName, out ICommand command))
                {
                    string[] args = parts.Skip(1).ToArray();
                    return command.Execute(args);
                }

                List<string> suggestions = GetCommandSuggestions(commandName);
                if (suggestions.Count > 0)
                {
                    string suggestionText = string.Join(", ", suggestions);
                    return $"Command not recognized. Did you mean: {suggestionText}?\r\n";
                }

                return "Command not recognized.\r\n";
            }
            else
            {
                return string.Empty;
            }
        }

        private List<string> GetCommandSuggestions(string input)
        {
            // Implement logic to generate command suggestions based on input
            List<string> suggestions = new List<string>();
            foreach (string commandName in _commands.Keys)
            {
                if (commandName.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                {
                    suggestions.Add(commandName);
                }
            }
            return suggestions;
        }

    }
}
