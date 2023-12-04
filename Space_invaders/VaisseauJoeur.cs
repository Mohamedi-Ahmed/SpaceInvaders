using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using NameSpaceGameObject;
using NameSpaceVecteur2D;
using Space_invaders.Properties;
using static System.Net.Mime.MediaTypeNames;


namespace NameSpaceVaisseauJoeur
{
    class VaisseauJoeur : GameObject
    {
        // Vitesse du joueur
        private double VitessePixelParSeconde = 5.0;
        // Position du joueur (si image -> coordonnees x,y de l'angle supérieur gauche de l'image)
        public Vecteur2D Position { get; set; }

        // Propriété publique pour le nombre de vies
        public int Vies { get; set; }

        // Propriété publique pour l'image du vaisseau
        public Bitmap Image { get; private set; }

        // Constructeur
        public VaisseauJoeur(Vecteur2D position_initiale, int vies_initiales)
        {
            Position = position_initiale;
            Vies = vies_initiales;
            Image = Resources.joueur;
        }


        public override void Dessiner(Graphics graphics)
        {
            int largeur = 100; // Nouvelle largeur
            int hauteur = 100; // Nouvelle hauteur

            graphics.DrawImage(Image, (float)Position.x, (float)Position.y, largeur, hauteur);
            
        }

        public override void MaJPosition(Keys key, Size gameSize)
        {
            if (key == Keys.Left)
            {
                Position.x = Math.Max(-17, Position.x - VitessePixelParSeconde); // Empêche le vaisseau de sortir à gauche
            }
            else if (key == Keys.Right)
            {
                // TO DO : mettre 100 en variable
                Position.x = Math.Min(gameSize.Width - 100 + 17, Position.x + VitessePixelParSeconde); // Empêche le vaisseau de sortir à droite
            }
            Console.WriteLine($"Position X: {Position.x}, Limite: {gameSize.Width - Image.Width}");


        }

        public override bool EstVivant()
        {
            return Vies > 0;
        }
    }
}
