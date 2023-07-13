﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONEditor.Classes.Settings
{
    public class Settings
    {
        public WindowPosition WindowPosition { get; set; }
        public WindowSize WindowSize { get; set; }
        public int SplitterDistance { get; set; }
        public bool Maximized { get; set; }
    }

    public class WindowPosition
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class WindowSize
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}