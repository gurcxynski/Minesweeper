using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.EasyInput;
using System;
using System.Collections.Generic;

namespace Minesweeper
{
    public static class Globals
    {
        public static int size = 10;
        public static SpriteBatch spriteBatch;
        public static Texture2D field;
        public static Texture2D field2;
        public static Texture2D mine;
        public static SpriteFont font;
        public static Dictionary<Vector2, Field> fields = new Dictionary<Vector2, Field>();
        public static Random rand = new Random();
        public static EasyMouse mouse = new EasyMouse();
    }
    public class Game1 : Game
    {
        readonly GraphicsDeviceManager graphics;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            Globals.spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.field = Content.Load<Texture2D>("field");
            Globals.field2 = Content.Load<Texture2D>("field2");
            Globals.font = Content.Load<SpriteFont>("fonts/MarkerFelt-16");
            Globals.mine = Content.Load<Texture2D>("mine");


            IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = graphics.PreferredBackBufferWidth = 50 * Globals.size;
            graphics.ApplyChanges();

            for (int i = 0; i < Globals.size; i++)
            {
                for (int j = 0; j < Globals.size; j++)
                {
                    Globals.fields.Add(new Vector2(i, j), new Field(i, j));
                }
            }
            foreach (var item in Globals.fields)
            {
                item.Value.Link(new Vector2(item.Key.X + 1, item.Key.Y));
                item.Value.Link(new Vector2(item.Key.X,     item.Key.Y + 1));
                item.Value.Link(new Vector2(item.Key.X + 1, item.Key.Y + 1));
                item.Value.Link(new Vector2(item.Key.X - 1, item.Key.Y + 1));
            }
            foreach (var item in Globals.fields.Values)
            {
                item.Calc();
            }
            base.Initialize();
        }
        protected override void LoadContent()
        {
            
        }
        protected override void Update(GameTime gameTime)
        {
            Globals.mouse.Update();
            if (Globals.mouse.ReleasedThisFrame(MouseButtons.Left))
            {
                Vector2 pos = new Vector2(Globals.mouse.Position.X / 50, Globals.mouse.Position.Y / 50);
                if (Globals.fields.ContainsKey(pos)) Globals.fields[pos].Reveal();
            }
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            foreach (var item in Globals.fields.Values)
            {
                item.Draw();
            }
            base.Draw(gameTime);
        }

    }
}
