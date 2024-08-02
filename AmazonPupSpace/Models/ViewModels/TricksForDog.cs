using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmazonPupSpace.Models.ViewModels
{
    public class TricksForDog
    {
        //existing dog information
        public DogDto SelectedDog { get; set; }
        
        //all tricks  that this dog knows
        public IEnumerable<DogxTrickDto> TricksLearnt { get; set; }

        //public IEnumerable<TrickDto> TrickOptions { get; set; }

    }
}