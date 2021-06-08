using System;
using System.Collections.Generic;
using System.Linq;

namespace Library
{
    public class LevelOne
    {
        /// <summary>
        ///     Remove all negatives values from the list
        /// </summary>
        public static void FilterNegative(List<int> list)
        {
            list.RemoveAll((int i) => i < 0);
        }

        /// <summary>
        ///     Return same as string.ToUpper() but in a List of char
        /// </summary>
        public static List<char> ToUpper(List<char> characters)
        {
            return characters.Select(char.ToUpper).ToList();
        }

        /// <summary>
        ///     Use the function book.ToString() to print the list of books
        /// </summary>
        public static void PrintBooks(List<Book> books)
        {
            foreach (Book book in books) { Console.WriteLine(book.Summary.ToString());}
        }

        /// <summary>
        ///     Returns a list of books whose author is the one we want
        /// </summary>
        public static List<Book> FindAuthorBooks(List<Book> books, string author)
        {
            return books.Where(book => book.Summary.GetAuthor() == author).ToList();
        }

        /// <summary>
        ///     Return the maximum number of pages a book has in the list
        /// </summary>
        public static int GetLongestBookPages(List<Book> books)
        {
            return books.Max(book => book.Summary.GetPages());
        }

        /// <summary>
        ///     Return the number of books which content is containing the substring in parameters
        /// </summary>
        public static int FindOccurences(List<Book> books, string substring)
        {
            return books.Count(book => book.Content.Contains(substring));
        }
    }
}