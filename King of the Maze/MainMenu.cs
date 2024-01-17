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
        private Maze _maze;
        private Texture2D _menuTexture;
        public MainMenu (Maze maze)
        {
            _maze = maze;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            _menuTexture = new Texture2D(graphicsDevice, 1, 1);
            _menuTexture.SetData(new[] { Color.White });
            spriteBatch.Draw(_menuTexture, new Rectangle(0, 0, (int)(graphicsDevice.Viewport.Width), (int)(graphicsDevice.Viewport.Height)), Color.Red);
            spriteBatch.DrawString(textFont, "   King\nof the \n  Maze", new Vector2((graphicsDevice.Viewport.Width / 2 - 200), (graphicsDevice.Viewport.Height / 2-400)), Color.Black);
        }
    }
}
