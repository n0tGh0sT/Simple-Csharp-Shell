using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Shpellbook
{
    public class Builtins
    {
        public static int Sleep(string[] args)
        {
            if (args.Length == 1)
            {
                Console.Error.WriteLine("sleep: Not enough args");
                return 1;
            }

            if (!int.TryParse(args[1], out var time))
            {
                Console.Error.WriteLine("sleep: argument should be a positive integer");
                return 1;
            }

            // Convert seconds into milliseconds
            Console.WriteLine($"Sleeping for {time} seconds.");
            Thread.Sleep(time * 1000);
            Console.WriteLine("Sleep finished");
            return 0;
        }

        private static bool IsHidden(string file)
        {
            if (file.Length > 0 && file[0] != '.')
                return false;

            return true;
        }

        private static int LsDir(string dir)
        {
            var files = new List<string>();

            foreach (var file in Directory.GetFileSystemEntries(dir))
            {
                var fileName = Path.GetFileName(file);
                if (!IsHidden(fileName))
                    files.Add(fileName);
            }

            if (files.Count > 0)
                Console.Write(Path.GetFileName(files[0]));
            for (var i = 1; i < files.Count; i++)
                Console.Write(" " + Path.GetFileName(files[i]));

            Console.WriteLine();

            return 0;
        }

        public static int Ls(string[] args)
        {
            if (args.Length != 1)
                for (var i = 1; i < args.Length; i++)
                {
                    var file = args[i];
                    if (Directory.Exists(file))
                    {
                        Console.WriteLine("{0}:", file);
                        LsDir(file);
                    }
                    else if (File.Exists(file))
                    {
                        Console.WriteLine(file);
                    }
                    else
                    {
                        Console.Error.WriteLine("ls: cannot access '"
                                                + file + "': No such file or directory");
                        return 1;
                    }
                }
            else
                LsDir(Directory.GetCurrentDirectory());

            return 0;
        }

        public static int Pwd(string[] args)
        {
            if (args.Length > 1)
            {
                Console.Error.WriteLine("pwd: too many arguments");
                return 1;
            }
            
            Console.WriteLine(Directory.GetCurrentDirectory());
            return 0;
        }

        // Bonus: cd method
        // Implemented a small bonus, calling only cd takes you to the parent directory
        
        public static int Cd(string[] dir)
        {
            if (dir.Length == 1)
            {
                Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).FullName);
                return 0;
            }
            
            string currentDir = Directory.GetCurrentDirectory().Split('/', '\\')[^1];
            string[] dirs = dir[1].Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (dir.Length > 2)
            {
                Console.Error.WriteLine("cd: too many arguments");
                return 1;
            }

            foreach (string direction in dirs)
            {
                if (direction == "..")
                {
                    Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).FullName);
                }
                else
                {
                    if (File.Exists(Directory.GetCurrentDirectory() + $"/{direction}"))
                    {
                        Console.Error.WriteLine($"cd: {direction}: Not a directory");
                        return 1;
                    }
                    
                    if (Directory.Exists(Directory.GetCurrentDirectory() + $"/{direction}"))
                    {
                        Directory.SetCurrentDirectory(Directory.GetCurrentDirectory() + $"/{direction}");
                    }
                    else
                    {
                        Console.Error.WriteLine($"cd: {direction}: No such file or directory");
                        return 1;
                    }
                }
                
            }

            return 0;
        }

        // Bonus: clear method
        // Clears the shell
        
        public static int Clear(string[] args)
        {
            if (args.Length > 1)
            {
                Console.Error.WriteLine("clear: too many arguments");
                return 1;
            }
            
            Console.Clear();
            return 0;
        }

        public static int Echo(string[] args)
        {
            if (args[1..] == null) return 1;
            
            foreach (var arg in args[1..])
            {
                Console.Write(arg + " ");
            }
            
            Console.WriteLine();
            return 0;
        }

        public static int Exit(string[] args)
        {
            int res;

            if (args.Length == 1)
            {
                Console.Error.WriteLine("exit: missing argument");
                return 1;
            }

            if (args.Length > 2)
            {
                Console.Error.WriteLine("exit: too many arguments");
                return 1;
            }

            if (Int32.TryParse(args[1], out res))
            {
                if (res < 0)
                {
                    Console.Error.WriteLine("First argument must be a positive integer");
                    return 1;
                }

                Environment.Exit(res);
                return 0;
            }

            return 0;
        }

        public static int Cat(string[] args)
        {
            string[] files = args[1..];

            foreach (var file in files)
            {
                if (!File.Exists(Directory.GetCurrentDirectory() + "/" + file))
                {
                    Console.Error.WriteLine($"cat: {file}: No such file or directory");
                    return 1;
                }

                if (Directory.Exists(Directory.GetCurrentDirectory() + $"/{file}"))
                {
                    Console.Error.WriteLine($"cat: {file}: Is a directory");
                    return 1;
                }

                using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + $"/{file}"))
                {
                    Console.WriteLine(sr.ReadToEnd());
                }
            }

            return 0;
        }

        // Bonus: touch method
        //Creates a file named after the first argument
        
        public static int Touch(string[] args)
        {
            foreach (var arg in args[1..])
            {
                if (File.Exists(arg))
                {
                    Console.WriteLine($"touch: file {arg} already exists in the current directory");
                }
                else
                {
                    File.Create(arg);
                }
            }

            return 0;
        }

        // Bonus: rm method
        // Deletes files passed as arguments
        
        public static int RemoveFile(string[] args)
        {
            foreach (var file in args[1..])
            {
                if (!File.Exists(file))
                {
                    Console.WriteLine($"rm: file {file} does not exist in the current directory");
                }
                else
                {
                    File.Delete(file);
                }
            }

            return 0;
        }

        // Bonus: mkdir method
        // Creates directories based on the given arguments
        
        public static int MakeDirectory(string[] args)
        {
            foreach (var dir in args[1..])
            {
                if (Directory.Exists(dir))
                {
                    Console.WriteLine($"mkdir: directory {dir} already exists in the current directory");
                }
                else
                {
                    Directory.CreateDirectory(dir);
                }
            }

            return 0;
        }

        // Bonus: dragonsay method
        // Prints a dragon saying the text passed as a parameter
        
        public static int DragonSay(string[] args)
        {
            if (args.Length == 1)
            {
                Console.WriteLine("dragonsay: at least one argument is expected");
                return 1;
            }
            
            string[] art = new string[]
            {
                @"      \                    / \  //\",
                @"       \    |\___/|      /   \//  \\",
                @"            /0  0  \__  /    //  | \ \",
                @"           /     /  \/_/    //   |  \  \",
                @"           @_^_@'/   \/_   //    |   \   \",
                @"           //_^_/     \/_ //     |    \    \",
                @"        ( //) |        \///      |     \     \",
                @"      ( / /) _|_ /   )  //       |      \     _\",
                @"    ( // /) '/,_ _ _/  ( ; -.    |    _ _\.-~        .-~~~^-.",
                @"  (( / / )) ,-{        _      `-.|.-~-.           .~         `.",
                @" (( // / ))  '/\      /                 ~-. _ .-~      .-~^-.  \",
                @" (( /// ))      `.   {            }                   /      \  \",
                @"  (( / ))     .----~-.\        \-'                 .~         \  `. \^-.",
                @"             ///.----..>        \             _ -~             `.  ^-`  ^-_",
                @"               ///-._ _ _ _ _ _ _}^ - - - - ~                     ~-- ,.-~",
                @"                                                                  /.-~          "
            };

            string message = null;
            
            foreach (var arg in args[1..])
            {
                message += arg + " ";
            }

            string topLine = " ";
            string bottomLine = " ";
            
            for (int i = 0; i < message.Length; i++)
            {
                topLine += "_";
                bottomLine += "-";
            }

            topLine += " ";
            bottomLine += " ";
            
            Console.Write(topLine + "\n");
            Console.WriteLine($"< {message} >");
            Console.WriteLine(bottomLine);

            foreach (var line in art)
            {
                Console.WriteLine(line);
            }
            
            return 0;
        }
        
        // Bonus: lsb
        // Prints the current directory content with colors depending on their type
        
        public static int LsBonus(string[] args)
        {
            if (args.Length != 1)
                for (var i = 1; i < args.Length; i++)
                {
                    var file = args[i];
                    if (Directory.Exists(file))
                    {
                        Console.WriteLine("{0}:", file);
                        LsDirColored(file);
                    }
                    else if (File.Exists(file))
                    {
                        Console.WriteLine(file);
                    }
                    else
                    {
                        Console.Error.WriteLine("ls: cannot access '"
                                                + file + "': No such file or directory");
                        return 1;
                    }
                }
            else
                LsDirColored(Directory.GetCurrentDirectory());

            return 0;
        }
        
        // Bonus: lsb
        // Prints the current directory content with colors depending on their type
        private static int LsDirColored(string dir)
        {
            var files = new List<string>();

            foreach (var file in Directory.GetFileSystemEntries(dir))
            {
                var fileName = Path.GetFileName(file);
                if (!IsHidden(fileName))
                    files.Add(fileName);
            }

            if (files.Count > 0)
            {
                if (Directory.Exists(Path.GetFileName(files[0])))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(Path.GetFileName(files[0]));
                    Console.ResetColor();
                }

                else if (File.Exists(Path.GetFileName(files[0])))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(Path.GetFileName(files[0]));
                    Console.ResetColor();
                }
            }
            for (var i = 1; i < files.Count; i++)
            {
                if (Directory.Exists(Path.GetFileName(files[i])))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" " + Path.GetFileName(files[i]));
                    Console.ResetColor();
                }

                else if (File.Exists(Path.GetFileName(files[i])))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" " + Path.GetFileName(files[i]));
                    Console.ResetColor();
                }
            }

            Console.WriteLine();

            return 0;
        }
    }
}