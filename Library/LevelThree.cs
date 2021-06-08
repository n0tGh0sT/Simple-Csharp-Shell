using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Library
{
    public class LevelThree
    {
        /// <summary>
        ///     Generates all primes numbers up to n included
        /// </summary>
        public static List<int> UsualPrimesGenerator(int n)
        {
            if (n < 0)
                return new List<int>();
            
            var numbers = Enumerable.Range(2, n).ToList();
            numbers.RemoveAll(x =>
            {
                for (var i = 2; i < x; i++)
                    if (x % i == 0 && x != i)
                        return true;

                return false;
            });

            return numbers;
        }


        /// <summary>
        ///     Remove integers divisible by n in the list
        /// </summary>
        /// <param name="n">the base multiple</param>
        /// <param name="primes">the list to remove from</param>
        public static void RemoveNotPrimes(int n, List<int> primes)
        {
            lock (primes)
            {
                primes.RemoveAll(x => x % n == 0 && x != n);
            }
        }

        public static List<int> MagicPrimesGenerator(int n, int nbTasks)
        {
            if (n < 2) return new List<int>();

            List<int> primes = Enumerable.Range(2, n - 1).ToList();

            Task[] tasks = new Task[nbTasks];

            List<int>[] array = new List<int>[nbTasks]; 
            
            for (int i = 0; i < nbTasks; i++)
            {
                array[i] = primes.GetRange(i * primes.Count / nbTasks, primes.Count / nbTasks);
            }

            for (int i = 2; i < Math.Sqrt(n); i++)
            {
                for (int j = 0; j < nbTasks; j++)
                {
                    tasks[j] = Task.Run(() => RemoveNotPrimes(i, array[j % nbTasks]));
                }

                Task.WaitAll(tasks);
            }

            List<int> res = new List<int>();

            for (int i = 0; i < array.Length; i++)
            {
                var list = res.Concat(array[i]).ToList();
                res = list;
            }
            
            return res;
        }
    }
}
