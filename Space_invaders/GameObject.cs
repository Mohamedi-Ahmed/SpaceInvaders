using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NameSpaceGameObject
{
    public abstract class GameObject
    {
        public abstract void Draw(Graphics graphics);
        public abstract bool IsAlive();
        public abstract void Update(Keys key, Size gameSize);
    }
}
