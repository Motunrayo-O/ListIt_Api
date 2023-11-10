using System.ComponentModel.DataAnnotations;

public record HouseDetailDTO(int Id, [property: Required] string? Address, string? Country, [property: Required]int Price, string? Description, string? Photo);