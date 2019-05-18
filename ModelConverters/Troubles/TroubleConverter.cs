using System;
using Model = Models.Troubles;
using Client = ClientModels.Troubles;

namespace ModelConverters.Troubles
{
    public static class TroubleConverter
    {
        public static Client.Trouble Convert(Model.Trouble modelTrouble)
        {
            if (modelTrouble == null)
            {
                throw new ArgumentNullException(nameof(modelTrouble));
            }

            var coordinates = new [] {modelTrouble.Coordinates.Latitude, modelTrouble.Coordinates.Longitude};

            var clientTrouble = new Client.Trouble
            {
                Id = modelTrouble.Id.ToString(), 
                Name = modelTrouble.Name,
                Description = modelTrouble.Description,
                Images = modelTrouble.Images,
                Coordinates = coordinates,
                Address = modelTrouble.Address,
                Tags = modelTrouble.Tags,
                Status = modelTrouble.Status.ToString(),
                CreatedAt = modelTrouble.CreatedAt,
                LastUpdateAt = modelTrouble.LastUpdateAt
            };
            
            return clientTrouble;
        }
    }
}