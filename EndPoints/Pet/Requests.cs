using Petstore_Bdd_Tests_Csharp.Models;
using Petstore_Bdd_Tests_Csharp.Utilities;

namespace Petstore_Bdd_Tests_Csharp.EndPoints.Pet
{
    public interface IRequests
    {
        Task<PetModel> CreatePetAsync(PetModel pet);
        Task<PetModel> GetPetAsync(long id);
        Task<PetModel> UpdatePetAsync(PetModel pet);
        Task<ApiErrorResponse> DeletePetAsync(long id);
    }

    public class Requests(IPostEndPoint postEndPoint, IGetEndPoint getEndPoint,
        IUpdateEndPoint updateEndPoint, IDeleteEndPoint deleteEndPoint) : IRequests
    {
        public async Task<PetModel> CreatePetAsync(PetModel pet)
        {
            return await postEndPoint.CreateAsync("/pet", pet);
        }

        public async Task<PetModel> GetPetAsync(long id)
        {
            return await getEndPoint.GetAsync<PetModel>("/pet", id);
        }

        public async Task<PetModel> UpdatePetAsync(PetModel pet)
        {
            return await updateEndPoint.PutAsync<PetModel>("/pet", pet);
        }

        public async Task<ApiErrorResponse> DeletePetAsync(long id)
        {
            return await deleteEndPoint.DeleteAsync<ApiErrorResponse>("/pet", id);
        }
    }
}
