namespace API.Helpers
{
    /// <summary>
    ///     Represents parameters with info about pagination passed from query string
    /// </summary>
    public class UserParams
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;
        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}