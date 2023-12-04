using System;
using NameSpaceVaisseauJoeur;
using NameSpaceVecteur2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NameSpaceJeu
{
    internal class Jeu
    {
        private VaisseauJoeur vaisseau_joueur;
        private Jeu(VaisseauJoeur vaisseau_joueur)
        {
            int largeur = 100; // Nouvelle largeur
            int hauteur = 100; // Nouvelle hauteur

            // Calculer la position X pour centrer l'image
            int positionX = (Form.ActiveForm.Width - largeur) / 2;

            // Calculer la position Y pour placer l'image en bas de la fenêtre
            int positionY = Form.ActiveForm.Height - hauteur;

            this.vaisseau_joueur = new VaisseauJoeur(new Vecteur2D(positionX, positionY), 3);
        }
    }
}
