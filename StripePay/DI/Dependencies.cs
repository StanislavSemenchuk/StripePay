using Microsoft.Extensions.DependencyInjection;
using Stripe;
using Stripe.Checkout;

namespace StripePay.DI;

public static class Dependencies
{
    public static void AddStripePay(this IServiceCollection services, StripeClient stripeClient)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        services.AddScoped<SessionService>((serviceProvider) => new SessionService(stripeClient));
        services.AddScoped<WebhookEndpointService>((serviceProvider) => new WebhookEndpointService(stripeClient));
    }
}
