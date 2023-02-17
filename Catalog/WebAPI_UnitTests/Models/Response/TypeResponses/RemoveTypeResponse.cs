namespace WebAPI_UnitTests.Models.Response.TypeResponses
{
    public class RemoveTypeResponse<T>
    {
        public T RemoveState { get; set; } = default!;
    }
}
