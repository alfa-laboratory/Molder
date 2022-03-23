namespace Molder.Zephyr.Models.Dto
{
    public record TestStep
    {
        public int Id { get; set; }
        public string Step { get; set; }
        public string Data { get; set; }
        public int OrderId { get; set; }
    }
}