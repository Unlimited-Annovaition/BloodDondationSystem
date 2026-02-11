namespace BloodDonation.Core.Entities;

public class FeedBack
{
    public int Id { get; set; }
    public string Message { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}