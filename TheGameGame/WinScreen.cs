
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
public class WinScreen
{
    private Texture2D startButtonTexture;
    private Texture2D gameOverTexture;
    private Texture2D winTexture;
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

    public WinScreen(Texture2D startButtonTexture, Rectangle startButtonRectangle, SpriteFont font, Texture2D gameOverTexture, Texture2D winTexture)
    {
        this.startButtonTexture = startButtonTexture;
        this.startButtonRectangle = startButtonRectangle;
        this.font = font;
        didWin = true;
        this.gameOverTexture = gameOverTexture;
        this.winTexture = winTexture;
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
            const float scale = 0.5f;
            Rectangle winRectangle = new((int)(400 - (winTexture.Width * scale) / 2), 0, (int)(winTexture.Width * scale), (int)(winTexture.Height * scale));
            spriteBatch.Draw(winTexture, winRectangle, new(0, 0, winTexture.Width, winTexture.Height), Color.White);
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
