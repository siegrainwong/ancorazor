#region

using System;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;

#endregion

namespace Blog.API.Messages.Users
{
    public class ArticleParameter
    {
        [Range(0, int.MaxValue, ErrorMessage = "Invalid Id")]
        public int Id { get; set; }

        [StringLength(200, MinimumLength = 0, ErrorMessage = "Cover must be less than 200 characters")]
        public string Cover { get; set; }

        public int Author { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "Title must be less than 256 characters")]
        public string Title { get; set; }

        public string Category { get; set; }

        [Required]
        public string Content { get; set; }

        [StringLength(500, MinimumLength = 0, ErrorMessage = "Digest must be less than 500  characters")]
        public string Digest { get; set; }
        
        public int? ViewCount { get; set; }
        
        public int? CommentCount { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? CreatedAt { get; set; }

        public bool? IsDeleted { get; set; }

        public string Remark { get; set; }
    }
}