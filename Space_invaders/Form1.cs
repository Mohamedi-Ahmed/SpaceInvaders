using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NameSpaceGameObject;
using NameSpaceGame;
using NameSpaceMissile;
using NameSpaceSpaceShip;
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

        //Mon jeu
        private Game game;
        public static List<GameObject> ObjetsDuJeu { get; set; }

        public Form1()
        {
            InitializeComponent();

            //Mes objets 
            ObjetsDuJeu = new List<GameObject>();

            // Connexion la méthode "Form1_Paint" à l'evenement Paint 
            this.Paint += new PaintEventHandler(Form1_Paint);

            // Connexion la méthode "BoutonDeplacement" à l'evenement Key 
            this.KeyDown += new KeyEventHandler(BoutonPresse);
            this.KeyPreview = true;


            // Désactiver le redimensionnement
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // Modifier l'image de fond
            this.BackgroundImage = Properties.Resources.fond_2;
            this.BackgroundImageLayout = ImageLayout.Stretch;

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
            foreach (var objet in ObjetsDuJeu)
            {
                objet.Draw(e.Graphics);
            }
        }


        private void BoutonPresse(object sender, KeyEventArgs e)
        {
            /* Autre méthode 
                 // Créer une copie temporaire de la liste pour l'itération
                var tempObjetsDuJeu = new List<GameObject>(ObjetsDuJeu);

                foreach (var gameObject in tempObjetsDuJeu)
                {
                    gameObject.Update(e.KeyCode, this.ClientSize);
                }
             */

            for (int i = 0; i < ObjetsDuJeu.Count; i++)
            {
                ObjetsDuJeu[i].Update(e.KeyCode, this.ClientSize);
            }

            this.Invalidate(); // Pour redessiner le formulaire
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            game = new Game(this.Width, this.Height);
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
