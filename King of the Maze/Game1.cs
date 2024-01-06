using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
//using SharpDX.Direct2D1;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace King_of_the_Maze
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        private Maze _maze;
        private Base _base;
        private Player _player;
        private Minion _minion;
        private RenderTarget2D _mazeBuffer;
        private List<Minion> _minions = new List<Minion>();
        private List<Base> _bases = new List<Base>();
        private int _mazeSize = 91;
        private int _cellSize = 10;
        public int _maxMinionCount = 10;
        public int _spawnTimer;
        public int _spawnInterval = 300;
        private int _numberOfPlayers = 2;
        private List<Player> _players = new List<Player>();
        public Random rng = new Random();
        public Color[] _colours = { Color.Green, Color.Purple, Color.Blue, Color.HotPink}; 

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _maze = new Maze(_mazeSize, _cellSize);
            for (int i = 0; i < _numberOfPlayers; i++)
            {
                _player = new Player(_maze, _cellSize, _base, _bases, _colours, _players, GraphicsDevice);
                _players.Add(_player);
            }
            for (int i = 0; i < _mazeSize; i++)
            {
                for (int j = 0; j < _mazeSize; j++)
                {
                    if (_maze.maze[i, j] == 2)
                    {
                        _base = new Base(new Vector2(i, j), _cellSize, GraphicsDevice, _maze, _players);
                        _bases.Add(_base);
                    }
                }
            }
            _graphics.PreferredBackBufferWidth = _maze.mazeSize * _maze.cellSize;
            _graphics.PreferredBackBufferHeight = _maze.mazeSize * _maze.cellSize;
            _graphics.ApplyChanges();
            _mazeBuffer = new RenderTarget2D(GraphicsDevice, _maze.mazeSize * _maze.cellSize, _maze.mazeSize * _maze.cellSize);
            //_minion = new Minion(_maze, _cellSize, GraphicsDevice);
            //for (int i = 0; i < numberOfMinions; i++)
            //{
            //    _minion = new Minion(_maze, _player, _cellSize, GraphicsDevice);
            //    _minions.Add(_minion);
            //}

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            GraphicsDevice.SetRenderTarget(_mazeBuffer);
            GraphicsDevice.Clear(Color.Transparent); // Clear the buffer
            _spriteBatch.Begin();
            _maze.PrintMaze(_spriteBatch, _graphics);
            _spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            List<Player> removePlayers = new List<Player>();
            foreach (Player player in _players)
            {
                player.Move();
                removePlayers.Add(player.DespawnPlayer());
            }

            foreach (Player removePlayer in removePlayers)
            {
                _players.Remove(removePlayer);
            }

            //_minion.Move();
            if (_spawnTimer >= _spawnInterval && _minions.Count < _maxMinionCount)
            {
                _bases[rng.Next(0, _bases.Count)].SpawnMinions(_minions);
                _spawnTimer = 0;
            }
            _spawnTimer++;

            List<Minion> removeMinions = new List<Minion>();
            foreach (Minion minion in _minions)
            {
                minion.HandleMovement();
                minion.Attack();
                removeMinions.Add(minion.DespawnMinion());
            }

            foreach (Minion removeMinion in removeMinions)
            {
                _minions.Remove(removeMinion);
            }

            //string fileName = "RemoveMinions.txt";
            //FileStream appendText = File.Open(fileName, FileMode.Append);
            //using StreamWriter write = new StreamWriter(appendText);
            //{
            //    foreach (Minion removeMinion in _minions)
            //    {
            //        write.WriteLine(removeMinion);
            //    }
            //}
            //write.Close();

            //string fileName1 = "MinionsCount.txt";
            //FileStream appendText1 = File.Open(fileName1, FileMode.Append);
            //using StreamWriter write1 = new StreamWriter(appendText1);
            //{
            //    write1.WriteLine(_minions.Count);
            //}
            //write1.Close();

            if (_maze.GameOver(_players, _bases) != null)
            {
                //add some gameover screen and logic to maybe restart the game
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(_mazeBuffer, Vector2.Zero, Color.White);
            foreach (Player player in _players)
            {
                player.Draw(_spriteBatch);
            }
            //_minion.Draw(_spriteBatch);
            foreach (Minion minion in _minions)
            {
                minion.Draw(_spriteBatch);
            }
            foreach (Base Base in _bases)
            {
                Base.Draw(_spriteBatch);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}