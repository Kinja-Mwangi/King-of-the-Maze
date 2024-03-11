using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
//using SharpDX.Direct2D1;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;

namespace King_of_the_Maze
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager _graphics;
        public static SpriteBatch _spriteBatch;
        private MainMenu _mainMenu;
        public static StartMenu _startMenu;
        private Maze _maze;
        private Base _base;
        private Player _player;
        public static Player _winner;
        private PauseMenu _pauseMenu;
        private GameOver _gameOver;
        private Instructions _instructions;
        //private Minion _minion;
        private RenderTarget2D _mazeBuffer;
        private List<Minion> _minions = new List<Minion>();
        public static List<Base> _bases = new List<Base>();
        private int _mazeSize = 91;
        private int _cellSize = 10;
        public int _maxMinionCount = 10;
        public int _spawnTimer;
        public int _spawnInterval = 300;
        public static int _numberOfPlayers = 1;
        public static List<Player> _players = new List<Player>();
        public Random rng = new Random();
        public static Color[] _colours = { Color.Green, Color.Purple, Color.Blue};
        public GameState _currentState;
        public static MouseState _mouseState;
        private bool _playersSpawned;
        public static bool _gameInitialized;
        public static bool _gameSaved = false;
        private int _minionBoostTimer;
        private int _minionBoostInterval = 5300;
        private Song _menuMusic;
        private Song _playingMusic;
        private Song _victoryMusic;
        private Song _gameOverMusic;

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
            _graphics.PreferredBackBufferWidth = _maze.mazeSize * _maze.cellSize;
            _graphics.PreferredBackBufferHeight = _maze.mazeSize * _maze.cellSize;
            _graphics.ApplyChanges();
            _mazeBuffer = new RenderTarget2D(GraphicsDevice, _maze.mazeSize * _maze.cellSize, _maze.mazeSize * _maze.cellSize);
            _mainMenu = new MainMenu(GraphicsDevice);
            _startMenu = new StartMenu(GraphicsDevice);
            _pauseMenu = new PauseMenu(GraphicsDevice);
            _instructions = new Instructions(GraphicsDevice);
            _gameOver = new GameOver(GraphicsDevice);
            MediaPlayer.IsRepeating = true;

            for (int i = 0; i < _mazeSize; i++) //adds bases to the maze
            {
                for (int j = 0; j < _mazeSize; j++)
                {
                    if (_maze.maze[i, j] == 2)
                    {
                        _base = new Base(new Vector2(i, j), _cellSize, GraphicsDevice, _maze);
                        _bases.Add(_base);
                    }
                }
            }

            //for (int i = 0; i < _numberOfPlayers; i++)
            //{
            //    _player = new Player(_maze, _cellSize, _base, _bases, _players, GraphicsDevice);
            //    _players.Add(_player);
            //}

            _currentState = GameState.MainMenu;

            //_minion = new Minion(_maze, _cellSize, GraphicsDevice);
            //for (int i = 0; i < numberOfMinions; i++)
            //{
            //    _minion = new Minion(_maze, _player, _cellSize, GraphicsDevice);
            //    _minions.Add(_minion);
            //}
            _playersSpawned = false;
            _gameInitialized = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //Content.RootDirectory = "Content";

            // TODO: use this.Content to load your game content here
            _mainMenu._titleFont = Content.Load<SpriteFont>("Crang85");
            _mainMenu._buttonsTextFont = Content.Load<SpriteFont>("Crang45");
            _startMenu._titleFont = Content.Load<SpriteFont>("Crang85");
            _startMenu._buttonsTextFont = Content.Load<SpriteFont>("Crang45");
            _pauseMenu._buttonsTextFont = Content.Load<SpriteFont>("Crang45");
            _gameOver._buttonsTextFont = Content.Load<SpriteFont>("Crang45");
            _gameOver._gameOverFont = Content.Load<SpriteFont>("Crang65");
            _instructions._titleFont = Content.Load<SpriteFont>("Crang65");
            _instructions._instructionsFont = Content.Load<SpriteFont>("DigitalDisco9");
            _menuMusic = Content.Load<Song>("Menu Music");
            _playingMusic = Content.Load<Song>("Playing Music");
            _victoryMusic = Content.Load<Song>("Victory Music");
            _gameOverMusic = Content.Load<Song>("Game Over Music");
            GraphicsDevice.SetRenderTarget(_mazeBuffer);
            GraphicsDevice.Clear(Color.Transparent); // Clear the buffer
            _spriteBatch.Begin();
            _maze.PrintMaze(_spriteBatch, _graphics);
            _spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || _currentState == GameState.GameExit)
                Exit();

            // TODO: Add your update logic here
            if (_currentState == GameState.MainMenu)
            {
                if (!_gameInitialized)
                {
                    ResetGame();
                }
                _mainMenu.HandleButtons(ref _currentState);
            }
            else if (_currentState == GameState.GameInstructions)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Back))
                {
                    _currentState = GameState.MainMenu;
                }
            }
            else if (_currentState == GameState.StartMenu)
            {
                _startMenu.HandleButtons(ref _currentState);
                if (Keyboard.GetState().IsKeyDown(Keys.Back))
                {
                    _currentState = GameState.MainMenu;
                }
            }
            else if (_currentState == GameState.GamePaused)
            {
                _pauseMenu.HandleButtons(ref _currentState);
            }
            else if (_currentState == GameState.GameSaving)
            {
                SaveGame();
                _currentState = GameState.GamePaused;
                _gameSaved = true;
            }
            else if (_currentState == GameState.GameLoading)
            {
                ClearGame();
                if (Directory.Exists("saveFile"))
                {
                    LoadGame();
                    _currentState = GameState.GamePlaying;
                }
                else
                {
                    _currentState = GameState.StartMenu;
                }
            }
            //_gameSaved = false;
            else if (_currentState == GameState.GameRestarting)
            {
                ResetGame();
                _currentState = GameState.GamePlaying;
            }
            else if (_currentState == GameState.GamePlaying)
            {
                _gameSaved = false;
                if (!_playersSpawned)
                {
                    for (int i = 0; i < _numberOfPlayers; i++)
                    {
                        _player = new Player(_maze, _cellSize, _bases, _players, GraphicsDevice);
                        _players.Add(_player);
                    }
                    _playersSpawned = true;
                }
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
                if (_spawnTimer >= _spawnInterval && _minions.Count <= _maxMinionCount)
                {
                    _bases[rng.Next(0, _bases.Count)].SpawnMinions(ref _minions);
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

                if (_minionBoostTimer == _minionBoostInterval)
                {
                    foreach (Minion minion in _minions)
                    {
                        if (minion._movementInterval > 0)
                        {
                            minion._movementInterval--;
                        }
                        if (minion._trackingRange < 87)
                        {
                            minion._trackingRange += 4;
                        }
                    }
                    _maxMinionCount += 3;
                    if (_spawnInterval > 0)
                    {
                        _spawnInterval -= 20;
                    }
                    _minionBoostTimer = 0;
                }
                _minionBoostTimer++;

                if (Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    _currentState = GameState.GamePaused;
                }

                _winner = _maze.GameOver(_players, _bases, ref _currentState);
            }
            else if (_currentState == GameState.GameOver)
            {
                if (_winner != null)
                {
                    _gameOver._gameOverColour = _winner._playerColour;
                }
                else
                {
                    _gameOver._gameOverColour = Color.Red;
                }
                _gameOver.HandleButtons(ref _currentState);
            }
            HandleMusic();
            base.Update(gameTime);
        }

        protected void ResetGame()
        {
            ClearGame();
            Initialize();
            LoadContent();
        }

        protected void ClearGame()
        {
            _players.Clear();
            _minions.Clear();
            _bases.Clear();
        }

        protected void SaveGame()
        {
            Directory.CreateDirectory("saveFile");
            using (StreamWriter saveGame = new StreamWriter("saveFile/maze.txt"))
            {
                for (int y = 0; y < _mazeSize; y++)
                {
                    for (int x = 0; x < _mazeSize; x++)
                    {
                        saveGame.WriteLine(_maze.maze[x, y]);
                    }
                }
            }

            using (StreamWriter saveGame = new StreamWriter("saveFile/bases.txt"))
            {
                foreach (Base Base in _bases)
                {
                    if (Base._owner != null)
                    {
                        saveGame.Write("{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n", (int)Base._position.X, (int)Base._position.Y, Base._owner._playerNumber, Base._baseNumber, _spawnInterval, _maxMinionCount);
                    }
                    else
                    {
                        saveGame.Write("{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n", (int)Base._position.X, (int)Base._position.Y, 0, Base._baseNumber, _spawnInterval, _maxMinionCount);
                    }
                }
            }   
            
            using (StreamWriter saveGame = new StreamWriter("saveFile/players.txt"))
            {
                foreach (Player player in _players)
                {
                    saveGame.Write("{0}\n{1}\n{2}\n{3}\n", player._lives, player._playerNumber, (int)player._position.X, (int)player._position.Y);
                }
            }

            using (StreamWriter saveGame = new StreamWriter("saveFile/minions.txt"))
            {
                foreach (Minion minion in _minions)
                {
                    saveGame.Write("{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n", minion._lives, minion._minionLifeTimer, minion._base._baseNumber, (int)minion._position.X, (int)minion._position.Y, minion._movementInterval, minion._trackingRange);
                }
            }
        }

        protected void LoadGame()
        {
            using (StreamReader loadGame = new StreamReader("saveFile/maze.txt"))
            {
                for (int y = 0; y < _mazeSize; y++)
                {
                    for (int x = 0; x < _mazeSize; x++)
                    {
                        _maze.maze[x, y] = int.Parse(loadGame.ReadLine());
                    }
                }
            }

            using (StreamReader loadGame = new StreamReader("saveFile/players.txt"))
            {
                while (!loadGame.EndOfStream)
                {
                    int lives = int.Parse(loadGame.ReadLine());
                    int playerNumber = int.Parse(loadGame.ReadLine());
                    int positionX = int.Parse(loadGame.ReadLine());
                    int positionY = int.Parse(loadGame.ReadLine());
                    Player player = new Player(_maze, _cellSize, _bases, _players, GraphicsDevice);
                    player._position = new Vector2(positionX, positionY);
                    player._lives = lives;
                    player._playerNumber = playerNumber;
                    _players.Add(player);
                }
            }

            _numberOfPlayers = _players.Count;

            using (StreamReader loadGame = new StreamReader("saveFile/bases.txt"))
            {
                while (!loadGame.EndOfStream)
                {
                    int positionX = int.Parse(loadGame.ReadLine());
                    int positionY = int.Parse(loadGame.ReadLine());
                    int ownerNumber = int.Parse(loadGame.ReadLine());
                    int baseNumber = int.Parse(loadGame.ReadLine());
                    int spawnInterval = int.Parse(loadGame.ReadLine());
                    int maxMinionCount = int.Parse(loadGame.ReadLine());
                    Base Base = new Base(new Vector2(positionX, positionY), _cellSize, GraphicsDevice, _maze);
                    Base._baseNumber = baseNumber;
                    foreach (Player player in _players)
                    {
                        if (player._playerNumber == ownerNumber)
                        {
                            Base.CaptureBase(player);
                            break;
                        }
                    }
                    _spawnInterval = spawnInterval;
                    _maxMinionCount = maxMinionCount;
                    _bases.Add(Base);
                }
            }           
                        
            using (StreamReader loadGame = new StreamReader("saveFile/minions.txt"))
            {
                while (!loadGame.EndOfStream)
                {
                    int lives = int.Parse(loadGame.ReadLine());
                    int minionLifeTimer = int.Parse(loadGame.ReadLine());
                    int baseNumber = int.Parse(loadGame.ReadLine());
                    int positionX = int.Parse(loadGame.ReadLine());
                    int positionY = int.Parse(loadGame.ReadLine());   
                    int movementInterval = int.Parse(loadGame.ReadLine());
                    int trackingRange = int.Parse(loadGame.ReadLine());
                    Base minionBase = null;
                    foreach (Base Base in _bases)
                    {
                        if (Base._baseNumber == baseNumber)
                        {
                            minionBase = Base;
                        }
                    }
                    Minion minion = new Minion(_maze, _cellSize, GraphicsDevice, minionBase);
                    minion._lives = lives;
                    minion._position = new Vector2(positionX, positionY);
                    minion._minionLifeTimer = minionLifeTimer;
                    minion._trackingRange = trackingRange;
                    minion._movementInterval = movementInterval;
                    _minions.Add(minion);
                }
            }

            //_mazeBuffer = new RenderTarget2D(GraphicsDevice, _maze.mazeSize * _maze.cellSize, _maze.mazeSize * _maze.cellSize);
            _playersSpawned = true;
            LoadContent();
        }

        protected void HandleMusic()
        {
            if (_currentState == GameState.MainMenu)
            {
                MediaPlayer.Volume = 0.8f;
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Play(_menuMusic);
                }
            }
            else if (_currentState == GameState.GamePlaying)
            {
                MediaPlayer.Volume = 1f;
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Play(_playingMusic);
                }
            }
            else if (_currentState == GameState.GamePaused)
            {
                MediaPlayer.Volume = 0.4f;
            }
            else if (_currentState == GameState.GameOver)
            {
                if (_winner == null)
                {
                    if (MediaPlayer.State != MediaState.Playing)
                    {
                        MediaPlayer.Play(_gameOverMusic);
                    }
                }
                else
                {
                    if (MediaPlayer.State != MediaState.Playing)
                    {
                        MediaPlayer.Play(_victoryMusic);
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            if (_currentState == GameState.MainMenu)
            {
                _mainMenu.Draw(_spriteBatch);
            }
            else if (_currentState == GameState.StartMenu || _currentState == GameState.GameLoading)
            {
                _startMenu.Draw(_spriteBatch);
            }
            else if (_currentState == GameState.GameInstructions)
            {
                _instructions.Draw(_spriteBatch);
            }
            else if (_currentState == GameState.GamePlaying)
            {
                _spriteBatch.Draw(_mazeBuffer, Vector2.Zero, Color.White);

                //_minion.Draw(_spriteBatch);

                foreach (Base Base in _bases)
                {
                    Base.Draw(_spriteBatch);
                }

                foreach (Player player in _players)
                {
                    player.Draw(_spriteBatch);
                }

                foreach (Minion minion in _minions)
                {
                    minion.Draw(_spriteBatch);
                }
            }
            else if (_currentState == GameState.GamePaused || _currentState == GameState.GameSaving)
            {
                _spriteBatch.Draw(_mazeBuffer, Vector2.Zero, Color.White);
                _pauseMenu.Draw(_spriteBatch);
            }
            else if (_currentState == GameState.GameRestarting)
            {
                _spriteBatch.Draw(_mazeBuffer, Vector2.Zero, Color.White);
            }
            else if (_currentState == GameState.GameOver)
            {
                _spriteBatch.Draw(_mazeBuffer, Vector2.Zero, Color.White);
                _gameOver.Draw(_spriteBatch);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}