namespace CampusNet.Models
{
    public enum Role
    {
        Estudiante,
        Profesor,
        Egresado
    }

    public class Vertex
    {
        public string Id { get; }
        public string Name { get; set; }
        public Role Role { get; set; }

        public Vertex(string id, string name, Role role)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name;
            Role = role;
        }

        public override string ToString() => $"{Id} ({Name}, {Role})";
    }
}
