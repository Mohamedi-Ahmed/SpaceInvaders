using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders.GameObjects
{

    public class Missile : SimpleObject
    {
        private double speedMissile;
        public Missile(Vector2D position,Bitmap Image, int lifePoints, Side side)
           : base(position, Image, lifePoints, side)
        {
            this.speedMissile = 20.0;
            this.Image = Image;
        }

        public override void Update(HashSet<Keys> pressedKeys, Size gameSize)
        {
            if(this.ObjectSide == Side.Ally)
            {
                this.Position.y -= speedMissile;
                if (this.Position.y < 0) LifePoints = 0;
            }
            else
            {
                this.Position.y += speedMissile;
                if (Position.y > gameSize.Height) LifePoints = 0; 
            }
            if (!this.IsAlive()) gameInstance.GameObjects.Remove(this); 

        }
        protected override void OnCollision(Missile missile, int numberOfPixelsInCollision)
        {
            this.LifePoints = 0;
            missile.LifePoints = 0;
        }

        /* //Decommenter pour le debug
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
        */

    }


    
}
