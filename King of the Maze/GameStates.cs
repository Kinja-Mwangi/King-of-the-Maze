using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King_of_the_Maze
{
    public class GameStates
    {
        public enum GameState
        {
            GameMenu,
            GamePlaying,
            GamePaused,
            GameOver
        }
        public GameState CurrentState { get; private set; }

        public void ChangeState(GameState newState)
        {
            CurrentState = newState;
            // Add any additional logic for state transitions, such as initializing state-specific content.
        }

        public void Update(GameTime gameTime)
        {
            // Update logic based on the current state.
            switch (CurrentState)
            {
                case GameState.GameMenu:
                    // Update main menu logic.
                    break;
                case GameState.GamePlaying:
                    // Update gameplay logic.
                    break;
                case GameState.GamePaused:
                    // Update pause menu logic.
                    break;
                case GameState.GameOver:
                    // Update game over logic.
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw content based on the current state.
            switch (CurrentState)
            {
                case GameState.GameMenu:
                    // Draw main menu content.
                    break;
                case GameState.GamePlaying:
                    // Draw gameplay content.
                    break;
                case GameState.GamePaused:
                    // Draw pause menu content.
                    break;
                case GameState.GameOver:
                    // Draw game over content.
                    break;
            }
        }
    }
}
