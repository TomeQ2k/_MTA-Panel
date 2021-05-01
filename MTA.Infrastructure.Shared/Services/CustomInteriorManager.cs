using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Services;
using MTA.Core.Application.ServicesUtils;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class CustomInteriorManager : ICustomInteriorManager
    {
        private readonly IXmlReader xmlReader;
        private readonly IFilesManager filesManager;
        private readonly IDatabase database;
        private readonly IHttpContextReader httpContextReader;

        public CustomInteriorManager(IXmlReader xmlReader, IFilesManager filesManager,
            IDatabase database, IHttpContextReader httpContextReader)
        {
            this.xmlReader = xmlReader;
            this.filesManager = filesManager;
            this.database = database;
            this.httpContextReader = httpContextReader;
        }

        public (IEnumerable<GameTempObject>, GameTempInterior) InitGameTempObjectsAndInteriors(Estate estate,
            PremiumFile premiumFile)
        {
            var (gameTempObjectsElements, gameTempInteriorsElements) = (
                xmlReader.GetDescendantNodes(premiumFile.Path, (node) => node.Name.LocalName.Equals("object")),
                xmlReader.GetDescendantNodes(premiumFile.Path, (node) => node.Name.LocalName.Equals("marker")));

            int gameTempObjectsCount = gameTempObjectsElements.Count();

            if (gameTempObjectsCount > Constants.MaximumTempObjectsCount)
            {
                filesManager.DeleteByFullPath(premiumFile.Path);

                throw new ServerException(
                    $"Maximum game temp objects count is: {Constants.MaximumTempObjectsCount}. Your interior file has: {gameTempObjectsCount}");
            }

            return (
                TempObjectsAndInteriorsUtils.ConvertXElementsToTempObjects(gameTempObjectsElements, estate.Id, estate.InteriorId,
                    httpContextReader.CurrentUsername),
                TempObjectsAndInteriorsUtils.ConvertXElementsToTempInterior(gameTempInteriorsElements, estate.Id,
                    estate.InteriorId, httpContextReader.CurrentUserId));
        }

        public async Task ExecuteAddCustomInterior(PremiumFile premiumFile,
            IEnumerable<GameTempObject> gameTempObjects, GameTempInterior gameTempInterior)
        {
            if (!await database.GameTempObjectRepository.InsertRange(gameTempObjects))
            {
                filesManager.DeleteByFullPath(premiumFile.Path);
                throw new DatabaseException();
            }

            if (!await database.GameTempInteriorRepository.Insert(gameTempInterior, false))
            {
                filesManager.DeleteByFullPath(premiumFile.Path);
                throw new DatabaseException();
            }
        }
    }
}