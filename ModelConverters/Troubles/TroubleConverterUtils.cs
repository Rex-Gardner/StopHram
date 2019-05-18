using System;
using System.IO;
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
        
        
    }
}