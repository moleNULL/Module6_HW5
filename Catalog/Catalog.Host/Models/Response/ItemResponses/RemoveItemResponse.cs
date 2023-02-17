namespace Catalog.Host.Models.Response.ItemResponses
{
    public class RemoveItemResponse<T>
    {
        public T RemoveState { get; set; } = default!;
    }
}
