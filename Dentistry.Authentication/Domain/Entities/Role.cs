namespace Domain.Entities
{
    public class Role : BaseEntity
    {
        private Role() { }
        public string Name { get; private init; }
        public IEnumerable<User>? Users { get; private init; }
        public static Role Create(string name, IEnumerable<User>? users = null)  
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            return new Role() { Name = name, Users = users};
        }
    }
}
