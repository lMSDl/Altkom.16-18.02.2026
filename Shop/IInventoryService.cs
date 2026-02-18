namespace Shop
{
    public interface IInventoryService
    {
        bool CheckStock(int productId, int quantity);
        void ReserveStock(int productId, int quantity);
    }
}
