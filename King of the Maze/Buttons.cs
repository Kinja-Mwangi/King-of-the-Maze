
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
        }

        public void ButtonPressed(ref GameState currentState)
        {
            switch (_buttonType)
            {
                case 0:
                    currentState = ChangeGameState(GameState.StartMenu);
                    break;
                case 1:
                    currentState = ChangeGameState(GameState.GameInstructions);
                    break;
                case 2:
                    currentState = ChangeGameState(GameState.GameExit);
                    break;
                case 3:
                    currentState = ChangeGameState(GameState.GamePlaying);
                    break;
                case 4:
                    //add logic to load a saved game and change game state to playing
                    break;
                case 5:
                    Game1._numberOfPlayers = 1;
                    break;
                case 6:
                    Game1._numberOfPlayers = 2;
                    break;
                case 7:
                    Game1._numberOfPlayers = 3;
                    break;
            }

            GameState ChangeGameState(GameState newState)
            {
                return newState;
            }
        }

        public void DrawButton(string buttonText, Rectangle button, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_buttonTexture, button, Color.Black);
            spriteBatch.DrawString(_buttonTextFont, buttonText, new Vector2(button.X + ((button.Width - _buttonTextFont.MeasureString(buttonText).X) / 2), button.Y + ((button.Height - _buttonTextFont.MeasureString(buttonText).Y) / 2)), Color.White);
        }
    }
}
