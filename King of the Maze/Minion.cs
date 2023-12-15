using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private Vector2 _minionPosition;
        private int _currentDirectionX;
        private int _currentDirectionY;
        private int _movementTimer = 0;
        private int _movementInterval = 5; // Adjust this value for slower or faster movement
        private Texture2D _minionTexture;

        public Minion(Maze maze, Player player, int cellSize, GraphicsDevice graphicsDevice)
        {
            _maze = maze;
            _player = player;
            _cellSize = cellSize;
            _minionPosition = new Vector2(5, 6);

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
            ////write the _player._playerPosition to a txt file to check it is accurate and updating
            //string fileName = "Test-PlayerPositionTracking.txt";
            //FileStream appendText = File.Open(fileName, FileMode.Append);
            //using StreamWriter write = new StreamWriter(appendText);
            //{
            //    write.WriteLine(_player._playerPosition);
            //}
            //write.Close();

            //Move the minion continuously in the current direction if valid
            if (_movementTimer == _movementInterval)
            {
                //RandomMovement(_currentDirectionX, _currentDirectionY);
                FollowPath(FindShortestPath(_player._playerPosition));
                _movementTimer = 0;
            }
            _movementTimer++;
        }

        public List<Vector2> FindShortestPath(Vector2 targetPosition)
        {
            // Create a queue for BFS
            Queue<Vector2> queue = new Queue<Vector2>();

            // Create a dictionary to track visited nodes and their parent nodes
            Dictionary<Vector2, Vector2> parentMap = new Dictionary<Vector2, Vector2>();

            // Enqueue the starting position
            queue.Enqueue(_minionPosition);

            // Mark the starting position as visited
            parentMap[_minionPosition] = _minionPosition;

            // Perform BFS
            while (queue.Count > 0)
            {
                Vector2 current = queue.Dequeue();

                // Check if we've reached the target position
                if (current == targetPosition)
                {
                    //write the ReconstructPath(parentMap, _minionPosition, targetPosition) to a txt file to check it is accurate and updating
                    string fileName = "Test-ShortestPathTracking.txt";
                    FileStream appendText = File.Open(fileName, FileMode.Append);
                    using StreamWriter write = new StreamWriter(appendText);
                    {
                        write.WriteLine(String.Join(", ", ReconstructPath(parentMap, _minionPosition, targetPosition)));
                    }
                    write.Close();

                    // Reconstruct the path
                    return ReconstructPath(parentMap, _minionPosition, targetPosition);
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
            // Move towards the next node in the path
            Vector2 nextNode = path[0];

            // Update the minion's position
            _minionPosition = nextNode;

            // Remove the reached node from the path
            path.RemoveAt(0);
        }

        private void RandomMovement(int dx, int dy)
        {
            int newX = (int)(_minionPosition.X + dx);
            int newY = (int)(_minionPosition.Y + dy);

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
            _minionPosition.X = newX;
            _minionPosition.Y = newY;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the minion as a single pixel
            Rectangle minionRect = new Rectangle((int)(_minionPosition.X * _cellSize), (int)(_minionPosition.Y * _cellSize), _cellSize, _cellSize);
            spriteBatch.Draw(_minionTexture, minionRect, Color.CornflowerBlue);
            //to do - draw the minion as a triangle making it easier to differentiate from players
        }
    }
}