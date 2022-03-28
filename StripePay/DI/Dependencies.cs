using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using Stripe.Checkout;
using StripePay.Data;

namespace StripePay.DI;

public static class Dependencies
{
    public static void AddStripePay(this IServiceCollection services, StripeClient stripeClient)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        services.AddScoped((serviceProvider) => new SessionService(stripeClient));
        services.AddScoped((serviceProvider) => new WebhookEndpointService(stripeClient));
        services.AddScoped((customerService) => new CustomerService(stripeClient));
        services.AddScoped((setupIntentService) => new SetupIntentService(stripeClient));
        services.AddScoped((paymentIntentService) => new PaymentIntentService(stripeClient));
        services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("test"));
    }
}