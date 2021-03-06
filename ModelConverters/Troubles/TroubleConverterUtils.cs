using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model = Models.Troubles;
using Client = ClientModels.Troubles;

namespace ModelConverters.Troubles
{
    public static class TroubleConverterUtils
    {
        public static Model.TroubleStatus? ConvertStatus(string status)
        {
            Model.TroubleStatus? modelStatus = null;

            if (Enum.TryParse(status, true, out Model.TroubleStatus tmpStatus))
            {
                modelStatus = tmpStatus;
            }

            return modelStatus;
        }

        public static Guid ConvertId(string id)
        {
            if (Guid.TryParse(id, out var guid))
            {
                return guid;
            }

            throw new InvalidDataException($"{id} is invalid Guid.");
        }
        
        public static IEnumerable<string> FilterWrongTagIds(IReadOnlyList<string> clientTags, 
            IReadOnlyList<Models.Tags.Tag> modelTags)
        {
            if (clientTags == null)
            {
                throw new ArgumentNullException(nameof(clientTags));
            }

            if (modelTags == null)
            {
                throw new ArgumentNullException(nameof(modelTags));
            }

            var modelTypeIds = clientTags.Where(item => modelTags.Any(type => type.Id == item));
            return modelTypeIds;
        }
    }
}