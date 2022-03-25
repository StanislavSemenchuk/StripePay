using Stripe;
using Stripe.Checkout;

namespace StripePay.Services.Billing;

public interface IBillingService
{
    public Task<string> CheckoutAsync(SessionCreateOptions options);
}

public class BillingService : IBillingService
{
    private readonly SessionService _sessionService;
    public BillingService(SessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public async Task<string> CheckoutAsync(SessionCreateOptions options)
    {
        var session = await _sessionService.CreateAsync(options);
        return session.Id;
    }
}