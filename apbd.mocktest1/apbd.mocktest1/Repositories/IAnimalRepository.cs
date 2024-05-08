using apbd.mocktest1.Models.DTOs;

namespace apbd.mocktest1.Repositories;

public interface IAnimalRepository
{
    Task<bool> DoesAnimalExist(int id);
    Task<AnimalDto> GetAnimalById(int id);
    Task<bool> DoesOwnerExist(int id);
    Task<bool> DoesProcedureExist(int id);
    Task AddAnimal(AddAnimalDto addAnimalDto);
    // Task AddAnimalWithProc(AddAnimalWitProcDto addAnimalWitProcDto);
}