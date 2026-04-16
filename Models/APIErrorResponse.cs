namespace Petstore_Bdd_Tests_Csharp.Models
{
    public class ApiErrorResponse
    {
        public int Code { get; set; }

        public required string Type { get; set; }

        public required string Message { get; set; }
    }
}
