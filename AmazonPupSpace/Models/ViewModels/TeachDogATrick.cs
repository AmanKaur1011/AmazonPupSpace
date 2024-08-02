using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmazonPupSpace.Models.ViewModels
{
    public class TeachDogATrick
    {

        //viewmodel stores trick class and dog class. It will help display all available tricks for a dog to learn

        //existing dog information
        public DogDto SelectedDog { get; set; }

        //all tricks
        public IEnumerable<TrickDto> TrickOptions { get; set; }

        //all tricks  that this dog knows
        public IEnumerable<DogxTrick> TricksLearnt { get; set; }
    }
}