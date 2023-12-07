using System;
using System.Drawing;
using System.Windows.Forms;
using Space_invaders.Properties;


namespace SpaceInvaders.GameObjects
{
    class SpaceShip : SimpleObject
    {

        // Constructeur
        public SpaceShip(Vecteur2D position_initiale, Bitmap image, int vies_initiales)
            : base(position_initiale, image, vies_initiales)
        {
        }
        public override void Update(Keys key, Size gameSize)
        {
        }

        //Methode tirer pour le vaisseau
        private Missile missile;

        public void Shoot()
        {
            if (missile == null || !missile.IsAlive())
            {
                double positionXMissile = Position.x + gameInstance.largeurImageSpaceShip / 2 - gameInstance.largeurImageMissile / 2;
                double positionYMissile = Position.y - gameInstance.hauteurImageMissile / 2 ;

                Vecteur2D positionMissile = new Vecteur2D(positionXMissile, positionYMissile);
                missile = new Missile(positionMissile, 1);
                gameInstance.ObjetsDuJeu.Add(missile);

            }

        }
    }

    class PlayerSpaceShip : SpaceShip
    {

        // Vitesse du joueur
        private readonly double VitessePixelParSeconde = 10.0;

        public PlayerSpaceShip(Vecteur2D position_initiale, int vies_initiales) 
            : base(position_initiale, Resources.joueur, vies_initiales) { }   

        public override void Update(Keys key, Size gameSize)
        {
            if (key == Keys.Left)
            {
                Position.x = Math.Max(0 + -17, Position.x - VitessePixelParSeconde); // Empêche le vaisseau de sortir à gauche
            }
            else if (key == Keys.Right)
            {
                Position.x = Math.Min(gameSize.Width - gameInstance.largeurImageSpaceShip + 17, Position.x + VitessePixelParSeconde); // Empêche le vaisseau de sortir à droite
            }
            else if (key == Keys.Space)
            {
                Shoot();

            }
            //Console.WriteLine($"Position X: {Position.x}, Limite: {gameSize.Width - Image.Width}");
        }
    }
}
