using apbd.mocktest1.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace apbd.mocktest1.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private readonly IConfiguration _configuration;
    
    public AnimalRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<bool> DoesAnimalExist(int id)
    {
        var query = "SELECT 1 FROM Animal WHERE ID = @ID";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }

    public async Task<AnimalDto> GetAnimalById(int id)
    {
        var query =
            @"SELECT DISTINCT Animal.ID AS AnimalId, Animal.Name AS AnimalName, Animal.Type AS AnimalType, Animal.AdmissionDate AS AnimalAdmissionDate, " +
            "Owner.ID AS OwnerId, Owner.FirstName AS OwnerFirstName, Owner.LastName AS OwnerLastName, " +
            "p.Name AS ProcedureName, p.Description AS ProcedureDesc, " +
            "Procedure_Animal.Date AS ProcedureDate " +
            "FROM Animal " +
            "JOIN Owner ON Owner.ID = Animal.Owner_ID " +
            "JOIN Procedure_Animal ON Procedure_Animal.Animal_ID = Animal.ID " +
            "JOIN [Procedure] p ON p.ID = Procedure_Animal.Procedure_ID " +
            "WHERE Animal.ID = @ID";
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var reader = await command.ExecuteReaderAsync();

        var animalIdOrdinal = reader.GetOrdinal("AnimalId");
        var animalNameOrdinal = reader.GetOrdinal("AnimalName");
        var animalTypeOrdinal = reader.GetOrdinal("AnimalType");
        var animalAdmissionDateOrdinal = reader.GetOrdinal("AnimalAdmissionDate");
        var ownerIdOrdinal = reader.GetOrdinal("OwnerId");
        var ownerFirstNameOrdinal = reader.GetOrdinal("OwnerFirstName");
        var ownerLastNameOrdinal = reader.GetOrdinal("OwnerLastName");
        var procedureNameOrdinal = reader.GetOrdinal("ProcedureName");
        var procedureDescOrdinal = reader.GetOrdinal("ProcedureDesc");
        var procedureDateOrdinal = reader.GetOrdinal("ProcedureDate");

        AnimalDto animalDto = null;

        while (await reader.ReadAsync())
        {
            if (animalDto is not null)
            {
                animalDto.Procedures.Add(new ProcedureDto()
                {
                    Date = reader.GetDateTime(procedureDateOrdinal),
                    Name = reader.GetString(procedureNameOrdinal),
                    Description = reader.GetString(procedureDescOrdinal)
                });
            }
            else
            {
                animalDto = new AnimalDto()
                {
                    Id = reader.GetInt32(animalIdOrdinal),
                    Name = reader.GetString(animalNameOrdinal),
                    Type = reader.GetString(animalTypeOrdinal),
                    AdmissionDate = reader.GetDateTime(animalAdmissionDateOrdinal),
                    Owner = new OwnerDto()
                    {
                        Id = reader.GetInt32(ownerIdOrdinal),
                        FirstName = reader.GetString(ownerFirstNameOrdinal),
                        LastName = reader.GetString(ownerLastNameOrdinal)
                    },
                    Procedures = new List<ProcedureDto>()
                    {
                        new ProcedureDto()
                        {
                            Name = reader.GetString(procedureNameOrdinal),
                            Description = reader.GetString(procedureDescOrdinal),
                            Date = reader.GetDateTime(procedureDateOrdinal)
                        }
                    }
                };
            }
        }
        if (animalDto is null) throw new Exception();

        return animalDto;
    }
    
    
}