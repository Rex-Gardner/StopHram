using System;
using Model = Models.Tags;
using Client = ClientModels.Tags;

namespace ModelConverters.Tags
{
    public static class TagPatchInfoConveter
    {
        public static Model.TagPatchInfo Convert(string id, Client.TagPatchInfo clientPatchInfo)
        {
            if (clientPatchInfo == null)
            {
                throw new ArgumentNullException(nameof(clientPatchInfo));
            }

            var modelPatchInfo = new Model.TagPatchInfo(id, clientPatchInfo.Name);
            return modelPatchInfo;
        }
    }
}