using Ancorazor.Entity.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ancorazor.Entity
{
    public class ImageStorage : BaseEntity
    {
        public ImageStorage()
        {
            Article = new HashSet<Article>();
        }

        [Required]
        public int Uploader { get; set; }
        [Required]
        public long Size { get; set; }
        [Required]
        [StringLength(500)]
        public string Path { get; set; }
        [StringLength(500)]
        public string ThumbPath { get; set; }
        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [ForeignKey("Uploader")]
        [InverseProperty("ImageStorage")]
        public virtual Users UploaderNavigation { get; set; }

        [InverseProperty("ImageStorageNavigation")]
        public virtual ICollection<Article> Article { get; set; }
    }
}
