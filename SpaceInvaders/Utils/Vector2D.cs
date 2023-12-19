using System;

namespace SpaceInvaders
{
    public class Vector2D
    {
        public double x, y;

        public Vector2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double Norme()
        {
            return Math.Sqrt(x * x + y * y);
        }

        // Operations vectorielles
        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        { return new Vector2D(v1.x + v2.x, v1.y + v2.y); }

        public static Vector2D operator -(Vector2D v1, Vector2D v2)
        { return new Vector2D(v1.x - v2.x, v1.y - v2.y); }

        public static Vector2D operator *(Vector2D v1, Vector2D v2)
        { return new Vector2D(v1.x * v2.x, v1.y * v2.y); }

        //Scalaire
        // Multiplication : 2 cas pour gérer la commutativité
        public static Vector2D operator *(Vector2D v1, double k)
        { return new Vector2D(v1.x * k, v1.y * k); }

        public static Vector2D operator *(double k, Vector2D v1)
        { return new Vector2D(v1.x * k, v1.y * k); }

        // Division : idem
        public static Vector2D operator /(Vector2D v1, double k)
        { if (k != 0)
            {
                return new Vector2D(v1.x / k, v1.y / k);
            }
            else { return null; }
        }

        public static Vector2D operator /(double k, Vector2D v1)
        { if(k != 0)
            {
                return new Vector2D(v1.x / k, v1.y / k);
            } else { return null; }
        }

        // Moins unaire
        public static Vector2D operator -(Vector2D v1)
        {
            return (new Vector2D(v1.x * -1, v1.y * -1));
        }


    }
}
