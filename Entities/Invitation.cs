public class Invitation
{
    public long Id { get; set; }

    public string Token { get; set; }

    public string Email { get; set; }

    public long ProjectId { get; set; }
     public Project Project { get; set; } 

    public DateTime ExpiredAt { get; set; }   // ✅ thêm
    public bool IsUsed { get; set; }    
}