using System;
using System.Drawing;
using System.Windows.Forms;
using SpaceInvaders.Properties;
using SpaceInvaders;
using SpaceInvaders.GameObjects;

namespace SpaceInvaders.GameObjects
{
    internal class Bunker : SimpleObject
    {
        public Bunker(Vecteur2D position_initiale, Side side): base(position_initiale, Resources.bunker, 1, side){   }
        public override void Update(Keys key, Size gameSize)
        {
            // Ne fais rien !
        }

        protected override void OnCollision(Missile missile, int numberOfPixelsInCollision)
        {
            Bitmap bunkerBitmap = new Bitmap(this.Image, this.ObjectWidth, this.ObjectHeight);

            // Définir les rectangles pour les deux objets
            Rectangle bunkerRect = new Rectangle((int)this.Position.x, (int)this.Position.y, this.ObjectWidth, this.ObjectHeight);
            Rectangle missileRect = new Rectangle((int)missile.Position.x, (int)missile.Position.y, missile.ObjectWidth, missile.ObjectHeight);

            // Calculer l'intersection des deux rectangles
            Rectangle intersection = Rectangle.Intersect(bunkerRect, missileRect);

            if (!intersection.IsEmpty)
            {
                // Parcourir chaque pixel dans la zone de chevauchement
                for (int x = intersection.Left; x < intersection.Right; x++)
                {
                    for (int y = intersection.Top; y < intersection.Bottom; y++)
                    {
                        // Calculer les positions relatives des pixels dans le bitmap du bunker
                        int bunkerRelativeX = x - bunkerRect.Left;
                        int bunkerRelativeY = y - bunkerRect.Top;

                        // Vérifier si les positions sont dans les limites de l'image du bunker
                        if (bunkerRelativeX >= 0 && bunkerRelativeX < bunkerBitmap.Width && bunkerRelativeY >= 0 && bunkerRelativeY < bunkerBitmap.Height)
                        {
                            // Rendre le pixel à ces coordonnées transparent
                            bunkerBitmap.SetPixel(bunkerRelativeX, bunkerRelativeY, Color.Transparent);
                        }
                    }
                }
            }

            // Mettre à jour l'image du bunker
            this.Image = bunkerBitmap;

            // Décrémenter les vies du missile
            missile.Vies -= numberOfPixelsInCollision;
        }


        /* Decommenter pour le debug
        // Rectangle englobant pour débugger
        public override void Draw(Graphics graphics, int largeur, int hauteur)
        {
            base.Draw(graphics, largeur, hauteur);

            // Dessiner le rectangle englobant
            Rectangle bunkerRect = new Rectangle((int)this.Position.x, (int)this.Position.y, this.ObjectWidth, this.ObjectHeight);
            using (Pen pen = new Pen(Color.Yellow, 2))  // Couleur jaune pour le rectangle, épaisseur de 2
            {
                graphics.DrawRectangle(pen, bunkerRect);
            }
        }
        */

        // ...
    }
}

