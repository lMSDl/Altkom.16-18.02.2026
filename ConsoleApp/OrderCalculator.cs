namespace ConsoleApp
{
    public class OrderCalculator
    {
        public decimal CalculateTotal(decimal unitPrice, int quantity, decimal discount = 0)
        {
            if (unitPrice < 0 || quantity < 0 || discount < 0)
                throw new ArgumentException("Values must be non-negative");

            decimal subtotal = unitPrice * quantity;
            decimal total = subtotal - discount;

            return total < 0 ? 0 : total;
        }


        public decimal CalculateShippingCost(decimal orderTotal)
        {
            if (orderTotal < 0)
                throw new ArgumentException("Order total cannot be negative");

            if (orderTotal == 0)
                return 0;

            if (orderTotal >= 100)
                return 0;

            return 15;
        }

        public decimal CalculateOrderTotal(IEnumerable<decimal> itemPrices)
        {
            if (itemPrices == null)
                throw new ArgumentNullException(nameof(itemPrices));

            decimal total = 0;
            foreach (var price in itemPrices)
            {
                if (price < 0)
                    throw new ArgumentException("Item price cannot be negative");

                total += price;
            }

            return total;
        }

        public event EventHandler<string>? DiscountApplied;

        public decimal ApplyDiscount(decimal total, decimal discount)
        {
            if (discount > 0)
            {
                DiscountApplied?.Invoke(this, $"Applied discount: {discount} zł");
            }

            return total - discount;
        }

        public async Task<decimal> SimulateRemoteDiscountFetchAsync(decimal baseAmount)
        {
            await Task.Yield(); //symulacja operacji asynchronicznej
            var discount = baseAmount > 100 ? 20 : 0;
            return baseAmount - discount;
        }
    }
}
