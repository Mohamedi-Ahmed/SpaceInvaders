using SpaceInvaders.GameObjects;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public enum Side { Ally, Enemy, Neutral}

    public abstract class GameObject
    {
        public int ObjectWidth { get; set; }
        public int ObjectHeight { get; set; }
        public abstract void Draw(Graphics graphics, int largeur, int hauteur);
        public abstract bool IsAlive();
        public abstract void Update(HashSet<Keys> pressedKeys, Size gameSize);
        public abstract void Collision(Missile m);

        public Side ObjectSide { get; private set; }
        public GameObject(Side objectSide)
        {
            ObjectSide = objectSide;
            ObjectWidth = ObjectWidth;
            ObjectHeight = ObjectHeight;
        }
    }
}
