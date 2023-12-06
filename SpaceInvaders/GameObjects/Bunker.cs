using System;
using System.Drawing;
using System.Windows.Forms;
using Space_invaders.Properties;
using SpaceInvaders;
using SpaceInvaders.GameObjects;

namespace Space_invaders.GameObjects
{
    internal class Bunker : SimpleObject
    {
        public Bunker(Vecteur2D position_initiale): base(position_initiale, Resources.bunker, 1){   }
        public override void Update(Keys key, Size gameSize)
        {
            // Ne fais rien !
        }
        public override void Collision(Missile missile)
        {
            // Calculez les rectangles englobants pour le missile et le bunker
            Rectangle bunkerRect  = new Rectangle((int)this.Position.x, (int)this.Position.y,
                                                  gameInstance.largeurImageBunker, gameInstance.hauteurImageBunker);
            Rectangle missileRect = new Rectangle((int)missile.Position.x, (int)missile.Position.y,
                                                  gameInstance.largeurImageMissile, gameInstance.hauteurImageMissile);

            // Vérifiez si les rectangles englobants s'intersectent
            if (bunkerRect.IntersectsWith(missileRect))
            {
                // Effectuez le test pixel par pixel
                Console.WriteLine("Je teste la collision");
                TestCollisionPixel(missile);
            }
        }

        // Rectangle englobant pour débugger
        public override void Draw(Graphics graphics, int largeur, int hauteur)
        {
            base.Draw(graphics, largeur, hauteur);

            // Dessiner le rectangle englobant
            Rectangle bunkerRect = new Rectangle((int)this.Position.x, (int)this.Position.y, largeur, hauteur);
            using (Pen pen = new Pen(Color.Yellow, 2))  // Couleur jaune pour le rectangle, épaisseur de 2
            {
                graphics.DrawRectangle(pen, bunkerRect);
            }
        }

        private void TestCollisionPixel(Missile missile)
        {
            int collisionCount = 0;
            Bitmap missileBitmap = new Bitmap(Resources.projectile);
            Bitmap originalBunkerBitmap = new Bitmap(Resources.bunker);
            //Bitmap bunkerBitmap = new Bitmap(gameInstance.largeurImageBunker, gameInstance.hauteurImageBunker, originalBunkerBitmap);
            bool test = false;
            int missilePixelX = (int)missile.Position.x;
            int missilePixelY = (int)missile.Position.y;
            Console.WriteLine($"missilePixelX : {missilePixelX} | missilePixelY : {missilePixelY}");
            Bitmap gameSurface = new Bitmap(gameInstance.ActiveForm.Width, gameInstance.ActiveForm.Height);

            for (int x = 0; x < gameInstance.largeurImageMissile; x++)
            {
                for (int y = 0; y < gameInstance.hauteurImageMissile; y++)
                {
                    // Calculer la position du pixel sur la surface de jeu
                    int gameX = (int)missile.Position.x + x;
                    int gameY = (int)missile.Position.y + y;

                    // Vérifier si le pixel est dans les limites de la surface de jeu
                    if (gameX >= 0 && gameX < gameInstance.ActiveForm.Width && gameY >= 0 && gameY < gameInstance.ActiveForm.Height)
                    {
                        // Obtenir la couleur du pixel à cette position
                        Color colorAtPixel = gameSurface.GetPixel(gameX, gameY+10);
                        Console.WriteLine($"colorAtPixel : {colorAtPixel}");

                        // Vérifier si la couleur correspond à notre critère (vert avec G = 252)
                        if (colorAtPixel.G == 252)
                        {
                            test = true;
                        }
                    }
                }
            }
            Console.WriteLine($"test : {test}");
            //return false; // Aucune collision détectée


            /*
            for (int x = 0; x < gameInstance.largeurImageMissile; x++)
            {
                for (int y = 0; y < gameInstance.hauteurImageMissile; y++)
                {
                    // Calculez la position du pixel du missile par rapport au bunker
                    int missilePixelX = (int)missile.Position.x + x;
                    int missilePixelY = (int)missile.Position.y + y;

                    // Vérifiez si le pixel est à l'intérieur du rectangle du bunker
                    if (missilePixelX >= this.Position.x && missilePixelX < this.Position.x + gameInstance.largeurImageBunker &&
                        missilePixelY >= this.Position.y && missilePixelY < this.Position.y + gameInstance.hauteurImageBunker)
                    {
                        Console.WriteLine("Il y a collision entre les rectangles!");
                        // Obtenez les couleurs des pixels correspondants
                        Color missilePixelColor = missileBitmap.GetPixel(x, y);
                        Color bunkerPixelColor = bunkerBitmap.GetPixel(missilePixelX - (int)this.Position.x, missilePixelY - (int)this.Position.y);
                        Console.WriteLine("missilePixelColor.A : "+ missilePixelColor + " bunkerPixelColor.A" + bunkerPixelColor);
                        // Vérifiez si les deux pixels sont non-transparents
                        if (bunkerPixelColor.A != 0)
                        {
                            collisionCount++;
                            bunkerBitmap.SetPixel(missilePixelX - (int)this.Position.x, missilePixelY - (int)this.Position.y, Color.Transparent);

                        }
                    }
                }
            }
            
            // Mettez à jour l'image du bunker
            this.Image = bunkerBitmap;

            Console.WriteLine("CollisionCount : "+collisionCount);
            // Soustrayez le nombre de collisions du nombre de vies du missile
            missile.Vies -= collisionCount;
            */

        }
        // ...
    }
}

