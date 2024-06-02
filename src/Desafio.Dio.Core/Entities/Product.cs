namespace Desafio.Dio.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int IdCategory { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public virtual Category Category { get; set; }
    }
}