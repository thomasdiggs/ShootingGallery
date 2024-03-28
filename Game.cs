using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShootingGallery
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        // A SpriteBatch is used to draw 2D images to the screen.
        private SpriteBatch _spriteBatch;

        Texture2D targetSprite;
        Texture2D crosshairsSprite;
        Texture2D backgroundSprite;
        SpriteFont gameFont; // .spritefont files are used to store font data for use in the game.
        Vector2 targetPosition = new(300, 300);
        const int targetRadius = 45;
        MouseState mState;
        bool mReleased = true;
        int score = 0;
        double timer = 10;
        int crosshairsRadius = 25;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        // Runs once at the start of the game to initialize the game world such as setting up variables, etc.
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        // Runs once at the start of the game to load game content such as images, sounds, etc.
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            // The Load method is used to load a texture from the Content Pipeline.
            // The Content Pipeline is a tool that processes game assets such as images, sounds, and 3D models into a format that can be used by the game at runtime.
            targetSprite = Content.Load<Texture2D>("target");
            crosshairsSprite = Content.Load<Texture2D>("crosshairs");
            backgroundSprite = Content.Load<Texture2D>("sky");
            gameFont = Content.Load<SpriteFont>("galleryFont");
        }

        // Runs once per frame to update the game world such as checking for collisions, gathering input, etc.
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            // GameTime is a class that provides a snapshot of the game timing state. It provides the elapsed time since the last update, the total time the game has been running, etc.
            if (timer > 0)
            {
                timer -= gameTime.ElapsedGameTime.TotalSeconds; // gameTime.ElapsedGameTime.TotalSeconds is the time elapsed since the last update in seconds.
            }
            if (timer < 0)
            {
                timer = 0;
            }
            mState = Mouse.GetState(); // Get the current state of the mouse. States could be the position of the mouse, the state of the buttons, etc.
            if (mState.LeftButton == ButtonState.Pressed && mReleased)
            {
                float mouseTagetDist = Vector2.Distance(targetPosition, mState.Position.ToVector2());
                if (mouseTagetDist < targetRadius && timer > 0)
                {
                    score++;
                    Random rand = new Random();
                    targetPosition.X = rand.Next(0 + 45, _graphics.PreferredBackBufferWidth - 45); // PreferredBackBufferWidth is the width of the window.
                    targetPosition.Y = rand.Next(0 + 45, _graphics.PreferredBackBufferHeight - 45); // PreferredBackBufferHeight is the height of the window.
                }
                mReleased = false;
            }
            if (mState.LeftButton == ButtonState.Released)
            {
                mReleased = true;
            }

            base.Update(gameTime);
        }

        // Runs once per frame and is used to draw game content such as sprites, text, etc.
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(); // The Begin method must be called before any calls to draw sprites.
            // We draw the background sprite first so that it appears behind the other sprites. This is the concept of drawing order.
            _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.White); // Draw the background sprite at position (0,0) with no tint.
            _spriteBatch.DrawString(gameFont, "Score: " + score.ToString(), new Vector2(5, 5), Color.White); // We choose White as the tint color because we want the sprite to be drawn as it is.
            _spriteBatch.DrawString(gameFont, "Timer: " + Math.Ceiling(timer).ToString(), new Vector2(5, 45), Color.White);
            if (timer > 0)
            {
                _spriteBatch.Draw(targetSprite, new Vector2(targetPosition.X - targetRadius, targetPosition.Y - targetRadius), Color.White);
            }
            _spriteBatch.Draw(crosshairsSprite, new Vector2(mState.X - crosshairsRadius, mState.Y - crosshairsRadius), Color.White);
            _spriteBatch.End(); // The End method must be called after all calls to draw sprites.

            base.Draw(gameTime);
        }
    }
}
