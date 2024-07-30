namespace Incharge.Service.PagingService
{
    public class PaginationForm
    {
        public string SortOrder { get; set; }
        public string CurrentFilter { get; set; }
        public string SearchString { get; set; }
        public int? PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
