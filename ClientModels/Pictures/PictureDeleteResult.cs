using System;
namespace ClientModels.Pictures
{
    public enum DeleteCode { Success, Error };

    public class PictureDeleteResult
    {
        public DeleteCode Code { get; set; }
        public string Message { get; set; }
        public Picture Picture { get; set; }
    }
}
