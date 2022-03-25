using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;
using Stripe.Checkout;

namespace StripePay.DI;

public class Dependencies
{
    static IHostBuilder CreateHostBuilder(string[] args)
    {
        DotNetEnv.Env.Load();
        StripeClient stripeClient = new StripeClient(
            apiKey: Environment.GetEnvironmentVariable("STRIPE-APIKEY"),
            clientId: Environment.GetEnvironmentVariable("STRIPE-CLIENTID"),
            httpClient: null,
            apiBase: Environment.GetEnvironmentVariable("STRIPE-APIBASE"),
            connectBase: Environment.GetEnvironmentVariable("STRIPE-CONNECTIONBASE"),
            filesBase: null
            );
        return Host.CreateDefaultBuilder(args)
                   .ConfigureServices((_, services) =>
                   {
                       services.AddScoped<SessionService>((serviceProvider) => new SessionService(stripeClient));
                       services.AddScoped<WebhookEndpointService>((serviceProvider) => new WebhookEndpointService(stripeClient));
                   });
    }
}
