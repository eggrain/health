using System.ComponentModel.DataAnnotations.Schema;

namespace health.Models;

public abstract class Entity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = null!;

    public User User { get; set; } = null!;
}
