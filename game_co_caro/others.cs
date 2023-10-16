using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_co_caro
{
    public class others
    {
        public const int cellSize = 25;
        public static int broadSize
        {
            get
            {
                return Form1.cellNums * cellSize;
            }
        }
    }
}
