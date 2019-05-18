namespace Models.Tags
{
    public class TagPatchInfo
    {
        public string Id { get; }
        public string Name { get; set; }

        public TagPatchInfo(string id, string name = null)
        {
            Id = id;
            Name = name;
        }
    }
}