using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace King_of_the_Maze
{
    public class Player
    {
        private Random rng = new Random();
        public GameTime gameTime;
        private Maze _maze;
        private int _cellSize;
        public Vector2 _position;
        private Base _base;
        private List<Base> _bases = new List<Base>();
        //public string _playerID;
        private int _movementTimer = 0;
        private int _movementInterval = 5; // Adjust this value for slower or faster movement
        public Texture2D _playerTexture; // Texture for drawing a single pixel
        private Color[] _colours;
        public Color _playerColour;
        public int _playerNumber;
        private List<Player> _players = new List<Player>();
        public bool _initialHit = true;
        public int _inititalHitResetTimer;
        public int _initialHitResetInterval = 180;
        public int _lives;

        public Player(Maze maze, int cellSize, Base Base, List<Base> bases, List<Player> players, GraphicsDevice graphicsDevice)
        {
            _lives = 4;
            _maze = maze;
            _cellSize = cellSize;
            _base = Base;
            _bases = bases;
            _colours = Game1._colours;
            _players = players;
            _playerNumber = _players.Count + 1;
            _playerColour = _colours[_playerNumber - 1];

            //for (int i = 0; i < 9; i++)
            //{
            //    _playerID += rng.Next(0, 9).ToString();
            //}
            _position = new Vector2(1, 1); // Starting position of the player (assuming a maze with walls around it)
            // Create a 1x1 pixel texture
            _playerTexture = new Texture2D(graphicsDevice, 1, 1);
            _playerTexture.SetData(new Color[] { Color.White });
        }

        public void Move()
        {
            HandleInput();
            foreach (Base Base in _bases)
            {
                if (_position == Base._position)
                {
                    Base.CaptureBase(this);
                }
            }
        }

        public void HandleInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            switch (_playerNumber)
            {
                case 1:
                    // Move the player based on the WASD keys
                    if (keyboardState.IsKeyDown(Keys.W))
                    {
                        PlayerMoveMent(0, -1); // Up
                    }
                    /*else*/
                    if (keyboardState.IsKeyDown(Keys.S))
                    {
                        PlayerMoveMent(0, 1); // Down
                    }

                    /*else*/
                    if (keyboardState.IsKeyDown(Keys.A))
                    {
                        PlayerMoveMent(-1, 0); // Left
                    }
                    /*else*/
                    if (keyboardState.IsKeyDown(Keys.D))
                    {
                        PlayerMoveMent(1, 0); // Right
                    }
                    break;
                case 2:
                    // Move the player based on the arrow keys
                    if (keyboardState.IsKeyDown(Keys.Up))
                    {
                        PlayerMoveMent(0, -1); // Up
                    }
                    /*else*/
                    if (keyboardState.IsKeyDown(Keys.Down))
                    {
                        PlayerMoveMent(0, 1); // Down
                    }

                    /*else*/
                    if (keyboardState.IsKeyDown(Keys.Left))
                    {
                        PlayerMoveMent(-1, 0); // Left
                    }
                    /*else*/
                    if (keyboardState.IsKeyDown(Keys.Right))
                    {
                        PlayerMoveMent(1, 0); // Right
                    }
                    break;
                case 3:
                    // Move the player based on the keypad keys
                    if (keyboardState.IsKeyDown(Keys.NumPad8))
                    {
                        PlayerMoveMent(0, -1); // Up
                    }
                    /*else*/
                    if (keyboardState.IsKeyDown(Keys.NumPad5))
                    {
                        PlayerMoveMent(0, 1); // Down
                    }

                    /*else*/
                    if (keyboardState.IsKeyDown(Keys.NumPad4))
                    {
                        PlayerMoveMent(-1, 0); // Left
                    }
                    /*else*/
                    if (keyboardState.IsKeyDown(Keys.NumPad6))
                    {
                        PlayerMoveMent(1, 0); // Right
                    }
                    break;

            }
            //// Move the player based on the WASD keys
            //if (keyboardState.IsKeyDown(Keys.W))
            //{
            //    PlayerMoveMent(0, -1); // Up
            //}
            ///*else*/
            //if (keyboardState.IsKeyDown(Keys.S))
            //{
            //    PlayerMoveMent(0, 1); // Down
            //}

            ///*else*/
            //if (keyboardState.IsKeyDown(Keys.A))
            //{
            //    PlayerMoveMent(-1, 0); // Left
            //}
            ///*else*/
            //if (keyboardState.IsKeyDown(Keys.D))
            //{
            //    PlayerMoveMent(1, 0); // Right
            //}
            //if (keyboardState.IsKeyDown(Keys.T))
            //{
            //    do
            //    {
            //        _position = new Vector2(rng.Next(1, _maze.mazeSize), rng.Next(1, _maze.mazeSize));
            //    }
            //    while (_maze.maze[(int)_position.X, (int)_position.Y] == 0);
            //}

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

        public Player DespawnPlayer()
        {
            if (_lives == 0)
            {
                return this;
            }
            return null;
        }

        private void PlayerMoveMent(int dx, int dy)
        {
            if (_movementTimer == _movementInterval)
            {
                int newX = (int)(_position.X + dx);
                int newY = (int)(_position.Y + dy);

                if (_maze.ValidPosition(newX, newY) && _maze.maze[newX, newY] != 0)
                {
                    _position.X = newX;
                    _position.Y = newY;
                }

                _movementTimer = 0;
            }
            _movementTimer++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the player as a single pixel
            Rectangle playerRect = new Rectangle((int)(_position.X * _cellSize), (int)(_position.Y * _cellSize), _cellSize, _cellSize);
            spriteBatch.Draw(_playerTexture, playerRect, _playerColour); // You can customize the player's appearance here
        }
    }
}