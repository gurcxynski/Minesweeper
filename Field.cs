using Microsoft.Xna.Framework;
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
            UpdateTexture();
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.Draw(texture, 50 * Position, Color.White);
            if(state == State.clicked && value > 0) Globals.spriteBatch.DrawString(Globals.font, value.ToString(), Position * 50 + new Vector2(20, 10), Color.Black);
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
            if(state == State.flagged) return;
            if (hasMine)
            {
                texture = Globals.mine;
                Globals.gameRunning = false;
                Draw();
                return;
            }
            state = State.clicked;
            if (value == 0)
            {
                foreach (var item in linked)
                {
                    if(item.state == State.hidden) item.Reveal();
                }
            }
        }
        public void Flag()
        {
            if (state == State.hidden) state = State.flagged;
            else if(state == State.flagged) state = State.hidden;
        }
        private void UpdateTexture()
        {
            if (!Globals.gameRunning && hasMine)
            {
                texture = Globals.mine;
                return;
            }
            if (state == State.hidden)
            {
                texture = Globals.field; 
                return;
            }
            if (state == State.flagged)
            {
                texture = Globals.flag;
                return;
            }
            if (state == State.clicked && !hasMine)
            {
                texture = Globals.field2; 
                return;
            }
        }
    }
}
