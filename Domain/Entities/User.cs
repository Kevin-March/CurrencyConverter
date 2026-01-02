
namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public string Password { get; set; } = null!;

        public List<Address> Addresses { get; set; } = new();
    }
}