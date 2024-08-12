using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace AmazonPupSpace.Models.ViewModels
{
    public class PostDetailsViewModel
    {
        public Post Post { get; set; }
        public Employee Employee { get; set; }
        public string AspNetUserId { get; set; }
        public int EmployeeId { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IEnumerable<CommentDto> Comments { get; set; }
    }
}