using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Security.Cryptography;

namespace King_of_the_Maze
{
    public class Player
    {
        private Random rng = new Random();
        public GameTime gameTime;
        private Maze _maze;
        private int _cellSize;
        public Vector2 _playerPosition;
        private int _movementTimer = 0;
        private int _movementInterval = 5; // Adjust this value for slower or faster movement
        private Texture2D _playerTexture; // Texture for drawing a single pixel

        public Player(Maze maze, int cellSize, GraphicsDevice graphicsDevice)
        {
            _maze = maze;
            _cellSize = cellSize;
            _playerPosition = new Vector2(1, 1); // Starting position of the player (assuming a maze with walls around it)

            // Create a 1x1 pixel texture
            _playerTexture = new Texture2D(graphicsDevice, 1, 1);
            _playerTexture.SetData(new Color[] { Color.White });
        }

        public void Move() 
        {
            HandleInput();
        }

        public void HandleInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // Move the player based on the WASD keys
            if (keyboardState.IsKeyDown(Keys.W))
            {
                PlayerMoveMent(0, -1); // Up
            }
            /*else*/ if (keyboardState.IsKeyDown(Keys.S))
            {
                PlayerMoveMent(0, 1); // Down
            }

            /*else*/ if (keyboardState.IsKeyDown(Keys.A))
            {
                PlayerMoveMent(-1, 0); // Left
            }
            /*else*/ if (keyboardState.IsKeyDown(Keys.D))
            {
                PlayerMoveMent(1, 0); // Right
            }
            if (keyboardState.IsKeyDown(Keys.T))
            {
                _playerPosition = new Vector2(rng.Next(1, 50), rng.Next(1, 50));
            }

            //for random movement
            //switch(rng.Next(0, 3))
            //{
            //    case 0:
            //        Move(0, -1); // Up
            //        break;
            //    case 1:
            //        Move(0, 1); // Down
            //        break;
            //    case 2:
            //        Move(-1, 0); // Left 
            //        break;
            //    case 3:
            //        Move(1, 0); // Right
            //        break;
            //}
        }

        private void PlayerMoveMent(int dx, int dy)
        {
            if (_movementTimer == _movementInterval)
            {
                int newX = (int)(_playerPosition.X + dx);
                int newY = (int)(_playerPosition.Y + dy);

                if (_maze.ValidPosition(newX, newY) && _maze.maze[newX, newY] != 0)
                {
                    _playerPosition.X = newX;
                    _playerPosition.Y = newY;
                }

                _movementTimer = 0;
            }
            _movementTimer++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the player as a single pixel
            Rectangle playerRect = new Rectangle((int)(_playerPosition.X * _cellSize), (int)(_playerPosition.Y * _cellSize), _cellSize, _cellSize);
            spriteBatch.Draw(_playerTexture, playerRect, Color.Green); // You can customize the player's appearance here
        }
    }
}