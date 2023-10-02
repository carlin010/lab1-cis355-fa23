public interface IBookService
{
    IEnumerable<Book> GetAllBooks();
    Book GetBookById(int id);
    Book AddBook(BookRequest book);
    void UpdateBook(Book book);
    void DeleteBook(int id);
}