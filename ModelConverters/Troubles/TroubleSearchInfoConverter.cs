using System;
using System.Linq;
using Model = Models.Troubles;
using Client = ClientModels.Troubles;

namespace ModelConverters.Troubles
{
    public static class TroubleSearchInfoConverter
    {
        public static Model.TroubleSearchInfo Convert(Client.TroubleSearchInfo clientSearchInfo)
        {
            if (clientSearchInfo == null)
            {
                throw new ArgumentNullException(nameof(clientSearchInfo));
            }

            Model.TroubleStatus?[] statuses = null;
            
            if (clientSearchInfo.Status != null)
            {
                statuses = clientSearchInfo.Status.Select(TroubleConverterUtils.ConvertStatus).ToArray();   
            }
            
            var modelSearchInfo = new Model.TroubleSearchInfo
            {
                Offset = clientSearchInfo.Offset,
                Limit = clientSearchInfo.Limit,
                Tags = clientSearchInfo.Tag,
                Statuses = statuses
            };

            return modelSearchInfo;
        }
    }
}