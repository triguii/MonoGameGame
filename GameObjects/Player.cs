using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Graphics;
using MonoGameLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace PilotGame.GameObjects;

public class Player
{
    private Vector2 _position;

    private const float Speed = 10.0f;

    //Esto usa la libreria de Monogame.Extended y no la propia porque es lo mismo solo que mejor mantenida 
    private Texture2DAtlas _playerAtlas;
    private SpriteSheet _playerSpriteSheet;
    private AnimatedSprite _playerSprite;
    private float _spriteScale = 3.0f;

    private TimeSpan _animationDuration = TimeSpan.FromSeconds(0.175);


    public void Initialize()
    {

    }

    public void LoadContent()
    {
        _playerAtlas = Core.Content.Load<Texture2DAtlas>("images/adventurer");
        _playerSpriteSheet = new SpriteSheet("images/adventurer-texture", _playerAtlas);

        _playerSpriteSheet.DefineAnimation("idle", builder =>
        {
            builder.IsLooping(true)
                   .AddFrame("adventurer-idle-2-00", _animationDuration)
                   .AddFrame("adventurer-idle-2-01", _animationDuration)
                   .AddFrame("adventurer-idle-2-02", _animationDuration)
                   .AddFrame("adventurer-idle-2-03", _animationDuration);

        });

        _playerSprite = new AnimatedSprite(_playerSpriteSheet, "idle");

    }

    public void Update(GameTime gameTime)
    {
        _playerSprite.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {

        Core.SpriteBatch.Draw(_playerSprite, _playerSprite.Origin * _spriteScale, 0, new Vector2(_spriteScale));

    }

}
