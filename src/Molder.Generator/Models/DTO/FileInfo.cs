namespace Molder.Generator.Models.DTO
{
    public record FileInfo
    {
        public string Name { get; init; }

        private string _path;
        public string Path
        {
            get => _path;
            init => _path = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private string _content;
        public string Content
        {
            get => _content;
            init => _content = string.IsNullOrWhiteSpace(value) ? null : value;
        }
    }
}