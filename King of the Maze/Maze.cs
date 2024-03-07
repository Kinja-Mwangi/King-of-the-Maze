using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King_of_the_Maze
{
    public class Maze
    {
        public int cellSize;
        public int[,] maze;
        public int mazeSize;
        private Random rng = new Random();

        public Maze(int size, int sizeOfCell)
        {
            //note - change maze so that it can be a rectangle rather than a square
            mazeSize = size;
            cellSize = sizeOfCell;
            maze = new int[mazeSize, mazeSize];
            GenerateMaze();
        }

        private void NewBlankMaze()
        {
            for (int i = 0; i < mazeSize; i++)
            {
                for (int j = 0; j < mazeSize; j++)
                {
                    maze[i, j] = 0; // Initialize all cells as walls
                }
            }
        }

        private void GenerateMaze()
        {
            NewBlankMaze();
            RecursiveBacktracking(1, 1);
            //Mark a 3 * 3 space at center of sub - mazes
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    for (int p = 5; p <= mazeSize; p += 15)
                    {
                        for (int q = 5; q <= mazeSize; q += 15)
                        {
                            if (i == 0 && j == 0 && rng.Next(0, 2) == 0)
                            {
                                maze[q + i, p + j] = 2;
                            }
                            else //randomly spawns bases
                            {
                                maze[q + i, p + j] = 1;
                            }
                        }
                    }
                }
            }
        }

        private void RecursiveBacktracking(int x, int y)
        {
            maze[x, y] = 1; // Mark the current cell as an open path

            // Define the order of the directions: up, right, down, left
            int[] directions = { 0, 1, 2, 3 };
            RandomizeArray(directions);

            foreach (int direction in directions)
            {
                int newX = x;
                int newY = y;

                if (direction == 0) // Up
                {
                    newY -= 2;
                }
                else if (direction == 1) // Right
                {
                    newX += 2;
                }
                else if (direction == 2) // Down
                {
                    newY += 2;
                }
                else if (direction == 3) // Left
                {
                    newX -= 2;
                }

                if (ValidPosition(newX, newY) && maze[newX, newY] == 0)//creates surrounding walls by not allowing path finder to go beyond the perimeter of the original square
                {
                    maze[newX, newY] = 1; // Mark the new cell as an open path
                    maze[(newX + x) / 2, (newY + y) / 2] = 1; // Remove the wall between the current and new cell
                    RecursiveBacktracking(newX, newY); // Recursively create the maze from the new cell
                }
            }
        }

        public bool ValidPosition(int newX, int newY)
        {
            if (newX >= 1 && newX < mazeSize && newY >= 1 && newY < mazeSize)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RandomizeArray(int[] array)
        {
            Random random = new Random();

            for (int i = 0; i < array.Length; i++)
            {
                int j = random.Next(0, i + 1);
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }

        public Player GameOver(List<Player> players, List<Base> bases, ref GameState currentState)
        {
            if (players.Count == 1 && Game1._numberOfPlayers > 1)
            {
                currentState = GameState.GameOver;
                return players[0];
            }
            else
            {
                foreach (Player player in players)
                {
                    int basesCaptured = 0;
                    foreach (Base Base in bases)
                    {
                        if (Base._owner == player)
                        {
                            basesCaptured++;
                        }
                    }
                    if (basesCaptured == bases.Count)
                    {
                        currentState = GameState.GameOver;
                        return player;
                    }
                }
            }
            return null;
        }

        public void PrintMaze(SpriteBatch sb, GraphicsDeviceManager gd)
        {
            for (int i = 0; i < mazeSize; i++)
            {
                for (int j = 0; j < mazeSize; j++)
                {
                    Texture2D _texture = new Texture2D(gd.GraphicsDevice, 1, 1); //make a pixel sized texture
                    if (maze[i, j] == 0)
                    {
                        _texture.SetData(new Color[] { Color.Black }); //walls are black squares
                    }
                    //else if (maze[i, j] == 2)
                    //{
                    //    _texture.SetData(new Color[] { Color.Red }); //bases are red squares
                    //}
                    else
                    {
                        _texture.SetData(new Color[] { Color.White }); //corridors are white squares
                    }
                    Rectangle position = new Rectangle(i * cellSize, j * cellSize, cellSize, cellSize);
                    sb.Draw(_texture, position, Color.White);

                }
            }
        }
        //public void PrintMaze() (old)
        //{
        //    // Print top border
        //    for (int i = 0; i < mazeSize + 1; i++)
        //    {
        //        Console.Write("██");
        //    }
        //    Console.WriteLine();

        //    // Print maze contents with side borders
        //    for (int i = 0; i < mazeSize; i++)
        //    {
        //        Console.Write("██"); // Left border

        //        for (int j = 0; j < mazeSize; j++)
        //        {
        //            if (maze[j, i] == 0)
        //            {
        //                Console.Write("██"); // Print walls
        //            }
        //            else if (maze[j, i] == 2)
        //            {
        //                Console.ForegroundColor = ConsoleColor.Red;
        //                Console.Write("██"); // Print bases
        //                Console.ForegroundColor = ConsoleColor.Gray;
        //            }
        //            else
        //            {
        //                Console.Write("  "); // Print open paths
        //            }
        //        }
        //        Console.WriteLine();
        //    }
        //    Console.WriteLine();
        //}
    }
}