using Stripe;
using StripePay.Data;
using StripePay.Data.Consts;
using StripePay.Data.Models;

namespace StripePay.Services.Charge;

public interface IChargeService
{
    public Task ChargeAsync(string customerId, string paymentMethodId,
                       string currency, long unitAmount, string customerEmail,
                       bool sendEmailAfterSuccess = false, string emailDescription = "");
}
public class ChargeService : IChargeService
{
    private readonly PaymentIntentService _paymentIntentService;
    private readonly AppDbContext _appDbContext;
    public ChargeService(PaymentIntentService paymentIntentService)
    {
        _paymentIntentService = paymentIntentService;
    }
    public async Task ChargeAsync(string customerId, string paymentMethodId,
                             string currency, long unitAmount, string customerEmail,
                             bool sendEmailAfterSuccess = false, string emailDescription = "")
    {
        int operationStatus;
        try
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = unitAmount,
                Currency = currency,
                Customer = customerId,
                PaymentMethod = paymentMethodId,
                Confirm = true,
                OffSession = true,
                ReceiptEmail = sendEmailAfterSuccess ? customerEmail : null,
                Description = emailDescription,
            };

            await _paymentIntentService.CreateAsync(options);
            operationStatus = OperationStatuses.Payed;
        }
        catch (StripeException e)
        {
            switch (e.StripeError.Type)
            {
                case "card_error":
                    var paymentIntentId = e.StripeError.PaymentIntent.Id;
                    var paymentIntent = _paymentIntentService.Get(paymentIntentId);
                    break;
                default:
                    break;
            }
            operationStatus = OperationStatuses.Cancelled;
        }
        await _appDbContext.AddAsync(new Operation()
        {
            OperationStatus = operationStatus,
            User = _appDbContext.Users.First(t => t.CustomerId == customerId)
        });
        await _appDbContext.SaveChangesAsync();
    }
}
