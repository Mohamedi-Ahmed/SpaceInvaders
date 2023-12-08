using System;
using System.Drawing;
using System.Windows.Forms;
using Space_invaders.Properties;


namespace SpaceInvaders.GameObjects
{
    class SpaceShip : SimpleObject
    {

        // Constructeur
        public SpaceShip(Vecteur2D position_initiale, Bitmap image, int vies_initiales, Side side)
            : base(position_initiale, image, vies_initiales, side)
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
                // Calcule la position x du missile pour qu'il soit centré par rapport au vaisseau/alien
                double positionXMissile = Position.x + this.ObjectWidth / 2 - gameInstance.largeurImageMissile / 2;

                // Calcule la position y du missile pour qu'il commence au centre du vaisseau/alien
                double positionYMissile = Position.y;

                // Choix de l'image du missile en fonction du camp
                Bitmap imageMissile = this.ObjectSide == Side.Ally ? Resources.projectile : Resources.bullet_enemies;

                // Ajuster la position Y pour les missiles ennemis pour qu'ils apparaissent en bas de l'alien
                if (this.ObjectSide == Side.Enemy)
                {
                    positionYMissile *= -1;
                    positionYMissile += gameInstance.hauteurImageMissile;
                    
                }
                else // Pour les missiles alliés, apparaissent en haut du vaisseau
                {
                    positionYMissile -= gameInstance.hauteurImageMissile / 2;
                }

                Vecteur2D positionMissile = new Vecteur2D(positionXMissile, positionYMissile);

                missile = new Missile(positionMissile, imageMissile, 1, this.ObjectSide)
                {
                    ObjectHeight = gameInstance.hauteurImageMissile,
                    ObjectWidth = gameInstance.largeurImageMissile
                };
                gameInstance.ObjetsDuJeu.Add(missile);
            }

        }
        protected override void OnCollision(Missile missile, int numberOfPixelsInCollision)
        {
            this.Vies -= 1; 
            missile.Vies = 0; 
        }
    }

    class PlayerSpaceShip : SpaceShip
    {

        // Vitesse du joueur
        private readonly double VitessePixelParSeconde = 10.0;

        public PlayerSpaceShip(Vecteur2D position_initiale, int vies_initiales, Side side) 
            : base(position_initiale, Resources.joueur, vies_initiales, side) { }   

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
