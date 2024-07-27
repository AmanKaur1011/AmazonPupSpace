using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AmazonPupSpace.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        public string Title { get; set; }

        public string Caption { get; set; }

 
        public bool ImageURL { get; set; }
        //images deposited into /Content/Images/Animals/{id}.{extension}
        public string PicExtension { get; set; }

        public DateTime PostDate { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        
      

    }
}