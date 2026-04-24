namespace RestfulAPI_01.DTOs
{
    public class TodoReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsDone { get; set; }

    }
}
