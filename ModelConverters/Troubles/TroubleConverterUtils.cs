using System;
using Model = Models.Troubles;
using Client = ClientModels.Troubles;

namespace ModelConverters.Troubles
{
    public static class TroubleConverterUtils
    {
        public static Model.TroubleStatus? ConvertStatus(string avgCheck)
        {
            Model.TroubleStatus? modelStatus = null;

            if (Enum.TryParse(avgCheck, true, out Model.TroubleStatus tmpStatus))
            {
                modelStatus = tmpStatus;
            }

            return modelStatus;
        }
    }
}