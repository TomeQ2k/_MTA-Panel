using System.Threading.Tasks;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;

namespace MTA.Infrastructure.Shared.Services
{
    public class CharacterManager : ICharacterManager
    {
        private readonly IDatabase database;
        private readonly IRolesService rolesService;
        private readonly IHttpContextReader httpContextReader;

        public CharacterManager(IDatabase database, IRolesService rolesService,
            IHttpContextReader httpContextReader)
        {
            this.database = database;
            this.rolesService = rolesService;
            this.httpContextReader = httpContextReader;
        }

        public async Task<BlockCharacterResult> ToggleBlockCharacter(int characterId)
        {
            if (!await rolesService.IsPermitted(httpContextReader.CurrentUserId, Constants.AdminRoles))
                throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            var character = await database.CharacterRepository.FindById(characterId) ??
                            throw new EntityNotFoundException("Character not found");

            character.ToggleActive();

            return await database.CharacterRepository.Update(character)
                ? new BlockCharacterResult(true, character.Active == 0, character.Name, character.AccountId)
                : throw new DatabaseException();
        }
    }
}