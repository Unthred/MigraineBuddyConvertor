using System.ComponentModel.DataAnnotations;

namespace MigraineBuddyConvertor.Data;

public class PainUser
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}
