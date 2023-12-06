using System;
using SpaceInvaders.GameObjects;
using System.Windows.Forms;
using System.Drawing;
using SpaceInvaders;
using Space_invaders.GameObjects;
using System.Linq;

namespace SpaceInvaders
{
    internal class Game
    {
        // Mes variables de jeu
        private GameState state;
        private SpaceShip playerShip;
        private Bunker bunker1;
        private Bunker bunker2;
        private Bunker bunker3;

        enum GameState { Play, Pause, WelcomeScreen, Win, Loose }

        public void AddNewGameObject()
        {
            throw new NotImplementedException();
        }

        public void Draw(Graphics graphics)
        {
            if (state == GameState.Pause)
            {
                string pauseText = "Pause";
                Font pauseFont = new Font("Arial", 16);
                SizeF textSize = graphics.MeasureString(pauseText, pauseFont);

                // Calculer le point de départ pour centrer le texte
                float x = (Form.ActiveForm.Width - textSize.Width) / 2;
                float y = (Form.ActiveForm.Height - textSize.Height) / 2;

                // Dessiner le texte centré
                graphics.DrawString(pauseText, pauseFont, Brushes.White, new PointF(x, y));
            }

            else if (state == GameState.Play)
            {
                // Dessinez ici les éléments du jeu
                foreach (var objet in gameInstance.ObjetsDuJeu)
                {
                    if (objet.GetType() == typeof(Missile))
                    {

                        objet.Draw(graphics, gameInstance.largeurImageMissile, gameInstance.hauteurImageMissile);
                    }
                    else if (objet.GetType() == typeof(SpaceShip))
                    {

                        objet.Draw(graphics, gameInstance.largeurImageSpaceShip, gameInstance.hauteurImageSpaceShip);
                    }
                    else if(objet.GetType() == typeof(Bunker))
                    {
                        objet.Draw(graphics, gameInstance.largeurImageBunker, gameInstance.hauteurImageBunker);

                    }

                }
            }
        }

        public void ReleaseKey()
        {
            throw new NotImplementedException();
        }

        public void Update(Keys key, System.Drawing.Size screenSize)
        {
            if (key == Keys.P)
            {
                Console.WriteLine("J'appuie sur P");
                if (state == GameState.Play)
                {
                    state = GameState.Pause;
                    Console.WriteLine("Game Paused"); // Ajout pour le débogage

                }
                else if (state == GameState.Pause)
                {
                    state = GameState.Play;
                    Console.WriteLine("Game Resumed"); // Ajout pour le débogage

                }
            }

            if (state == GameState.Play)
            {
                for (int i = 0; i < gameInstance.ObjetsDuJeu.Count; i++)
                {
                    gameInstance.ObjetsDuJeu[i].Update(key, screenSize);
                    if (gameInstance.ObjetsDuJeu[i] is Missile missile)
                    {
                        // Vérifiez la collision entre le missile et chaque bunker
                        foreach (var bunker in gameInstance.ObjetsDuJeu.OfType<Bunker>())
                        {
                            bunker.Collision(missile);

                        }
                    }
                }
                // Supprimer les missiles détruits après les mises à jour
                gameInstance.ObjetsDuJeu.RemoveAll(objet => objet is Missile missile && missile.Vies <= 0);

            }
        }


        public Game(int Width, int Height)
        {
            int positionXVaisseau = (Width - gameInstance.largeurImageSpaceShip) / 2;
            int positionYVaisseau = Height - gameInstance.hauteurImageSpaceShip;

            // Creation du vaisseau
            playerShip = new SpaceShip(new Vecteur2D(positionXVaisseau, positionYVaisseau), 3);

            // Ajoutez le vaisseau à la liste des objets du jeu
            gameInstance.ObjetsDuJeu.Add(playerShip);

            state = GameState.Play;
            Console.WriteLine($"First state : {state}");

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
                double y = Height - gameInstance.hauteurImageBunker - margeBas; // margeBas est la marge entre le bunker et le bas de la fenêtre
                Console.WriteLine($"Bunker {i} | Position x : {x}, y : {y}");
                gameInstance.ObjetsDuJeu.Add(new Bunker(new Vecteur2D(x, y)));

            }



        }
    }
}
