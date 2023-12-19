using System.Drawing;

namespace SpaceInvaders.GameObjects
{
    public abstract class SimpleObject : GameObject
    {
        // Position du joueur (si image -> coordonnees x,y de l'angle supérieur gauche de l'image)
        public Vector2D Position { get; set; }
        public int LifePoints { get; set; }
        public Bitmap Image { get; set; }

        protected SimpleObject(Vector2D position, Bitmap image, int lifepoints, Side side) : base(side)
        {
            Position = position;
            Image = image;
            LifePoints = lifepoints;

        }

        public override void Draw(Graphics graphics, int width, int height)
        {
            if (Image != null)
            {
                graphics.DrawImage(Image, (float)Position.x, (float)Position.y, width, height);
            }
        }

        public override bool IsAlive()
        {
            return LifePoints > 0;
        }

        public override void Collision(Missile missile)
        {
            if (this.ObjectSide != missile.ObjectSide)
            {
                Rectangle thisRect    = new Rectangle((int)this.Position.x, (int)this.Position.y, this.ObjectWidth, this.ObjectHeight);
                Rectangle missileRect = new Rectangle((int)missile.Position.x, (int)missile.Position.y, missile.ObjectWidth, missile.ObjectHeight);

                if (thisRect.IntersectsWith(missileRect))
                {
                    Bitmap thisBitmap = new Bitmap(this.Image, this.ObjectWidth, this.ObjectHeight);
                    Bitmap missileBitmap = new Bitmap(missile.Image, missile.ObjectWidth, missile.ObjectHeight);

                    int nbOfPixelsInCollision = CheckPixelCollision(thisRect, thisBitmap, missileRect, missileBitmap);

                    if (nbOfPixelsInCollision > 0)
                    {
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
                    int thisRelativeX = x - thisRect.Left;
                    int thisRelativeY = y - thisRect.Top;
                    int missileRelativeX = x - missileRect.Left;
                    int missileRelativeY = y - missileRect.Top;

                    if (thisRelativeX >= 0 && thisRelativeX < thisBitmap.Width && thisRelativeY >= 0 && thisRelativeY < thisBitmap.Height &&
                        missileRelativeX >= 0 && missileRelativeX < missileBitmap.Width && missileRelativeY >= 0 && missileRelativeY < missileBitmap.Height)
                    {
                        Color thisPixelColor    = thisBitmap.GetPixel(thisRelativeX, thisRelativeY);
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
