using System;
using System.Drawing;
using System.Windows.Forms;
using Space_invaders.Properties;


namespace SpaceInvaders.GameObjects
{
    class SpaceShip : SimpleObject
    {
        // Vitesse du joueur
        private readonly double VitessePixelParSeconde = 5.0;


        // Constructeur

        public SpaceShip(Vecteur2D position_initiale, int vies_initiales)
            : base(position_initiale, Resources.joueur, vies_initiales)
        {
            // Propriétés spécifiques à SpaceShip
        }

        public override void Update(Keys key, Size gameSize)
        {
            if (key == Keys.Left)
            {
                Position.x = Math.Max(-17, Position.x - VitessePixelParSeconde); // Empêche le vaisseau de sortir à gauche
            }
            else if (key == Keys.Right)
            {
                Position.x = Math.Min(gameSize.Width - Form1.largeurImageSpaceShip + 17, Position.x + VitessePixelParSeconde); // Empêche le vaisseau de sortir à droite
            }
            else if (key == Keys.Space)
            {
                Shoot();

            }
            //Console.WriteLine($"Position X: {Position.x}, Limite: {gameSize.Width - Image.Width}");
        }

        //Methode tirer pour le vaisseau
        private Missile missile;

        public void Shoot()
        {
            if (missile == null || !missile.IsAlive())
            {
                Vecteur2D positionMissile = new Vecteur2D(Position.x + Form1.largeurImageMissile / 2, Position.y - Form1.hauteurImageMissile / 2);
                missile = new Missile(positionMissile, 1);
                Form1.ObjetsDuJeu.Add(missile);

            }

        }
    }
}
