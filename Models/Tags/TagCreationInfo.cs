using System;

namespace Models.Tags
{
    public class TagCreationInfo
    {
        public string Id { get; }
        public string Name { get; }

        public TagCreationInfo(string id, string name)
        {
            Id = id?.ToLower() ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}