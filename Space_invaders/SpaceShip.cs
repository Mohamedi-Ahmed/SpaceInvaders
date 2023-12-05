using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using NameSpaceGameObject;
using NameSpaceMissile;
using NameSpaceVecteur2D;
using Space_invaders.Properties;
using NameSpaceGame;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using NameSpaceGame;
using Space_invaders;


namespace NameSpaceSpaceShip
{
    class SpaceShip : GameObject
    {
        // Vitesse du joueur
        private readonly double VitessePixelParSeconde = 5.0;

        // Position du joueur (si image -> coordonnees x,y de l'angle supérieur gauche de l'image)
        public Vecteur2D Position { get; set; }

        // Propriété publique pour le nombre de vies
        public int Vies { get; set; }

        // Propriété publique pour l'image du vaisseau
        public Bitmap Image { get; private set; }

        // Constructeur
        public SpaceShip(Vecteur2D position_initiale, int vies_initiales)
        {
            Position = position_initiale;
            Vies = vies_initiales;
            Image = Resources.joueur;
        }


        public override void Draw(Graphics graphics)
        {
            int largeur = 100; // Nouvelle largeur
            int hauteur = 100; // Nouvelle hauteur

            graphics.DrawImage(Image, (float)Position.x, (float)Position.y, largeur, hauteur);
            
        }

        public override void Update(Keys key, Size gameSize)
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
            else if (key == Keys.Space)
            {
                Shoot();

            }
            //Console.WriteLine($"Position X: {Position.x}, Limite: {gameSize.Width - Image.Width}");


        }

        public override bool IsAlive()
        {
            return Vies > 0 && Position.y > 0;
        }

        //Methode tirer pour le vaisseau
        private Missile missile;

        public void Shoot()
        {
            if (missile == null || !missile.IsAlive())
            {
                int widthImage = 50;
                int heightImage = 50;
                Console.WriteLine("Je suis ici");
                Vecteur2D positionMissile = new Vecteur2D(Position.x + widthImage / 2, Position.y - heightImage / 2);
                missile = new Missile(positionMissile, 10.0, 1);
                Form1.ObjetsDuJeu.Add(missile);

            }

        }
    }
}
