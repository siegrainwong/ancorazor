#region

using Ancorazor.API.Common.Constants;
using Siegrain.Common;
using System;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Ancorazor.API.Messages.Article
{
    public class ArticleUpdateParameter
    {
        // only provide single-user currently
        public int Author { get; } = 1;

        public string Category { get; set; } = Constants.Article.DefaultCategoryName;

        [Required]
        public string Content { get; set; }

        public int Cover { get; set; } = 1;

        public DateTime? CreatedAt { get; set; }

        [StringLength(500, MinimumLength = 0, ErrorMessage = "Digest must be less than 500 characters.")]
        public string Digest { get; set; }

        public string Alias { get; set; }

        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        public bool IsDraft { get; set; } = false;

        public string Remark { get; set; }

        public string[] Tags { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "Title must be less than 256 characters.")]
        public string Title { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}