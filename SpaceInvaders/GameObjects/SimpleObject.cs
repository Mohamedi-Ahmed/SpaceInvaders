using Space_invaders.GameObjects;
using Space_invaders.Properties;
using System;
using System.Drawing;
using System.Reflection;

namespace SpaceInvaders.GameObjects
{
    public abstract class SimpleObject : GameObject
    {
        // Position du joueur (si image -> coordonnees x,y de l'angle supérieur gauche de l'image)
        public Vecteur2D Position { get; set; }
        public int ObjectWidth { get; set; }
        public int ObjectHeight { get; set; }
        // Propriété publique pour le nombre de vies
        public int Vies { get; set; }

        // Propriété  pour l'image de l'objet
        public Bitmap Image { get; set; }

        protected SimpleObject(Vecteur2D position, Bitmap image, int vies, Side side) : base(side)
        {
            Position = position;
            Image = image;
            Vies = vies;

        }

        public override void Draw(Graphics graphics, int largeur, int hauteur)
        {
            if (Image != null)
            {
                graphics.DrawImage(Image, (float)Position.x, (float)Position.y, largeur, hauteur);
            }
        }

        public override bool IsAlive()
        {
            return Vies > 0;
        }



        public override void Collision(Missile missile)
        {
            if (this.ObjectSide != missile.ObjectSide)
            {
                Bitmap thisBitmap = new Bitmap(this.Image, this.ObjectWidth, this.ObjectHeight);
                Bitmap missileBitmap = new Bitmap(Resources.projectile, missile.ObjectWidth, missile.ObjectHeight);

                int nbOfPixelsInCollision = 0;

                Rectangle thisRect = new Rectangle((int)this.Position.x, (int)this.Position.y, this.ObjectWidth, this.ObjectHeight);
                Rectangle missileRect = new Rectangle((int)missile.Position.x, (int)missile.Position.y, missile.ObjectWidth, missile.ObjectHeight);

                Rectangle intersection = Rectangle.Intersect(thisRect, missileRect);

                if (intersection.IsEmpty)
                {
                    return; // Pas de collision
                }
                else { Console.WriteLine("Il y a une collision ! "); }

                // Parcourir chaque pixel dans la zone de chevauchement pour effacer les pixels du bunker
                for (int x = intersection.Left; x < intersection.Right; x++)
                {
                    for (int y = intersection.Top; y < intersection.Bottom; y++)
                    {
                        int thisRelativeX = x - thisRect.Left;
                        int thisRelativeY = y - thisRect.Top;

                        // Vérifiez que les indices sont dans les limites de l'image du bunker
                        if (thisRelativeX >= 0 && thisRelativeX < thisBitmap.Width && thisRelativeY >= 0 && thisRelativeY < thisBitmap.Height)
                        {
                            Color thisPixelColor = thisBitmap.GetPixel(thisRelativeX, thisRelativeY);
                            if (this is Bunker && thisPixelColor.A > 128) // Vérifiez si le pixel n'est pas transparent
                            {
                                Console.WriteLine("thisPixelColor " + thisPixelColor);

                                nbOfPixelsInCollision++;
                                thisBitmap.SetPixel(thisRelativeX, thisRelativeY, Color.Transparent); // Rendre le pixel transparent
                                this.Image = thisBitmap;
                            }
                            else if (!(this is Bunker) && thisPixelColor.A > 128)
                            {
                                Console.WriteLine("thisPixelColor " + thisPixelColor);

                                nbOfPixelsInCollision++;

                            }
                        }
                    }
                }

                if (nbOfPixelsInCollision > 0)
                {

                    OnCollision(missile, nbOfPixelsInCollision);
                }
            }
           
        }

        protected abstract void OnCollision(Missile missile, int nbOfPixelsInCollision);
    }
}
