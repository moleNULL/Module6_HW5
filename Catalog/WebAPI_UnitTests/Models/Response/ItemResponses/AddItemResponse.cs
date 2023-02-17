namespace WebAPI_UnitTests.Models.Response.ItemResponses;

public class AddItemResponse<T>
{
    public T Id { get; set; } = default!;
}