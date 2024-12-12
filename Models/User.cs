namespace AuthService.Models;

public class User
{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FirstLastName { get; set; }
        public string SecondLastName { get; set; }
        public string RUT { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public int CareerId { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }  
}
