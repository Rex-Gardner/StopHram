using System;
using System.IO;
using System.Linq;
using Model = Models.Troubles;
using Client = ClientModels.Troubles;

namespace ModelConverters.Troubles
{
    public static class TroubleCreationInfoConverter
    {
        private const int coorDinates = 2;
        
        public static Model.TroubleCreationInfo Convert(Client.TroubleCreationInfo creationInfo)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            var coordinates = creationInfo.Coordinates.ToArray();

            if (coordinates.Length != 2)
            {
                throw new InvalidDataException(nameof(creationInfo.Coordinates));
            }

            var modelCreationInfo = new Model.TroubleCreationInfo(creationInfo.Name, creationInfo.Description,
                coordinates[0], coordinates[1], creationInfo.Address, creationInfo.Tags);

            return modelCreationInfo;
        }
    }
}