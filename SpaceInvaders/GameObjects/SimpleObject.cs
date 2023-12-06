using System.Drawing;

namespace SpaceInvaders.GameObjects
{
    public abstract class SimpleObject : GameObject
    {
        // Position du joueur (si image -> coordonnees x,y de l'angle supérieur gauche de l'image)
        public Vecteur2D Position { get; set; }

        // Propriété publique pour le nombre de vies
        public int Vies { get; set; }

        // Propriété  pour l'image de l'objet
        protected Bitmap Image;

        protected SimpleObject(Vecteur2D position, Bitmap image, int vies)
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
    }
}
