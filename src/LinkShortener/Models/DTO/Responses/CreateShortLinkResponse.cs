namespace LinkShortener.Models.DTO.Responses
{
    public class CreateShortLinkResponse
    {
        public string OriginalLink { get; set; }
        public string ShortLink { get; set; }
    }
}