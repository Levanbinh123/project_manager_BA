public class InviteRequest
{
    public long ProjectId{get;set;}
    public string Email{get;set;}
    InviteRequest(long  projectId, string email)
    {
        ProjectId=projectId;
        Email=email;
    }
    InviteRequest(){}
}