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
            if (playerShip != null && !playerShip.IsAlive())
            {
                state = GameState.Loose;
            }
            else if (enemies != null && !enemies.IsAlive())
            {
                state = GameState.Win;
            }
            else if (enemies != null && playerShip != null && enemies.ReachedPlayerLevel(playerShip.Position.y))
            {
                state = GameState.Loose;
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
            PointF positionTexte = new Point(0, 0);

            if (Form.ActiveForm != null)
            {
               positionTexte = new PointF((Form.ActiveForm.Width - textSize.Width) / 2, (Form.ActiveForm.Height - textSize.Height) / 2);

            }
            graphics.DrawString(message, font, Brushes.White, positionTexte);
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
        private void RestartGame(Size screenSize)
        {
            // Réinitialiser le jeu
            // ...
            state = GameState.Play;
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
            if (state != GameState.Pause)
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

            pressedKeys.Remove(e.KeyCode);
            if (e.KeyCode == Keys.Space)
            {
                playerShip.Shoot();
            }
        }
        private void GameLoop()
        {
            if (state != GameState.Play)
            {
                // Force le redessin même en pause
                gameInstance.ActiveForm?.Invalidate(); 
                return; // Ne rien faire si le jeu est en pause
            }   
            // Mise à jour
            Update(currentScreenSize);

            // Redessiner le formulaire
            gameInstance.ActiveForm?.Invalidate();
        }
    }
}
