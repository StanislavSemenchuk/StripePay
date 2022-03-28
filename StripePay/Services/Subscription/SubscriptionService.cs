using Stripe;
using Stripe.Checkout;

namespace StripePay.Services.Billing;

public interface ISubscriptionService
{
    public Task<Subscription> CreateSubscriptionAsync(string customerId, string priceId);
    public Task<SetupIntent> PrepareForFuturePaymentAsync(string customerId);
}

public class SubscriptionService : ISubscriptionService
{
    private readonly CustomerService _customerService;
    private readonly SetupIntentService _setupIntentService;
    public SubscriptionService(CustomerService customerService, SetupIntentService setupIntentService)
    {
        _customerService = customerService;
        _setupIntentService = setupIntentService;
    }
    public async Task<Subscription> CreateSubscriptionAsync(string customerId, string priceId)
    {
        Subscription subscription = new Subscription();
        if (!await TryCreateSubscriptionAsync(customerId, priceId, subscription))
        {
            throw new InvalidOperationException(subscription.StripeResponse.Content);
        }
        return subscription;
    }
    public async Task<SetupIntent> PrepareForFuturePaymentAsync(string customerId)
    {
        var options = new SetupIntentCreateOptions
        {
            Customer = customerId,
            Expand = new List<string>()
            {
               "customer"
            }
        };

        var intent = await _setupIntentService.CreateAsync(options);

        return intent;
    }
    private async Task<bool> TryCreateSubscriptionAsync(string customerId, string priceId, Subscription subscription)
    {
        var stripeCustomer = await _customerService.GetAsync(customerId);
        if (stripeCustomer == null)
            return false;
        var subscriptionOptions = new SubscriptionCreateOptions
        {
            Customer = stripeCustomer.Id,
            Items = new List<SubscriptionItemOptions>
            {
               new SubscriptionItemOptions
               {
                  Price = priceId,
               },
            },
        };
        subscriptionOptions.AddExpand("latest_invoice.payment_intent");
        var subscriptionService = new Stripe.SubscriptionService();
        try
        {
            subscription = await subscriptionService.CreateAsync(subscriptionOptions);
            return true;
        }
        catch (StripeException ex)
        {
            subscription = new Subscription()
            {
                StripeResponse = ex.StripeResponse
            };
            return false;
        }
    }
}