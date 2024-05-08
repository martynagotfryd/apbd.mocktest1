namespace apbd.mocktest1.Models.DTOs;

public class AddAnimalWitProcDto
{
    public string Name { get; set; } = String.Empty;
    public string Type { get; set; } = String.Empty;
    public DateTime AdmissionDate { get; set; }
    public int OwnerId { get; set; }
    public List<ProcedureAnimalDto> Procedures { get; set; } = null!;
}

public class ProcedureAnimalDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
}