namespace MariTech.ORX.Api.Models.Common;

public class PagedResponse<T>
{
    public IEnumerable<T> Data { get; set; } = [];
    
    public int Page { get; set; }
    
    public int PageSize { get; set; }
    
    public int TotalRecords { get; set; }
    
    public int TotalPages { get; set; }
}