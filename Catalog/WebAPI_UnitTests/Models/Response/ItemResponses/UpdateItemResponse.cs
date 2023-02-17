namespace WebAPI_UnitTests.Models.Response.ItemResponses
{
    public class UpdateItemResponse<T>
    {
        public T UpdateState { get; set; } = default!;
    }
}
