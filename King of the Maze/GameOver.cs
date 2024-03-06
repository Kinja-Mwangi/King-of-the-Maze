using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King_of_the_Maze
{
    internal class GameOver
    {
        public List<Buttons> _buttons = new List<Buttons>();
        public SpriteFont _buttonsTextFont;
        private Buttons _restartButton;
        private Buttons _quitButton;
        protected Texture2D _buttonTexture;
        protected Texture2D _gameOverTexture;
        protected GraphicsDevice _graphicsDevice;
        public Rectangle _gameOverRectangle;
        private bool _buttonsInitialized = false;
        public Color _gameOverColour;

        public GameOver(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _gameOverTexture = new Texture2D(_graphicsDevice, 1, 1);
            _gameOverTexture.SetData(new[] { Color.White });
            _buttonTexture = new Texture2D(_graphicsDevice, 1, 1);
            _buttonTexture.SetData(new[] { Color.White });
        }

        public void HandleButtons(ref GameState currentState)
        {
            if (!_buttonsInitialized)
            {
                InitializeButtons();
            }
            Game1._mouseState = Mouse.GetState();
            foreach (Buttons button in _buttons)
            {
                button._buttonTextColour = new Color(255 - button._buttonColour.R, 255 - button._buttonColour.G, 255 - button._buttonColour.B);
                if (button._buttonShape.Contains(Game1._mouseState.Position))
                {
                    button._buttonColour = Color.White;
                    if (Game1._mouseState.LeftButton == ButtonState.Pressed)
                    {
                        button.ButtonPressed(ref currentState);
                    }
                }
                else
                {
                    button._buttonColour = Color.Black;
                }
            }
        }

        public void InitializeButtons()
        {
            //position the buttons to the center of the screen
            //preferably get the pause menu to be drawn over the game
            _restartButton = new Buttons("Restart", _graphicsDevice, (_graphicsDevice.Viewport.Width - (int)_buttonsTextFont.MeasureString("Restart").X) / 2, (_graphicsDevice.Viewport.Height - (int)_buttonsTextFont.MeasureString("Restart").Y) / 2, _buttonsTextFont, 4);
            _buttons.Add(_restartButton);
            _quitButton = new Buttons("Quit", _graphicsDevice, _quitButton._buttonShape.X, _quitButton._buttonShape.Y - (int)_buttonsTextFont.MeasureString("Quit").Y - 100, _buttonsTextFont, 10);
            _buttons.Add(_quitButton);
            _buttonsInitialized = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_buttonsInitialized)
            {
                spriteBatch.Draw(_gameOverTexture, _gameOverRectangle, _gameOverColour);
                foreach (Buttons button in _buttons)
                {
                    button.DrawButton(button._buttonText, button._buttonShape, spriteBatch);
                }
            }
        }
    }
}
