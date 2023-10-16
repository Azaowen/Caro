using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_co_caro
{
    public class Player
    {
        private string name;
        private Image avatar;

        public string Name { get => name; set => name = value; }
        public Image Avatar { get => avatar; set => avatar = value; }

        

        public Player(string name, Image avatar) 
        {
            this.name = name;
            this.avatar = avatar;
        }
    }
}
