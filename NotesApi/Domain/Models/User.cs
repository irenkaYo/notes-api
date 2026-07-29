namespace Domain.Models;

public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public string PasswordHashed { get; private set; }
    public List<Note> Notes { get; private set; }

    public User(string userName, string passwordHashed)
    {
        Id = Guid.NewGuid();
        Username = userName;
        PasswordHashed = passwordHashed;
        Notes = new List<Note>();
    }
    
    private User()
    {
        
    }
}