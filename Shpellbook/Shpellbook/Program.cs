using System;
using System.IO;

namespace Shpellbook
{
    public class Program
    {
        public static void Run(TextReader input, bool isConsole)
        {
            Parser parser = new Parser(input);

            while (true)
            {
                if (isConsole)
                    Console.Write("Shpellbook$ ");
    
                Command command = parser.ParseInput();
            
                if (command == null)
                    break;
                if (command.args.Length == 0)
                    continue;

                int res = Eval.Evaluate(command);
                
                if (res == -1)
                    Console.WriteLine("Program in running in background");
                else
                    Console.WriteLine($"Command ended with exit code {res}");
                
                Eval.UpdateJobs();
            }
        }

        public static void Main(string[] args)
        {
            if (args.Length == 0)
                Run(Console.In, true);
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (File.Exists(args[i]))
                    {
                        TextReader tx = new StreamReader($"{args[i]}");
                        Run(tx, true);
                    }
                    else
                    {
                        Console.Error.WriteLine($"The path {args[i]} does not exist or is not a file");
                    }
                }
            }
        }
    }
}