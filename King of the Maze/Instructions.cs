using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King_of_the_Maze
{
    internal class Instructions
    {
        private Texture2D _instructionsTexture;
        public SpriteFont _titleFont; 
        public SpriteFont _instructionsFont;
        private GraphicsDevice _graphicsDevice;
        string _titleText;
        string _instructionsText;

        public Instructions(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _instructionsTexture = new Texture2D(_graphicsDevice, 1, 1);
            _instructionsTexture.SetData(new[] { Color.White });
            _titleText = "Instructions";
            using (StreamReader loadInstructions = new StreamReader("instructions.txt"))
            {
                _instructionsText = loadInstructions.ReadToEnd();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_instructionsTexture, new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.Red);
            spriteBatch.DrawString(_titleFont, _titleText, new Vector2((_graphicsDevice.Viewport.Width - _titleFont.MeasureString(_titleText).X) / 2, 50), Color.Black);
            spriteBatch.DrawString(_instructionsFont, _instructionsText, new Vector2(20, _instructionsFont.MeasureString(_titleText).Y + 200), Color.Black);    
        }
    }
}