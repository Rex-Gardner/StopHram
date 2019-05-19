using System;
namespace ClientModels.Pictures
{
    public class Picture
    {
        public string Url { get; set; }

        public Picture(string url)
        {
            Url = url;
        }
    }
}
