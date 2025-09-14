namespace Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nome obrigatório");
            if (price < 0) throw new ArgumentException("Preço invalido");

            Name = name;
            Price = price;
        }

        public void UpdatePrice(string name, decimal newPrice)
        {
            if (newPrice < 0 && !string.IsNullOrEmpty(name)) throw new ArgumentException("Preço Invalido");
            Price = newPrice;
            Name = name;
        }
    }
}
