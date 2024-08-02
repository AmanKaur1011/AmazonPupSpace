using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmazonPupSpace.Models.ViewModels
{
    public class UpdateMyDog
    {
        public DogDto SelectedDog { get; set; }

        //all tricks
        public IEnumerable<TrickDto> AvailableTrickOptions { get; set; }

        public IEnumerable<EmployeeDto> EmployeeOptions { get; set; }
        //all tricks  that this dog knows
        public IEnumerable<DogxTrickDto> TricksLearnt { get; set; }
    }
}