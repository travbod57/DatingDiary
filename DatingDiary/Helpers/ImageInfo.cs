using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatingDiary.Helpers
{
    public class ImageInfo
    {
        public string FileName { get; set; }
        public string Directory { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public bool IsSquare { get; set; }
        public int SquareSize
        {
            get { return Width;  }
        }
    }
}
