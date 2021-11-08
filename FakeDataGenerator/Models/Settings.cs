using System;

namespace FakeDataGenerator.Models
{
    public class Settings
    {
        private Region _region = Region.Usa;
        public Region Region
        {
            get => _region;
            set
            {
                if (value == Region.Usa)
                {
                    LeftBorder = 'a';
                    RightBorder = 'z';
                }
                else
                {
                    LeftBorder = 'а';
                    RightBorder = 'я';
                }
                _region = value;
            }
        }

        public Random Random { get; set; }
        public double ErrorsCount { get; set; }
        public char LeftBorder { get; private set; } = 'a';
        public char RightBorder { get; private set; } = 'я';
        public char LeftDigit { get; private set; } = '0';
        public char RightDigit { get; private set; } = '9';
    }
}