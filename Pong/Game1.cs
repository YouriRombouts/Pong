using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont Font;
        Texture2D m_BarShape1;
        Texture2D m_BarShape2;
        Texture2D m_BallShape;
        Bar m_Bar1;
        Bar m_Bar2;
        Ball m_Ball;

        public class Ball
        {            
            int m_Size = 0;
            Vector2 m_Vel = new Vector2(0, 0);
            Vector2 m_Pos = new Vector2(0, 0);
            public Ball(Vector2 Pos,int Size, Vector2 Vel) { m_Pos = Pos; m_Size = Size; m_Vel = Vel; }
            public int GetSize() { return m_Size; }
            public Vector2 GetPos() { return m_Pos; }
            public void MoveVertical(int distance) { m_Pos.Y += distance; }
            public void MoveHorizontal(int distance) { m_Pos.X += distance; }
            public float GetPosY() { return m_Pos.Y; }
            public float GetPosX() { return m_Pos.X; }
            public float GetVelY() { return m_Vel.Y; }
            public float GetVelX() { return m_Vel.X; }
            public void SetPosX(float NewBallPosX) { m_Pos.X = NewBallPosX; }
            public void SetPosY(float NewBallPosY) { m_Pos.Y = NewBallPosY; }
            public void InverseVelX() { m_Vel.X *= -1; }
            public void InverseVelY() { m_Vel.Y *= -1; }
        }

        public class Bar
        {
            int m_Height = 150;
            int m_Width = 10;
            float m_Vel = 0;
            float m_MaxVel = 200;
            Vector2 m_Pos = new Vector2(0, 0);
            public Bar(Vector2 Pos){ m_Pos = Pos; }
            public int GetHeight() { return m_Height; }
            public int GetWidth() { return m_Width; }
            public float GetVel() { return m_Vel; }
            public Vector2 GetPos() { return m_Pos; }
            public float GetPosY() { return m_Pos.Y; }
            public void MoveVertical(int distance) { m_Pos.Y += distance; }
            public void MoveHorizontal(int distance) { m_Pos.X += distance; }
            public void SetPos(float NewPos) { m_Pos.Y = NewPos; }
            public void SetVel(float Vel) { m_Vel = Vel; }
            public float GetMaxVel() { return m_MaxVel; }
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            m_Bar1 = new Bar(new Vector2(0, (graphics.GraphicsDevice.Viewport.Height / 2)));
            m_Bar2 = new Bar(new Vector2(0, (graphics.GraphicsDevice.Viewport.Height / 2)));
            m_Ball = new Ball(new Vector2((graphics.GraphicsDevice.Viewport.Width / 2), (graphics.GraphicsDevice.Viewport.Height / 2)), 10, new Vector2(100,100));
            m_Bar1.MoveVertical(-(m_Bar1.GetHeight()) / 2);
            m_Bar2.MoveVertical(-(m_Bar2.GetHeight()) / 2);
            m_Ball.MoveVertical(-m_Ball.GetSize() / 2);
            m_Bar2.MoveHorizontal(graphics.GraphicsDevice.Viewport.Width-m_Bar2.GetWidth());

            Font = Content.Load<SpriteFont>("Score");
            m_Ball.MoveHorizontal(-m_Ball.GetSize() / 2);
            //SpriteFont font = Content.Load<SpriteFont>("Score.spritefont");

            m_BarShape1 = new Texture2D(graphics.GraphicsDevice, m_Bar1.GetWidth(), m_Bar1.GetHeight());
            m_BarShape2 = new Texture2D(graphics.GraphicsDevice, m_Bar2.GetWidth(), m_Bar2.GetHeight());
            m_BallShape = new Texture2D(graphics.GraphicsDevice, m_Ball.GetSize(), m_Ball.GetSize());
            Color[] data = new Color[80 * 30];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            m_BarShape1.SetData(data);
            m_BarShape2.SetData(data);
            m_BallShape.SetData(data);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            KeyboardState Keystate = Keyboard.GetState();
<<<<<<< HEAD
            if (Keyboard.GetState().IsKeyDown(Keys.S)) { m_Bar1.SetVel(m_Bar1.GetMaxVel()); }
            if (Keyboard.GetState().IsKeyDown(Keys.W)) { m_Bar1.SetVel(-m_Bar1.GetMaxVel()); }
            if (Keystate.IsKeyDown(Keys.Down)) { m_Bar2.SetVel(m_Bar2.GetMaxVel()); }
            if (Keystate.IsKeyDown(Keys.Up)) { m_Bar2.SetVel(-m_Bar2.GetMaxVel()); }
            if (m_Bar1.GetPosY() <= 0 && m_Bar1.GetVel() < 0 || Keyboard.GetState().IsKeyUp(Keys.S) && m_Bar1.GetVel() > 0) { m_Bar1.SetVel(0); }
            if (m_Bar1.GetPosY() + m_Bar2.GetHeight() >= graphics.GraphicsDevice.Viewport.Height && m_Bar1.GetVel() > 0 || Keyboard.GetState().IsKeyUp(Keys.W) && m_Bar1.GetVel() < 0) { m_Bar1.SetVel(0); }
            if (m_Bar2.GetPosY() <= 0 && m_Bar2.GetVel() < 0 || Keyboard.GetState().IsKeyUp(Keys.Down) && m_Bar2.GetVel() > 0) { m_Bar2.SetVel(0); }
            if (m_Bar2.GetPosY() + m_Bar2.GetHeight() >= graphics.GraphicsDevice.Viewport.Height && m_Bar2.GetVel() > 0 || Keyboard.GetState().IsKeyUp(Keys.Up) && m_Bar2.GetVel() < 0) { m_Bar2.SetVel(0); }
=======
            if (Keystate.IsKeyDown(Keys.S) && m_Bar1.GetPosY() <= (graphics.GraphicsDevice.Viewport.Height - m_Bar1.GetHeight())) { m_Bar1.MoveVertical(10); } 
            if (Keystate.IsKeyDown(Keys.W)) { m_Bar1.MoveVertical(-10); }
            if (Keystate.IsKeyDown(Keys.Down)) { m_Bar2.MoveVertical(10); }
            if (Keystate.IsKeyDown(Keys.Up)) { m_Bar2.MoveVertical(-10); }
            //if (m_Bar1.GetPosY() <= 0) { m_Bar1.StopUp(); }
            //if (m_Bar1.GetPosY() + m_Bar1.GetHeight() >= graphics.GraphicsDevice.Viewport.Height) { m_Bar1.StopDown(); }
            //if (m_Bar2.GetPosY() <= 0) { m_Bar2.StopUp(); }
            //if (m_Bar2.GetPosY() + m_Bar1.GetHeight() >= graphics.GraphicsDevice.Viewport.Height) { m_Bar2.StopDown(); }
>>>>>>> 46376dcb7038a31ef3994f3c313598f70bdcab82
            float MovedPos1 = m_Bar1.GetPosY() + m_Bar1.GetVel() * (float)gameTime.ElapsedGameTime.TotalSeconds;
            float MovedPos2 = m_Bar2.GetPosY() + m_Bar2.GetVel() * (float)gameTime.ElapsedGameTime.TotalSeconds;
            float MovedBallPosX = m_Ball.GetPosX() + m_Ball.GetVelX() * (float)gameTime.ElapsedGameTime.TotalSeconds;
            float MovedBallPosY = m_Ball.GetPosY() + m_Ball.GetVelY() * (float)gameTime.ElapsedGameTime.TotalSeconds;
            m_Bar1.SetPos(MovedPos1);
            m_Bar2.SetPos(MovedPos2);
            m_Ball.SetPosX(MovedBallPosX);
            m_Ball.SetPosY(MovedBallPosY);
            if (m_Ball.GetPosY() + m_Ball.GetSize() >= graphics.GraphicsDevice.Viewport.Height && m_Ball.GetVelX() > 0)
            {
                m_Ball.InverseVelY();
            }
            else if ( m_Ball.GetPosY() < 0 && m_Ball.GetVelY() < 0)
            {
                m_Ball.InverseVelY();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGreen);
            // TODO: Add your drawing code here
            // test
            spriteBatch.Begin();
            spriteBatch.DrawString(Font, "Test", new Vector2(graphics.GraphicsDevice.Viewport.Width/2, 100), Color.White);
            spriteBatch.Draw(m_BarShape1, m_Bar1.GetPos(), Color.Red);
            spriteBatch.Draw(m_BarShape2, m_Bar2.GetPos(), Color.White);
            spriteBatch.Draw(m_BallShape, m_Ball.GetPos(), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
