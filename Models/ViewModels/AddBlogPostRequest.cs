using Microsoft.AspNetCore.Mvc.Rendering;

namespace Models.ViewModels
{
    public class AddBlogPostRequest
    {
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturesImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }
        public IEnumerable<SelectListItem> Tags { get; set; } //to display the tags
        public string[] SelectedTags { get; set; } = Array.Empty<string>(); //to capture the selected tags from the form

    }
}