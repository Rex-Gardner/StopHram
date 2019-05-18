using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model = Models.Troubles;
using Client = ClientModels.Troubles;

namespace ModelConverters.Troubles
{
    public static class TroublePatchInfoConverter
    {
        private const int CoordinatesLength = 2;
        
        public static Model.TroublePatchInfo Convert(string id, Client.TroublePatchInfo clientPatchInfo,
            IReadOnlyList<Models.Tags.Tag> modelTags)
        {
            if (clientPatchInfo == null)
            {
                throw new ArgumentNullException(nameof(clientPatchInfo));
            }
            
            if (modelTags == null)
            {
                throw new ArgumentNullException(nameof(modelTags));
            }

            string[] filteredTagIds = null;
            
            if (clientPatchInfo.Tags != null)
            {
                filteredTagIds = TroubleConverterUtils.FilterWrongTagIds(clientPatchInfo.Tags, modelTags).ToArray();

                if (!filteredTagIds.Any())
                {
                    throw new InvalidDataException($"{nameof(filteredTagIds)} can't be empty.");
                }
            }

            double? latitude = null, longitude = null;
            
            if (clientPatchInfo.Coordinates != null)
            {
                if (clientPatchInfo.Coordinates.Count != CoordinatesLength)
                {
                    throw new InvalidDataException(
                        $"{nameof(clientPatchInfo.Coordinates)} must contain 2 values (lat and long).");
                }

                latitude = clientPatchInfo.Coordinates[0];
                longitude = clientPatchInfo.Coordinates[1];
            }

            var guid = TroubleConverterUtils.ConvertId(id);
            var status = TroubleConverterUtils.ConvertStatus(clientPatchInfo.Status);

            var modelPathInfo = new Models.Troubles.TroublePatchInfo(guid)
            {
                Name = clientPatchInfo.Name,
                Description = clientPatchInfo.Description,
                Images = clientPatchInfo.Images,
                Latitude = latitude,
                Longitude = longitude,
                Address = clientPatchInfo.Address,
                Tags = filteredTagIds,
                Status = status
            };

            return modelPathInfo;
        }
    }
}