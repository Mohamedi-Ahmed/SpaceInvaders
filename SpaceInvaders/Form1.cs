using SpaceInvaders.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public partial class gameInstance : Form
    {
        // Mes boutons
        private Button BoutonCommencer;

        // Mon jeu
        private Game game ;
        public static HashSet<GameObject> ObjetsDuJeu { get; set; }

        // Mes dimensions images
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

        private int keyPressCount = 0;
        public gameInstance()
        {
            InitializeComponent();
            //debug
            Console.WriteLine("Debut | Compteur: " + keyPressCount);

            //Mes objets 
            ObjetsDuJeu = new HashSet<GameObject>();

            // Connexion la méthode "gameInstance_Paint" à l'evenement Paint 
            this.Paint += new PaintEventHandler(gameInstance_Paint);

            // Suppression du double abonnement
            this.KeyDown -= gameInstance_KeyDown;
            // Connexion la méthode "BoutonDeplacement" à l'evenement Key 
            this.KeyDown += new KeyEventHandler(gameInstance_KeyDown);


            // Désactiver le redimensionnement
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

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

        private void gameInstance_Paint(object sender, PaintEventArgs e)
        {
                game.Draw(e.Graphics);   
        }


        private void gameInstance_KeyDown(object sender, KeyEventArgs e)
        {
            keyPressCount++;
            Console.WriteLine("Touche pressée: " + e.KeyCode + " | Compteur: " + keyPressCount);
            game.Update(e.KeyCode, this.ClientSize);
            this.Invalidate(); // Ceci demandera le redessin du formulaire
        }



        private void gameInstance_Load(object sender, EventArgs e)
        {
            game = new Game(this.Width, this.Height);
        }

        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Remplacez 'MonFormulaire' par le nom de votre classe de formulaire
            Application.Run(new gameInstance());
        }
    }
}
