namespace Domain.Entities
{
    public class Role : BaseEntity
    {
        private Role() { }
        public string Name { get; private set; }
        public IEnumerable<User>? Users { get; private set; }
        public static Role Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            return new Role() { Name = name };
        }
    }
}
