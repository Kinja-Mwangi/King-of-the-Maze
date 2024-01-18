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
        private Rectangle _settingsButton;
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

        public GameState Buttons(MouseState mouseState)
        {
            _startButton = InitializeButton((int)_titleFont.MeasureString(_titleText).Y + 120, _startButtonText);
            _instructionsButton = InitializeButton(_startButton.Y + 150, _instructionsButtonText);
            _settingsButton = InitializeButton(_instructionsButton.Y + 150, _settingsButtonText);
            mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_startButton.Contains(mouseState.Position))
                {
                    return GameState.GamePlaying;
                }
            }
            return GameState.GameMenu;
        }

        private Rectangle InitializeButton(int heightSpacing, string buttonText)
        {
            return new Rectangle((_graphicsDevice.Viewport.Width - (int)_buttonsText.MeasureString(buttonText).X) / 2, heightSpacing, (int)_buttonsText.MeasureString(buttonText).X + 20, (int)_buttonsText.MeasureString(buttonText).Y + 30);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_menuTexture, new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.Red);
            spriteBatch.DrawString(_titleFont, _titleText, new Vector2((_graphicsDevice.Viewport.Width - _titleFont.MeasureString(_titleText).X) / 2, 50), Color.Black);
            DrawButton(_startButtonText, _startButton, spriteBatch);
            DrawButton(_instructionsButtonText, _instructionsButton, spriteBatch);
            DrawButton(_settingsButtonText, _settingsButton, spriteBatch);
        }

        private void DrawButton(string buttonText, Rectangle button, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_buttonTexture, button, Color.Black);
            spriteBatch.DrawString(_buttonsText, buttonText, new Vector2(button.X + ((button.Width - _buttonsText.MeasureString(buttonText).X) / 2), button.Y + ((button.Height - _buttonsText.MeasureString(buttonText).Y) / 2)), Color.White);
        }
    }
}
