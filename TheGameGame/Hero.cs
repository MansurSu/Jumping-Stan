using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGameGame.interfaces;
using TheGameGame.Animation;


namespace TheGameGame
{
    public class Hero:IGameObject
    {

        Texture2D heroTexture;
        Animatie animatie;
        public Hero(Texture2D texture)
        {
            heroTexture = texture;
            animatie = new Animatie();
            animatie.AddFrame(new AnimationFrame(new Rectangle(0,300,300,300)));
            animatie.AddFrame(new AnimationFrame(new Rectangle(600, 300, 300, 300)));
            animatie.AddFrame(new AnimationFrame(new Rectangle(900, 300, 300, 300)));
            
        }

        public void Update()
        {
            animatie.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(heroTexture, new Vector2(10, 10), animatie.CurrentFrame.SourceRectangle,Color.White);
        }

       
    }
}
