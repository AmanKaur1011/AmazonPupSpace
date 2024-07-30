using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmazonPupSpace.Models.ViewModels
{
    public class AddEmployee
    {
        public IEnumerable<DepartmentDto> DepartmentOptions { get; set; }
    }
}