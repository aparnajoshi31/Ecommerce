public class ProductModel
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public List<ProductDetail> Details { get; set; } = new List<ProductDetail>();
}