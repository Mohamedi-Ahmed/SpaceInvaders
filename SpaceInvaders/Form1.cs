using SpaceInvaders.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public partial class gameInstance : Form
    {
        // Variables et collections
        private Game game ;
        public static HashSet<GameObject> ObjetsDuJeu { get; set; }

        // Dimensions images
          // Missile
        public static int largeurImageMissile = 20;
        public static int hauteurImageMissile = 40;
            // SpaceShip
        public static int largeurImageSpaceShip = 100;
        public static int hauteurImageSpaceShip = 100;
            // Bunkers
        public static int largeurImageBunker = 200;
        public static int hauteurImageBunker = 80;
        // Petits ennemies -- taille par defaut
        public static int largeurImagePetitEnnemie = 100;
        public static int hauteurImagePetitEnnemie = 100;
        // Grands ennemies
        public static int largeurImageGrandEnnemie = 150;
        public static int hauteurImageGrandEnnemie = 100;


        public gameInstance()
        {
            InitializeComponent();

            // Initialisation et configuration de base du formulaire
            ConfigureForm();

            ObjetsDuJeu = new HashSet<GameObject>();
            // Initialisation du jeu
            game = new Game(this.Width, this.Height);
            game.Run(); // Démarrer la boucle de jeu

            // Liaison de l'événement de clavier pour KeyDown
            this.KeyDown += new KeyEventHandler(game.OnKeyDown);
            this.KeyUp += new KeyEventHandler(game.OnKeyUp); // Ajoutez cette ligne

        }


        private void ConfigureForm()
        {
            // Désactiver le redimensionnement
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // Modifier l'image de fond
            this.BackgroundImage = Resources.fond_2; // Assurez-vous que Resources.fond_2 est correct
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

            // Connexion la méthode "gameInstance_Paint" à l'evenement Paint 
            this.Paint += new PaintEventHandler(gameInstance_Paint);

            // Modifier le titre de l'écran de jeu
            this.Text = "SPACE INVADERS !";
        }

        private void gameInstance_Paint(object sender, PaintEventArgs e)
        {
                game.Draw(e.Graphics);   
        }

            static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new gameInstance());
        }

        private void gameInstance_Load(object sender, EventArgs e)
        {

        }
    }
}
