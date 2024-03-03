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
using System.Threading;
using System.Threading.Tasks;

namespace King_of_the_Maze
{
    public class StartMenu
    {
        public List<Buttons> _buttons = new List<Buttons>();
        public SpriteFont _titleFont;
        public SpriteFont _buttonsTextFont;
        private string _titleText;
        private Buttons _newGameButton;
        private Buttons _loadGameButton;
        private Buttons _1player;
        private Buttons _2players;
        private Buttons _3players;
        protected Texture2D _startMenuTexture;
        protected Texture2D _buttonTexture;
        protected GraphicsDevice _graphicsDevice;
        private bool _buttonsInitialized = false;

        public StartMenu(GraphicsDevice graphicsDevice)
        {
            _titleText = "Start";
            _graphicsDevice = graphicsDevice;
            _startMenuTexture = new Texture2D(_graphicsDevice, 1, 1);
            _startMenuTexture.SetData(new[] { Color.White });
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
            _newGameButton = new Buttons("NewGame", _graphicsDevice, (_graphicsDevice.Viewport.Width - (int)_buttonsTextFont.MeasureString("NewGame").X) / 2, (int)_titleFont.MeasureString(_titleText).Y + 120, _buttonsTextFont, 3);
            _buttons.Add(_newGameButton);
            _loadGameButton = new Buttons("LoadGame", _graphicsDevice, (_graphicsDevice.Viewport.Width - (int)_buttonsTextFont.MeasureString("LoadGame").X) / 2, _newGameButton._buttonShape.Y + 150, _buttonsTextFont, 9);
            _buttons.Add(_loadGameButton);
            _1player = new Buttons("1", _graphicsDevice, ((_graphicsDevice.Viewport.Width - (int)_buttonsTextFont.MeasureString("1").X) / 2) - 100, _loadGameButton._buttonShape.Y + (int)_buttonsTextFont.MeasureString("Number of Players :").Y + 180, _buttonsTextFont, 5);
            _buttons.Add(_1player);
            _2players = new Buttons("2", _graphicsDevice, ((_graphicsDevice.Viewport.Width - (int)_buttonsTextFont.MeasureString("2").X) / 2), _1player._buttonShape.Y, _buttonsTextFont, 6);
            _buttons.Add(_2players); 
            _3players = new Buttons("3", _graphicsDevice, ((_graphicsDevice.Viewport.Width - (int)_buttonsTextFont.MeasureString("3").X) / 2) + 100, _1player._buttonShape.Y, _buttonsTextFont, 7);
            _buttons.Add(_3players);
            _buttonsInitialized = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_buttonsInitialized)
            {
                spriteBatch.Draw(_startMenuTexture, new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.Red);
                spriteBatch.DrawString(_titleFont, _titleText, new Vector2((_graphicsDevice.Viewport.Width - _titleFont.MeasureString(_titleText).X) / 2, 50), Color.Black);
                //draw a rectangle for the number of players text
                Rectangle numberOfPlayersRectangle = new Rectangle((_graphicsDevice.Viewport.Width - (int)_buttonsTextFont.MeasureString("Number of Players :").X) / 2, _loadGameButton._buttonShape.Y + 150, (int)_buttonsTextFont.MeasureString("Number of Players :").X + 20, (int)_buttonsTextFont.MeasureString("Number of Players :").Y + 30);
                spriteBatch.Draw(_buttonTexture, numberOfPlayersRectangle, Color.Black);
                spriteBatch.DrawString(_buttonsTextFont, "Number of Players :", new Vector2(numberOfPlayersRectangle.X + (numberOfPlayersRectangle.Width - _buttonsTextFont.MeasureString("Number of Players :").X) / 2, numberOfPlayersRectangle.Y + (numberOfPlayersRectangle.Height - _buttonsTextFont.MeasureString("Number of Players :").Y) / 2), Color.White);

                foreach (Buttons button in _buttons)
                {
                    button.DrawButton(button._buttonText, button._buttonShape, spriteBatch);
                }
            }
        }
    }
}