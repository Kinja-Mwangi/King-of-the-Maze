
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace King_of_the_Maze
{
    public class Buttons
    {
        List<Rectangle> _changePlayerCountButtons = new List<Rectangle>();
        public int _buttonType;
        private KeyboardState _keyboardState;
        private GraphicsDevice _graphicsDevice; 
        public string _buttonText;
        protected Texture2D _buttonTexture;
        public Rectangle _buttonShape;
        public SpriteFont _buttonTextFont;
        public int _verticalSpacing;
        public int _horizontalSpacing;
        public Color _buttonColour;
        public Color _buttonTextColour;
        //private int _buttonTimer;
        //private int _buttonTimerInterval;

        public Buttons(string buttonText, GraphicsDevice graphicsDevice, int horizontalSpacing, int verticalSpacing, SpriteFont buttonTextFont, int buttonType)
        {
            _graphicsDevice = graphicsDevice;
            _buttonText = buttonText;
            _buttonType = buttonType;
            _buttonTextFont = buttonTextFont;
            _verticalSpacing = verticalSpacing;
            _horizontalSpacing = horizontalSpacing;
            _buttonText = buttonText;
            _buttonShape = new Rectangle(_horizontalSpacing, _verticalSpacing, (int)_buttonTextFont.MeasureString(buttonText).X + 20, (int)_buttonTextFont.MeasureString(buttonText).Y + 30);
            _buttonTexture = new Texture2D(_graphicsDevice, 1, 1);
            _buttonTexture.SetData(new[] { Color.White });
            _buttonColour = Color.Black;
        }

        public void ButtonPressed(ref GameState currentState)
        {
            switch (_buttonType)
            {
                case 0:
                    currentState = GameState.StartMenu;
                    break;
                case 1:
                    currentState = GameState.GameInstructions;
                    break;
                case 2:
                    currentState = GameState.GameExit;
                    break;
                case 3:
                    currentState = GameState.GamePlaying;
                    break;
                case 4:
                    currentState = GameState.GameSaving;
                    break;
                case 5:
                    Game1._numberOfPlayers = 1;
                    _buttonColour = Color.Green;
                    break;
                case 6:
                    Game1._numberOfPlayers = 2;
                    _buttonColour = Color.Purple;
                    break;
                case 7:
                    Game1._numberOfPlayers = 3;
                    _buttonColour = Color.Blue;
                    break;
                case 8:
                    Game1._gameInitialized = false;
                    currentState = GameState.MainMenu;
                    break;
                case 9:
                    currentState = GameState.GameLoading;
                    break;
                case 10:
                    currentState = GameState.GameRestarting;
                    break;
            }
        }

        public void DrawButton(string buttonText, Rectangle button, SpriteBatch spriteBatch)
        {
            if (_buttonType == Game1._numberOfPlayers + 4)
            {
                _buttonColour = Game1._colours[Game1._numberOfPlayers - 1];
            }
            else if (_buttonType == 4 && Game1._gameSaved)
            {
                _buttonTextColour = Color.Green;
            }
            spriteBatch.Draw(_buttonTexture, button, _buttonColour);
            spriteBatch.DrawString(_buttonTextFont, buttonText, new Vector2(button.X + ((button.Width - _buttonTextFont.MeasureString(buttonText).X) / 2), button.Y + ((button.Height - _buttonTextFont.MeasureString(buttonText).Y) / 2)), _buttonTextColour);
        }
    }
}