using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.XAudio2;
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
        public List<Buttons> _buttons = new List<Buttons>();
        public SpriteFont _titleFont;
        public SpriteFont _buttonsTextFont;
        private Texture2D _mainMenuTexture;
        private string _titleText;
        private Buttons _startButton;
        private Buttons _exitButton;
        protected Texture2D _buttonTexture;
        private Buttons _instructionsButton;
        protected GraphicsDevice _graphicsDevice;

        public MainMenu(GraphicsDevice graphicsDevice)
        {
            _titleText = "King of the\n       Maze";
            _graphicsDevice = graphicsDevice;
            _mainMenuTexture = new Texture2D(_graphicsDevice, 1, 1);
            _mainMenuTexture.SetData(new[] { Color.White });
        }

        public void HandleButtons(ref GameState currentState)
        {
            InitializeButtons();
            Game1._mouseState = Mouse.GetState();
            if (Game1._mouseState.LeftButton == ButtonState.Pressed)
            {
                foreach (Buttons button in _buttons)
                {
                    for (int i = 0; i < _buttons.Count; i++)
                    {
                        if (button._buttonType == i && button._buttonShape.Contains(Game1._mouseState.Position))
                        {
                            button.ButtonPressed(ref currentState);
                        }
                    }
                }
            }
        }

        private void InitializeButtons()
        {
            _startButton = new Buttons("Start", _graphicsDevice, (_graphicsDevice.Viewport.Width - (int)_buttonsTextFont.MeasureString("Start").X) / 2, (int)_titleFont.MeasureString(_titleText).Y + 120, _buttonsTextFont, 0);
            _buttons.Add(_startButton);
            _instructionsButton = new Buttons("Instructions", _graphicsDevice, (_graphicsDevice.Viewport.Width - (int)_buttonsTextFont.MeasureString("Instructions").X) / 2, _startButton._buttonShape.Y + 150, _buttonsTextFont, 1);
            _buttons.Add(_instructionsButton);
            _exitButton = new Buttons("Exit", _graphicsDevice, (_graphicsDevice.Viewport.Width - (int)_buttonsTextFont.MeasureString("Exit").X) / 2, _instructionsButton._buttonShape.Y + 150, _buttonsTextFont, 2);
            _buttons.Add(_exitButton);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_mainMenuTexture, new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.Red);
            spriteBatch.DrawString(_titleFont, _titleText, new Vector2((_graphicsDevice.Viewport.Width - _titleFont.MeasureString(_titleText).X) / 2, 50), Color.Black);
            foreach (Buttons button in _buttons)
            {
                button.DrawButton(button._buttonText, button._buttonShape, spriteBatch);
            }
        }
    }
}