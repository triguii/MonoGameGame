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
using PilotGame.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;

namespace PilotGame.GameObjects;

public class Player : Entity
{
    private Vector2 _dir;

    private const float _speed = 200f;

    private float _maxHealth = 100f;
    private float _currentHealth = 100f;

    //Hitbox

    private new Vector2 hitboxOffset = new Vector2(-10, -5);
    private new SizeF hitboxSize = new SizeF(20, 30);


    public enum CharacterState
    {
        Idle,
        Walking,
        Attacking,
        Damaged
    }

    private CharacterState _characterState;
    public CharacterState characterState
    {
        get { return _characterState; }
        set
        {
            // Only trigger if the value is ACTUALLY changing
            if (_characterState != value)
            {
                _characterState = value;

                // Trigger the internal method/event
                OnStateChanged(_characterState);
            }
        }
    }

    //Esto usa la libreria de Monogame.Extended y no la propia porque es lo mismo solo que mejor mantenida 
    private Texture2DAtlas _playerAtlas;
    private SpriteSheet _playerSpriteSheet;
    private AnimatedSprite _playerSprite;
    private const float _spriteScale = 1.5f;

    //Attack properties
    private Hurtbox _attackHurtbox;
    private const float _attackDamage = 25f;
    private const float _attackKnockback = 200f;

    //Knockback from getting damaged
    private float _currentKnockback;
    private Vector2 _knockbackDirection;
    private const float _knockbackFriction = 50;

    private TimeSpan _animationDuration = TimeSpan.FromSeconds(0.175);


    public void Initialize(Scene scene)
    {   
        base.Initialize(new Vector2(450, 270), scene);
        _dir = new Vector2(1, 1);
        characterState = CharacterState.Idle;

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

        if (characterState == CharacterState.Damaged) { 
            if (_currentKnockback > 0)
            {
                Position -= _knockbackDirection * _currentKnockback * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentKnockback -= _knockbackFriction; //Add friction
            }

            return;
        }
        handleInput(gameTime);

        //Handle health
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }

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

        _playerSpriteSheet.DefineAnimation("hurt", builder =>
        {
            builder.IsLooping(false)
                   .AddFrame("adventurer-hurt-00", _animationDuration)
                   .AddFrame("adventurer-hurt-01", _animationDuration)
                   .AddFrame("adventurer-hurt-02", _animationDuration);
        });


        _playerSprite = new AnimatedSprite(_playerSpriteSheet, "idle");


    }

    private void handleInput(GameTime gameTime)
    {
        if (GameController.Moving())
        {
            handleMovement(gameTime);

        }
        else if (characterState == CharacterState.Walking)
        {
            characterState = CharacterState.Idle;
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

        characterState = CharacterState.Walking;

        Position += nextPositionVector;

        _dir = Vector2.Zero;

    }

    private void handleAttack()
    {

        characterState = CharacterState.Attacking;


    }

    public override void OnCollision(CollisionEventArgs collisionInfo)
    {

        if (collisionInfo.Other is Enemy && characterState != CharacterState.Damaged)
        {
            takeDamage(collisionInfo);
        }
        else if (collisionInfo.Other is Prop || collisionInfo.Other is CollisionObject) 
        {

            Bounds.Position -= collisionInfo.PenetrationVector;
            Position -= collisionInfo.PenetrationVector;

            if (characterState == CharacterState.Walking)
            {
                characterState = CharacterState.Idle;
            }

        }
    }

    private void takeDamage(CollisionEventArgs collisionInfo)
    {

        _currentHealth -= ((Enemy)collisionInfo.Other).enemyDamage;
        characterState = CharacterState.Damaged;

        _currentKnockback = ((Enemy)collisionInfo.Other).damageKnockback;
        _knockbackDirection = collisionInfo.PenetrationVector.NormalizedCopy();

        _playerSprite.Color = Color.Red;

    }

    private void OnStateChanged(CharacterState newState)
    {
        // Handle state changes (mainly for animations)

        // If we had an active attack hurtbox, remove it when changing states to avoid staying active after the attack animation finishes
        if (_attackHurtbox != null)
        {
            ((MainGameScene)currentScene).RemoveEntity(_attackHurtbox);
        }
        
        switch (newState)
        {
            case CharacterState.Idle:
                _playerSprite.SetAnimation("idle");

                break;

            case CharacterState.Walking:
                _playerSprite.SetAnimation("walk");

                break;
            case CharacterState.Attacking:
                // Subscribe to the event with our handler
                _playerSprite.SetAnimation("attack").OnAnimationEvent += (IAnimationController animSender, AnimationEventTrigger trigger) =>
                {
                    if (trigger == AnimationEventTrigger.AnimationCompleted)
                    {
                        characterState = CharacterState.Idle;


                    }
                    else if (trigger == AnimationEventTrigger.FrameBegin)
                    {
                        // Check if we're on the frame where the attack should hit
                        if (animSender.CurrentFrame == 13) 
                        {
                            //Activate attack hurtbox
                            int xOffset = _playerSprite.Effect == SpriteEffects.FlipHorizontally ? -45 : 10; // Adjust the offset based on the facing direction
                            _attackHurtbox = new Hurtbox(new RectangleF(Position + new Vector2(xOffset, - 10), new SizeF(35, 40)), false, _attackDamage, _attackKnockback);
                            ((MainGameScene)currentScene).AddEntity(_attackHurtbox);
                        }
                    }


                };
                break;
            case CharacterState.Damaged:
                _playerSprite.SetAnimation("hurt").OnAnimationEvent += (IAnimationController animSender, AnimationEventTrigger trigger) =>
                {
                    if (trigger == AnimationEventTrigger.AnimationCompleted)
                    {
                        _playerSprite.Color = Color.White;
                        characterState = CharacterState.Idle;
                    }

                };
                break;


            default: break;
          
        }


    }

    public void UnloadContent()
    {


    }

}
