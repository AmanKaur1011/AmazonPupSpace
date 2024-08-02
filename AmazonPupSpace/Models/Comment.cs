using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace AmazonPupSpace.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        [StringLength(100)]
        public string CommentText { get; set; } = string.Empty;

        [Required]

        public DateTime DateCommented { get; set; } = DateTime.UtcNow;

        // A post can have many Comments
        [Required]
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public virtual Post Post { get; set; }

        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
    public class CommentDto
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public DateTime DateCommented { get; set; }
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        public bool ImageURL { get; set; }
        //images deposited into /Content/Images/Animals/{id}.{extension}
        public string PicExtension { get; set; }
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}