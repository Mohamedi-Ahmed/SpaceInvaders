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
        private Size currentScreenSize;
        private SpaceShip playerShip;
        private EnemyBlock enemies;
        private Bunker bunker;
        enum GameState { Play, Pause, WelcomeScreen, Win, Loose }
        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();

        // Gestion de la pause
        private bool showResumeMessage = true;
        private DateTime lastToggle = DateTime.Now;

        // Gestion de la fin de jeu
        private bool showRestartMessage = true;
        private DateTime endGameTime;
        private bool canRestart = false;
        private const int RestartDelayInSeconds = 2;

        public Game(int Width, int Height)
        {
            currentScreenSize = new Size(Width, Height);
            InitializeGame(Width, Height);
        }

        private void InitializeGame(int Width, int Height)
        {
            state = GameState.Play;

            int shipXPosition = (Width - gameInstance.spaceShipImageWidth) / 2;
            int shipYPosition = Height - gameInstance.spaceShipImageHeight;

            // Creation du vaisseau
            playerShip = new PlayerSpaceShip(new Vector2D(shipXPosition, shipYPosition), 3, Side.Ally)
            {
                ObjectWidth  = gameInstance.spaceShipImageWidth,
                ObjectHeight = gameInstance.spaceShipImageHeight
            };

            // Ajoutez le vaisseau à la liste des objets du jeu
            gameInstance.GameObjects.Add(playerShip);

            //Creation de 3 bunkers
            int nbBunkers = 3;
            int spaceBetweenBunkers = 300; // Espace entre les bunkers
            int bottomMargin = gameInstance.spaceShipImageHeight + 50; // Marge entre le bunker et le bas de la fenêtre
            int bunkersTotalWidth = gameInstance.bunkerImageWidth * 3 + spaceBetweenBunkers * 2;
            int lateralMargin = (Width - bunkersTotalWidth) / 2;
            _ = Math.Max(lateralMargin, 0);

            for (int i = 0; i < nbBunkers; i++)
            {
                double x = lateralMargin + i * (gameInstance.bunkerImageWidth + spaceBetweenBunkers);
                double y = Height - gameInstance.bunkerImageHeight - bottomMargin;
                bunker = new Bunker(new Vector2D(x, y), Side.Neutral)
                {
                    ObjectWidth  = gameInstance.bunkerImageWidth,
                    ObjectHeight = gameInstance.bunkerImageHeight
                };
                gameInstance.GameObjects.Add(bunker);

            }

            // Creation du bloc d'ennemies
            enemies = new EnemyBlock(Width, new Vector2D(0.0, 0.0), Side.Enemy);
            // Ajout des lignes d'ennemies
            enemies.AddLine(8, 1, Resources.alienYellow, false);
            enemies.AddLine(3, 3, Resources.alienBlue  , true );
            enemies.AddLine(8, 1, Resources.alienYellow, false);

            gameInstance.GameObjects.Add(enemies);
        }

        public void Update(Size screenSize)
        {
            if (state == GameState.Play)
            {
                foreach (var gameObject in gameInstance.GameObjects.ToList())
                {
                    gameObject.Update(pressedKeys, screenSize);
                }

                HandleCollisions();
                CheckEndGameConditions();
            }
        }
        private void CheckEndGameConditions()
        {
            if ((playerShip != null && !playerShip.IsAlive()) || //Defaite 
                (enemies != null && !enemies.IsAlive()) || //Victoire
                (enemies != null && playerShip != null && enemies.ReachedPlayerLevel(playerShip.Position.y))) // Defaite
            {
                state = playerShip.IsAlive() && enemies != null && !enemies.IsAlive() ? GameState.Win : GameState.Loose;
                endGameTime = DateTime.Now;
                canRestart = false;
            }
        }

        private void HandleCollisions()
        {
            foreach (var missile in gameInstance.GameObjects.OfType<Missile>())
            {
                // Collision avec le vaisseau du joueur
                playerShip.Collision(missile);

                // Collision avec les bunkers
                foreach (var bunker in gameInstance.GameObjects.OfType<Bunker>())
                {
                    bunker.Collision(missile);
                }

                // Collision avec les ennemis
                foreach (var enemy in gameInstance.GameObjects.OfType<EnemyBlock>())
                {
                    enemy.Collision(missile);
                }
            }
        }

        private void DrawScreen(Graphics graphics, string message)
        {
            Font font = new Font("Arial", 16);
            SizeF textSize = graphics.MeasureString(message, font);
            PointF positionTexte = new PointF((currentScreenSize.Width - textSize.Width) / 2, (currentScreenSize.Height - textSize.Height) / 2);

            // Message principal
            graphics.DrawString(message, font, Brushes.White, positionTexte);

            if ((state == GameState.Pause && showResumeMessage) || (state == GameState.Win || state == GameState.Loose) && showRestartMessage)
            {
                string additionalMessage = state == GameState.Pause ? "Press P button to resume game" : "Press Spacebar to restart the game";
                SizeF additionalTextSize = graphics.MeasureString(additionalMessage, font);
                PointF additionalPosition = new PointF((currentScreenSize.Width - additionalTextSize.Width) / 2, positionTexte.Y + 50);
                graphics.DrawString(additionalMessage, font, Brushes.White, additionalPosition);
            }

            // Maj de l'état de clignotement toutes les 500 ms
            if ((DateTime.Now - lastToggle).TotalMilliseconds > 500)
            {
                if (state == GameState.Pause)
                {
                    showResumeMessage = !showResumeMessage;
                }
                else if (state == GameState.Win || state == GameState.Loose)
                {
                    showRestartMessage = !showRestartMessage;
                }
                lastToggle = DateTime.Now;
            }
        }


        public void Draw(Graphics graphics)
        {
            switch (state)
            {
                case GameState.Play:
                    var tempGameObjects = new List<GameObject>(gameInstance.GameObjects);

                    // Dessinez ici les éléments du jeu
                    foreach (var objet in tempGameObjects)
                    {
                        objet.Draw(graphics, objet.ObjectWidth, objet.ObjectHeight);
                    }
                    break;

                case GameState.Pause:
                    DrawScreen(graphics, "Pause !");
                    break;

                case GameState.Win:
                    DrawScreen(graphics, "Victory !");
                    break;

                case GameState.Loose:
                    DrawScreen(graphics, "Loose !");
                    break;
            }
        }
        private void RestartGame()
        {
            gameInstance.GameObjects.Clear();
            pressedKeys.Clear(); 
            state = GameState.Play;
            canRestart = false;
            InitializeGame(currentScreenSize.Width, currentScreenSize.Height);

        }

        public void Run()
        {
            //Console.WriteLine("Début de la boucle de jeu.");
            Timer gameTimer = new Timer();
            gameTimer.Interval = 16; // 60 FPS environ
            gameTimer.Tick += (sender, e) => GameLoop();
            gameTimer.Start();
        }

        public void PauseGame()
        {
            if (state == GameState.Play)
            {
                state = GameState.Pause;
            }
        }

        public void ResumeGame()
        {
            if (state == GameState.Pause)
            {
                state = GameState.Play;
            }
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            //Console.WriteLine($"KeyDown: {e.KeyCode}");

            if (e.KeyCode == Keys.P)
            {
                if (state == GameState.Play)
                {
                    PauseGame();
                }
                else if (state == GameState.Pause)
                {
                    ResumeGame();
                }
            }else 
            {
                // Ajoutez la touche appuyée à l'ensemble des touches pressées
                pressedKeys.Add(e.KeyCode);
            }
        }
        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            //Console.WriteLine($"KeyUp: {e.KeyCode}");

            if (e.KeyCode == Keys.Space && (state == GameState.Win || state == GameState.Loose) && canRestart)
            {
                RestartGame();
                return;
            }

            pressedKeys.Remove(e.KeyCode);
            if (e.KeyCode == Keys.Space && state == GameState.Play)
            {
                playerShip.Shoot();
            }
        }
        private void GameLoop()
        {
            switch (state)
            {
                case GameState.Play:
                    Update(currentScreenSize);
                    break;

                case GameState.Pause:
                    break;

                case GameState.Win:
                case GameState.Loose:
                    if (!canRestart && (DateTime.Now - endGameTime).TotalSeconds > RestartDelayInSeconds)
                    {
                        canRestart = true;
                    }
                    break;
            }

            // Redessiner le formulaire
            gameInstance.ActiveForm?.Invalidate();
        }
    }
}
