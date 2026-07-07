public class Job
    {
        public int Id {get; set;}
        public string Title{get; set;}
        public int RequiredYears{get; set;}
         public string? EmbeddingJson{get; set;}
        public Job(){ }
        public Job(int id, string title, int requiredyears)
        {
            Id = id;
            Title = title;
            RequiredYears = requiredyears;
        }
    }
    