using System.Threading.Tasks;
using PackIT.Domain.Repositories;
using PackIT.Shared.Abstractions.Commands;

namespace PackIT.Application.Commands.Handlers
{
    internal sealed class RemoveAllPackingListsHandler : ICommandHandler<RemoveAllPackingLists>
    {
        private readonly IPackingListRepository _repository;

        public RemoveAllPackingListsHandler(IPackingListRepository repository)
        {
            _repository = repository;
        }
        public async Task HandleAsync(RemoveAllPackingLists command)
        {
            var allLists = await _repository.GetAll();
            foreach (var item in allLists)
            {
                await _repository.DeleteAsync(item);
            }
        }
    }
}