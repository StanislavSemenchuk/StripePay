using Stripe;
using Stripe.Checkout;

namespace StripePay.Services.Checkout;

public interface ICheckoutService
{
    Task<string> CheckoutAsync(SessionCreateOptions options);
}

public class CheckoutService : ICheckoutService
{
    private readonly SessionService _sessionService;
    public CheckoutService(SessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public async Task<string> CheckoutAsync(SessionCreateOptions options)
    {
        var session = new Session();
        if (!await TryCheckoutAsync(options, session))
        {
            throw new Exception(session.StripeResponse.Content);
        }
        return session.Id;
    }
    private async Task<bool> TryCheckoutAsync(SessionCreateOptions options, Session session)
    {
        try
        {
            session = await _sessionService.CreateAsync(options);
            return true;
        }
        catch (StripeException ex)
        {
            session = new Session()
            {
                StripeResponse = ex.StripeResponse
            };
            return false;
        }
    }
}
