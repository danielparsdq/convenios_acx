using DocumentFormat.OpenXml.Office.CoverPageProps;

namespace ConveniosApi.Models
{
    public class ApiResponse
    {

        public string? Error { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public Links? Links { get; set; }
        public int? Total { get; set; }

    }

    public class Links
    {
        public string? Next { get; set; }
        public string? Prev { get; set; }
    }
}
