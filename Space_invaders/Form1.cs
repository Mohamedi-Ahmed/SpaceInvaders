using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NameSpaceJeu;
using NameSpaceVaisseauJoeur;
using NameSpaceVecteur2D;

namespace Space_invaders
{
    public partial class Form1 : Form
    {
        // Mes labels
        private Label labelScore;
        private Label labelVies;
        // Mes boutons
        private Button BoutonCommencer;
        private VaisseauJoeur vaisseau;


        public Form1()
        {
            InitializeComponent();

            // Connexion la méthode "Form1_Paint" à l'evenement Paint 
            this.Paint += new PaintEventHandler(Form1_Paint);

            this.KeyDown += new KeyEventHandler(BoutonPresse);
            this.KeyPreview = true;


            // Désactiver le redimensionnement
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // Modifier l'image de fond

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

            // Labels
            labelScore = new Label
            {
                Text = "Score: 0",
                AutoSize = true,
                Location = new Point(10, 10)
            };

            labelVies = new Label
            {
                Text = "Vie : 3",
                AutoSize = true
            };

            // Ajout des labels au Form
            this.Controls.Add(labelScore);
            this.Controls.Add(labelVies);

            // Positionner le labelVies après son ajout au Form
            PositionLabelInTopRightCorner();



            int largeur = 100; // Nouvelle largeur
            int hauteur = 100; // Nouvelle hauteur

            // Calculer la position X pour centrer l'image
            int positionX = (this.Width - largeur) / 2;

            // Calculer la position Y pour placer l'image en bas de la fenêtre
            int positionY = this.Height - hauteur;

            vaisseau = new VaisseauJoeur(new Vecteur2D(positionX, positionY), 3);
            this.Invalidate(); // Force le formulaire à se redessiner


            // Boutons
            /*
            BoutonCommencer = new Button
            {
                Text = "Commencer",
                AutoSize = true,
            };
            BoutonCommencer.Location = new Point(this.ClientSize.Width  / 2 - BoutonCommencer.Width / 2,
                                                 this.ClientSize.Height / 2 - BoutonCommencer.Height / 2);
            this.Controls.Add(BoutonCommencer);
            */
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            PositionLabelInTopRightCorner();
        }

        private void PositionLabelInTopRightCorner()
        {
            if (labelVies != null)
            {
                labelVies.Location = new Point(this.ClientSize.Width - labelVies.Width - 10, 10);
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (vaisseau != null)
            {
                vaisseau.Dessiner(e.Graphics);
            }
        }


        private void BoutonPresse(object sender, KeyEventArgs e)
        {
            
            vaisseau.MaJPosition(e.KeyCode, this.ClientSize);
            this.Invalidate();
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Remplacez 'MonFormulaire' par le nom de votre classe de formulaire
            Application.Run(new Form1());
        }
    }
}
