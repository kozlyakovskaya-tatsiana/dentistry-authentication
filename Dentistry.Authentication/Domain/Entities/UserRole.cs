namespace Domain.Entities
{
    public class UserRole : BaseEntity
    {
        public string Name { get; set; }
        public List<User> Users { get; set; }
    }
}
