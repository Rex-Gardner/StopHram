using System;
using Model = Models.Tags;
using Client = ClientModels.Tags;

namespace ModelConverters.Tags
{
    public static class TagConverter
    {
        public static Client.Tag Convert(Model.Tag modelTag)
        {
            if (modelTag == null)
            {
                throw new ArgumentNullException(nameof(modelTag));
            }

            var clientTag = new Client.Tag
            {
                Id = modelTag.Id, 
                Name = modelTag.Name, 
            };
            
            return clientTag;
        }
    }
}