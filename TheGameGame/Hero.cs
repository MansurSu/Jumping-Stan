using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGameGame.interfaces;
using TheGameGame.Animation;
using SharpDX.MediaFoundation;
using System.Diagnostics;
using SharpDX.DXGI;
using Microsoft.Xna.Framework.Input;
using TheGameGame.Input;


namespace TheGameGame
{
    public class Hero:IGameObject
    {

        Texture2D heroTexture;
        Animatie animatie;
        private Vector2 positie;
        private Vector2 snelheid;
        private Vector2 versnelling;
        private Vector2 mouseVector;
        IInputReader inputReader;

        public Hero(Texture2D texture, IInputReader reader)
        {
            heroTexture = texture;
            animatie = new Animatie();
            animatie.AddFrame(new AnimationFrame(new Rectangle(0,300,300,300)));
            animatie.AddFrame(new AnimationFrame(new Rectangle(300, 300, 300, 300)));
            animatie.AddFrame(new AnimationFrame(new Rectangle(600, 300, 300, 300)));
            positie = new Vector2(10, 10);
            snelheid = new Vector2(1, 1);
            versnelling = new Vector2(0.1f, 0.1f);


            //input lezen voor hero klasse
            this.inputReader = reader;
        }

        public void Update(GameTime gameTime)
        {
            var direction = inputReader.ReadInput();
            direction *= 6;
            positie += direction;

            
            //Move(GetMouseState());
            animatie.Update(gameTime);
        }

        private Vector2 GetMouseState()
        {
            MouseState state = Mouse.GetState();
            mouseVector = new Vector2(state.X, state.Y);
            return mouseVector;

        }

        private void Move(Vector2 mouse)
        {
            var direction = Vector2.Add(mouse, -positie);
            direction.Normalize();
            direction = Vector2.Multiply(direction, 0.5f);
            snelheid += direction;
            snelheid = Limit(snelheid, 15);
            positie += snelheid;

            if (positie.X >600 || positie.X < 0)
            {
                snelheid.X *= -1;
                versnelling.X *= -1;
            }
            if (positie.Y > 600 || positie.Y < 0)
            {
                snelheid.Y *= -1;
                versnelling.Y *= -1;
            }

        }

        private Vector2 Limit(Vector2 v, float max)
        {
            if(v.Length() > max)
            {
                var ratio = max / v.Length();
                v.X *= ratio;
                v.Y *= ratio;
            }
            return v;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(heroTexture, positie, animatie.CurrentFrame.SourceRectangle,Color.White,0, new Vector2(0,0),0.5f, SpriteEffects.None,0);
        }

       
    }
}
