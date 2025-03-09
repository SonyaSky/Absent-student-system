using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public class Department
{
    public Guid Id { get; set; }
    public Faculty Faculty { get; set; }
    public string UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
}