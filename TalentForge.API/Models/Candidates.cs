public class Candidate
{
    public int Id {get; set;}
    public string Name{get; set;}
    public string Email{ get; set;}
    public int YearsofExperience{get; set;}
    public string? ResumeUrl{get; set;}
    public string? EmbeddingJson{get; set;}
public Candidate(){  }
    public Candidate(int id, string name, string email, int yearsOfExperience)
    {
        Id = id;
        Name = name;
        Email = email;
        YearsofExperience = yearsOfExperience;
    
}
}