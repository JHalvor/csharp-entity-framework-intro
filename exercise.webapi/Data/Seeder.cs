using exercise.webapi.Models;

namespace exercise.webapi.Data
{
    public class Seeder
    {
        private List<string> _firstnames = new List<string>()
        {
            "Audrey",
            "Donald",
            "Elvis",
            "Barack",
            "Oprah",
            "Jimi",
            "Mick",
            "Kate",
            "Charles",
            "Kate"
        };
        private List<string> _lastnames = new List<string>()
        {
            "Hepburn",
            "Trump",
            "Presley",
            "Obama",
            "Winfrey",
            "Hendrix",
            "Jagger",
            "Winslet",
            "Windsor",
            "Middleton"

        };
        private List<string> _domain = new List<string>()
        {
            "bbc.co.uk",
            "google.com",
            "theworld.ca",
            "something.com",
            "tesla.com",
            "nasa.org.us",
            "gov.us",
            "gov.gr",
            "gov.nl",
            "gov.ru"
        };
        private List<string> _firstword = new List<string>()
        {
            "The",
            "Two",
            "Several",
            "Fifteen",
            "A bunch of",
            "An army of",
            "A herd of"


        };
        private List<string> _secondword = new List<string>()
        {
            "Orange",
            "Purple",
            "Large",
            "Microscopic",
            "Green",
            "Transparent",
            "Rose Smelling",
            "Bitter"
        };
        private List<string> _thirdword = new List<string>()
        {
            "Buildings",
            "Cars",
            "Planets",
            "Houses",
            "Flowers",
            "Leopards"
        };

        private List<Author> _authors = new List<Author>();
        private List<Book> _books = new List<Book>();
        private List<AuthorBook> _authorBooks = new List<AuthorBook>();

        public Seeder()
        {
            Random authorRandom = new Random();
            Random bookRandom = new Random();

            int entitiesToCreate = 30;

            for (int x = 1; x < entitiesToCreate; x++)
            {
                Author author = new Author();
                author.Id = x;
                author.FirstName = _firstnames[authorRandom.Next(_firstnames.Count)];
                author.LastName = _lastnames[authorRandom.Next(_lastnames.Count)];
                author.Email = $"{author.FirstName}.{author.LastName}@{_domain[authorRandom.Next(_domain.Count)]}".ToLower();
                author.Books = _authorBooks.Where(ab => ab.AuthorId == author.Id).SelectMany(ab => _books.Where(b => b.Id == ab.BookId)).ToList();
                _authors.Add(author);
            }

            for (int y = 1; y < entitiesToCreate; y++)
            {
                Book book = new Book();
                book.Id = y;
                book.Title = $"{_firstword[bookRandom.Next(_firstword.Count)]} {_secondword[bookRandom.Next(_secondword.Count)]} {_thirdword[bookRandom.Next(_thirdword.Count)]}";
                book.Authors = _authorBooks.Where(ab => ab.BookId == book.Id).SelectMany(ab => _authors.Where(a => a.Id == ab.AuthorId)).ToList();
                _books.Add(book);
            }

            for (int z = 1; z < entitiesToCreate * 3; z++)
            {
                AuthorBook authorBook = new AuthorBook();
                authorBook.Id = z;
                authorBook.AuthorId = _authors[authorRandom.Next(_authors.Count)].Id;
                authorBook.BookId = _books[bookRandom.Next(_books.Count)].Id;

                _authorBooks.Add(authorBook);
            }
        }
        public List<Author> Authors { get { return _authors; } }
        public List<Book> Books { get { return _books; } }
        public List<AuthorBook> AuthorBooks { get { return _authorBooks; } }
    }
}
