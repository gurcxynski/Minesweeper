using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using MonoGame.EasyInput;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper
{
    public class Field
    {
        enum State { hidden, clicked, flagged }
        State state = State.hidden;
        public bool hasMine = false;
        public int value = 0;
        Vector2 Position = new Vector2();
        public List<Field> linked = new List<Field>();
        Texture2D texture;
        public Field(int x, int y)
        {
            Position = new Vector2(x, y);
            if (Globals.rand.Next(100) > 90) hasMine = true;
            texture = Globals.field;
        }
        public void Draw()
        {
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.Draw(texture, 50 * Position, Color.White);
            if(state == State.clicked) Globals.spriteBatch.DrawString(Globals.font, value.ToString(), Position * 50 + new Vector2(20, 10), Color.Black);
            Globals.spriteBatch.End();
        }
        public void Link(Vector2 arg)
        {
            if (Globals.fields.ContainsKey(arg))
            {
                linked.Add(Globals.fields[arg]);
                Globals.fields[arg].linked.Add(this);
            }
        }
        public void Calc()
        {
            foreach (var item in linked)
            {
                if (item.hasMine) value++;
            }
        }
        public void Reveal()
        {
            if (hasMine)
            {
                texture = Globals.mine;
                return;
            }
            state = State.clicked;
            texture = Globals.field2;
            if (value == 0)
            {
                foreach (var item in linked)
                {
                    item.state = State.clicked;
                    item.texture = Globals.field2;
                }
            }
        }
    }
}
