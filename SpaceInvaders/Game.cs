using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SpaceInvaders.GameObjects;
using SpaceInvaders.Properties;

namespace SpaceInvaders
{
    internal class Game
    {
        // Mes variables de jeu
        private GameState state;
        private HashSet<GameObject> ObjetsDuJeu;
        private Keys currentKey = Keys.None;
        private Size currentScreenSize;
        private SpaceShip playerShip;
        private EnemyBlock enemies;
        private Bunker bunker;
        enum GameState { Play, Pause, WelcomeScreen, Win, Loose }

        public Game(int Width, int Height)
        {
            currentScreenSize = new Size(Width, Height);
            ObjetsDuJeu = new HashSet<GameObject>();
            InitializeGame(Width, Height);
        }

        private void InitializeGame(int Width, int Height)
        {
            state = GameState.Play;

            int positionXVaisseau = (Width - gameInstance.largeurImageSpaceShip) / 2;
            int positionYVaisseau = Height - gameInstance.hauteurImageSpaceShip;

            // Creation du vaisseau
            playerShip = new PlayerSpaceShip(new Vecteur2D(positionXVaisseau, positionYVaisseau), 3, Side.Ally)
            {
                ObjectWidth = gameInstance.largeurImageSpaceShip,
                ObjectHeight = gameInstance.hauteurImageSpaceShip
            };

            // Ajoutez le vaisseau à la liste des objets du jeu
            ObjetsDuJeu.Add(playerShip);

            //Creation de 3 bunkers
            int nbBunkers = 3;
            int espaceEntreBunkers = 300; // Espace entre les bunkers
            int margeBas = gameInstance.hauteurImageSpaceShip + 50; // Marge entre le bunker et le bas de la fenêtre
            int largeurTotaleBunkers = gameInstance.largeurImageBunker * 3 + espaceEntreBunkers * 2;
            int margeLaterale = (Width - largeurTotaleBunkers) / 2;
            margeLaterale = Math.Max(margeLaterale, 0);

            for (int i = 0; i < nbBunkers; i++)
            {
                double x = margeLaterale + i * (gameInstance.largeurImageBunker + espaceEntreBunkers);
                double y = Height - gameInstance.hauteurImageBunker - margeBas;
                bunker = new Bunker(new Vecteur2D(x, y), Side.Neutral)
                {
                    ObjectWidth = gameInstance.largeurImageBunker,
                    ObjectHeight = gameInstance.hauteurImageBunker
                };
                ObjetsDuJeu.Add(bunker);

            }

            // Creation du bloc d'ennemies
            EnemyBlock enemies = new EnemyBlock(Width, new Vecteur2D(0.0, 0.0), Side.Enemy);
            // Ajout des lignes d'ennemies
            enemies.AddLine(8, 1, Resources.alien_jaune, false);
            enemies.AddLine(3, 3, Resources.alien_bleu, true);
            enemies.AddLine(8, 1, Resources.alien_jaune, false);

            //A la fin
            ObjetsDuJeu.Add(enemies);
        }

        public void Update(Keys key, Size screenSize)
        {
            // Gestion de la pause
            if (key == Keys.P)
            {
                state = (state == GameState.Play) ? GameState.Pause : GameState.Play;
            }

            if (state == GameState.Play)
            {
                HandlePlayerInput(key);

                foreach (var gameObject in ObjetsDuJeu.ToList())
                {
                    gameObject.Update(key, screenSize);
                }

                HandleCollisions();
                CheckEndGameConditions();
            }
        }
        private void HandlePlayerInput(Keys key)
        {
            // Gestion de la pause
            if (key == Keys.P)
            {
                if (state == GameState.Play)
                {
                    state = GameState.Pause;
                }
                else if (state == GameState.Pause)
                {
                    state = GameState.Play;
                }
            }
        }

        private void CheckEndGameConditions()
        {
                // Vérifier si le joueur est mort
                if (!playerShip.IsAlive())
                {
                    state = GameState.Loose;
                }
                // Vérifier si tous les ennemis sont détruits
                else if (enemies != null && !enemies.IsAlive())
                {
                    state = GameState.Win;
                }
                /*
                // Vérifier si les ennemis ont atteint le niveau du joueur
                else if (enemies.ReachedPlayerLevel(playerShip.Position.y))
                {
                    state = GameState.Loose;
                }
                */
        }

        private void HandleCollisions()
        {
            foreach (var missile in ObjetsDuJeu.OfType<Missile>())
            {
                // Collision avec le vaisseau du joueur
                playerShip.Collision(missile);

                // Collision avec les bunkers
                foreach (var bunker in ObjetsDuJeu.OfType<Bunker>())
                {
                    bunker.Collision(missile);
                }

                // Collision avec les ennemis
                foreach (var enemy in ObjetsDuJeu.OfType<EnemyBlock>())
                {
                    enemy.Collision(missile);
                }
            }
        }

        private void DrawEndScreen(Graphics graphics, string message)
        {
            // Afficher un message de fin de jeu (Victoire ou Défaite)
            Font font = new Font("Arial", 16);
            SizeF textSize = graphics.MeasureString(message, font);
            PointF positionTexte = new PointF((Form.ActiveForm.Width - textSize.Width) / 2, (Form.ActiveForm.Height - textSize.Height) / 2);
            graphics.DrawString(message, font, Brushes.White, positionTexte);
        }

        public void Draw(Graphics graphics)
        {
            switch (state)
            {
                case GameState.Play:
                    var tempObjetsDuJeu = new List<GameObject>(ObjetsDuJeu);

                    // Dessinez ici les éléments du jeu
                    foreach (var objet in tempObjetsDuJeu)
                    {
                        objet.Draw(graphics, objet.ObjectWidth, objet.ObjectHeight);
                    }
                    break;

                case GameState.Pause:
                    string pauseText = "Pause";
                    Font pauseFont = new Font("Arial", 16);
                    SizeF textSize = graphics.MeasureString(pauseText, pauseFont);

                    // Calculer le point de départ pour centrer le texte
                    float x = (Form.ActiveForm.Width - textSize.Width) / 2;
                    float y = (Form.ActiveForm.Height - textSize.Height) / 2;

                    // Dessiner le texte centré
                    graphics.DrawString(pauseText, pauseFont, Brushes.White, new PointF(x, y));
                    break;

                case GameState.Win:
                    DrawEndScreen(graphics, "Victoire !");
                    break;

                case GameState.Loose:
                    DrawEndScreen(graphics, "Défaite !");
                    break;
            }
        }

        private void RestartGame(Size screenSize)
        {
            // Réinitialiser le jeu
            // ...
            state = GameState.Play;
        }

        public void Run()
        {
            Console.WriteLine("Début de la boucle de jeu.");
            Timer gameTimer = new Timer();
            gameTimer.Interval = 16; // 60 FPS environ
            gameTimer.Tick += (sender, e) => GameLoop();
            gameTimer.Start();
        }

        // Gestionnaires d'événements pour les touches
        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            currentKey = e.KeyCode;
        }
        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                // Appeler la méthode Shoot du vaisseau si la touche espace est relâchée
                playerShip.Shoot();
            }
        }
        private void GameLoop()
        {
            // Logique de mise à jour
            Update(currentKey, currentScreenSize);

            // Demande de redessiner le formulaire
            gameInstance.ActiveForm?.Invalidate();
        }
    }
}
