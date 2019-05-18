using System;
namespace ClientModels
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
