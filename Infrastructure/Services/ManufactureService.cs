
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public class ManufactureService(ManufactureRepository manufactureRepository)
{
    private readonly ManufactureRepository _manufactureRepository = manufactureRepository;

    //ändrat bool till entity istället
    public async Task<ManufactureEntity> GetOrCreateManufactureAsync(string publisherName)
    {
        try
        {
            if (!await _manufactureRepository.ExistingAsync(x => x.PublisherName == publisherName))
            {
                var manufactureEntity = await _manufactureRepository.CreateAsync(new ManufactureEntity { PublisherName = publisherName });
                if (manufactureEntity != null)
                {
                    return manufactureEntity;
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public async Task<ManufactureDto> GetManufactureAsync(Expression<Func<ManufactureEntity, bool>> predicate)
    {
        try
        {
            var manufactureEntity = await _manufactureRepository.GetAsync(predicate);
            if (manufactureEntity != null)
            {
                var publisherDto = new ManufactureDto { Id = manufactureEntity.Id, PublisherName = manufactureEntity.PublisherName };
                return publisherDto;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public async Task<IEnumerable<ManufactureDto>> GetAllManufacturiesAsync()
    {
        try
        {
            var manufactureEntities = await _manufactureRepository.GetAllAsync();
            if (manufactureEntities != null)
            {
                var publisherList = new List<ManufactureDto>();
                foreach (var manufactureEntity in manufactureEntities)
                {
                    publisherList.Add(new ManufactureDto
                    {
                        Id = manufactureEntity.Id,
                        PublisherName = manufactureEntity.PublisherName,
                    });
                }
                return publisherList;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public async Task<ManufactureDto> UpdateManufactureAsync(ManufactureDto manufactureDto)
    {
        try
        {
            // hämta först en utgivare om den finns i databasen
            var manufactureEntity = await _manufactureRepository.GetAsync(x => x.Id == manufactureDto.Id);
            if (manufactureEntity != null)
            {
                manufactureEntity.PublisherName = manufactureDto.PublisherName;
                var updatedManufactureEntity = await _manufactureRepository.UpdateOneAsync(manufactureEntity);
                if (updatedManufactureEntity != null)
                {
                    var newManufacture = new ManufactureDto { Id = updatedManufactureEntity.Id, PublisherName = updatedManufactureEntity.PublisherName };
                    return newManufacture;
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public async Task<bool> DeleteManufactureAsync(string publisherName)
    {
        try
        {
            var result = await _manufactureRepository.DeleteAsync(x => x.PublisherName == publisherName);
            return true;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }
}

