public class Product : BaseEntity
{
    public string Name { get; set; }
    public string ProductCode { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ProductImage { get; set; }
}