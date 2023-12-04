using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;

namespace King_of_the_Maze
{
    public class Minion
    {
        private Random rng = new Random();
        private Maze _maze;
        private int _cellSize;
        private Vector2 _position;
        private int _currentDirectionX;
        private int _currentDirectionY;
        private int _movementTimer = 0;
        private int _movementInterval = 5; // Adjust this value for slower or faster movement
        private Texture2D _minionTexture;

        public Minion(Maze maze, int cellSize, GraphicsDevice graphicsDevice)
        {
            _maze = maze;
            _cellSize = cellSize;
            _position = new Vector2(1, 1);

            _minionTexture = new Texture2D(graphicsDevice, 1, 1);
            _minionTexture.SetData(new Color[] { Color.CornflowerBlue });

            // Start with a random initial direction
            do
            {
                _currentDirectionX = rng.Next(-1, 2);
                _currentDirectionY = rng.Next(-1, 2);
            }
            while (!((_currentDirectionX == 0 && _currentDirectionY != 0) || (_currentDirectionX != 0 && _currentDirectionY == 0)));
        }

        public void HandleMovement()
        {
            // Move the minion continuously in the current direction if valid
            RandomMovement(_currentDirectionX, _currentDirectionY);
        }

        private void RandomMovement(int dx, int dy)
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
                else
                {
                    // If hitting a wall, change the direction
                    do
                    {
                        _currentDirectionX = rng.Next(-1, 2);
                        _currentDirectionY = rng.Next(-1, 2);
                    }
                    while (!((_currentDirectionX == 0 && _currentDirectionY != 0) || (_currentDirectionX != 0 && _currentDirectionY == 0)));
                }

                _movementTimer = 0;
            }

            _movementTimer++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the minion as a single pixel
            Rectangle minionRect = new Rectangle((int)(_position.X * _cellSize), (int)(_position.Y * _cellSize), _cellSize, _cellSize);
            spriteBatch.Draw(_minionTexture, minionRect, Color.CornflowerBlue);
            //to do - draw the minion as a triangle making it easier to differentiate from players
        }
    }
}