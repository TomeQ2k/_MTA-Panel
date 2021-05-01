using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.ServicesUtils
{
    public static class TempObjectsAndInteriorsUtils
    {
        public static IEnumerable<GameTempObject> ConvertXElementsToTempObjects(IEnumerable<XElement> tempObjects,
            int interiorId, int estateInterior, string username)
        {
            var gameTempObjects = new List<GameTempObject>();

            foreach (var tempObj in tempObjects)
            {
                var gameTempObject = new GameTempObjectBuilder()
                    .SetModel(tempObj.Attribute("model").Value)
                    .SetPosition(tempObj.Attribute("posX").Value, tempObj.Attribute("posY").Value,
                        tempObj.Attribute("posZ").Value)
                    .SetRotation(tempObj.Attribute("rotX").Value, tempObj.Attribute("rotY").Value,
                        tempObj.Attribute("rotZ").Value)
                    .SetComment($"{username} | {DateTime.Now}")
                    .SetDimension(interiorId)
                    .SetInterior(estateInterior)
                    .Build();

                if (tempObj.Attribute("collisions") != null && tempObj.Attribute("collisions").Value == "false")
                    gameTempObject.SetSolid(0);
                else
                    gameTempObject.SetSolid(1);

                if (tempObj.Attribute("doublesided") != null && tempObj.Attribute("doublesided").Value == "true")
                    gameTempObject.SetDoublesided(1);
                else
                    gameTempObject.SetDoublesided(0);

                if (tempObj.Attribute("scale") != null)
                    gameTempObject.SetScale(tempObj.Attribute("scale").Value);
                else
                    gameTempObject.SetScale("1");

                if (tempObj.Attribute("breakable") != null && tempObj.Attribute("breakable").Value == "false")
                    gameTempObject.SetBreakable(0);
                else
                    gameTempObject.SetBreakable(1);

                if (tempObj.Attribute("alpha") != null)
                    gameTempObject.SetAlpha(tempObj.Attribute("alpha").Value);
                else
                    gameTempObject.SetAlpha("255");

                if (gameTempObject.PosX > 3000 || gameTempObject.PosX < -3000)
                    throw new ServerException(
                        $"Obiekt z ID: {interiorId} jest poza obszarem wyznaczonym do budowy wnętrza w pozycji X");

                if (gameTempObject.PosY > 3000 || gameTempObject.PosY < -3000)
                    throw new ServerException(
                        $"Obiekt z ID: {interiorId} jest poza obszarem wyznaczonym do budowy wnętrza w pozycji Y");

                if (gameTempObject.PosZ > 3000 || gameTempObject.PosZ < -3000)
                    throw new ServerException(
                        $"Obiekt z ID: {interiorId} jest poza obszarem wyznaczonym do budowy wnętrza w pozycji Z");

                gameTempObjects.Add(gameTempObject);
            }

            return gameTempObjects;
        }

        public static GameTempInterior ConvertXElementsToTempInterior(IEnumerable<XElement> tempInteriors,
            int interiorId, int estateInterior, int currentUserId)
        {
            XElement tempInteriorElement = tempInteriors.FirstOrDefault() ?? throw new ServerException(
                "Mapa nie zawiera żadnych markerów ani 'cylindrów' oznaczających wejście lub wyjście");

            return new GameTempInteriorBuilder()
                .SetInteriorId(interiorId)
                .SetPosition(tempInteriorElement.Attribute("posX").Value, tempInteriorElement.Attribute("posY").Value,
                    tempInteriorElement.Attribute("posZ").Value)
                .SetEstateInterior(estateInterior)
                .UploadedBy(currentUserId)
                .Build();
        }
    }
}