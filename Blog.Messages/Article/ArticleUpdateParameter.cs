#region

using Blog.API.Common.Constants;
using Siegrain.Common;
using System;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Blog.API.Messages.Article
{
    public class ArticleUpdateParameter
    {
        public int Author { get; } = 1;

        public string[] Categories { get; set; } = { Constants.Article.DefaultCategoryName };

        [Required]
        public string Content { get; set; }

        [StringLength(200, MinimumLength = 0, ErrorMessage = "Cover must be less than 200 characters.")]
        public string Cover { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [StringLength(500, MinimumLength = 0, ErrorMessage = "Digest must be less than 500 characters.")]
        public string Digest { get; set; }

        [RegularExpression("^[a-zA-Z0-9 ]$", ErrorMessage = "Alias must be English alphabet and number.")]
        public string Alias { get; set; }

        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        // only provide single-user currently

        public bool IsDraft { get; set; } = false;

        public string Remark { get; set; }

        public string[] Tags { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "Title must be less than 256 characters.")]
        public string Title { get; set; }

        public string TitlePinyin => CHNToPinyin.ConvertToPinYin(Title);

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}