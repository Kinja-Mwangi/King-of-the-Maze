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
        public Random _rng = new Random();
        public List<Buttons> _buttons = new List<Buttons>();
        public SpriteFont _buttonsTextFont;
        public SpriteFont _gameOverFont;
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
            _restartButton = new Buttons("Restart", _graphicsDevice, (_graphicsDevice.Viewport.Width - (int)_buttonsTextFont.MeasureString("Restart").X) / 2, ((_graphicsDevice.Viewport.Height - (int)_buttonsTextFont.MeasureString("Restart").Y) / 2), _buttonsTextFont, 10);
            _buttons.Add(_restartButton);
            _quitButton = new Buttons("Quit", _graphicsDevice, (_graphicsDevice.Viewport.Width - (int)_buttonsTextFont.MeasureString("Quit").X) / 2, _restartButton._buttonShape.Bottom + 90, _buttonsTextFont, 8);
            _buttons.Add(_quitButton);
            Buttons button1 = new Buttons("", _graphicsDevice, 0, 0, _buttonsTextFont, 0);
            foreach (Buttons button in _buttons) //finds the button with the largest width to use to size the pause menu rectangle
            {
                if (button._buttonShape.Width > button1._buttonShape.Width)
                {
                    button1 = button;
                }
                _gameOverRectangle = new Rectangle(_restartButton._buttonShape.Left - 150, _restartButton._buttonShape.Top - 250, button1._buttonShape.Width + 300, _quitButton._buttonShape.Bottom); //for some reason the bottom of the quit button is not where it should be
            }
            _buttonsInitialized = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_buttonsInitialized)
            {
                //_gameOverTextColour = ColourCycle();
                spriteBatch.Draw(_gameOverTexture, _gameOverRectangle, _gameOverColour);
                foreach (Buttons button in _buttons)
                {
                    button.DrawButton(button._buttonText, button._buttonShape, spriteBatch);
                }
                if (Game1._winner == null)
                {
                    spriteBatch.DrawString(_gameOverFont, "Game Over!", new Vector2(((_gameOverRectangle.Width - (int)_buttonsTextFont.MeasureString("Game Over!").X) / 2) + 50, _gameOverRectangle.Top + 50), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(_gameOverFont, "Player " + Game1._winner._playerNumber + "\n     Wins!", new Vector2(((_gameOverRectangle.Width - (int)_buttonsTextFont.MeasureString("Player" + Game1._winner._playerNumber + "\n     Wins!").X) / 2) + 50, _gameOverRectangle.Top + 25), new Color(255 - Game1._winner._playerColour.R, 255 - Game1._winner._playerColour.G, 255 - Game1._winner._playerColour.B));
                }
            }
        }
    }
}