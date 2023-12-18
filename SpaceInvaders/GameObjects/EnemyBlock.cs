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
                SpaceShip newShip = new SpaceShip(new Vecteur2D(xPosition, yPosition), shipImage, nbLives, Side.Enemy)
                {
                    ObjectWidth  = shipWidth,
                    ObjectHeight = shipHeight
                };
                enemyShips.Add(newShip);
            }

            // Mise à jour de la taille du bloc
            UpdateSize(newWidth, newHeight);
        }
        public void UpdateSize(int maxWidth, int maxHeight) 
        {
            size = new Size(maxWidth, maxHeight);
        }


        double vitesseHorizontale = 1.5;
        int deplacementVertical = 10;
        double incrementVitesse = 0.2;
        public override void Update(HashSet<Keys> pressedKeys, Size gameSize)
        {
            bool changeDirection = false;

            if (Position.x  <= -size.Width || Position.x  >= size.Width)
            {
                // Inverser la direction
                vitesseHorizontale *= - 1;
                changeDirection = true;

                // Décaler le bloc vers le bas
                Position.y += deplacementVertical;

                // Augmenter la vitesse horizontale
                vitesseHorizontale += Math.Sign(vitesseHorizontale) * incrementVitesse;
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
            foreach (var vaisseau in toRemove)
            {
                enemyShips.Remove(vaisseau);
            }
        }

    }
}
