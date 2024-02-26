namespace Domain.Enumerations
{
    public class UserRoleType : Enumeration
    {
        public static UserRoleType Admin = new(1, "admin");
        public static UserRoleType Doctor = new(2, "doctor");
        public static UserRoleType Patient = new(3, "patient");
        public UserRoleType(int id, string name) : base(id, name) { }
    }
}
