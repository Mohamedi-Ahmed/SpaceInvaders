using SpaceInvaders.Properties;
using SpaceInvaders;
using SpaceInvaders.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders.GameObjects
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

        private double randomShootProbability;
        private Random random = new Random();
        public EnemyBlock(int baseWidth, Vecteur2D position, Side side) : base(side)
        {
            this.baseWidth = baseWidth;
            Position = position;
            enemyShips = new HashSet<SpaceShip>();
            nbLines = 0;
            randomShootProbability = 0.01; // Valeur initiale, ajustez selon le niveau de difficulté souhaité

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
            // Calculer la position y pour la nouvelle ligne de vaisseaux
            int yPosition = (nbLines - 1) * (shipHeight + bottomSpacing);

            // Ajouter chaque vaisseau à la nouvelle ligne
            for (int i = 0; i < nbShips; i++)
            {
                // Calculer la position x du vaisseau
                int xPosition = spacing + i * (shipWidth + spacing);

                // Créer et ajouter un nouveau vaisseau
                SpaceShip newShip = new SpaceShip(new Vecteur2D(xPosition, yPosition), shipImage, nbLives, Side.Enemy)
                {
                    ObjectWidth  = shipWidth,
                    ObjectHeight = shipHeight
                };
                enemyShips.Add(newShip);
            }

            // Mise à jour de la taille du bloc si nécessaire
            UpdateSizeBasedOnShips();
        }
        private void UpdateSizeBasedOnShips()
        {
            if (enemyShips.Count == 0)
            {
                size = new Size(0, 0);
                return;
            }

            // Trouver les positions x et y maximales et minimales parmi tous les vaisseaux ennemis
            int minX = enemyShips.Min(ship => (int)ship.Position.x);
            int maxX = enemyShips.Max(ship => (int)ship.Position.x + ship.ObjectWidth);
            int minY = enemyShips.Min(ship => (int)ship.Position.y);
            int maxY = enemyShips.Max(ship => (int)ship.Position.y + ship.ObjectHeight);

            // Calculer la nouvelle largeur et hauteur du bloc en fonction de ces valeurs
            int newWidth = maxX - minX;
            int newHeight = maxY - minY;

            // Mettre à jour la taille du bloc
            size = new Size(newWidth, newHeight);

            // Mettre à jour également la position du bloc si nécessaire
            Position = new Vecteur2D(minX, minY);
        }


        double vitesseHorizontale = 3.0;
        int deplacementVertical = 10;
        double incrementVitesse = 0.2;
        public override void Update(HashSet<Keys> pressedKeys, Size gameSize)
        {
            bool changeDirection = false;

            if (Position.x <= 0 || Position.x + size.Width >= gameSize.Width)
            {
                vitesseHorizontale *= -1;
                changeDirection = true;
            }

            // Déplacer le bloc horizontalement
            Position.x += vitesseHorizontale;

            foreach (var ship in enemyShips)
            {
                ship.Position.x += vitesseHorizontale; // Déplacer chaque vaisseau horizontalement

                if (changeDirection)
                {
                    ship.Position.y += deplacementVertical; // Déplacer verticalement seulement lors du changement de direction
                }
            }

            if (changeDirection)
            {
                Position.y += deplacementVertical; // Déplacer le bloc vers le bas une seule fois par changement de direction
                vitesseHorizontale += Math.Sign(vitesseHorizontale) * incrementVitesse;
            }

            foreach (var ship in enemyShips)
            {
                if (random.NextDouble() <= randomShootProbability * 0.4)
                {
                    ship.Shoot(); 
                }
            }

            // Augmentez randomShootProbability si le bloc descend
            if (Position.y > Position.y+ deplacementVertical) // Remplacez par une condition appropriée
            {
                randomShootProbability += 0.5; // Ajustez cette valeur pour augmenter la difficulté
            }
        }


        public override void Draw(Graphics graphics, int largeur, int hauteur)
        {
                foreach (SpaceShip ship in enemyShips)
                {
                /* Decommenter pour le debug
                // Dessiner le rectangle englobant
                Rectangle shipRect = new Rectangle((int)ship.Position.x, (int)ship.Position.y, ship.ObjectWidth, ship.ObjectHeight);
                using (Pen pen = new Pen(Color.Yellow, 2))  // Couleur jaune pour le rectangle, épaisseur de 2
                {
                    graphics.DrawRectangle(pen, shipRect);
                }
                */
                graphics.DrawImage(ship.Image, (float)ship.Position.x, (float)ship.Position.y, ship.ObjectWidth, ship.ObjectHeight);
                }
        }

        public override bool IsAlive()
        {
            return enemyShips.Count > 0;

        }

        public override void Collision(Missile missile)
        {
            // Créer un rectangle englobant pour le missile
            Rectangle missileRect = new Rectangle(
                (int)missile.Position.x,
                (int)missile.Position.y,
                missile.ObjectWidth,
                missile.ObjectHeight);

            // Ensemble pour enregistrer les vaisseaux à supprimer
            HashSet<SpaceShip> toRemove = new HashSet<SpaceShip>();

            // Itérer sur chaque vaisseau ennemi pour vérifier la collision
            foreach (var vaisseau in enemyShips)
            {
                // Créer un rectangle englobant pour le vaisseau ennemi
                Rectangle shipRect = new Rectangle((int)vaisseau.Position.x, (int)vaisseau.Position.y, vaisseau.ObjectWidth, vaisseau.ObjectHeight);

                // Vérifier si les rectangles se chevauchent
                if (missileRect.IntersectsWith(shipRect))
                {
                    // Si il y a une collision, appeler la méthode Collision du vaisseau
                    vaisseau.Collision(missile);

                    // Vérifier si le vaisseau est toujours en vie après la collision
                    if (!vaisseau.IsAlive())
                    {
                        toRemove.Add(vaisseau);
                    }
                }
            }

            // Retirer les vaisseaux détruits de l'ensemble des vaisseaux ennemis
            enemyShips.RemoveWhere(v => toRemove.Contains(v));

            // Mettre à jour la taille et la position du bloc après la gestion de toutes les collisions
            UpdateBlockSizeAndPosition();
        }

        public void UpdateBlockSizeAndPosition()
        {
            if (enemyShips.Count > 0)
            {
                // Trouver les positions minimales et maximales en x et y
                int minX = enemyShips.Min(ship => (int)ship.Position.x);
                int maxX = enemyShips.Max(ship => (int)ship.Position.x + ship.ObjectWidth);
                int minY = enemyShips.Min(ship => (int)ship.Position.y);
                int maxY = enemyShips.Max(ship => (int)ship.Position.y + ship.ObjectHeight);

                // Mettre à jour la position du bloc
                Position.x = minX;
                Position.y = minY;

                // Mettre à jour la taille du bloc
                size = new Size(maxX - minX, maxY - minY);
            }
        }

        public bool ReachedPlayerLevel(double playerYPosition)
        {
            // Vérifie si l'un des vaisseaux ennemis a atteint ou est passé sous le niveau y du joueur
            return enemyShips.Any(ship => ship.Position.y  >= playerYPosition - gameInstance.hauteurImageSpaceShip );
        }


    }
}
