using System;
using System.Drawing;
using System.Windows.Forms;
using Space_invaders.Properties;
using SpaceInvaders;
using SpaceInvaders.GameObjects;

namespace Space_invaders.GameObjects
{
    internal class Bunker : SimpleObject
    {
        public Bunker(Vecteur2D position_initiale): base(position_initiale, Resources.bunker, 1){   }
        public override void Update(Keys key, Size gameSize)
        {
            // Ne fais rien !
        }
    }

    
}
