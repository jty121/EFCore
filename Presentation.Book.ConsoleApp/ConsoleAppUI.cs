

using Infrastructure.Dtos;
using Infrastructure.Services;

namespace Presentation.Book.ConsoleApp;

public class ConsoleAppUI(BookService bookService, DescriptionService descriptionService, ManufactureService manufactureService)
{
    private readonly BookService _bookService = bookService;
    private readonly DescriptionService _descriptionService = descriptionService;
    private readonly ManufactureService _manufacturerService = manufactureService;

    public async Task Show()
    {
        await CreateNewBook();
        await CreateDescription();
        await ShowInformationAboutBook();
        await ShowAllBooks();
        await ShowUpdate();
        await ShowDelete();
    }


    public async Task CreateNewBook()
    {
        Console.Clear();
        var bookDto = new BookDto();

        Console.WriteLine("------ CREATE NEW BOOK ------");
        Console.WriteLine("ISBN: ");
        bookDto.ISBN = Console.ReadLine()!;

        Console.WriteLine("Title: ");
        bookDto.Title = Console.ReadLine()!;

        Console.WriteLine("Author: ");
        bookDto.Author = Console.ReadLine()!;

        Console.WriteLine("Price: ");
        bookDto.Price = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Category: ");
        bookDto.CategoryName = Console.ReadLine()!;

        Console.WriteLine("Publisher: ");
        bookDto.PublisherName = Console.ReadLine()!;

        var result = await _bookService.CreateBookAsync(bookDto);

        if (result != null)
        {
            Console.WriteLine("Book was created successfully!");
        }
        else
        {
            Console.WriteLine("Something went wrong!");
        }
        Console.ReadKey();
    }

    public async Task CreateDescription()
    {
        Console.Clear();
        var addDescriptionToBookDto = new AddDescriptionToBookDto();

        Console.WriteLine("------ CREATE DESCRIPTION ------");
        Console.WriteLine("Enter ISBN: ");
        string isbn = Console.ReadLine()!;

        Console.WriteLine("Add Description: ");
        addDescriptionToBookDto.Description = Console.ReadLine()!;

        Console.WriteLine("Add Language: ");
        addDescriptionToBookDto.Language = Console.ReadLine()!;

        Console.WriteLine("Add Narrator: ");
        addDescriptionToBookDto.Narrator = Console.ReadLine()!;

        Console.WriteLine("Add Illustrator: ");
        addDescriptionToBookDto.Illustrator = Console.ReadLine()!;

        await _descriptionService.CreateDescriptionToBookAsync(addDescriptionToBookDto, isbn);
        Console.ReadKey();
    }

    public async Task ShowInformationAboutBook()
    {
        Console.Clear();
        Console.WriteLine("------ VIEW INFORMATION ABOUT BOOK ------");
        Console.WriteLine("Enter ISBN: ");
        string isbn = Console.ReadLine()!;
        var bookInformation = await _bookService.GetBookDescriptionAsync(isbn);

        Console.WriteLine($"Title: {bookInformation.Title}");
        Console.WriteLine($"Author: {bookInformation.Author}");

        foreach (var descriptionDto in bookInformation.Descriptions)
        {
            Console.WriteLine($"Description: {descriptionDto.Description}, Language: {descriptionDto.Language}, Narrator: {descriptionDto.Narrator}, Illustrator: {descriptionDto.Illustrator}");
        }
        Console.ReadKey();
    }

    public async Task ShowAllBooks()
    {
        Console.Clear();
        Console.WriteLine("------ VIEW ALL BOOKS ------");
        var booksInStorage = await _bookService.GetAllBooksAsync();

        foreach (var book in booksInStorage)
        {
            Console.WriteLine($"Title: {book.Title}, Author: {book.Author}, Price: {book.Price} SEK");
        }
        Console.ReadKey();
    }

    public async Task ShowUpdate()
    {
        Console.Clear();
        var bookDto = new BookDto();

        Console.WriteLine("------ UPDATE BOOK ------");
        Console.WriteLine("Enter ISBN: ");
        bookDto.ISBN = Console.ReadLine()!;

        Console.WriteLine("Update Titel: ");
        bookDto.Title = Console.ReadLine()!;

        Console.WriteLine("Update Author: ");
        bookDto.Author = Console.ReadLine()!;

        Console.WriteLine("Update Price: ");
        bookDto.Price = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Update category: ");
        bookDto.CategoryName = Console.ReadLine()!;

        Console.WriteLine("Update Publisher: ");
        bookDto.PublisherName = Console.ReadLine()!;

        var manufactureEntity = await _manufacturerService.GetOrCreateManufactureAsync(bookDto.PublisherName);
        var updatedBook = await _bookService.UpdateBook(bookDto, manufactureEntity);

        if (updatedBook != null)
        {
            Console.WriteLine("Book was updated successfully!");
            Console.WriteLine($"Title: {bookDto.Title}, Author: {bookDto.Author}, Price: {bookDto.Price}, Category: {bookDto.CategoryName}, Publisher: {bookDto.PublisherName}");
        }
        else { Console.WriteLine("Something went wrong!"); }

        Console.ReadKey();
    }

    public async Task ShowDelete()
    {
        Console.Clear();
        var bookDto = new BookDto();

        Console.WriteLine("------ DELETE BOOK ------");
        Console.WriteLine("Enter ISBN: ");
        bookDto.ISBN = Console.ReadLine()!;

        if (bookDto.ISBN != null)
        {
            await _bookService.DeleteBookByIsbn(bookDto.ISBN);
        }
        else
        {
            Console.WriteLine($"Can´t find book with ISBN: {bookDto.ISBN}");
        }
        Console.ReadKey();
    }
}
