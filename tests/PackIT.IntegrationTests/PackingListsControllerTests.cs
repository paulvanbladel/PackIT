using PackIT.Application.Commands;
using PackIT.Application.DTO;
using PackIT.Domain.Consts;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace PackIT.IntegrationTests;

public class PackingListsControllerTests
{
    private readonly TestApplication _testApplication;
    public PackingListsControllerTests()
    {
        //await ResetState();

        this._testApplication = new TestApplication();
    }
    [Fact]
    public async void When_Root_Of_Application_Is_Requested_Should_Show_Application_Name()
    {
        //ARRANGE
        var client = _testApplication.CreateDefaultClient();
        //ACT
        var result = await client.GetStringAsync("/");
        //ASSERT
        result.ShouldBe("Packing List Application");
    }
    [Fact]
    public async void When_SearchPackingLists_Is_Requested_List_Of_Packages_Is_Returned()
    {
        //ARRANGE
        var client = _testApplication.CreateDefaultClient();
        //ACT
        var response = await client.GetAsync("api/PackingLists");
        //ASSERT
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async void When_CreatePackingListWithItems_Is_Requested_Returned_Data_Must_Match()
    {
        //ARRANGE
        var client = _testApplication.CreateDefaultClient();
        //ACT
        Guid packingListId = Guid.NewGuid();
        string packingListName = $"test-{packingListId}";
        var postCommand = new CreatePackingListWithItems(packingListId, packingListName, 7, Gender.Female,
            new LocalizationWriteModel("leuven", "belgium"));
        var response = await client.PostAsync("api/PackingLists", postCommand.Serialize());
        //ASSERT
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Created);

        var getResponse = await client.GetAsync($"api/PackingLists/{packingListId.ToString()}");
        var responseData = await getResponse.Content.DeserializeAsync<PackingListDto>();
        responseData.Id.ShouldBe(packingListId, "Id field does not match");
        responseData.Name.ShouldBe(packingListName, "packing list name does not match");
        responseData.Localization.City.ShouldBe("leuven");
        responseData.Localization.Country.ShouldBe("belgium");
    }
    [Fact]
    public async void When_AddPackingItem_Is_Requested_Returned_Data_Must_Match()
    {
        //ARRANGE
        var client = _testApplication.CreateDefaultClient();
        //ACT
        Guid packingListId = Guid.NewGuid();
        string packingListName = $"test-{packingListId}";
        var postCommand = new CreatePackingListWithItems(packingListId, packingListName, 7, Gender.Female,
            new LocalizationWriteModel("leuven", "belgium"));
        var response = await client.PostAsync("api/PackingLists", postCommand.Serialize());

        string packingItemName = $"{Guid.NewGuid().ToString()}-packingitem";
        var postItemCommmand = new AddPackingItem(packingListId, packingItemName, 42);

        response = await client.PutAsync($"api/PackingLists/{packingListId.ToString()}/items", postItemCommmand.Serialize());
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        //ASSERT

        var getResponse = await client.GetAsync($"api/PackingLists/{packingListId.ToString()}");
        var responseData = await getResponse.Content.DeserializeAsync<PackingListDto>();
        responseData.Items.FirstOrDefault(i => i.Name == packingItemName).ShouldNotBeNull("package item not found");
    }
}
