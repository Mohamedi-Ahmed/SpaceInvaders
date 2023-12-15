using SpaceInvaders.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public enum Side { Ally, Enemy, Neutral}

    public abstract class GameObject
    {
        public abstract void Draw(Graphics graphics, int largeur, int hauteur);
        public abstract bool IsAlive();
        public abstract void Update(Keys key, Size gameSize);
        public abstract void Collision(Missile m);

        public Side ObjectSide { get; private set; }
        public GameObject(Side objectSide)
        {
            ObjectSide = objectSide;
        }
    }
}
