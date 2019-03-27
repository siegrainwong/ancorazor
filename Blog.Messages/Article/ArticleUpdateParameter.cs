#region

using System;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Blog.API.Messages.Article
{
    public class ArticleUpdateParameter
    {
        [Range(0, int.MaxValue, ErrorMessage = "Invalid Id")]
        public int Id { get; set; }

        [StringLength(200, MinimumLength = 0, ErrorMessage = "Cover must be less than 200 characters")]
        public string Cover { get; set; }

        public int Author { get; } = 1;    // only provide single-user currently

        [Required]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "Title must be less than 256 characters")]
        public string Title { get; set; }

        public string[] Categories { get; set; } = {"uncategorized"};

        public string[] Tags { get; set; }

        [Required]
        public string Content { get; set; }

        [StringLength(500, MinimumLength = 0, ErrorMessage = "Digest must be less than 500  characters")]
        public string Digest { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime CreatedAt { get; set; }

        public bool? IsDeleted { get; set; }

        public string Remark { get; set; }
    }
}