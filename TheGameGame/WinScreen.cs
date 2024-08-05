
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
public class WinScreen
{
    private Texture2D startButtonTexture;
    private Rectangle startButtonRectangle;
    private Vector2 victoryTextLocation;
    private SpriteFont font;
    private bool didWin;
    private bool isStartButtonClicked;

    public bool IsStartButtonClicked => isStartButtonClicked;
    public void ResetStartButton()
    {
        isStartButtonClicked = false;
    }

    public WinScreen(Texture2D startButtonTexture, Rectangle startButtonRectangle, Vector2 victoryTextLocation, SpriteFont font)
    {
        this.startButtonTexture = startButtonTexture;
        this.startButtonRectangle = startButtonRectangle;
        this.victoryTextLocation = victoryTextLocation;
        this.font = font;
        didWin = false;
    }

    public void Win()
    {
        didWin = true;
    }

    public void Update(GameTime gameTime, MouseState mouseState)
    {
        Debug.WriteLine(1);
        if (mouseState.LeftButton == ButtonState.Pressed && startButtonRectangle.Contains(mouseState.Position))
        {
            Debug.WriteLine(2);
            isStartButtonClicked = true;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        string victoryText = didWin ? "Victory" : "Defeat";
        spriteBatch.Draw(startButtonTexture, startButtonRectangle, Color.White);
        spriteBatch.DrawString(font, victoryText , victoryTextLocation, Color.Black);
    }
}
