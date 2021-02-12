namespace VintageBookshelf.UI.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string Message { get; set; }
        public string Title { get; set; }
        public int ErrorStatusCode { get; set; }
    }
}
