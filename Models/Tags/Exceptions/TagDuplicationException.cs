using System;

namespace Models.Tags.Exceptions
{
    public class TagDuplicationException : Exception
    {
        public TagDuplicationException(string tagId)
            : base($"Tag \"{tagId}\" already exists.")
        {
        
        }
    }
}