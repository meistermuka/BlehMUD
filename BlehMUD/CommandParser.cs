using BlehMUD.Interfaces;
using BlehMUD.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using BlehMUD.Entities;

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
                {"quit", new QuitCommand() },
                {"north", new MoveCommand() },
                {"south", new MoveCommand() },
                {"west", new MoveCommand() },
                {"east", new MoveCommand() },
                {"up", new MoveCommand() },
                {"down", new MoveCommand() }
            };
        }

        public string ParseAndExecute(string input, Player player)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(input))
                {
                    string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    string commandName = parts[0].ToLower();
                    string[] args = { "" };

                    if (_commands.TryGetValue(commandName, out ICommand command))
                    {
                        if (CommandType.IsTypeDirection(commandName))
                        {
                            args[0] = commandName;
                        }
                        else
                        {
                            args = parts.Skip(1).ToArray();
                        }
                        
                        return command.Execute(player, args);
                    }

                    return "Command not recognized.\r\n";
                }
                else
                {
                    return "Empty or null string!\r\n";
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"Exception: {e}");
                return "Nothing.\r\n";
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
