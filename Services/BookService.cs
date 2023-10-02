public class BookService : IBookService
{
    private readonly List<Book> _books;

    public BookService()
    {
        _books = new List<Book>
        {
            new Book { Id = 1, Title = "1984", Author = "George Orwell" },
            new Book { Id = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee" },
            new Book { Id = 3, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald" },
        };
    }

    public IEnumerable<Book> GetAllBooks() => _books;

    public Book GetBookById(int id) => _books.FirstOrDefault(b => b.Id == id);

    public Book AddBook(BookRequest request)
    {

        // Convert the BookRequest to Book and add it to the list of books.
        var book = new Book { Id = _books.Count + 1, Title = request.Title, Author = request.Author };
        _books.Add(book);
        return book;
    }

    public void UpdateBook(Book book)
    {
        var existingBook = GetBookById(book.Id);
        if (existingBook == null)
        {
            return;
        }
        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
    }

    public void DeleteBook(int id)
    {
        var book = GetBookById(id);
        if (book == null)
        {
            return;
        }
        _books.Remove(book);
    }
}