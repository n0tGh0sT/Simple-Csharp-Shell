using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Library
{
    public class LevelTwo
    {
        /// <summary>
        ///     Suppose we can read a book in 1000 milliseconds, simulate someone reading this list of books
        /// </summary>
        public static void Reading(List<Book> books)
        {
            foreach (var book in books)
            {
                Thread.Sleep(1000);
                Console.WriteLine(book.Summary.ToString());
            }
        }

        /// <summary>
        ///     Create /parallel/ readers who are reading together
        ///     Make the reader1 start reading first
        /// </summary>
        public static void PairReading(List<Book> reader1, List<Book> reader2)
        {
            Task books1 = new Task(() => Reading(reader1));
            Task books2 = new Task(() => Reading(reader2));

            books1.Start();
            books2.Start();
            Task.WaitAll(new[] {books1, books2});
        }


        /// <summary>
        ///     Should find the first shelf which satisfy the criteria
        ///     from each book until the time is over.
        /// </summary>
        /// <param name="books">the books to sort, shall not be modified</param>
        /// <param name="shelves">all the shelves there is</param>
        /// <param name="time">the time of the detention in milliseconds</param>
        public static void Detention(List<Book> books, Shelf[] shelves, int time)
        {
            
        }
    }
}