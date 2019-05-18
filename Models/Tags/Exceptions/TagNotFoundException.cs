using System;

namespace Models.Tags.Exceptions
{
    public class TagNotFoundException : Exception
    {
        public TagNotFoundException(string tagId)
            : base($"Tag \"{tagId}\" not found.")
        {
        
        }
    }
}