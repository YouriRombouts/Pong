using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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
        Texture2D Heart;
        Bar m_Bar1;
        Bar m_Bar2;
        Lives m_Lives1;
        Lives m_Lives2;
        Ball m_Ball;
        Song Music;
        Button PlayButton;


        public class Ball
        {            
            int m_Size = 0;
            Vector2 m_Vel = new Vector2(0, 0);
            Vector2 m_Pos = new Vector2(0, 0);
            public Ball(Vector2 Pos,int Size, Vector2 Vel) { m_Pos = Pos; m_Size = Size; m_Vel = Vel; }
            public int GetSize() { return m_Size; }
            public Vector2 GetPos() { return m_Pos; }
            public int GetMidPos() { return (int)m_Pos.Y + m_Size/2; }
            public void MoveVertical(int distance) { m_Pos.Y += distance; }
            public void MoveHorizontal(int distance) { m_Pos.X += distance; }
            public float GetPosY() { return m_Pos.Y; }
            public float GetPosX() { return m_Pos.X; }
            public float GetVelY() { return m_Vel.Y; }
            public float GetVelX() { return m_Vel.X; }
            public void SetPosX(float NewBallPosX) { m_Pos.X = NewBallPosX; }
            public void SetPosY(float NewBallPosY) { m_Pos.Y = NewBallPosY; }
            public void SetPos(Vector2 NewBallPos) { m_Pos = NewBallPos; }
            public void SetVelX(int NewVelX) { m_Vel.X = NewVelX; }
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
        public class Lives
        {
            int m_Lives = 3;
            public void RemoveOne() { m_Lives -= 1; }
            public string GetLivesStr() { return m_Lives.ToString(); }
            public int GetLivesInt() { return m_Lives; }
        }

        enum Gamestate
        {
            MainMenu,
            Options,
            Playing,
            GameOver
        }

        Gamestate CurrentGameState = Gamestate.MainMenu;

        class Button
        {
            Texture2D Texture;
            Vector2 Position;
            Rectangle Rectangle;

            Color Colour = new Color(255, 255, 255, 255);
            public Vector2 Size;
            public Button(Texture2D NewTexture ,GraphicsDevice graphics)
            {
                Texture = NewTexture;
                Size = new Vector2( graphics.Viewport.Width/8 , graphics.Viewport.Height / 25);
            }

            bool Down;
            public bool IsClicked;
            public void Update(MouseState Mouse)
            {
                Rectangle = new Rectangle((int)Position.X , (int)Position.Y , (int)Size.X , (int)Size.Y);

                Rectangle MouseRectangle = new Rectangle(Mouse.X, Mouse.Y, 1, 1);
                if (MouseRectangle.Intersects(Rectangle))
                {
                    if (Colour.A == 10) Down = false;
                    if (Colour.A == 0) Down = true;
                    if (Down) Colour.A += 3; else Colour.A -= 3;
                    if (Mouse.LeftButton == ButtonState.Pressed) IsClicked = true;  
                }
                else if(Colour.A < 255)
                {
                    Colour.A += 3;
                    IsClicked = false;
                } 
            }
            public void SetPostion(Vector2 NewPosition)
            {
                Position = NewPosition;
            }
            public void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(Texture, Rectangle, Colour);
            }

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
            m_Ball = new Ball(new Vector2((graphics.GraphicsDevice.Viewport.Width / 2), (graphics.GraphicsDevice.Viewport.Height / 2)), 10, new Vector2(-100,100));
            m_Lives1 = new Lives();
            m_Lives2 = new Lives();
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

            Texture2D Heart = Content.Load<Texture2D>("Heart");

            Music = Content.Load<Song>("BeepBox-Song loop");
            MediaPlayer.Play(Music);
            MediaPlayer.IsRepeating = true;

            IsMouseVisible = true;
            PlayButton = new Button(Content.Load<Texture2D>("Play") , graphics.GraphicsDevice);
            PlayButton.SetPostion(new Vector2(GraphicsDevice.Viewport.Width / 2 - GraphicsDevice.Viewport.Width / 16, GraphicsDevice.Viewport.Height / 2 ));

            //public bool Intersects(BoundingBox kaas);
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
            MouseState mouse = Mouse.GetState();
            switch (CurrentGameState)
            {
                case Gamestate.MainMenu:
                    if (PlayButton.IsClicked == true) CurrentGameState = Gamestate.Playing;
                    PlayButton.Update(mouse);
                    break;
                case Gamestate.Playing:
                    if (Keyboard.GetState().IsKeyDown(Keys.S)) { m_Bar1.SetVel(m_Bar1.GetMaxVel()); }
                    if (Keyboard.GetState().IsKeyDown(Keys.W)) { m_Bar1.SetVel(-m_Bar1.GetMaxVel()); }
                    if (Keystate.IsKeyDown(Keys.Down)) { m_Bar2.SetVel(m_Bar2.GetMaxVel()); }
                    if (Keystate.IsKeyDown(Keys.Up)) { m_Bar2.SetVel(-m_Bar2.GetMaxVel()); }
                    if (m_Bar1.GetPosY() <= 0 && m_Bar1.GetVel() < 0 || Keyboard.GetState().IsKeyUp(Keys.S) && m_Bar1.GetVel() > 0) { m_Bar1.SetVel(0); }
                    if (m_Bar1.GetPosY() + m_Bar2.GetHeight() >= graphics.GraphicsDevice.Viewport.Height && m_Bar1.GetVel() > 0 || Keyboard.GetState().IsKeyUp(Keys.W) && m_Bar1.GetVel() < 0) { m_Bar1.SetVel(0); }
                    if (m_Bar2.GetPosY() <= 0 && m_Bar2.GetVel() < 0 || Keyboard.GetState().IsKeyUp(Keys.Down) && m_Bar2.GetVel() > 0) { m_Bar2.SetVel(0); }
                    if (m_Bar2.GetPosY() + m_Bar2.GetHeight() >= graphics.GraphicsDevice.Viewport.Height && m_Bar2.GetVel() > 0 || Keyboard.GetState().IsKeyUp(Keys.Up) && m_Bar2.GetVel() < 0) { m_Bar2.SetVel(0); }
                    if (m_Ball.GetPosX() <= m_Bar1.GetWidth())
                    {
                        if (m_Ball.GetMidPos() <= m_Bar1.GetPosY() + m_Bar1.GetHeight() && m_Ball.GetMidPos() >= m_Bar1.GetPosY())
                        {
                            m_Ball.InverseVelX();
                        }
                    }
                    if (m_Ball.GetPosX() + m_Ball.GetSize() >= graphics.GraphicsDevice.Viewport.Width - m_Bar2.GetWidth())
                    {
                        if (m_Ball.GetMidPos() <= m_Bar2.GetPosY() + m_Bar2.GetHeight() && m_Ball.GetMidPos() >= m_Bar2.GetPosY())
                        {
                            m_Ball.InverseVelX();
                        }
                    }
                    else if (m_Ball.GetPosX() <= 0)
                    {
                        m_Ball.SetPos(new Vector2((graphics.GraphicsDevice.Viewport.Width / 2), (graphics.GraphicsDevice.Viewport.Height / 2)));
                        m_Ball.SetVelX(-100);
                        m_Lives1.RemoveOne();
                        if (m_Lives1.GetLivesInt() == 0)
                        {
                            CurrentGameState = Gamestate.GameOver;
                        }
                    }
                    else if (m_Ball.GetPosX() >= graphics.GraphicsDevice.Viewport.Width)
                    {
                        m_Ball.SetPos(new Vector2((graphics.GraphicsDevice.Viewport.Width / 2), (graphics.GraphicsDevice.Viewport.Height / 2)));
                        m_Ball.SetVelX(100);
                        m_Lives2.RemoveOne();
                        if (m_Lives2.GetLivesInt() == 0)
                        {
                            CurrentGameState = Gamestate.GameOver;
                        }
                    }                 
                                    

                    float MovedPos1 = m_Bar1.GetPosY() + m_Bar1.GetVel() * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    float MovedPos2 = m_Bar2.GetPosY() + m_Bar2.GetVel() * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    float MovedBallPosX = m_Ball.GetPosX() + m_Ball.GetVelX() * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    float MovedBallPosY = m_Ball.GetPosY() + m_Ball.GetVelY() * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    m_Bar1.SetPos(MovedPos1);
                    m_Bar2.SetPos(MovedPos2);
                    m_Ball.SetPosX(MovedBallPosX);
                    m_Ball.SetPosY(MovedBallPosY);

                    if (m_Ball.GetPosY() + m_Ball.GetSize() >= graphics.GraphicsDevice.Viewport.Height)
                    {
                        m_Ball.InverseVelY();
                    }
                    else if (m_Ball.GetPosY() < 0 && m_Ball.GetVelY() < 0)
                    {
                        m_Ball.InverseVelY();
                    }
                    break;
                case Gamestate.GameOver:
                    
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            // test
            spriteBatch.Begin();
            switch (CurrentGameState)
            {
                case Gamestate.MainMenu:
                    spriteBatch.Draw(Content.Load<Texture2D>("Menu"), new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height) , Color.White);
                    PlayButton.Draw(spriteBatch);   
                    break;
                case Gamestate.Playing: 
                    GraphicsDevice.Clear(Color.LightGreen);
                    spriteBatch.DrawString(Font, m_Lives1.GetLivesStr(), new Vector2(graphics.GraphicsDevice.Viewport.Width / 4, 50), Color.Black);
                    spriteBatch.DrawString(Font, m_Lives2.GetLivesStr(), new Vector2(3*(graphics.GraphicsDevice.Viewport.Width / 4), 50), Color.Black);
                    spriteBatch.Draw(m_BarShape1, m_Bar1.GetPos(), Color.Red);
                    spriteBatch.Draw(m_BarShape2, m_Bar2.GetPos(), Color.White);
                    spriteBatch.Draw(m_BallShape, m_Ball.GetPos(), Color.White);
                    break;
                case Gamestate.GameOver:
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
