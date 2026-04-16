namespace Petstore_Bdd_Tests_Csharp.Models
{
    public class PetModel
    {
        public long? Id { get; set; }

        public Category? Category { get; set; }

        public string? Name { get; set; }

        public List<string>? PhotoUrls { get; set; }

        public List<Tag>? Tags { get; set; }

        public string? Status { get; set; }
    }
}
