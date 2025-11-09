namespace CampusNet.Model
{
    public class Vertex
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        public Vertex(string id, string name, string role)
        {
            Id = id;
            Name = name;
            Role = role;
        }

        public override string ToString() => $"{Name} ({Role})";
    }
}