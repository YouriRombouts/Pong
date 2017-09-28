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
        Bar m_Bar1;
        Bar m_Bar2;
        Lives m_Lives1;
        Lives m_Lives2;
        Ball m_Ball;
        Song Music;
        Button Back, PlayButton, Options, Full;
        SoundEffect Ping, Pong, Pang2;

        public class Ball
        {            
            int m_Size = 0;
            Vector2 m_Vel = new Vector2(0, 0);
            Vector2 m_Pos = new Vector2(0, 0);
            Vector2 m_StartVel = new Vector2(150, 100);
            float m_MaxVelY = 150;
            public Ball(Vector2 Pos,int Size, Vector2 Vel) { m_Pos = Pos; m_Size = Size; m_Vel = Vel; }
            public int GetSize() { return m_Size; }
            public Vector2 GetPos() { return m_Pos; }
            public float GetMidPos() { return m_Pos.Y + m_Size/2; }
            public void MoveVertical(int distance) { m_Pos.Y += distance; }
            public void MoveHorizontal(int distance) { m_Pos.X += distance; }
            public float GetPosY() { return m_Pos.Y; }
            public float GetPosX() { return m_Pos.X; }
            public float GetVelY() { return m_Vel.Y; }
            public float GetVelX() { return m_Vel.X; }
            public float GetStartVelX() { return m_StartVel.X; }
            public float GetStartVelY() { return m_StartVel.Y; }
            public void SetPosX(float NewBallPosX) { m_Pos.X = NewBallPosX; }
            public void SetPosY(float NewBallPosY) { m_Pos.Y = NewBallPosY; }
            public void SetPos(Vector2 NewBallPos) { m_Pos = NewBallPos; }
            public void SetVel(Vector2 NewVel) { m_Vel = NewVel; }
            public void SetVelX(float NewVelX) { m_Vel.X = NewVelX; }
            public void SetVelY(float NewVelY) { m_Vel.Y = NewVelY; }
            public void InverseVelX() { m_Vel.X *= -1; }
            public void InverseVelY() { m_Vel.Y *= -1; }
            public void IncreaseVel() { m_Vel.X *= 1.1f; m_MaxVelY *= 1.1f; }
            public void ModVelY(float DistanceToMid) { m_Vel.Y = m_MaxVelY * DistanceToMid; }
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
            public float GetMiddlePos() { return m_Pos.Y + m_Height / 2; }
        }
        public class Lives
        {
            int m_Lives = 3;
            public void RemoveOne() { m_Lives -= 1; }
            public string GetLivesStr() { return m_Lives.ToString(); }
            public int GetLivesInt() { return m_Lives; }
            public void Reset() { m_Lives = 3; }
        }

        enum Gamestate
        {
            MainMenu,
            Options,
            Playing,
            GameOver,
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
                Size = new Vector2( graphics.Viewport.Width/6 , graphics.Viewport.Height / 18);
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
            m_Ball = new Ball(new Vector2((graphics.GraphicsDevice.Viewport.Width / 2), (graphics.GraphicsDevice.Viewport.Height / 2)), 10, new Vector2 (0, 0));
            m_Lives1 = new Lives();
            m_Lives2 = new Lives();
            m_Bar1.MoveVertical(-(m_Bar1.GetHeight()) / 2);
            m_Bar2.MoveVertical(-(m_Bar2.GetHeight()) / 2);
            m_Ball.MoveVertical(-m_Ball.GetSize() / 2);
            m_Bar2.MoveHorizontal(graphics.GraphicsDevice.Viewport.Width-m_Bar2.GetWidth());
            m_Ball.SetVel(new Vector2(m_Ball.GetStartVelX(), m_Ball.GetStartVelY()));

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

            Music = Content.Load<Song>("BeepBox-Song loop");
            MediaPlayer.Play(Music);
            MediaPlayer.IsRepeating = true;


            IsMouseVisible = true;

            PlayButton = new Button(Content.Load<Texture2D>("Play") , graphics.GraphicsDevice);
            PlayButton.SetPostion(new Vector2(GraphicsDevice.Viewport.Width / 2 - GraphicsDevice.Viewport.Width / 16, GraphicsDevice.Viewport.Height / 2));

            Back = new Button(Content.Load<Texture2D>("Back"), graphics.GraphicsDevice);
            Back.Size = new Vector2(graphics.GraphicsDevice.Viewport.Width / 9, graphics.GraphicsDevice.Viewport.Height / 18);
            Back.SetPostion(new Vector2(GraphicsDevice.Viewport.Width / 2 - GraphicsDevice.Viewport.Width / 16, GraphicsDevice.Viewport.Height / 2 + 100));

            Options = new Button(Content.Load<Texture2D>("Options"), graphics.GraphicsDevice);
            Options.SetPostion(new Vector2(GraphicsDevice.Viewport.Width / 2 - GraphicsDevice.Viewport.Width / 16, (GraphicsDevice.Viewport.Height / 2 + 20) + 10));

            Full = new Button(Content.Load<Texture2D>("Fullscreen"), graphics.GraphicsDevice);
            Full.SetPostion(new Vector2(GraphicsDevice.Viewport.Width / 2 - GraphicsDevice.Viewport.Width / 16, GraphicsDevice.Viewport.Height / 2));

            Ping = Content.Load<SoundEffect>("ping");
            Pong = Content.Load<SoundEffect>("pong");
            Pang2 = Content.Load<SoundEffect>("pang2");       
                 
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
                    MouseState PrevMouseState = Mouse.GetState();
                    m_Lives1.Reset();
                    m_Lives2.Reset();
                    if (PlayButton.IsClicked == true && PrevMouseState.LeftButton == ButtonState.Released) CurrentGameState = Gamestate.Playing;
                    PlayButton.Update(mouse);
                    if (Options.IsClicked == true && PrevMouseState.LeftButton == ButtonState.Released) { CurrentGameState = Gamestate.Options; }
                    Options.Update(mouse);
                    break;
                case Gamestate.Options:
                    MouseState CurrentMouseState = Mouse.GetState();
                    MouseState PrevMouseState2 = CurrentMouseState;
                    if (Full.IsClicked == true && PrevMouseState2.LeftButton == ButtonState.Released && CurrentMouseState.LeftButton == ButtonState.Released)
                    {
                        graphics.ToggleFullScreen();
                        //Full.IsClicked = false;
                    }
                    Full.Update(mouse);
                    if (Back.IsClicked == true && PrevMouseState2.LeftButton == ButtonState.Released)
                        CurrentGameState = Gamestate.MainMenu;
                    Back.Update(mouse);
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
                            m_Ball.SetPosX(m_Bar1.GetWidth() + 1);
                            m_Ball.IncreaseVel();
                            float DTM = (m_Bar1.GetMiddlePos() - m_Ball.GetMidPos()) / (-m_Bar1.GetHeight() / 2);
                            m_Ball.ModVelY(DTM);
                            Ping.Play();
                        }
                        else
                        {
                            if (m_Ball.GetPosX() <= -m_Ball.GetSize())
                            { 
                                m_Ball.SetPos(new Vector2((graphics.GraphicsDevice.Viewport.Width / 2), (graphics.GraphicsDevice.Viewport.Height / 2)));
                                m_Ball.SetVelX(-m_Ball.GetStartVelX());
                                m_Ball.SetVelY((float)(m_Bar1.GetMiddlePos() - (m_Ball.GetMidPos())) / ((graphics.GraphicsDevice.Viewport.Width / 2) / m_Ball.GetStartVelX()));
                                m_Lives1.RemoveOne();
                                if (m_Lives1.GetLivesInt() == 0)
                                {
                                    CurrentGameState = Gamestate.GameOver;
                                }   
                            }
                        }
                    }
                    if (m_Ball.GetPosX() + m_Ball.GetSize() >= graphics.GraphicsDevice.Viewport.Width - m_Bar2.GetWidth())
                    {
                        if (m_Ball.GetMidPos() <= m_Bar2.GetPosY() + m_Bar2.GetHeight() && m_Ball.GetMidPos() >= m_Bar2.GetPosY())
                        {
                            m_Ball.InverseVelX();
                            m_Ball.SetPosX(graphics.GraphicsDevice.Viewport.Width - m_Bar2.GetWidth() - m_Ball.GetSize() - 1);
                            m_Ball.IncreaseVel();
                            float DTM = (m_Bar2.GetMiddlePos() - m_Ball.GetMidPos()) / (-m_Bar2.GetHeight() / 2);
                            m_Ball.ModVelY(DTM);
                            Pong.Play();

                        }
                        else
                        {
                            if (m_Ball.GetPosX() >= graphics.GraphicsDevice.Viewport.Width)
                            {
                                m_Ball.SetPos(new Vector2((graphics.GraphicsDevice.Viewport.Width / 2), (graphics.GraphicsDevice.Viewport.Height / 2)));
                                m_Ball.SetVelX(m_Ball.GetStartVelX());
                                m_Ball.SetVelY((float)(m_Bar2.GetMiddlePos() - (m_Ball.GetMidPos())) / ((graphics.GraphicsDevice.Viewport.Width / 2) / m_Ball.GetStartVelX()));
                                m_Lives2.RemoveOne();
                                if (m_Lives2.GetLivesInt() == 0)
                                {
                                    CurrentGameState = Gamestate.GameOver;
                                }
                            }
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
                        Pang2.Play();

                    }
                    else if (m_Ball.GetPosY() < 0 && m_Ball.GetVelY() < 0)
                    {
                        m_Ball.InverseVelY();
                        Pang2.Play();
                    }
                    break;
                case Gamestate.GameOver:
                    MouseState PrevMouseState3 = Mouse.GetState();
                    if (Back.IsClicked == true && PrevMouseState3.LeftButton == ButtonState.Released)
                        CurrentGameState = Gamestate.MainMenu;
                        Back.Update(mouse);
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
                    Options.Draw(spriteBatch);
                    break;
                case Gamestate.Options:
                    spriteBatch.Draw(Content.Load<Texture2D>("Menu"), new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
                    Full.Draw(spriteBatch);
                    Back.Draw(spriteBatch);
                    break;
                case Gamestate.Playing: 
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.DrawString(Font, m_Lives1.GetLivesStr(), new Vector2(graphics.GraphicsDevice.Viewport.Width / 4, 50), Color.White);
                    spriteBatch.DrawString(Font, m_Lives2.GetLivesStr(), new Vector2(3*(graphics.GraphicsDevice.Viewport.Width / 4), 50), Color.White);
                    spriteBatch.Draw(m_BarShape1, m_Bar1.GetPos(), Color.White);
                    spriteBatch.Draw(m_BarShape2, m_Bar2.GetPos(), Color.White);
                    spriteBatch.Draw(m_BallShape, m_Ball.GetPos(), Color.White);
                    break;
                case Gamestate.GameOver:
                    GraphicsDevice.Clear(Color.Black);
                    if (m_Lives1.GetLivesInt() == 0)
                    {
                        string Text = ("The winner is: Player 2, with " + m_Lives2.GetLivesStr() + " lives left.");
                        float StringLength = Font.MeasureString(Text).X;
                        spriteBatch.DrawString(Font, Text, new Vector2(GraphicsDevice.Viewport.Width / 2 - (StringLength) / 2, GraphicsDevice.Viewport.Height / 2), Color.White);
                    }
                    else if (m_Lives2.GetLivesInt() == 0)
                    {
                        string Text = ("The winner is: Player 1, with " + m_Lives1.GetLivesStr() + " lives left.");
                        float StringLength = Font.MeasureString(Text).X;
                        spriteBatch.DrawString(Font, Text, new Vector2(GraphicsDevice.Viewport.Width / 2 - (StringLength) / 2, GraphicsDevice.Viewport.Height / 2), Color.White);
                    }
                    Back.Draw(spriteBatch);
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
