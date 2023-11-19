using System.ComponentModel.DataAnnotations;

namespace API;

public class AppUser
{
    //[Key] // You need to use this if you give the name other then Id
    public int Id { get; set; }
    //[Required]
    // Or <Nullable>disable</Nullable> set this in csproj to true
    // Or add validation at dto level
    public string UserName { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}
