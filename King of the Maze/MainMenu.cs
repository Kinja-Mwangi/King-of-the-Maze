using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King_of_the_Maze
{
    public class MainMenu
    {
        public SpriteFont textFont;
        public MainMenu ()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(textFont, "King of the Maze", new Vector2(100, 100), Color.Black);
        }
    }
}
