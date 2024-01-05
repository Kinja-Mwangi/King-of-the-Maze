using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace King_of_the_Maze
{
    public class Base
    {
        public Vector2 _position;
        public Player _owner;
        public bool _captured;
        public int _cellSize;
        public Player _player;
        public Maze _maze;
        private Texture2D _baseTexture;
        public GraphicsDevice _graphicsDevice;
        public Color _baseColour;
        public List<Player> _players = new List<Player>();
        private Random rng = new Random();

        public Base(Vector2 position, int cellSize, GraphicsDevice graphicsDevice, Maze maze, List<Player> players)
        {
            _position = position;
            _players = players;
            _cellSize = cellSize;
            _maze = maze;
            _baseColour = Color.Red;
            _graphicsDevice = graphicsDevice;
            _baseTexture = new Texture2D(_graphicsDevice, 1, 1);
            _baseTexture.SetData(new Color[] { Color.White });
        }

        public void CaptureBase(Player player)
        {
            _baseColour = player._playerColour;
            _owner = player;
        }

        public void SpawnMinions(List<Minion> minions)
        {
            Minion minion = new Minion(_maze, _cellSize, _graphicsDevice, minions, this);
            minions.Add(minion);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle baseRect = new Rectangle((int)(_position.X * _cellSize), (int)(_position.Y * _cellSize), _cellSize, _cellSize);
            spriteBatch.Draw(_baseTexture, baseRect, _baseColour); // You can customize the base's appearance here
        }
    }
}