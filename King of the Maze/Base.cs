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
        public int _owner;
        public bool _captured;
        public int _cellSize;
        public Player _player;
        public Maze _maze;
        private Texture2D _baseTexture;
        public GraphicsDevice _graphicsDevice;
        private int _maxMinionCount;
        private List<Base> _bases = new List<Base>();
        private Base _base;
        private Random rng = new Random();

        public Base(Vector2 position, int cellSize, GraphicsDevice graphicsDevice, Player player, Maze maze, int maxMinionCount, Base Base, List<Base> bases)
        {
            _base = Base;
            _bases = bases;
            _position = position;
            _cellSize = cellSize;
            _player = player;
            _maze = maze;
            _maxMinionCount = maxMinionCount;
            _graphicsDevice = graphicsDevice;
            _baseTexture = new Texture2D(_graphicsDevice, 1, 1);
            _baseTexture.SetData(new Color[] { Color.White });
        }

        public void CaptureBase()
        {
            if(_player._position == _position)
            {
            }
        }

        public void SpawnMinions(List<Minion> minions)
        {
            Minion minion = new Minion(_maze, _player, _cellSize, _graphicsDevice, _position, minions);
            minions.Add(minion);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle baseRect = new Rectangle((int)(_position.X * _cellSize), (int)(_position.Y * _cellSize), _cellSize, _cellSize);
            spriteBatch.Draw(_baseTexture, baseRect, Color.Red); // You can customize the player's appearance here
        }

    }
}
