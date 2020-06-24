using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoGUI
{
    class Led
    {
        public int color;
        public int pattern;
        public int time;

        public static readonly String[] colors = { "None", "Red", "Green", "Blue", "White", "Yellow", "Orange", "Pink",
            "Purple"};
        public static readonly String[] patterns = { "Solid", "Dim Up", "Dim Down", "Blink Slow", "Blink Fast", "Off" };
        public static readonly String[] times = { "0s", "0.5s", "1s", "1.5s", "2s", "2.5s", "3s", "3.5s", "4s", "4.5s", "5s", };

        public Led()
        {
            color = 0;
            pattern = 0;
            time = 0;
        }
    }
}
