using System;
using Model = Models.Tags;
using Client = ClientModels.Tags;

namespace ModelConverters.Tags
{
    public static class TagCreationInfoConverter
    {
        public static Model.TagCreationInfo Convert(Client.TagCreationInfo clientCreationInfo)
        {
            if (clientCreationInfo == null)
            {
                throw new ArgumentNullException(nameof(clientCreationInfo));
            }

            var modelCreationInfo = new Model.TagCreationInfo(clientCreationInfo.Id, clientCreationInfo.Name);
            return modelCreationInfo;
        }
    }
}