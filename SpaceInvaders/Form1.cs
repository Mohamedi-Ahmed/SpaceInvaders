using SpaceInvaders.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public partial class gameInstance : Form
    {
        private Game game ;
        public static HashSet<GameObject> GameObjects { get; set; }

        // Dimensions images
          // Missile
        public static int missileImageWidth      = 20;
        public static int missileImageHeight     = 40;
            // SpaceShip
        public static int spaceShipImageWidth    = 100;
        public static int spaceShipImageHeight   = 100;
            // Bunkers
        public static int bunkerImageWidth       = 200;
        public static int bunkerImageHeight      = 80;
        // Petits ennemies -- taille par defaut
        public static int smallEnnemyImageWidth  = 100;
        public static int smallEnnemyImageHeight = 100;
        // Grands ennemies
        public static int bigEnnemyImageWidth    = 150;
        public static int bigEnnemyImageHeight   = 100;

        public gameInstance()
        {
            InitializeComponent();
            // Initialisation et configuration de base du formulaire
            ConfigureForm();

            GameObjects = new HashSet<GameObject>();
            // Initialisation du jeu
            game = new Game(this.Width, this.Height);
            game.Run(); // Démarrer la boucle de jeu

            // Liaison de l'événement de clavier pour KeyDown
            this.KeyDown += new KeyEventHandler(game.OnKeyDown);
            this.KeyUp += new KeyEventHandler(game.OnKeyUp);
        }


        private void ConfigureForm()
        {
            // Désactiver le redimensionnement
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // Modifier l'image de fond
            this.BackgroundImage = Resources.bg2; // Assurez-vous que Resources.fond_2 est correct
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

            // Prise en compte de la reduction de l'écran de jeu
            this.Resize += new EventHandler(Form_Resize);

            // Ajouter le gestionnaire pour l'événement Deactivate
            this.Deactivate += new EventHandler(Form_Deactivate);

            // Activer la prévisualisation des touches
            this.KeyPreview = true; 

        }

        private void Form_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                game.PauseGame();
            }

        }
        
        private void Form_Deactivate(object sender, EventArgs e)
        {
            game.PauseGame();
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

    }
}
