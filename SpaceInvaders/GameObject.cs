using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public abstract class GameObject
    {
        public abstract void Draw(Graphics graphics, int largeur, int hauteur);
        public abstract bool IsAlive();
        public abstract void Update(Keys key, Size gameSize);
    }
}
