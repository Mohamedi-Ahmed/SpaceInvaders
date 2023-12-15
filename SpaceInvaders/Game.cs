using System;
using SpaceInvaders.GameObjects;
using System.Windows.Forms;
using System.Drawing;
using SpaceInvaders;
using System.Linq;
using System.Collections.Generic;
using SpaceInvaders.Properties;
using System.Reflection;

namespace SpaceInvaders
{
    internal class Game
    {
        // Mes variables de jeu
        private GameState state;
        private SpaceShip playerShip;
        private EnemyBlock enemies;
        private Bunker bunker;
        enum GameState { Play, Pause, WelcomeScreen, Win, Loose }

        public void AddNewGameObject()
        {
            throw new NotImplementedException();
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
                    var tempObjetsDuJeu = new List<GameObject>(gameInstance.ObjetsDuJeu);

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

        private bool isPauseKeyPressed = false;

        public void Update(Keys key, Size screenSize)
        {
            bool pKeyPressedNow = key == Keys.P;

            // Gestion de la pause
            if (pKeyPressedNow && !isPauseKeyPressed)
            {
                if (state == GameState.Play)
                {
                    state = GameState.Pause;
                }
                else if (state == GameState.Pause)
                {
                    state = GameState.Play;
                }
                isPauseKeyPressed = true;
            }
            else if (!pKeyPressedNow)
            {
                isPauseKeyPressed = false;
            }

            // Mise à jour des objets du jeu seulement si le jeu n'est pas en pause
            if (state == GameState.Play)
            {
                var tempObjetsDuJeu = new List<GameObject>(gameInstance.ObjetsDuJeu);

                foreach (var gameObject in tempObjetsDuJeu)
                {
                    gameObject.Update(key, screenSize);
                }

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
                // Gérer les collisions
                HandleCollisions();

                // Supprimer les objets détruits après les mises à jour
                gameInstance.ObjetsDuJeu.RemoveWhere(objet => !objet.IsAlive());
            }

            // Gestion des états de victoire et de défaite
            if (state == GameState.Win || state == GameState.Loose)
            {
                if (key == Keys.Space)
                {
                    // Implémentez cette méthode pour redémarrer le jeu
                    //RestartGame(screenSize); 
                }
            }
        }

        private void HandleCollisions()
        {
            foreach (var missile in gameInstance.ObjetsDuJeu.OfType<Missile>())
            {
                // Collision avec le vaisseau du joueur
                playerShip.Collision(missile);

                // Collision avec les bunkers
                foreach (var bunker in gameInstance.ObjetsDuJeu.OfType<Bunker>())
                {
                    bunker.Collision(missile);
                }

                // Collision avec les ennemis
                foreach (var enemy in gameInstance.ObjetsDuJeu.OfType<EnemyBlock>())
                {
                    enemy.Collision(missile);
                }
            }
        }

        private void RestartGame(Size screenSize)
        {
            // Réinitialiser le jeu
            // ...
            state = GameState.Play;
        }

        /*
        public void Update(Keys key, Size screenSize)
        {
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

            if (state == GameState.Play)
            {
                var tempObjetsDuJeu = new List<GameObject>(gameInstance.ObjetsDuJeu);

                foreach (var gameObject in tempObjetsDuJeu)
                {
                    gameObject.Update(key, screenSize);

                    if (gameObject is Missile missile)
                    {
                            // Collision avec le vaisseau du joueur
                            playerShip.Collision(missile);

                            // Collision avec les bunkers
                            foreach (var bunker in gameInstance.ObjetsDuJeu.OfType<Bunker>())
                            {
                                    bunker.Collision(missile);
                            }

                            // Collision avec les ennemis
                            foreach (var enemy in gameInstance.ObjetsDuJeu.OfType<EnemyBlock>())
                            {
                                    enemy.Collision(missile);
                            }
                        
                    }
                }
                // Supprimer les objets détruits après les mises à jour
                gameInstance.ObjetsDuJeu.RemoveWhere(objet => !objet.IsAlive());



            }
        }
        */

        public Game(int Width, int Height)
        {
            //////////////////////////////////////////////// MENU COMMENCER ////////////////////////////////////////////////


            //////////////////////////////////////////////// INITIALISATION ////////////////////////////////////////////////
            int positionXVaisseau = (Width - gameInstance.largeurImageSpaceShip) / 2;
            int positionYVaisseau = Height - gameInstance.hauteurImageSpaceShip;

            // Creation du vaisseau
            playerShip = new PlayerSpaceShip(new Vecteur2D(positionXVaisseau, positionYVaisseau), 3, Side.Ally)
            {
                ObjectWidth  = gameInstance.largeurImageSpaceShip,
                ObjectHeight = gameInstance.hauteurImageSpaceShip
            };

            // Ajoutez le vaisseau à la liste des objets du jeu
            gameInstance.ObjetsDuJeu.Add(playerShip);
            state = GameState.Play;

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
                    ObjectWidth  = gameInstance.largeurImageBunker,
                    ObjectHeight = gameInstance.hauteurImageBunker
                };
                gameInstance.ObjetsDuJeu.Add(bunker);

            }

            // Creation du bloc d'ennemies
            EnemyBlock enemies = new EnemyBlock(Width, new Vecteur2D(0.0, 0.0), Side.Enemy);
            // Ajout des lignes d'ennemies
            enemies.AddLine(8, 1, Resources.alien_jaune, false);
            enemies.AddLine(3, 3, Resources.alien_bleu, true);
            enemies.AddLine(8, 1, Resources.alien_jaune, false);

            //A la fin
            gameInstance.ObjetsDuJeu.Add(enemies);


            //////////////////////////////////////////////// BOUCLE DE JEU ////////////////////////////////////////////////


            //////////////////////////////////////////////// ECRAN DE FIN ////////////////////////////////////////////////


        }
    }
}
