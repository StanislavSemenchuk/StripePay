using Stripe;
using Stripe.Checkout;

namespace StripePay.Services.Billing;

public interface IBillingService
{
}

public class BillingService : IBillingService
{
    private readonly PayoutService _payoutService;
    private readonly SessionService _sessionService;
    public BillingService(PayoutService payoutService, SessionService sessionService)
    {
        _payoutService = payoutService;
        _sessionService = sessionService;
    }

    public async Task CheckoutAsync()
    {

    }
}