namespace apbd.mocktest1.Models.DTOs;

public class AddAnimalDto
{
    public string Name { get; set; } = String.Empty;
    public string Type { get; set; } = String.Empty;
    public DateTime AdmissionDate { get; set; }
    public int OwnerId { get; set; }
}