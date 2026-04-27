namespace RestfulAPI_01
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string? Title { get; set; } 
        public bool IsDone { get; set; }

        //Idegen kulcs
        public int CategoryId { get; set; }

        //Nav property
        public Category? Category { get; set; }

    }
}
