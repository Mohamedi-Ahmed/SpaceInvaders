using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Space_invaders
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Désactiver le redimensionnement
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            
            // Centrer l'écran de jeu
            this.StartPosition = FormStartPosition.CenterScreen;
            
            // Modifier la taille de la fenêtre de jeu
            StartPosition = FormStartPosition.Manual;
            Rectangle screen = Screen.FromPoint(Cursor.Position).WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            Location = new Point(screen.Left + (screen.Width - w) / 2, screen.Top + (screen.Height - h) / 2);
            Size = new Size(w, h);

            // Modifier le titre de l'écran de jeu
            this.Text = "SPACE INVADERS !";

            // A FAIRE ! : MODIFIER L'ICONE + BACKGROUND COLOR + AJOUTER UN ECRAN D ACCEUIL
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
