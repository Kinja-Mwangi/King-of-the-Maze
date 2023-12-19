using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
//using SharpDX.Direct2D1;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace King_of_the_Maze
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        private Maze _maze;
        private Player _player;
        private Minion _minion;
        private RenderTarget2D _mazeBuffer;
        private List<Minion> _minions = new List<Minion>();
        private int numberOfMinions = 5;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _maze = new Maze(91, 10);
            _graphics.PreferredBackBufferWidth = _maze.mazeSize * _maze.cellSize;
            _graphics.PreferredBackBufferHeight = _maze.mazeSize * _maze.cellSize;
            _graphics.ApplyChanges();
            _mazeBuffer = new RenderTarget2D(GraphicsDevice, _maze.mazeSize * _maze.cellSize, _maze.mazeSize * _maze.cellSize);
            _player = new Player(_maze, 10, GraphicsDevice);
            //_minion = new Minion(_maze, 10, GraphicsDevice);
            for (int i = 0; i < numberOfMinions; i++)
            {
                _minion = new Minion(_maze, _player, 10, GraphicsDevice);
                _minions.Add(_minion);
            }

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
            _player.Move();
            //_minion.Move();
            foreach (Minion minion in _minions)
            {
                minion.HandleMovement();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(_mazeBuffer, Vector2.Zero, Color.White);
            _player.Draw(_spriteBatch);
            //_minion.Draw(_spriteBatch);
            foreach (Minion minion in _minions)
            {
                minion.Draw(_spriteBatch);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}