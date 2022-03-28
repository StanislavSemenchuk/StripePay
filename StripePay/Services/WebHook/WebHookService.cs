using Stripe;

namespace StripePay.Services.WebHook;

public interface IWebHookService
{
    public Task<WebhookEndpoint> CreateWebHookAsync(WebhookEndpointCreateOptions options);
}

public class WebHookService : IWebHookService
{
    private readonly WebhookEndpointService _endpointService;
    public WebHookService(WebhookEndpointService endpointService)
    {
        _endpointService = endpointService;
    }
    public async Task<WebhookEndpoint> CreateWebHookAsync(WebhookEndpointCreateOptions options)
    {
        var webhookEndpoint = new WebhookEndpoint();
        if (await TryCreateWebHookAsync(options, webhookEndpoint))
        {
            return webhookEndpoint;
        }
        else 
        {
            throw new InvalidOperationException(webhookEndpoint.StripeResponse.Content);
        }
    }
    private async Task<bool> TryCreateWebHookAsync(WebhookEndpointCreateOptions options, WebhookEndpoint webhookEndpoint)
    {
        try
        {
            webhookEndpoint = await _endpointService.CreateAsync(options);
            return true;
        }
        catch (StripeException ex)
        {
            webhookEndpoint = new WebhookEndpoint()
            {
                StripeResponse = ex.StripeResponse
            };
            return false;
        }

    }
}