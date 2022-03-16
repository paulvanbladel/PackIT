
using PackIT.Application.Commands;
using PackIT.Application.DTO;
using PackIT.Domain.Consts;
using ScenarioTests;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace PackIT.ScenarioTests
{
    public partial class PackingListControllerScenarioTests
    {
        private readonly TestApplication _testApplication;
        public PackingListControllerScenarioTests()
        {
            //await ResetState();

            this._testApplication = new TestApplication();
        }

        [Scenario(NamingPolicy = ScenarioTestMethodNamingPolicy.Test)]
        public void should_present_basic_scenario(ScenarioContext scenario)
        {
            //ARRANGE
            var client = _testApplication.CreateDefaultClient();
            Guid packingListId = Guid.NewGuid();
            string packingListName = $"test-{packingListId}";
            scenario.Fact("When A new packing list is created status created is returned", async () =>
            {
                var postCommand = new CreatePackingListWithItems(packingListId, packingListName, 7, Gender.Female,
                new LocalizationWriteModel("leuven", "belgium"));
                var response = await client.PostAsync("api/PackingLists", postCommand.Serialize());
                response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Created);
            });

            scenario.Fact("When the packing list is retrieved, the data match with the initial data", async () =>
           {
               var getResponse = await client.GetAsync($"api/PackingLists/{packingListId.ToString()}");
               var responseData = await getResponse.Content.DeserializeAsync<PackingListDto>();
               responseData.Id.ShouldBe(packingListId, "Id field does not match");
               responseData.Name.ShouldBe(packingListName, "packing list name does not match");
               responseData.Localization.City.ShouldBe("leuven");
               responseData.Localization.Country.ShouldBe("belgium");
           });







        }
    }
}