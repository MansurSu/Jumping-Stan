
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
public class StartScreen
{
    private Texture2D startButtonTexture;
    private Rectangle startButtonRectangle;
    private bool isStartButtonClicked;

    public bool IsStartButtonClicked => isStartButtonClicked;

    public StartScreen(Texture2D startButtonTexture, Rectangle startButtonRectangle)
    {
        this.startButtonTexture = startButtonTexture;
        this.startButtonRectangle = startButtonRectangle;
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
        spriteBatch.Draw(startButtonTexture, startButtonRectangle, Color.White);
    }
}
