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
    internal class PauseMenu
    {
        public List<Buttons> _buttons = new List<Buttons>();
        public SpriteFont _buttonsTextFont;
        private Buttons _resumeButton;
        private Buttons _saveGameButton;
        private Buttons _quitButton;
        protected Texture2D _buttonTexture;
        protected Texture2D _pauseMenuTexture;
        protected GraphicsDevice _graphicsDevice;
        public Rectangle _pauseMenuRectangle;

        public PauseMenu()
        {
            _pauseMenuTexture = new Texture2D(_graphicsDevice, 1, 1);
            _pauseMenuTexture.SetData(new[] { Color.White });
            _buttonTexture = new Texture2D(_graphicsDevice, 1, 1);
            _buttonTexture.SetData(new[] { Color.White });
        }

        public void HandleButtons(ref GameState currentState)
        {
            //if (!_buttonsInitialized)
            //{
            //    InitializeButtons();
            //}
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
            _saveGameButton = new Buttons("Save Game", _graphicsDevice, (_graphicsDevice.Viewport.Width - (int)_buttonsTextFont.MeasureString("Save Game").X) / 2, (_graphicsDevice.Viewport.Height - (int)_buttonsTextFont.MeasureString("Save Game").Y) / 2,  _buttonsTextFont, 4);
            _buttons.Add(_saveGameButton);
            _resumeButton = new Buttons("Resume", _graphicsDevice, _saveGameButton._buttonShape.X, _saveGameButton._buttonShape.Y - (int)_buttonsTextFont.MeasureString("Resume").Y - 100, _buttonsTextFont, 3);
            _buttons.Add(_resumeButton);
            _quitButton = new Buttons("Quit", _graphicsDevice, _saveGameButton._buttonShape.X, _saveGameButton._buttonShape.Y + (int)_buttonsTextFont.MeasureString("Quit").Y + 100, _buttonsTextFont, 0);
            _buttons.Add(_quitButton);
            //_pauseMenuRectangle = new Rectangle(((_graphicsDevice.Viewport.Width - _resumeButton._buttonShape.Width) / 2) - 100, ((_graphicsDevice.Viewport.Height - _resumeButton._buttonShape.Height) / 2) - 100, _saveGameButton._buttonShape.Width + 100, _saveGameButton._buttonShape.Height + _resumeButton._buttonShape.Height + _quitButton._buttonShape.Height + 100);
            _pauseMenuRectangle = new Rectangle((_resumeButton._buttonShape.Y / 2) - 100, (_resumeButton._buttonShape.Height / 2) - 100, _saveGameButton._buttonShape.Width + 100, _quitButton._buttonShape.Y + 100);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_pauseMenuTexture, _pauseMenuRectangle, Color.Red);
            foreach (Buttons button in _buttons)
            {
                button.DrawButton(button._buttonText, button._buttonShape, spriteBatch);
            }
        }
    }
}
