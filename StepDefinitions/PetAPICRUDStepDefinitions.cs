using Petstore_Bdd_Tests_Csharp.EndPoints.Pet;
using Petstore_Bdd_Tests_Csharp.Models;
using Shouldly;

namespace Petstore_Bdd_Tests_Csharp.StepDefinitions
{
    [Binding]
    public class PetAPICRUDStepDefinitions(IRequests requests, ScenarioContext sc)
    {
        private PetModel? _petModel;

        [When("I create a new pet with the following details:")]
        public async Task WhenICreateANewPetWithTheFollowingDetails(DataTable dataTable)
        {
            var item = dataTable.Rows[0];
            var photoUrls = item["Photourls"]?.Split(',').Select(url => url.Trim()).ToList() ?? new List<string>();

            _petModel = new PetModel
            {
                Id = long.Parse(item["Id"]),
                Name = item["Name"],
                Status = item["Status"],
                PhotoUrls = photoUrls
            };

            await requests.CreatePetAsync(_petModel);
            Thread.Sleep(5000);
        }

        [Then("the pet should be created successfully")]
        public async Task ThenThePetShouldBeCreatedSuccessfully()
        {
            if (_petModel?.Id == null)
            {
                throw new InvalidOperationException("PetModel or PetModel.Id is null.");
            }

            long id = (long)_petModel.Id;

            _petModel = await requests.GetPetAsync(id);
            Thread.Sleep(5000);
        }

        [When("I retrieve the pet by its ID {int}")]
        public async Task WhenIRetrieveThePetByItsID(int petId)
        {
            _petModel = await requests.GetPetAsync(petId);
            Thread.Sleep(5000);
        }

        [Then("the pet should exist")]
        public async Task ThenThePetShouldExist()
        {
            _petModel!.Id.ShouldNotBeNull();
        }

        [When("I update the below pet with the following details:")]
        public async Task WhenIUpdateTheBelowPetWithTheFollowingDetails(DataTable dataTable)
        {
            var item = dataTable.Rows[0];

            _petModel = new PetModel
            {
                Id = long.Parse(item["Id"]),
                Name = item["Name"],
                Status = item["Status"]
            };

            sc.Set(item["Name"], "UpdatedPetName");
            await requests.UpdatePetAsync(_petModel);
            Thread.Sleep(5000);
        }

        [Then("the pet should be updated")]
        public async Task ThenThePetShouldBeUpdated()
        {
            long id = (long)_petModel!.Id!;

            var expectedPetName = sc.Get<string>("UpdatedPetName");
            _petModel = await requests.GetPetAsync(id);
            _petModel.Name.ShouldBe(expectedPetName);

            Thread.Sleep(5000);
        }

        [When("I delete the pet with ID {int}")]
        public async Task WhenIDeleteThePetWithID(int petId)
        {
            var response = await requests.DeletePetAsync(petId);
            sc.Set(response, "DeleteResponse");
            sc.Set(petId, "DeletedPetId");
            Thread.Sleep(5000);
        }

        [Then("the pet should not exist")]
        public void ThenThePetShouldNotExist()
        {
            var deletedPetId = sc.Get<int>("DeletedPetId");
            var deleteResponse = sc.Get<ApiErrorResponse>("DeleteResponse");

            deleteResponse.Code.ShouldBe(200);
            deleteResponse.Type.ShouldBe("unknown");
            deleteResponse.Message.ShouldNotBeNull(deletedPetId.ToString());
        }
    }
}
