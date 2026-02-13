namespace Domain.RequestFeatures
{
    public abstract class RequestParameters
    {
        const int maxPageSize = 50;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }

        public string? SearchTerm { get; set; }

        public int PageNumber { get; set; } = 1;
    }
}