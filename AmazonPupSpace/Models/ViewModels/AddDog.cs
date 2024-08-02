using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmazonPupSpace.Models.ViewModels
{
    public class AddDog
    {
        public IEnumerable<TrickDto> TrickOptions { get; set; }
        public IEnumerable<EmployeeDto> EmployeeOptions { get; set; }

    }
}