using Newtonsoft.Json;

namespace AppFourplaces.Dtos
{
	public class CreateCommentRequest
	{
		[JsonProperty("author_name")]
		public string AuthorName { get; set; }
		
		[JsonProperty("text")]
		public string Text { get; set; }
	}
}