using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shpellbook
{
    public class BackgroundEvent
    {
        
        // Read bout readonly (not so explicit):
        // https://docs.microsoft.com/fr-fr/dotnet/csharp/language-reference/keywords/readonly
        public readonly Task task;

        // Read about volatile:
        // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/volatile
        private volatile bool _stop;

        /// <summary>
        ///     For testing purposes
        /// </summary>
        public static bool ConsoleAvailable = true;
        public static BackgroundEvent CurrentEvent = null;

        /// <summary>
        ///     Launch an task waiting for the '$' key to be pressed
        /// </summary>
        public BackgroundEvent()
        {
            task = Task.Run(
                () =>
                {
                    if (!ConsoleAvailable)
                    {
                        while (!_stop) ;

                        return;
                    }

                    while (!_stop)
                    {
                        while (!Console.KeyAvailable && !_stop) ;

                        if (_stop)
                            break;

                        var key = Console.ReadKey();
                        if (key.KeyChar == '$')
                            break;
                    }
                }
            );
            CurrentEvent = this;
        }

        public void Stop()
        {
            CurrentEvent = null;
            _stop = true;
        }
    }
}