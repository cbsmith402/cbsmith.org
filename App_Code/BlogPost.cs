using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using Humanizer;
using Lucene.Net.Linq;
using System.Linq;

/// <summary>
/// Summary description for BlogPost
/// </summary>
public class BlogPost
{
	public string Url { get; set; }

	public string Title { get; set; }

	public DateTime PostedDate { get; set; }

	public IEnumerable<string> Tags { get; set; }

	public string Content { get; set; }

	public string Summary { get; set; }

    public BlogPost()
	{
	}
	
	public BlogPost(string filename, string input)
    {
		Url = "/Blog/" + filename;

		//Parse out the metadata (if there is any)
		Title = filename.Humanize(LetterCasing.Title);
		PostedDate = DateTime.Now;
		Tags = new List<string>();

		int contentStart = 0;
		foreach(Match match in Regex.Matches(input, "^(Title|Tags|Posted Date|Summary): (.*)$", RegexOptions.Multiline))
		{
			Console.WriteLine(match.Value);
			string key = match.Groups[1].Value;
			switch(key)
			{
				case "Title":
					Title = match.Groups[2].Value;
					break;
				case "Summary":
					Summary = match.Groups[2].Value;
					break;
				case "Posted Date":
					DateTime postedDate;
					DateTime.TryParse(match.Groups[2].Value, out postedDate);
					PostedDate = postedDate;
					break;
				case "Tags":
					Tags = match.Groups[2].Value.Split(',').ToList();
					break;
			}
			contentStart = match.Index + match.Length;
		}

		Content = input.Substring(contentStart).TrimStart();
    }
}