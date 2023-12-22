using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.XAudio2;
//using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace King_of_the_Maze
{
    public class Minion
    {
        private Random rng = new Random();
        private Maze _maze;
        private Player _player;
        private int _cellSize;
        private Vector2 _position;
        private float _movementSpeed = 0.3f;
        private int _movementTimer = 0;
        private int _movementInterval = 5; // Adjust this value for slower or faster movement
        private int _currentDirectionX;
        private int _currentDirectionY;
        private Texture2D _minionTexture;
        public List<Vector2> _playerPosList = new List<Vector2>();
        public List<Vector2> _path = new List<Vector2>();

        public Minion(Maze maze, Player player, int cellSize, GraphicsDevice graphicsDevice)
        {
            _maze = maze;
            _player = player;
            _cellSize = cellSize;
            _position = new Vector2(30, 1);
            _path = FindShortestPath(_player._position);

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
            if (_movementTimer == _movementInterval)
            {
                //_playerPosList.Add(_player._position);
                //for (int i = 0; i < _playerPosList.Count(); i++) 
                //{
                //    if (_playerPosList[i] != _playerPosList[i++])
                //    {
                //        _playerDPosList.Add(_playerPosList[i]);
                //    }
                //}
                //write the _player._position to a txt file to check it is accurate and updating
                string fileName = "Test-PlayerPositionTracking.txt";
                FileStream appendText = File.Open(fileName, FileMode.Append);
                using StreamWriter write = new StreamWriter(appendText);
                {
                    write.WriteLine(_player._position);
                }
                write.Close();

                //if (_position == _player._position)
                //{
                //    Teleport();
                //}

                string fileName1 = "Test-PlayerPosList.txt";
                FileStream appendText1 = File.Open(fileName1, FileMode.Append);
                using StreamWriter write1 = new StreamWriter(appendText1);
                {
                    write1.WriteLine(String.Join(", ", _playerPosList));
                }
                write1.Close();

                if (_playerPosList.Count() > 0 && _playerPosList.Last() != _player._position)
                {
                    _path = FindShortestPath(_player._position);
                }

                FollowPath(_path);

                //Move the minion continuously in the current direction if valid

                //RandomMovement(_currentDirectionX, _currentDirectionY);
                //if (_movementTimer == _movementInterval)
                //{
                //FollowPath(FindShortestPath(_player._position));
                //Teleport();
                //    _movementTimer = 0;
                //}
                //_movementTimer++;
                _playerPosList.Add(_player._position);
                _movementTimer = 0;
            }
            _movementTimer++;
        }

        public void Teleport()
        {
            _position = _player._position;
        }

        public List<Vector2> FindShortestPath(Vector2 targetPosition)
        {
            // Create a queue for BFS
            Queue<Vector2> queue = new Queue<Vector2>();

            // Create a dictionary to track visited nodes and their parent nodes
            Dictionary<Vector2, Vector2> parentMap = new Dictionary<Vector2, Vector2>();

            // Enqueue the starting position
            queue.Enqueue(_position);

            // Mark the starting position as visited
            parentMap[_position] = _position;

            // Perform BFS
            while (queue.Count > 0)
            {
                Vector2 current = queue.Dequeue();

                // Check if we've reached the target position
                if (current == targetPosition)
                {
                    //write the ReconstructPath(parentMap, _position, targetPosition) to a txt file to check it is accurate and updating
                    string fileName = "Test-ShortestPathTracking.txt";
                    FileStream appendText = File.Open(fileName, FileMode.Append);
                    using StreamWriter write = new StreamWriter(appendText);
                    {
                        write.WriteLine(String.Join(", ", ReconstructPath(parentMap, _position, targetPosition)));
                    }
                    write.Close();

                    // Reconstruct the path
                    return ReconstructPath(parentMap, _position, targetPosition);
                }

                // Get neighbors of the current position
                List<Vector2> neighbors = GetNeighbors(current);

                // Enqueue unvisited neighbors
                foreach (Vector2 neighbor in neighbors)
                {
                    if (!parentMap.ContainsKey(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        parentMap[neighbor] = current;
                    }
                }
            }

            // If we reach here, there is no path to the target
            return new List<Vector2>();
        }

        private List<Vector2> ReconstructPath(Dictionary<Vector2, Vector2> parentMap, Vector2 start, Vector2 end)
        {
            List<Vector2> path = new List<Vector2>();
            Vector2 current = end;

            // Reconstruct the path by following parent pointers
            while (current != start)
            {
                path.Add(current);
                current = parentMap[current];
            }

            // Add the start position
            path.Add(start);

            // Reverse the path to get it in the correct order
            path.Reverse();

            return path;
        }

        private List<Vector2> GetNeighbors(Vector2 position)
        {
            List<Vector2> neighbors = new List<Vector2>();

            // Consider 4-directional movement (up, down, left, right)
            Vector2[] directions = { Vector2.UnitX, -Vector2.UnitX, Vector2.UnitY, -Vector2.UnitY };

            foreach (Vector2 dir in directions)
            {
                Vector2 neighbor = position + dir;

                // Check if the neighbor is within bounds and is not a wall
                if (_maze.ValidPosition((int)neighbor.X, (int)neighbor.Y) && _maze.maze[(int)neighbor.X, (int)neighbor.Y] != 0)
                {
                    neighbors.Add(neighbor);
                }
            }

            //for (int i = 0; i < neighbors.Count; i++)
            //{
            //    neighbors[i] = new Vector2 (neighbors[i].X * _movementSpeed, neighbors[i].Y * _movementSpeed);
            //}

            string fileName = "Test-Neighbours.txt";
            FileStream appendText = File.Open(fileName, FileMode.Append);
            using StreamWriter write = new StreamWriter(appendText);
            {
                write.WriteLine(String.Join(", ", neighbors));
            }
            write.Close();

            return neighbors;
        }

        private void FollowPath(List<Vector2> path)
        {
            //List<Vector2> newPath = new List<Vector2>(); //this will contain the path to the player in small incremental steps - slowing down the minion
            //for (int i = 0; i < path.Count; i++)
            //{
            //    path[i] = path[i] * _movementSpeed;
            //}
            //for (int u = 0 u < )
                // Move towards the next node in the path
            if (path.Count() > 0/* && _movementTimer == _movementInterval*/)
            {
                Vector2 nextNode = path[0];

                string fileName = "nextNode.txt";
                FileStream appendText = File.Open(fileName, FileMode.Append);
                using StreamWriter write = new StreamWriter(appendText);
                {
                    write.WriteLine(nextNode);
                }
                write.Close();


                //// Update the minion's position
                //_position = nextNode;

                if (_maze.ValidPosition((int)nextNode.X, (int)nextNode.Y) && _maze.maze[(int)nextNode.X, (int)nextNode.Y] != 0)
                {
                    Move((int)nextNode.X, (int)nextNode.Y);
                }
                // Remove the reached node from the path
                path.RemoveAt(0);
                //_movementTimer = 0;
            }
            //_movementTimer++;
        }

        private void RandomMovement(int dx, int dy)
        {
            int newX = (int)(_position.X + dx);
            int newY = (int)(_position.Y + dy);

            if (_maze.ValidPosition(newX, newY) && _maze.maze[newX, newY] != 0)
            {
                Move(newX, newY);
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
        }

        private void Move(int newX, int newY)
        {
            //Vector2 direction = Vector2.Normalize(new Vector2(newX, newY) - _position);
            //while (_position != direction)
            //{
            //    direction = Vector2.Normalize(new Vector2(newX, newY) - _position);
            //    _position += direction * _movementSpeed;
            //}
            //do
            //{
            //    _position.X += _movementSpeed;
            //}
            //while (_position.X < newX);
            _position.X = newX;
            _position.Y = newY;
            string fileName = "_position.txt";
            FileStream appendText = File.Open(fileName, FileMode.Append);
            using StreamWriter write = new StreamWriter(appendText);
            {
                write.WriteLine(_position);
            }
            write.Close();
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