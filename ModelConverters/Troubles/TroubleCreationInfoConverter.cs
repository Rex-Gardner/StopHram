using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model = Models.Troubles;
using Client = ClientModels.Troubles;

namespace ModelConverters.Troubles
{
    public static class TroubleCreationInfoConverter
    {
        private const int CoordinatesLength = 2;
        
        public static Model.TroubleCreationInfo Convert(Client.TroubleCreationInfo creationInfo,
            IReadOnlyList<Models.Tags.Tag> modelTags)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            if (modelTags == null)
            {
                throw new ArgumentNullException(nameof(modelTags));
            }

            var filteredTagIds = TroubleConverterUtils.FilterWrongTagIds(creationInfo.Tags, modelTags).ToArray();

            if (filteredTagIds == null || !filteredTagIds.Any())
            {
                throw new InvalidDataException($"{nameof(filteredTagIds)} can't be empty.");
            }
            
            var coordinates = creationInfo.Coordinates.ToArray();

            if (coordinates.Length != CoordinatesLength)
            {
                throw new InvalidDataException(nameof(creationInfo.Coordinates));
            }

            var modelCreationInfo = new Model.TroubleCreationInfo(creationInfo.Name, creationInfo.Description,
                coordinates[0], coordinates[1], creationInfo.Address, filteredTagIds);

            return modelCreationInfo;
        }
    }
}