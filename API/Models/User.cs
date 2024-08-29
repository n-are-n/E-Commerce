using System.ComponentModel.DataAnnotations;
namespace API.Models;
public class User
{
    [Key]
    public int Id { get; }
    public required string Name { get; set; }
    public required string Number { get; set; }
    public required string Mail { get; set; }
    public required string Password { get; set; }
    public DateTime CreatedAt { get; }
}