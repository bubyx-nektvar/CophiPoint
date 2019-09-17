using FFImageLoading.Transformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.Resources
{
    public class StaticConsts
    {
        public static float[][] MakeBlue =
            new float[][]
            {
                new [] { 0f, 0f, 0f, 0f, 0f },
                new [] { 0f, 109f/255f, 0f, 0f, 0f },
                new [] { 0f, 0f, 240f/255f, 0f, 0f },
                new [] { 0f, 0f, 0f, 1f, 0f },
                new [] { 0f, 0f, 0f, 0f, 1f },
            }; //FFColorMatrix.ColorToTintMatrix(00, 109, 240, 255);//#006df0
    }
}
