using NameSpaceGameObject;
using NameSpaceVecteur2D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Space_invaders;
using Space_invaders.Properties;

namespace NameSpaceMissile
{
    
    internal class Missile : GameObject
    {
        public Vecteur2D Position { get; set; }
        public double Vitesse {  get; set; }
        public int Vies {  get; set; }
        public Bitmap Image { get; set; }

        public Missile(Vecteur2D position, double vitesse, int vies)
        {
            Position = position;
            Vitesse = 10.0;
            Vies = vies;
            Image = Resources.projectile;
        }

        public override void Draw(Graphics graphics)
        {
            int largeur = 50; // Nouvelle largeur
            int hauteur = 50; // Nouvelle hauteur

            graphics.DrawImage(Image, (float)Position.x, (float)Position.y, largeur, hauteur);

        }

        public override void Update(Keys key, Size gameSize)
        {
            Position.y = Position.y - Vitesse;
            if(Position.y < 0) { Vies = 0; }
            Console.WriteLine($"Position.y : {Position.y}");
        }

        public override bool IsAlive()
        {
            return Vies > 0;
        }

    }
    
}
