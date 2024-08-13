
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1.Effects;
using System.Diagnostics;
public class WinScreen
{
    private Texture2D startButtonTexture;
    private Texture2D gameOverTexture;
    private Rectangle startButtonRectangle;
    private Vector2 victoryTextLocation;
    private SpriteFont font;
    private bool didWin;
    private bool isStartButtonClicked;

    public bool IsStartButtonClicked => isStartButtonClicked;
    public void ResetStartButton()
    {
        isStartButtonClicked = false;
        didWin = true;
    }

    public WinScreen(Texture2D startButtonTexture, Rectangle startButtonRectangle, Vector2 victoryTextLocation, SpriteFont font, Texture2D gameOverTexture)
    {
        this.startButtonTexture = startButtonTexture;
        this.startButtonRectangle = startButtonRectangle;
        this.victoryTextLocation = victoryTextLocation;
        this.font = font;
        didWin = true;
        this.gameOverTexture = gameOverTexture;
    }

    public void Died()
    {
        didWin = false;
    }

    public void Update(GameTime gameTime, MouseState mouseState)
    {
        if (mouseState.LeftButton == ButtonState.Pressed && startButtonRectangle.Contains(mouseState.Position))
        {
            isStartButtonClicked = true;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        string victoryText = didWin ? "Victory" : "Defeat";
        if (didWin)
        {
            spriteBatch.DrawString(font, "Victory", victoryTextLocation, Color.Black);
        }
        else
        {
            const float scale = 0.5f;
            Rectangle gameOverRectangle = new((int)(400-(gameOverTexture.Width*scale)/2), 0, (int)(gameOverTexture.Width*scale), (int)(gameOverTexture.Height*scale));
            spriteBatch.Draw(gameOverTexture, gameOverRectangle, new(0,0,gameOverTexture.Width,gameOverTexture.Height), Color.White);
        }
        spriteBatch.Draw(startButtonTexture, startButtonRectangle, Color.White);
        
    }
}
