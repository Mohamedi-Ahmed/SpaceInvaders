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
        public override void Collision(Missile missile)
        {
            // Calculez les rectangles englobants pour le missile et le bunker
            Rectangle bunkerRect  = new Rectangle((int)this.Position.x, (int)this.Position.y,
                                                  gameInstance.largeurImageBunker, gameInstance.hauteurImageBunker);
            Rectangle missileRect = new Rectangle((int)missile.Position.x, (int)missile.Position.y,
                                                  gameInstance.largeurImageMissile, gameInstance.hauteurImageMissile);

            // Vérifiez si les rectangles englobants s'intersectent
            if (bunkerRect.IntersectsWith(missileRect))
            {
                // Effectuez le test pixel par pixel
                //Console.WriteLine("Je teste la collision");
                TestCollisionPixel(missile);
            }
        }

        // Rectangle englobant pour débugger
        public override void Draw(Graphics graphics, int largeur, int hauteur)
        {
            base.Draw(graphics, largeur, hauteur);

            // Dessiner le rectangle englobant
            Rectangle bunkerRect = new Rectangle((int)this.Position.x, (int)this.Position.y, largeur, hauteur);
            using (Pen pen = new Pen(Color.Yellow, 2))  // Couleur jaune pour le rectangle, épaisseur de 2
            {
                graphics.DrawRectangle(pen, bunkerRect);
            }
        }
        public void TestCollisionPixel(Missile missile)
        {
            Bitmap missileBitmap = new Bitmap(Resources.projectile, gameInstance.largeurImageMissile, gameInstance.hauteurImageMissile);
            Bitmap bunkerBitmap = new Bitmap(this.Image, gameInstance.largeurImageBunker, gameInstance.hauteurImageBunker);

            // Parcourir chaque pixel du missile
            for (int x = 0; x < missileBitmap.Width; x++)
            {
                for (int y = 0; y < missileBitmap.Height; y++)
                {
                    int missileGlobalX = (int)missile.Position.x + x;
                    int missileGlobalY = (int)missile.Position.y + y;

                    // Vérifier si le pixel est à l'intérieur des limites du bunker
                    if (missileGlobalX >= this.Position.x && missileGlobalX < this.Position.x + bunkerBitmap.Width &&
                        missileGlobalY >= this.Position.y && missileGlobalY < this.Position.y + bunkerBitmap.Height)
                    {
                        int bunkerRelativeX = missileGlobalX - (int)this.Position.x;
                        int bunkerRelativeY = missileGlobalY - (int)this.Position.y;

                        Color bunkerPixelColor = bunkerBitmap.GetPixel(bunkerRelativeX, bunkerRelativeY);

                        //(normalement bunkerPixelColor.G == 252 mais image pas totalement verte dont les contours)
                        if (bunkerPixelColor.A > 128 && bunkerPixelColor.G > 200)
                        {
                            bunkerBitmap.SetPixel(bunkerRelativeX, bunkerRelativeY, Color.Transparent);
                            
                            // Le missile est détruit
                            missile.Vies = 0;
                            // MaJ de l'image
                            this.Image = bunkerBitmap;
                        }
                    }
                }
            }
        }
        // ...
    }
}

