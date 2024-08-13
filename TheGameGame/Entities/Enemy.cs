using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using SharpDX.Direct3D9;
using TheGameGame.Entities;

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
    private SpriteEffects spriteEffect;

    public Enemy(Texture2D texture, Rectangle[] frames, Vector2 position, Vector2 velocity, float scale)
    {
        _texture = texture;
        _frames = frames;
        _position = position;
        _speed = velocity.X;
        _scale = scale;
        _currentFrame = 0;
        _timeSinceLastFrame = 0;
        _frameInterval = 0.1f; // Example interval
        _velocity = velocity;
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

        spriteEffect = _velocity.X < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        
        // Update position based on velocity
        _position += _velocity;


        // Update bounding box
        _boundingBox = new Rectangle((int)_position.X, (int)_position.Y, (int)(_frames[_currentFrame].Width * _scale), (int)(_frames[_currentFrame].Height * _scale));

        // Check collision with platforms and adjust position
        HandleCollisions(gameboard);
    }

    private void HandleCollisions(Tile[,] gameboard)
    {
        // Get current tile positions
        int xr = (int)(_boundingBox.Right / 100f);  // x right
        int xl = (int)(_boundingBox.Left / 100f);   // x left
        int x = (int)(_position.X / 100);           // x center
        int yb = (int)(_boundingBox.Bottom / 60f);  // y bottom
        int yt = (int)(_boundingBox.Top / 60f);     // y top
        if (_velocity.X > 0)
        {
            // obstacle on right
            if (gameboard[yb, xr]?.TileType == TileType.Impassable)
            {
                _velocity = -_velocity;
            }
        }
        else if (_velocity.X < 0)
        {
            // obstacle on left
            if (gameboard[yb, xl]?.TileType == TileType.Impassable)
            {
                _velocity = -_velocity;
            }
        }
        else if (_velocity.Y > 0)
        {
            // obstacle below
            if (gameboard[yb + 1, x]?.TileType == TileType.Impassable)
            {
                _velocity = -_velocity;
            }
        }
        else
        {
            // top of screen
            if (yt < 1)
            {
                _velocity = -_velocity;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, _frames[_currentFrame], Color.White, 0f, Vector2.Zero, _scale, spriteEffect, 0f);
    }
}
