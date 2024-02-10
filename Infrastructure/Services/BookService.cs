using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;

namespace Infrastructure.Services;

public class BookService(BookRepository bookRepository, CategoryRepository categoryRepository, ManufactureRepository manufactureRepository, CategoryService categoryService, ManufactureService manufactureService)
{
    private readonly BookRepository _bookRepository = bookRepository;
    private readonly CategoryRepository _categoryRepository = categoryRepository;
    private readonly CategoryService _categoryService = categoryService;
    private readonly ManufactureRepository _manufacturerRepository = manufactureRepository;
    private readonly ManufactureService _manufactureService = manufactureService;

    public async Task<BookDto> CreateBookAsync(BookDto book)
    {
        try
        {
            //kontrollera om det finns en bok med samma isbn nummer först
           if(!await _bookRepository.ExistingAsync(x => x.ISBN == book.ISBN))
            {
                var categoryEntity = await _categoryRepository.CreateAsync(new CategoryEntity { CategoryName = book.CategoryName });
                // skapa relation till kopplingstabellen, många-till-många = ICollection = lista
                var manufactureEntity = await _manufacturerRepository.CreateAsync(new ManufactureEntity { PublisherName = book.PublisherName });
                var bookPublisher = new List<BookPublisherEntity> { new() { ManufactureId = manufactureEntity.Id } };

                var bookEntity = await _bookRepository.CreateAsync(new BookEntity
                {
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Author = book.Author,
                    Price = book.Price,
                    CategoryId = categoryEntity.Id,
                    BookPublisher = bookPublisher, // lägg till listan här för att hantera den skapade kopplingstabellens properties
                });
                return book;
            }   
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
    {
        try
        {
           var bookEntities = await _bookRepository.GetAllAsync();
            if(bookEntities != null)
            {
                var ListOfBooks = new List<BookDto>();
                foreach (var bookEntity in bookEntities)
                {
                    ListOfBooks.Add(new BookDto
                    {
                        ISBN = bookEntity.ISBN,
                        Title = bookEntity.Title,
                        Author = bookEntity.Author,
                        Price = bookEntity.Price,
                    });
                }
                return ListOfBooks;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }
    
    public async Task<BookDto> GetOneBookAsync(string isbn)
    {
        try
        {
            var bookEntity = await _bookRepository.GetAsync(x => x.ISBN == isbn);
            if(bookEntity != null)
            {
                var bookDto = new BookDto
                {
                    ISBN = bookEntity.ISBN,
                    Title = bookEntity.Title,
                    Author = bookEntity.Author,
                    Price = bookEntity.Price,
                };
                return bookDto;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public async Task<BookDto> GetBookDescriptionAsync(string isbn)
    {
        try
        {
            var bookEntity = await _bookRepository.GetAsync(x => x.ISBN == isbn);
            if( bookEntity != null )
            {
                var bookDto = new BookDto
                {
                    ISBN = bookEntity.ISBN,
                    Title = bookEntity.Title,
                    Author = bookEntity.Author,
                    Price = bookEntity.Price,
                };
                foreach(var description in bookEntity.Description)
                {
                    bookDto.Descriptions.Add(new DescriptionDto
                    {
                        Description = description.Description,
                        Language = description.Language,
                        Narrator = description.Narrator,
                        Illustrator = description.Illustrator,
                        BookId = description.BookId,
                    });
                }

                return bookDto;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public async Task<IEnumerable<BookDto>> GetBooksByCategoryAsync(string categoryName)
    {
        try
        {
            var bookEntity = await _bookRepository.GetAllAsync();
            var byCategory = new List<BookDto>();
            foreach(var book in byCategory)
            {
                var categoryEntity = await _categoryRepository.GetAsync(x => x.Id == book.CategoryId);
                if(categoryEntity != null)
                {
                    if(categoryName == categoryEntity.CategoryName)
                    {
                        byCategory.Add(new BookDto
                        {
                            ISBN = book.ISBN,
                            Title = book.Title,
                            Author = book.Author,
                            Price = book.Price,
                            CategoryName = categoryEntity.CategoryName
                        });
                    }
                }
            }
            return byCategory; //får tillbaka böckerna från en specifik kategori
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public async Task<BookDto> UpdateBook(BookDto book, ManufactureEntity manufactureEntity)
    {
        try
        {
            //kolla så att boken finns med unika isbn nummret
            var existingBookEntity = await _bookRepository.GetAsync(x => x.ISBN == book.ISBN);
            if (existingBookEntity != null)
            {
                var categoryEntity = await _categoryService.GetCategoryAsync(existingBookEntity.CategoryId);
                var manufactureToUpdate = await _manufactureService.GetOrCreateManufactureAsync(book.PublisherName);
                var bookPublisher = new List<BookPublisherEntity> { new() { ManufactureId = manufactureEntity.Id } };

                existingBookEntity.Title = book.Title;
                existingBookEntity.Author = book.Author;
                existingBookEntity.Price = book.Price;
                existingBookEntity.CategoryId = categoryEntity.Id;
                existingBookEntity.BookPublisher = bookPublisher;

                //hämta metoden från bookrepository som uppdaterar, savechanges härleds tillbaka till baserepository som bookrepository ärver ifrån.
                await _bookRepository.UpdateOneAsync(existingBookEntity);
                return book;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public async Task<bool> DeleteBookByIsbn(string Isbn)
    {
        try
        {
            var result = await _bookRepository.DeleteAsync(x => x.ISBN == Isbn);
            return true;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }
}
