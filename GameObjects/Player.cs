using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Timers;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using PilotGame.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;

namespace PilotGame.GameObjects;

public class Player : Entity
{
    private Vector2 _dir;

    private const float _speed = 200f;

    private Vector2 _oldPosition;

    //Hitbox

    private new Vector2 hitboxOffset = new Vector2(-10, -5);
    private new SizeF hitboxSize = new SizeF(20, 30);


    private enum CharacterState
    {
        Idle,
        Walking,
        Attacking
    }
    private CharacterState _characterState;
    //Esto usa la libreria de Monogame.Extended y no la propia porque es lo mismo solo que mejor mantenida 
    private Texture2DAtlas _playerAtlas;
    private SpriteSheet _playerSpriteSheet;
    private AnimatedSprite _playerSprite;
    private const float _spriteScale = 1.5f;

    private TimeSpan _animationDuration = TimeSpan.FromSeconds(0.175);


    public void Initialize(Scene scene)
    {   
        base.Initialize(new Vector2(450, 270), scene);
        _dir = new Vector2(1, 1);
        _characterState = CharacterState.Idle;

    }

    public override void LoadContent()
    {
        _playerAtlas = Core.Content.Load<Texture2DAtlas>("images/adventurer");
        _playerSpriteSheet = new SpriteSheet("images/adventurer-texture", _playerAtlas);

        setAnimations();

        //Set hitbox

        Bounds = new RectangleF(Position + hitboxOffset, hitboxSize);

        _playerSprite.Origin = new Vector2(_playerSprite.Size.X / 2f, _playerSprite.Size.Y / 2f);
        Size = _playerSprite.Size;


    }

    public override void Update(GameTime gameTime)
    {
        _playerSprite.Update(gameTime);

        handleInput(gameTime);

        //Update bounds position
        Bounds.Position = Position + hitboxOffset;

    }

    public override void Draw(GameTime gameTime, float depth)
    {
        //Only to draw collision bounds for testing, remove later
        base.Draw(gameTime, depth);


        _playerSprite.Depth = depth;
        Core.SpriteBatch.Draw(_playerSprite, Position, 0, new Vector2(_spriteScale));


    }

    private void setAnimations()
    {
        _playerSpriteSheet.DefineAnimation("idle", builder =>
        {
            builder.IsLooping(true)
                   .AddFrame("adventurer-idle-2-00", _animationDuration)
                   .AddFrame("adventurer-idle-2-01", _animationDuration)
                   .AddFrame("adventurer-idle-2-02", _animationDuration)
                   .AddFrame("adventurer-idle-2-03", _animationDuration);

        });

        _playerSpriteSheet.DefineAnimation("walk", builder =>
        {
            builder.IsLooping(true)
                   .AddFrame("adventurer-run-00", _animationDuration)
                   .AddFrame("adventurer-run-01", _animationDuration)
                   .AddFrame("adventurer-run-02", _animationDuration)
                   .AddFrame("adventurer-run-03", _animationDuration)
                   .AddFrame("adventurer-run-04", _animationDuration)
                   .AddFrame("adventurer-run-05", _animationDuration);
        });

        _playerSpriteSheet.DefineAnimation("attack", builder =>
        {
            builder.IsLooping(false)
                   .AddFrame("adventurer-attack3-00", _animationDuration)
                   .AddFrame("adventurer-attack3-01", _animationDuration)
                   .AddFrame("adventurer-attack3-02", _animationDuration)
                   .AddFrame("adventurer-attack3-03", _animationDuration)
                   .AddFrame("adventurer-attack3-04", _animationDuration)
                   .AddFrame("adventurer-attack3-05", _animationDuration);
        });


        _playerSprite = new AnimatedSprite(_playerSpriteSheet, "idle");


    }

    private void handleInput(GameTime gameTime)
    {
        if (GameController.Moving())
        {
            handleMovement(gameTime);

        }
        else if (_characterState == CharacterState.Walking)
        {
            _playerSprite.SetAnimation("idle");
            _characterState = CharacterState.Idle;
        }

        if (GameController.Attack()) { 
            handleAttack();

        }
    }


    private void handleMovement(GameTime gameTime)
    {

        _dir = Vector2.Zero;

        if (GameController.MoveUp()) { 

            _dir.Y += -1;

        }
        if (GameController.MoveDown()) {

            _dir.Y += 1;

        }
        if (GameController.MoveRight())
        {

            _dir.X += 1;
            _playerSprite.Effect = SpriteEffects.None;

        }
        if (GameController.MoveLeft())
        {

            _dir.X += -1;
            _playerSprite.Effect = SpriteEffects.FlipHorizontally;

        }
        if (_dir != Vector2.Zero)
        {
            _dir.Normalize();
        }

        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 nextPositionVector = _dir * _speed * deltaTime;

        if (_characterState != CharacterState.Walking)
        {
            _characterState = CharacterState.Walking;
            _playerSprite.SetAnimation("walk");
        }
        _oldPosition = Position;
        Position += nextPositionVector;

        _dir = Vector2.Zero;

    }

    private void handleAttack()
    {
        _characterState = CharacterState.Attacking;

        // Subscribe to the event with our handler
        _playerSprite.SetAnimation("attack").OnAnimationEvent += (IAnimationController animSender, AnimationEventTrigger trigger) =>
        {
            if (trigger == AnimationEventTrigger.AnimationCompleted)
            {
                // Important: Unregister the handler first to prevent accumulation
                _playerSprite.SetAnimation("idle");
            }

        };

    }

    public override void OnCollision(CollisionEventArgs collisionInfo)
    {

        Bounds.Position -= collisionInfo.PenetrationVector;

        Position -= collisionInfo.PenetrationVector;

        if (_characterState == CharacterState.Walking && _oldPosition == Position)
        {
            _playerSprite.SetAnimation("idle");
            _characterState = CharacterState.Idle;
        }

    }

    public void UnloadContent()
    {


    }

}
