namespace AccountService.Dtos
{
    public class PaginationParameterDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchName { get; set; } = "";
    }

    public class PaginationResponse<T>
    {
        public int TotalRecords { get; set; } = 0;
        public T Payload { get; set; } = default!;

        public PaginationResponse()
        {
        }

        public PaginationResponse(int totalRecords, T payload)
        {
            this.TotalRecords = totalRecords;
            this.Payload = payload;
        }
    }
}