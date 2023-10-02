public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
}

public class BookRequest
{
    public string Title { get; set; }
    public string Author { get; set; }
}

public class BookResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
}