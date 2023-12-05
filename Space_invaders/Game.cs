using System;
using NameSpaceVecteur2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NameSpaceSpaceShip;
using NameSpaceGameObject;
using NameSpaceMissile;
using Space_invaders;

namespace NameSpaceGame
{
    internal class Game
    {
        private SpaceShip playerShip;
        public Game(int Width, int Height)
        {
            int largeurSpaceShip = 100;
            int hauteurSpaceShip = 100;

            int positionX = (Width - largeurSpaceShip) / 2;
            int positionY = Height - hauteurSpaceShip;

            playerShip = new SpaceShip(new Vecteur2D(positionX, positionY), 3);
            Console.WriteLine("IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII");
            Console.WriteLine(playerShip);
            // Ajoutez le vaisseau à la liste des objets du jeu
            Form1.ObjetsDuJeu.Add(playerShip);
            

        }
    }
}
