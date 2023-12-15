using SpaceInvaders.GameObjects;
using SpaceInvaders.Properties;
using System;
using System.Drawing;
using System.Reflection;

namespace SpaceInvaders.GameObjects
{
    public abstract class SimpleObject : GameObject
    {
        // Position du joueur (si image -> coordonnees x,y de l'angle supérieur gauche de l'image)
        public Vecteur2D Position { get; set; }
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
                Bitmap thisBitmap    = new Bitmap(this.Image, this.ObjectWidth, this.ObjectHeight);
                Bitmap missileBitmap = new Bitmap(missile.Image, missile.ObjectWidth, missile.ObjectHeight); // Obtention de l'image du missile

                Rectangle thisRect    = new Rectangle((int)this.Position.x, (int)this.Position.y, this.ObjectWidth, this.ObjectHeight);
                Rectangle missileRect = new Rectangle((int)missile.Position.x, (int)missile.Position.y, missile.ObjectWidth, missile.ObjectHeight);

                if (thisRect.IntersectsWith(missileRect))
                {
                    Console.WriteLine("Il y a une collision !");
                    // Effectuer la vérification pixel par pixel seulement si les rectangles se chevauchent
                    int nbOfPixelsInCollision = CheckPixelCollision(thisRect, thisBitmap, missileRect, missileBitmap);

                    if (nbOfPixelsInCollision > 0)
                    {
                        Console.WriteLine( "nbOfPixelsInCollision : "+ nbOfPixelsInCollision);
                        OnCollision(missile, nbOfPixelsInCollision);
                    }
                }
            }
        }

        private int CheckPixelCollision(Rectangle thisRect, Bitmap thisBitmap, Rectangle missileRect, Bitmap missileBitmap)
        {
            int nbOfPixelsInCollision = 0;

            // Calculer l'intersection des deux rectangles
            Rectangle intersection = Rectangle.Intersect(thisRect, missileRect);

            for (int x = intersection.Left; x < intersection.Right; x++)
            {
                for (int y = intersection.Top; y < intersection.Bottom; y++)
                {
                    // Calculer les positions relatives des pixels dans les bitmaps de chaque objet
                    int thisRelativeX = x - thisRect.Left;
                    int thisRelativeY = y - thisRect.Top;
                    int missileRelativeX = x - missileRect.Left;
                    int missileRelativeY = y - missileRect.Top;

                    // Vérifier si les positions sont dans les limites des images
                    if (thisRelativeX >= 0 && thisRelativeX < thisBitmap.Width && thisRelativeY >= 0 && thisRelativeY < thisBitmap.Height &&
                        missileRelativeX >= 0 && missileRelativeX < missileBitmap.Width && missileRelativeY >= 0 && missileRelativeY < missileBitmap.Height)
                    {
                        // Obtenir les couleurs des pixels à ces positions
                        Color thisPixelColor = thisBitmap.GetPixel(thisRelativeX, thisRelativeY);
                        Color missilePixelColor = missileBitmap.GetPixel(missileRelativeX, missileRelativeY);

                        // Vérifier la transparence des pixels
                        if (thisPixelColor.A > 128 && missilePixelColor.A > 128)
                        {
                            nbOfPixelsInCollision++;
                        }
                    }
                }
            }

            return nbOfPixelsInCollision;
        }

        protected abstract void OnCollision(Missile missile, int nbOfPixelsInCollision);
    }
}
