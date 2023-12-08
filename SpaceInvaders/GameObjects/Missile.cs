using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Space_invaders.Properties;

namespace SpaceInvaders.GameObjects
{

    public class Missile : SimpleObject
    {
        private double vitesse;
        public Vecteur2D LastPosition { get; private set; }

        public Missile(Vecteur2D position,Bitmap Image, int vies, Side side)
           : base(position, Image, vies, side)
        {
            this.vitesse = 10.0;
            this.Image = Image;
        }

        public override void Update(Keys key, Size gameSize)
        {
            LastPosition = new Vecteur2D(Position.x, Position.y);

            if(this.ObjectSide == Side.Ally)
            {
                this.Position.y -= vitesse;
                if (this.Position.y < 0)
                {
                    Vies = 0;
                }
            }
            else
            {
                this.Position.y += vitesse;
                if (this.Position.y > 0)
                {
                    Vies = 0;
                }
            }

        }
        // Rectangle englobant pour débugger
        public override void Draw(Graphics graphics, int largeur, int hauteur)
        {
            base.Draw(graphics, largeur, hauteur);

            // Dessiner le rectangle englobant
            
            Rectangle missileRect = new Rectangle((int)Position.x, (int)Position.y, this.ObjectWidth, this.ObjectHeight);
            using (Pen pen = new Pen(Color.AntiqueWhite, 2))  // Couleur jaune pour le rectangle, épaisseur de 2
            {
                graphics.DrawRectangle(pen, missileRect);
            }
            
        }

        protected override void OnCollision(Missile missile, int numberOfPixelsInCollision)
        {
            this.Vies    = 0;
            missile.Vies = 0; 
        }

    }
    
}
