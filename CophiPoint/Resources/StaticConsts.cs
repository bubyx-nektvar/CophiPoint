using FFImageLoading.Transformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.Resources
{
    public class StaticConsts
    {
        static StaticConsts()
        {
            var primary = (Xamarin.Forms.Color)Xamarin.Forms.Application.Current.Resources["primary"];
            MakePrimary = new float[][]
            {
                new [] { (float)primary.R, 0f, 0f, 0f, 0f },
                new [] { 0f, (float)primary.G, 0f, 0f, 0f },
                new [] { 0f, 0f, (float)primary.B , 0f, 0f },
                new [] { 0f, 0f, 0f, (float)primary.A, 0f },
                new [] { 0f, 0f, 0f, 0f, 1f },
            };
        }
        public static float[][] MakePrimary;
    }
}
