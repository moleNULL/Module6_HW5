namespace Catalog.Host.Models.Response.GetResponses
{
    public class GetItemBadRequestResponse<T>
    {
        public T ResponseState { get; set; } = default!;
    }
}
