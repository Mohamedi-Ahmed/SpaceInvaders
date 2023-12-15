using System;
using System.Drawing;
using System.Windows.Forms;
using SpaceInvaders.Properties;


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
                // Calcule la position x du missile pour qu'il soit centré par rapport au vaisseau
                double positionXMissile = Position.x + (this.ObjectWidth / 2.0) - (gameInstance.largeurImageMissile / 2.0);

                // Pour les vaisseaux ennemis, positionner le missile en bas du sprite
                double positionYMissile = Position.y + this.ObjectHeight;
                Bitmap imageMissile;

                // Créer le missile
                if (this.ObjectSide == Side.Ally)
                {
                    // Pour les vaisseaux alliés, positionner le missile en haut du sprite
                    positionYMissile = Position.y - gameInstance.hauteurImageMissile;
                    imageMissile = Resources.projectile; // Image du missile allié
                }
                else
                {
                    // Pour les vaisseaux ennemis, positionner le missile en bas du sprite
                    positionYMissile = Position.y + this.ObjectHeight;
                    imageMissile = Resources.bullet_enemies; // Image du missile ennemi
                }

                missile = new Missile(new Vecteur2D(positionXMissile, positionYMissile), imageMissile, 1, this.ObjectSide)
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




        /* Decommenter pour le debug
        // Rectangle englobant pour débugger
        public override void Draw(Graphics graphics, int largeur, int hauteur)
        {
            base.Draw(graphics, largeur, hauteur);

            // Dessiner le rectangle englobant
            Rectangle spaceshipRect = new Rectangle((int)this.Position.x, (int)this.Position.y, this.ObjectWidth, this.ObjectHeight);
            using (Pen pen = new Pen(Color.Yellow, 2))  // Couleur jaune pour le rectangle, épaisseur de 2
            {
                graphics.DrawRectangle(pen, spaceshipRect);
            }
        }
        */
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
        }
        public override void Draw(Graphics graphics, int largeur, int hauteur)
        {
            base.Draw(graphics, largeur, hauteur); // Dessiner le sprite du vaisseau

            // Appel de la méthode pour dessiner le nombre de vies
            DrawLives(graphics);
        }
        private void DrawLives(Graphics graphics)
        {
            // Position et style du texte
            PointF positionTexte = new PointF(10, 10); // Position au-dessus du vaisseau
            using (Font font = new Font("Arial", 14))
            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                string texteVies = $"Vies : {this.Vies}";
                graphics.DrawString(texteVies, font, brush, positionTexte);
            }
        }

    }
}
