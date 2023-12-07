using Space_invaders.Properties;
using SpaceInvaders;
using SpaceInvaders.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Space_invaders.GameObjects
{
    public class EnemyBlock : GameObject
    {
        // Ensemble des vaisseaux du bloc
        private HashSet<SpaceShip> enemyShips;
        // Largeur du bloc à la création
        private int baseWidth;
        // Ajout du champ nbLines
        private int nbLines; 

        // Taille du bloc (largeur, hauteur), adaptée au fur et à mesure du jeu.
        public Size size {get; set;}
        // Coin sup. gauche du bloc
        public Vecteur2D Position { get; set;}
        public EnemyBlock(int baseWidth, Vecteur2D position)
        {
            this.baseWidth = baseWidth;
            Position = position;
            enemyShips = new HashSet<SpaceShip>();
            nbLines = 0;
        }

        public void AddLine(int nbShips, int nbLives, Bitmap shipImage, bool isBigEnnemy)
        {
            nbLines++;

            // Choisir la taille appropriée en fonction du type d'ennemi
            int shipWidth  = isBigEnnemy ? gameInstance.largeurImageGrandEnnemie : gameInstance.largeurImagePetitEnnemie;
            int shipHeight = isBigEnnemy ? gameInstance.hauteurImageGrandEnnemie : gameInstance.hauteurImagePetitEnnemie;

            // Calculer l'espacement entre les vaisseaux
            int totalSpace = baseWidth - nbShips * shipWidth;
            int spacing = totalSpace / (nbShips + 1);
            int bottomSpacing = 10;

            // Determiner dimensions du bloc
            int newWidth = size.Width;
            int newHeight = size.Height;

            int totalShipsWidth = (nbShips * shipWidth) + (nbShips - 1) * spacing;
            int totalShipsHeight = nbLines * shipHeight + (nbLines - 1) * bottomSpacing;

            if (newWidth  < totalShipsWidth) {newWidth  = totalShipsWidth; }
            if (newHeight < totalShipsHeight){newHeight = totalShipsHeight; }

            // Déterminer la position y pour la nouvelle ligne
            int yPosition = 0;
            if (enemyShips.Count > 0)
            {
                // Si il y a déjà des vaisseaux, positionner la nouvelle ligne en dessous de la dernière
                yPosition = enemyShips.Max(ship => (int)ship.Position.y) + shipHeight + bottomSpacing;
            }

            // Ajouter chaque vaisseau à la nouvelle ligne
            for (int i = 0; i < nbShips; i++)
            {
                // Calculer la position x du vaisseau
                int xPosition = spacing + i * (shipWidth + spacing);

                // Créer et ajouter un nouveau vaisseau
                SpaceShip newShip = new SpaceShip(new Vecteur2D(xPosition, yPosition), shipImage, nbLives);
                enemyShips.Add(newShip);
            }

            // Mise à jour de la taille du bloc
            UpdateSize(newWidth, newHeight);
        }
        public void UpdateSize(int maxWidth, int maxHeight) 
        {
            size = new Size(maxWidth, maxHeight);
        }
        double vitesseHorizontale = 5.0;
        int deplacementVertical = 10;
        double incrementVitesse = -2.0;
        public override void Update(Keys key, Size gameSize)
        {
            bool changeDirection = false;

            Console.WriteLine($"Position.x  : {Position.x} -Form.ActiveForm.Width : {-Form.ActiveForm.Width} || size.Width : {size.Width}");
            if (Position.x  <= -Form.ActiveForm.Width || Position.x  >= size.Width)
            {
                // Inverser la direction
                vitesseHorizontale *= - 1;
                changeDirection = true;

                // Décaler le bloc vers le bas
                Position.y += deplacementVertical;

                // Augmenter la vitesse horizontale
                //vitesseHorizontale += incrementVitesse;
            }
            if (changeDirection)
            {
                foreach (var ship in enemyShips)
                {
                   ship.Position.y += deplacementVertical;
                }
            }
            // Déplacer le bloc horizontalement
            foreach (var ship in enemyShips)
            {
                ship.Position.x += vitesseHorizontale;
                Position.x += vitesseHorizontale;
            }
        }

        public override void Draw(Graphics graphics, int largeur, int hauteur)
        {
                foreach (SpaceShip ship in enemyShips)
                {
                    graphics.DrawImage(ship.Image, (float)ship.Position.x, (float)ship.Position.y, gameInstance.largeurImagePetitEnnemie, gameInstance.hauteurImagePetitEnnemie);
                }
        }

        public override bool IsAlive()
        {

            //return enemyShips.Count > 0;
            foreach (SpaceShip ship in enemyShips)
            {
                if(ship.Vies>0){ return true; }

            }
            return false;
        }
    

        public override void Collision(Missile m)
        {
        }
    }
}
