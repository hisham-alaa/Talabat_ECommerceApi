namespace Talabat.APIs.Helpers
{
    public class PaginationResponse<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

        public PaginationResponse(int index, int size, IReadOnlyList<T> data, int count)
        {
            Data = data;
            PageIndex = index;
            PageSize = size;
            Count = count;
        }
    }
}
