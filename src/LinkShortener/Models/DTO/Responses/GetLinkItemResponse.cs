namespace LinkShortener.Models.DTO.Responses
{
    public class GetLinkItemResponse
    {
        public string OriginalLink  { get; set; }
        public string ShortLink  { get; set; }
        public long VisitsCount { get; set; }
    }
}