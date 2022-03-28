using Stripe;

namespace StripePay.Data.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string CustomerId { get; set; }
    public ICollection<Operation> Operations { get; set; }
}
