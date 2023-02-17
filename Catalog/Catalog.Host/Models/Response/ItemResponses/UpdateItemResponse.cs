namespace Catalog.Host.Models.Response.ItemResponses
{
    public class UpdateItemResponse<T>
    {
        public T UpdateState { get; set; } = default!;
    }
}
