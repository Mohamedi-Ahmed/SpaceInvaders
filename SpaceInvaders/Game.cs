using System;
using SpaceInvaders.GameObjects;
using System.Windows.Forms;
using System.Drawing;
using SpaceInvaders;
using Space_invaders.GameObjects;

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
                foreach (var objet in Form1.ObjetsDuJeu)
                {
                    if (objet.GetType() == typeof(Missile))
                    {

                        objet.Draw(graphics, Form1.largeurImageMissile, Form1.hauteurImageMissile);
                    }
                    else if (objet.GetType() == typeof(SpaceShip))
                    {

                        objet.Draw(graphics, Form1.largeurImageSpaceShip, Form1.hauteurImageSpaceShip);
                    }
                    else if(objet.GetType() == typeof(Bunker))
                    {
                        objet.Draw(graphics, Form1.largeurImageBunker, Form1.hauteurImageBunker);

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
                /* Autre méthode 
                     // Créer une copie temporaire de la liste pour l'itération
                    var tempObjetsDuJeu = new List<GameObject>(ObjetsDuJeu);

                    foreach (var gameObject in tempObjetsDuJeu)
                    {
                        gameObject.Update(e.KeyCode, this.ClientSize);
                    }
                 */

                for (int i = 0; i < Form1.ObjetsDuJeu.Count; i++)
                {
                    Form1.ObjetsDuJeu[i].Update(key, screenSize);
                }

            }
        }


        public Game(int Width, int Height)
        {
            int positionXVaisseau = (Width - Form1.largeurImageSpaceShip) / 2;
            int positionYVaisseau = Height - Form1.hauteurImageSpaceShip;

            // Creation du vaisseau
            playerShip = new SpaceShip(new Vecteur2D(positionXVaisseau, positionYVaisseau), 3);

            // Ajoutez le vaisseau à la liste des objets du jeu
            Form1.ObjetsDuJeu.Add(playerShip);

            state = GameState.Play;
            Console.WriteLine($"First state : {state}");

            //Creation de 3 bunkers
            int nbBunkers = 3;
            int espaceEntreBunkers = 300; // Espace entre les bunkers
            int margeBas = Form1.hauteurImageSpaceShip + 50; // Marge entre le bunker et le bas de la fenêtre
            int largeurTotaleBunkers = Form1.largeurImageBunker * 3 + espaceEntreBunkers * 2;
            int margeLaterale = (Width - largeurTotaleBunkers) / 2;
            margeLaterale = Math.Max(margeLaterale, 0);

            for (int i = 0; i < nbBunkers; i++)
            {
                double x = margeLaterale + i * (Form1.largeurImageBunker + espaceEntreBunkers);
                double y = Height - Form1.hauteurImageBunker - margeBas; // margeBas est la marge entre le bunker et le bas de la fenêtre
                Form1.ObjetsDuJeu.Add(new Bunker(new Vecteur2D(x, y)));

            }



        }
    }
}
