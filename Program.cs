using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

# region Swagger / Build
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1"});
});

// This line registers the IBookService class with the application's "services container."
// The 'AddSingleton' method tells the application to create one instance of IBookService and
// use that same instance every time the service is needed. This way, you don't have to manually
// create a new IBookService each time you need to use it; the system will automatically give you
// the one it has created. This is a technique called "Dependency Injection."
builder.Services.AddSingleton<IBookService, BookService>();
builder.Services.AddScoped<IValidator<BookRequest>, BookValidator>();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json","My API");
});

# endregion

app.MapGet("/", () => "Hello World!");

// Mapping an HTTP GET request to the "/books" endpoint.
// When someone navigates to this URL, the list of all books will be returned.
app.MapGet("/books", (IBookService bookService) => {
    return bookService.GetAllBooks();
});

// Mapping an HTTP GET request to the "/books/{id}" endpoint.
// The {id} in the URL is a parameter, meaning it will be replaced by the ID of the book you're looking to get.
// For example, "/books/1" will look for the book with an ID of 1.
app.MapGet("/books/{id}", (int id, IBookService bookService) => 
{
    // Use LINQ's FirstOrDefault method to search for a book with a matching ID in the list.
    // If no book is found, 'book' will be null.
    var book = bookService.GetBookById(id);

    // Check if the book was found.
    if (book == null) return Results.NotFound();

    // If found, return the book along with a 200 OK status.
    return Results.Ok(book);
});

// Map a POST request to the "/books" endpoint.
// This will allow clients to create new books.
// The new book's data will be passed in the request body and mapped to the 'newBook' parameter.
app.MapPost("/books", async (BookRequest newBook, IBookService bookService, IValidator<BookRequest> validator) =>
{
    var validationResult = await validator.ValidateAsync(newBook);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }
    // Add the new book to the list of books in the IBookService class.
    var response = bookService.AddBook(newBook);

    // Return a 201 Created status along with the newly created book.
    // The Created status code indicates that the resource was successfully created.
    return Results.Created($"/books/{response.Id}", response);
});

// Map a PUT request to the "/books/{id}" endpoint.
// This will allow clients to update an existing book identified by its ID.
// The book's new data will be passed in the request body and mapped to the 'updatedBook' parameter.
app.MapPut("/books/{id}", (int id, Book updatedBook, IBookService bookService) =>
{
    bookService.UpdateBook(updatedBook);

    // Return a 200 OK status along with the updated book.
    // The OK status code indicates that the request was successful.
    return Results.Ok(updatedBook);
});

// Map a DELETE request to the "/books/{id}" endpoint.
// This will allow clients to delete an existing book identified by its ID.
app.MapDelete("/books/{id}", (int id, IBookService bookService) =>
{
    bookService.DeleteBook(id);

    // Return a 200 OK status along with the deleted book.
    // The OK status code indicates that the request was successful.
    return Results.Ok(null);
});

app.Run();
