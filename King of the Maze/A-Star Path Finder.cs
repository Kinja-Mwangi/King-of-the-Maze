using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King_of_the_Maze
{
    public class AStarPathfinder
    {
        private class Node
        {
            public int X;
            public int Y;
            public int G; // Cost from the start node to this node
            public int H; // Estimated cost from this node to the target node
            public int F => G + H; // Total cost (F = G + H)
            public Node Parent; // Parent node for reconstructing the path
        }

        public List<Vector2> FindPath(Vector2 start, Vector2 end, Maze maze)
        {
            int startX = (int)start.X;
            int startY = (int)start.Y;
            int endX = (int)end.X;
            int endY = (int)end.Y;

            List<Node> openList = new List<Node>();
            List<Node> closedList = new List<Node>();

            Node startNode = new Node { X = startX, Y = startY };
            Node targetNode = new Node { X = endX, Y = endY };

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                Node currentNode = openList.OrderBy(node => node.F).First();

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                if (currentNode.X == targetNode.X && currentNode.Y == targetNode.Y)
                {
                    return ReconstructPath(currentNode);
                }

                List<Node> neighbors = GetNeighbors(currentNode, maze);

                foreach (Node neighbor in neighbors)
                {
                    if (closedList.Any(node => node.X == neighbor.X && node.Y == neighbor.Y))
                        continue;

                    int tentativeG = currentNode.G + 1;

                    if (!openList.Any(node => node.X == neighbor.X && node.Y == neighbor.Y) || tentativeG < neighbor.G)
                    {
                        neighbor.G = tentativeG;
                        neighbor.H = ManhattanDistance(neighbor, targetNode);
                        neighbor.Parent = currentNode;

                        if (!openList.Any(node => node.X == neighbor.X && node.Y == neighbor.Y))
                            openList.Add(neighbor);
                    }
                }
            }

            return new List<Vector2>();
        }

        private int ManhattanDistance(Node nodeA, Node nodeB)
        {
            return Math.Abs(nodeA.X - nodeB.X) + Math.Abs(nodeA.Y - nodeB.Y);
        }

        private List<Vector2> ReconstructPath(Node node)
        {
            List<Vector2> path = new List<Vector2>();

            while (node != null)
            {
                path.Add(new Vector2(node.X, node.Y));
                node = node.Parent;
            }

            path.Reverse();
            return path;
        }

        private List<Node> GetNeighbors(Node node, Maze maze)
        {
            int[] dx = { 0, 0, -1, 1 };
            int[] dy = { -1, 1, 0, 0 };

            List<Node> neighbors = new List<Node>();

            for (int i = 0; i <= 3; i++)
            {
                int newX = node.X + dx[i];
                int newY = node.Y + dy[i];

                if (maze.ValidPosition(newX, newY) && maze.maze[newX, newY] != 0)
                {
                    neighbors.Add(new Node { X = newX, Y = newY });
                }
            }

            return neighbors;
        }
    }

}