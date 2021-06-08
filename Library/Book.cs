using System;
using System.Collections.Generic;

namespace Library
{
    public enum Difficulty
    {
        EASY = 0,
        MEDIUM = 1,
        HARD = 2,
        EXTREME = 3
    }

    public class BookSummary
    {
        /// <summary>
        ///     Used to convert the enum values into string of the the ToString() method
        /// </summary>
        private readonly string[] _diff2String = {"Easy", "Medium", "Hard", "Extreme"};


        private readonly string _name; // The Book name
        private readonly string _author; // The Book Author
        private readonly Difficulty _difficulty; // The difficulty to learn the book
        private readonly int _pages; // The number of pages

        public string GetName()
        {
            return _name;
        }

        public string GetAuthor()
        {
            return _author;
        }

        public Difficulty GetDifficulty()
        {
            return _difficulty;
        }

        public int GetPages()
        {
            return _pages;
        }

        /// <summary>
        ///     The BookSummary constructor
        /// </summary>
        public BookSummary(string name, string author, Difficulty difficulty, int pages)
        {
            _name = name;
            _author = author;
            _difficulty = difficulty;
            _pages = pages;
        }

        /// <summary>
        ///     Should create strings like this:
        ///     " "Harry Potter and the Philosopher's Stone" wrote by J.K. Rowling [ Pages: 663; Difficulty: Easy ] "
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var diff = _diff2String[(int) _difficulty];
            return $" \"{_name}\" written by {_author} [ Pages: {_pages}; Difficulty {diff} ] ";
        }
    }

    public class Book
    {
        public static List<Book> books = new List<Book>
        {
            new Book("Task pour les nuls", "Eliottness", Difficulty.EXTREME, 300, "lisez la doc et tester votre code!"),
            new Book("Qu'est ce qu'un shell ?", "Leiyks", Difficulty.EXTREME, 1200, "Lisez la SCL!"),
            new Book("MSDN pour les nuls", "Eliottness", Difficulty.EXTREME, 300, "lisez la doc!"),
            new Book("How to survive Ing1", "Leiyks", Difficulty.EASY, 200, "Write efficient Testsuite :)"),
            new Book("Python pour les nuls", "Eliottness", Difficulty.HARD, 500, "lisez la doc, et testez votre code!"),
            new Book("Moulinette is the new test", "Leiyks", Difficulty.EASY, 5,
                "Laissez la moulinette tester votre code."),
            new Book("Lambda pour les nuls", "Eliottness", Difficulty.EXTREME, 500, "lisez la doc!")
        };

        private static int _idCount;
        private readonly int _id;

        public readonly BookSummary Summary; // A book summary concerning the book
        public readonly string Content; // A String containing its content

        public Book(string name, string author, Difficulty difficulty, int pages, string content)
        {
            Summary = new BookSummary(name, author, difficulty, pages);
            Content = content;
            _id = _idCount++;
        }

        /// <summary>
        ///     Return an empty book whose content is still to write...
        /// </summary>
        public static Book EmptyBook(string name, string author, Difficulty difficulty, int pages)
        {
            return new Book(name, author, difficulty, pages, "");
        }

        /// <summary>
        ///     Create a string displayer for any book
        /// </summary>
        public override string ToString()
        {
            return Summary.ToString();
        }

        public static bool operator ==(Book lhs, Book rhs)
        {
            return rhs is { } && lhs is { } && lhs._id == rhs._id;
        }

        public static bool operator !=(Book rhs, Book lhs)
        {
            return rhs is { } && lhs is { } && lhs._id != rhs._id;
        }

        public override bool Equals(object obj)
        {
            return obj is Book book && _id == book._id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id);
        }
    }

    public struct Shelf
    {
        public readonly List<Book> Books;
        public readonly Func<Book, bool> Criteria;

        public Shelf(Func<Book, bool> criteria)
        {
            Books = new List<Book>();
            Criteria = criteria;
        }
    }
}