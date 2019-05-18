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
        public static Model.TroublePatchInfo Convert(string id, Client.TroublePatchInfo clientPatchInfo)
        {
            if (clientPatchInfo == null)
            {
                throw new ArgumentNullException(nameof(clientPatchInfo));
            }

            double? latitude = null, longitude = null;
            
            if (clientPatchInfo.Coordinates != null)
            {
                if (clientPatchInfo.Coordinates.Count != 2)
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
                Tags = clientPatchInfo.Tags,
                Status = status
            };

            return modelPathInfo;
        }
    }
}