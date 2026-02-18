namespace Shop
{
    public interface IPaymentService
    {
        bool ProcessPayment(string cardNumber, decimal amount);
    }
}
