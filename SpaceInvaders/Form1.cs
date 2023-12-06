using Space_invaders.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public partial class Form1 : Form
    {
        // Mes labels
        private Label labelScore;
        private Label labelVies;

        // Mes boutons
        private Button BoutonCommencer;

        // Mon jeu
        private Game game ;
        public static List<GameObject> ObjetsDuJeu { get; set; }

        // Mes dimensions images
          // Missile
        public static int largeurImageMissile = 30;
        public static int hauteurImageMissile = 30;
            // SpaceShip
        public static int largeurImageSpaceShip = 100;
        public static int hauteurImageSpaceShip = 100;

        private int keyPressCount = 0;
        public Form1()
        {
            InitializeComponent();
            //debug
            Console.WriteLine("Debut | Compteur: " + keyPressCount);

            //Mes objets 
            ObjetsDuJeu = new List<GameObject>();

            // Connexion la méthode "Form1_Paint" à l'evenement Paint 
            this.Paint += new PaintEventHandler(Form1_Paint);

            // Suppression du double abonnement
            this.KeyDown -= Form1_KeyDown;
            // Connexion la méthode "BoutonDeplacement" à l'evenement Key 
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);


            // Désactiver le redimensionnement
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // Modifier l'image de fond
            this.BackgroundImage = Resources.fond_2;
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
                game.Draw(e.Graphics);   
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            keyPressCount++;
            Console.WriteLine("Touche pressée: " + e.KeyCode + " | Compteur: " + keyPressCount);
            game.Update(e.KeyCode, this.ClientSize);
            this.Invalidate(); // Ceci demandera le redessin du formulaire
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
