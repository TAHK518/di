﻿using System.Drawing;
using TagsCloud.Interfaces;
using System;

namespace TagsCloud.WordProcessing
{
    public class DefaultColorScheme : IColorScheme
    {
        public Color GetColorForCurrentWord((string word, int frequency) wordFrequency, int positionByFrequency, int countWords)
        {
            var rnd = new Random();
            var alpha = (int)(255 * ((float)(countWords - positionByFrequency + 1) / countWords));
            alpha = 255 < alpha ? 255 : alpha;
            return Color.FromArgb(alpha, rnd.Next(50, 255), rnd.Next(50, 255), rnd.Next(50, 255));
        }
    }
}