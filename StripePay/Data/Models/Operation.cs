namespace StripePay.Data.Models;

public class Operation
{
    public int Id { get; set; }
    public int OperationStatus { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}
