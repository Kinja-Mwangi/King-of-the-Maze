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

        public void InitializeButtons()
        {
            _newGameButton = new Buttons("NewGame", _graphicsDevice, (_graphicsDevice.Viewport.Width - (int)_buttonsTextFont.MeasureString("NewGame").X) / 2, (int)_titleFont.MeasureString(_titleText).Y + 120, _buttonsTextFont, 3);
            _buttons.Add(_newGameButton);
            _loadGameButton = new Buttons("LoadGame", _graphicsDevice, (_graphicsDevice.Viewport.Width - (int)_buttonsTextFont.MeasureString("LoadGame").X) / 2, _newGameButton._buttonShape.Y + 150, _buttonsTextFont, 4);
            _buttons.Add(_loadGameButton);
            _1player = new Buttons("1", _graphicsDevice, ((_graphicsDevice.Viewport.Width - (int)_buttonsTextFont.MeasureString("1").X) / 2) - 100, _loadGameButton._buttonShape.Y + (int)_buttonsTextFont.MeasureString("Number of Players").Y + 150, _buttonsTextFont, 5);
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
                spriteBatch.DrawString(_buttonsTextFont, "Number of Players", new Vector2((_graphicsDevice.Viewport.Width - _buttonsTextFont.MeasureString("Number of Players").X) / 2, _loadGameButton._buttonShape.Y + 150), Color.Black);
                foreach (Buttons button in _buttons)
                {
                    button.DrawButton(button._buttonText, button._buttonShape, spriteBatch);
                }
            }
        }
    }
}