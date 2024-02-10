
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public class DescriptionService(DescriptionRepository descriptionRepository, BookRepository bookRepository)
{
    private readonly DescriptionRepository _descriptionRepository = descriptionRepository;
    private readonly BookRepository _bookRepository = bookRepository;

    public async Task<bool> CreateDescriptionAsync(AddDescriptionToBookDto addDescription)
    {
        try
        {
            if (!await _descriptionRepository.ExistingAsync(x => x.Id == addDescription.Id))
            {
                var descriptionEntity = await _descriptionRepository.CreateAsync(new DescriptionEntity 
                { 
                    Description = addDescription.Description,
                    Language = addDescription.Language,
                    Narrator = addDescription.Narrator,
                    Illustrator = addDescription.Illustrator,
                    BookId = addDescription.BookId,
                });
                if (descriptionEntity != null)
                {
                    return true;
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }

    public async Task<AddDescriptionToBookDto> CreateDescriptionToBookAsync(AddDescriptionToBookDto descriptionDto, string isbn)
    {
        try
        {
            //kolla först så att det finns en bok att lägga till en beskrivning till
            var bookExists = await _bookRepository.GetAsync(x => x.ISBN == isbn);

            if (bookExists != null)
            {
                var newDescription = new DescriptionEntity
                {
                    Description = descriptionDto.Description,
                    Language = descriptionDto.Language,
                    Narrator = descriptionDto.Narrator,
                    Illustrator = descriptionDto.Illustrator,
                    BookId = bookExists.Id
                };
                await _descriptionRepository.CreateAsync(newDescription); //skapa beskrivningen
                bookExists.Description.Add(newDescription); //lägg till den skapade beskrivningen till bok
                
                await _bookRepository.CreateAsync(bookExists); 
                return new AddDescriptionToBookDto
                {
                    Description = descriptionDto.Description,
                    Language = descriptionDto.Language,
                    Narrator = descriptionDto.Narrator,
                    Illustrator = descriptionDto.Illustrator,
                    BookId = bookExists.Id
                };
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public async Task<IEnumerable<DescriptionDto>> GetBookWithDescriptionAsync(string isbn)
    {
        try
        {
           var descriptionEntity = await _descriptionRepository.GetAllAsync();
            var bookDescription = new List<DescriptionDto>();
            foreach (var description in descriptionEntity)
            {
                var bookEntity = await _bookRepository.GetAsync(x => x.Id == description.BookId);
                if (bookEntity != null)
                {
                    if (isbn == bookEntity.ISBN)
                    {
                        bookDescription.Add(new DescriptionDto
                        {
                            Description = description.Description,
                            Language = description.Language,
                            Narrator = description.Narrator,
                            Illustrator = description.Illustrator,
                            BookId = bookEntity.Id
                        });
                    }
                }
            }
            return bookDescription;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public async Task<DescriptionDto> UpdateDescriptionAsync(DescriptionDto description)
    {
        try
        {
            var updatedDescription = await _descriptionRepository.UpdateAsync(x => x.Id == description.Id);
            if (updatedDescription != null)
            {
                var descriptionDto = new DescriptionDto 
                { 
                    Id = updatedDescription.Id, 
                    Description = updatedDescription.Description,
                    Language = updatedDescription.Language,
                    Narrator = updatedDescription.Narrator,
                    Illustrator = updatedDescription.Illustrator,
                    BookId = updatedDescription.BookId,
                };
                return descriptionDto;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public async Task<bool> DeleteDescriptionAsync(int id)
    {
        try
        {
            var result = await _descriptionRepository.DeleteAsync(x => x.Id == id);
            return true;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }
}
