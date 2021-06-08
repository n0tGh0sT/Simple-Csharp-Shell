using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Shpellbook
{
    public class Eval
    {
        public static Dictionary<string, Func<string[], int>> builtins =
            new Dictionary<string, Func<string[], int>>
            {
                {"ls", Builtins.Ls},
                {"sleep", Builtins.Sleep},
                {"pwd", Builtins.Pwd},
                {"clear", Builtins.Clear},
                {"echo", Builtins.Echo},
                {"exit", Builtins.Exit},
                {"cd", Builtins.Cd},
                {"cat", Builtins.Cat},
                {"touch", Builtins.Touch},
                {"rm", Builtins.RemoveFile},
                {"mkdir", Builtins.MakeDirectory},
                {"dragonsay", Builtins.DragonSay},
                {"lsb", Builtins.LsBonus}
            };

        public static List<Task<int>> jobs = new List<Task<int>>();

        /// <summary>
        ///     Launch a Process and wait for it in a Task
        /// </summary>
        /// <param name="command">
        ///     The command object to evaluate
        /// </param>
        /// <returns>
        ///     The return code of the process
        /// </returns>
        public static Task<int> EvaluatePath(Command command)
        {
            return Task.Run(() =>
            {
                using var process = new Process();
                var processInfo = new ProcessStartInfo(command.args[0]);

                for (var i = 1; i < command.args.Length; i++)
                    processInfo.ArgumentList.Add(command.args[i]);

                process.StartInfo = processInfo;

                try
                {
                    process.Start();
                    process.WaitForExit();
                }

                // This exception also works on Unix
                // Guess they are doing a good job here :)
                catch (Win32Exception)
                {
                    return 127;
                }

                return process.ExitCode;
            });
        }

        public static Task<int> EvaluateBuiltin(Command command)
        {
            return Task.Run(() =>
            {
                if (builtins.ContainsKey(command.args[0]))
                    return builtins[command.args[0]](command.args);
                return 127;
            });
        }
        
        /// <summary>
        ///     Launching the result gave by the parser
        /// </summary>
        /// <param name="command">
        ///     The command object returned by the parser
        /// </param>
        /// <returns>
        ///     Return the integer the execution of the command returned
        /// </returns>
        public static int Evaluate(Command command)
        {
            BackgroundEvent bg = new BackgroundEvent();
            Task<int> task = EvaluateBuiltin(command);
            int firstEnd = Task.WaitAny(new Task[] {bg.task, task});
            if (firstEnd == 1)
            {
                BackgroundEvent.CurrentEvent.Stop();
                return task.Result;
            }
            
            jobs.Add(task);
            return -1;
        }

        public static void UpdateJobs()
        {
            foreach (var job in jobs)
            {
                if (job.IsCompleted)
                {
                    Console.WriteLine($"Job number {jobs.FindIndex(jobby => jobby == job)} terminated with code {job.Result}");
                }
            }
            
            jobs.RemoveAll(job => job.IsCompleted);
        }
    }
}