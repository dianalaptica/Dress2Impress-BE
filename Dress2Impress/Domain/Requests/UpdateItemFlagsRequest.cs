namespace Dress2Impress.Domain.Requests;

public class UpdateItemFlagsRequest
{
    public bool? IsDirty { get; set; }
    public bool? WornOut { get; set; }
}
