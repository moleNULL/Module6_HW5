namespace WebAPI_UnitTests.Models.Requests.BrandRequests
{
    public class UpdateBrandRequest
    {
        public int Id { get; set; }
        public string Brand { get; set; } = null!;
    }
}
