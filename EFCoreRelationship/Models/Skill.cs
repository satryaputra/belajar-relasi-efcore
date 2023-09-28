using System.Text.Json.Serialization;

namespace EFCoreRelationship.Models;

public class Skill
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Damage { get; set; } = 10;
    [JsonIgnore]
    public ICollection<Character> Characters { get; set; }
}