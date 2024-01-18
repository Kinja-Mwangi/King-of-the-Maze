using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King_of_the_Maze
{
    public class MainMenu
    {
        public SpriteFont _titleFont;
        public SpriteFont _buttonsText;
        private Texture2D _menuTexture;
        private string _titleText;
        private string _startButtonText;
        private Rectangle _startButton;
        private Texture2D _buttonTexture;
        private string _instructionsButtonText;
        private Rectangle _instructionsButton;
        private string _settingsButtonText;
        private GraphicsDevice _graphicsDevice;

        public MainMenu(GraphicsDevice graphicsDevice)
        {
            _titleText = "King of the\n       Maze";
            _startButtonText = "Start";
            _instructionsButtonText = "Instructions";
            _settingsButtonText = "Settings";
            _graphicsDevice = graphicsDevice;
            _menuTexture = new Texture2D(_graphicsDevice, 1, 1);
            _menuTexture.SetData(new[] { Color.White });
            _buttonTexture = new Texture2D(_graphicsDevice, 1, 1);
            _buttonTexture.SetData(new[] { Color.White });
        }

        public void Buttons(ref GameState currentState, MouseState mouseState)
        {
            _startButton = new Rectangle((_graphicsDevice.Viewport.Width - (int)_buttonsText.MeasureString(_startButtonText).X) / 2, (int)_titleFont.MeasureString(_titleText).Y + 100, (int)_buttonsText.MeasureString(_startButtonText).X + 20, (int)_buttonsText.MeasureString(_startButtonText).Y + 20);
            _instructionsButton = new Rectangle((_graphicsDevice.Viewport.Width - (int)_buttonsText.MeasureString(_instructionsButtonText).X) / 2, _startButton.Y + 150, (int)_buttonsText.MeasureString(_instructionsButtonText).X + 20, (int)_buttonsText.MeasureString(_instructionsButtonText).Y + 20);
            mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_startButton.Contains(mouseState.Position))
                {
                    currentState = GameState.GamePlaying;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_menuTexture, new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.Red);
            spriteBatch.DrawString(_titleFont, _titleText, new Vector2((_graphicsDevice.Viewport.Width - _titleFont.MeasureString(_titleText).X) / 2, 50), Color.Black);
            DrawButton(_startButtonText, _startButton, spriteBatch);
            DrawButton(_instructionsButtonText, _instructionsButton, spriteBatch);
        }

        private void DrawButton(string buttonText, Rectangle button, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_buttonTexture, button, Color.Black);
            spriteBatch.DrawString(_buttonsText, buttonText, new Vector2(button.X + ((button.Width - _buttonsText.MeasureString(buttonText).X) / 2), button.Y + ((button.Height - _buttonsText.MeasureString(buttonText).Y) / 2)), Color.White);
        }
    }
}
