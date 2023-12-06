using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Space_invaders.Properties;

namespace SpaceInvaders.GameObjects
{

    internal class Missile : SimpleObject
    {
        private double vitesse;
        public Missile(Vecteur2D position, int vies)
           : base(position, Resources.projectile, vies)
        {
            this.vitesse = 5.0;
            // Autres initialisations
        }

        public override void Update(Keys key, Size gameSize)
        {
            Position.y = Position.y - vitesse;
            if(Position.y < 0)
            { 
                Vies = 0;
                // Suppression du missile de la liste des objets une fois que Vie = 0
                var missilesASupprimer = Form1.ObjetsDuJeu.OfType<Missile>().ToList();

                foreach (var missile in missilesASupprimer)
                {
                    Form1.ObjetsDuJeu.Remove(missile);
                }
            }
            Console.WriteLine($"Position.y : {Position.y}");
        }

    }
    
}
