namespace Petstore_Bdd_Tests_Csharp.Configurations
{
    public class TestSettings
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public required string BaseUri { get; set; }
    }
}
