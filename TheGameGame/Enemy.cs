using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using TheGameGame;

public class Enemy
{
    private Texture2D _texture;
    private Rectangle[] _frames;
    private Vector2 _position;
    private float _speed;
    private float _scale;
    private int _currentFrame;
    private float _timeSinceLastFrame;
    private float _frameInterval;
    private Rectangle _boundingBox;
    private Vector2 _velocity;
    private bool _isOnGround;

    public Enemy(Texture2D texture, Rectangle[] frames, Vector2 position, float speed, float scale)
    {
        _texture = texture;
        _frames = frames;
        _position = position;
        _speed = speed;
        _scale = scale;
        _currentFrame = 0;
        _timeSinceLastFrame = 0;
        _frameInterval = 0.1f; // Example interval
        _velocity = new Vector2(speed, 0);
    }

    public Rectangle GetBoundingBox() => _boundingBox;

    public void Update(GameTime gameTime, Tile[,] gameboard)
    {
        _timeSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_timeSinceLastFrame > _frameInterval)
        {
            _timeSinceLastFrame = 0;
            _currentFrame = (_currentFrame + 1) % _frames.Length;
        }

        // Update position based on velocity
        _position += _velocity;

        // Check collision with platforms and adjust position
        HandleCollisions(gameboard);

        // Update bounding box
        _boundingBox = new Rectangle((int)_position.X, (int)_position.Y, (int)(_frames[_currentFrame].Width * _scale), (int)(_frames[_currentFrame].Height * _scale));
    }

    private void HandleCollisions(Tile[,] gameboard)
    {
        // Get current tile positions
        int x = (int)Math.Floor(_position.X / 100);
        int y = (int)Math.Floor(_position.Y / 60);

        

        // Implement additional collision logic for left/right movement as needed
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, _frames[_currentFrame], Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
    }
}
