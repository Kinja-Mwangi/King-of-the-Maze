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
        private Texture2D _menuTexture;
        private string _titleText;
        private Buttons _startButton;
        protected Texture2D _buttonTexture;
        private Buttons _instructionsButton;
        private Buttons _settingsButton;
        protected GraphicsDevice _graphicsDevice;

        public MainMenu(GraphicsDevice graphicsDevice)
        {
            _titleText = "King of the\n       Maze";
            _graphicsDevice = graphicsDevice;
            _menuTexture = new Texture2D(_graphicsDevice, 1, 1);
            _menuTexture.SetData(new[] { Color.White });
        }

        public void HandleButtons(MouseState mouseState, ref GameState currentState)
        {
            InitializeButtons();
            mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                foreach (Buttons button in _buttons)
                {
                    for (int i = 0; i < _buttons.Count; i++)
                    {
                        if (button._buttonType == i && button._buttonShape.Contains(mouseState.Position))
                        {
                            button.ButtonPressed(ref currentState);
                        }
                    }
                }
            }
        }

        private void InitializeButtons()
        {
            _startButton = new Buttons("Start", _graphicsDevice, (int)_titleFont.MeasureString(_titleText).Y + 120, _buttonsTextFont, 0);
            _buttons.Add(_startButton);
            _instructionsButton = new Buttons("Instructions", _graphicsDevice, _startButton._buttonShape.Y + 150, _buttonsTextFont, 1);
            _buttons.Add(_instructionsButton);
            _settingsButton = new Buttons("Settings", _graphicsDevice, _instructionsButton._buttonShape.Y + 150, _buttonsTextFont, 2);
            _buttons.Add(_settingsButton);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_menuTexture, new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.Red);
            spriteBatch.DrawString(_titleFont, _titleText, new Vector2((_graphicsDevice.Viewport.Width - _titleFont.MeasureString(_titleText).X) / 2, 50), Color.Black);
            foreach (Buttons button in _buttons)
            {
                button.DrawButton(button._buttonText, button._buttonShape, spriteBatch);
            }
        }
    }
}