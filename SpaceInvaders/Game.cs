using System;
using SpaceInvaders.GameObjects;
using System.Windows.Forms;
using System.Drawing;
using SpaceInvaders;

namespace SpaceInvaders
{
    internal class Game
    {
        private GameState state;
        private SpaceShip playerShip;
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
            int positionX = (Width - Form1.largeurImageSpaceShip) / 2;
            int positionY = Height - Form1.hauteurImageSpaceShip;

            playerShip = new SpaceShip(new Vecteur2D(positionX, positionY), 3);

            // Ajoutez le vaisseau à la liste des objets du jeu
            Form1.ObjetsDuJeu.Add(playerShip);

            state = GameState.Play;
            Console.WriteLine($"First state : {state}");



        }
    }
}
