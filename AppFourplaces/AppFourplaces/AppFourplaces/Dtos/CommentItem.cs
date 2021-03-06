using System;
using Newtonsoft.Json;

namespace AppFourplaces.Dtos
{
	public class CommentItem
	{
		[JsonProperty("date")]
		public DateTime Date { get; set; }
		
		[JsonProperty("author_name")]
		public string AuthorName { get; set; }
		
		[JsonProperty("text")]
		public string Text { get; set; }
	}
}