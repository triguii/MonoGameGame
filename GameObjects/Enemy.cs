using FlatRedBall.Glue.StateInterpolation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Timers;
using MonoGameLibrary;
using PilotGame.Controllers;
using PilotGame.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;

namespace PilotGame.GameObjects;

public abstract class Enemy : Entity
{
    public AnimatedSprite enemySprite;
    public Texture2DAtlas enemySpriteAtlas;
    public SpriteSheet enemySpriteSheet;
    public float spriteScale = 1f;

    public float enemyDamage;
    public float enemyHealth;

    public float damageKnockback = 400f;

    private Hurtbox _lastHurtbox;

    // Variables for handling knockback when damaged
    public float currentKnockback;
    public Vector2 knockbackDirection;
    public const float knockbackFriction = 50f;

    public enum CharacterState
    {
        Idle,
        Walking,
        Attacking,
        Damaged,
        Dead
    }

    private CharacterState _enemyState;
    public CharacterState enemyState
    {
        get { return _enemyState; }
        set
        {
            // Only trigger if the value is ACTUALLY changing
            if (_enemyState != value || value == CharacterState.Damaged)
            {
                _enemyState = value;

                OnStateChanged(_enemyState);
            }
        }
    }



    public override void Draw(GameTime gameTime, float depth)
    {
        base.Draw(gameTime, depth);

        enemySprite.Depth = depth;
        Core.SpriteBatch.Draw(enemySprite, Position, 0, new Vector2(spriteScale));

    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        enemySprite.Update(gameTime);


        //Handle getting damaged
        if (enemyState == CharacterState.Damaged || enemyState == CharacterState.Dead)
        {
            if (currentKnockback > 0)
            {
                Position -= knockbackDirection * currentKnockback * (float)gameTime.ElapsedGameTime.TotalSeconds;
                currentKnockback -= knockbackFriction; //Add friction
            }
            return;
        }

    }

    public void takeDamage(CollisionEventArgs collisionInfo)
    {
        if ((Hurtbox)collisionInfo.Other != _lastHurtbox)
        {
            _lastHurtbox = (Hurtbox)collisionInfo.Other;

            enemyHealth -= ((Hurtbox)collisionInfo.Other).damageAmount;
            enemyState = CharacterState.Damaged;


            currentKnockback = ((Hurtbox)collisionInfo.Other).damageKnockback;
            knockbackDirection = collisionInfo.PenetrationVector.NormalizedCopy();

            enemySprite.Color = Color.Red;

        }


    }

    public void handleDeath()
    {
        //Make hitbox dissapear by sending it outside the map
        Bounds.Position = new Vector2(-100, -100);
        enemyState = CharacterState.Dead;
        
    }

    private void OnStateChanged(CharacterState newState)
    {
        // Handle state changes (mainly for animations)


        switch (newState)
        {
            case CharacterState.Idle:
                enemySprite.SetAnimation("idle");

                break;

            case CharacterState.Walking:
                enemySprite.SetAnimation("walk");

                break;
            case CharacterState.Attacking:
                // Subscribe to the event with our handler
                enemySprite.SetAnimation("attack").OnAnimationEvent += (IAnimationController animSender, AnimationEventTrigger trigger) =>
                {
                    if (trigger == AnimationEventTrigger.AnimationCompleted)
                    {
                        enemyState = CharacterState.Idle;


                    }


                };
                break;
            case CharacterState.Damaged:
                enemySprite.SetAnimation("hurt").OnAnimationEvent += (IAnimationController animSender, AnimationEventTrigger trigger) =>
                {
                    if (trigger == AnimationEventTrigger.AnimationCompleted)
                    {
                        enemySprite.Color = Color.White;
                        enemyState = CharacterState.Idle;
                    }

                };
                break;
            case CharacterState.Dead:

                enemySprite.SetAnimation("death").OnAnimationEvent += (IAnimationController animSender, AnimationEventTrigger trigger) =>
                {
                    if (trigger == AnimationEventTrigger.AnimationCompleted)
                    {
                        // Remove the enemy from the scene
                        ((MainGameScene)currentScene).RemoveEntity(this);


                    }
                };
                break;


            default: break;

        }


    }

    public override void OnCollision(CollisionEventArgs collisionInfo)
    {

        if (collisionInfo.Other is Hurtbox)
        {
            if (((Hurtbox)collisionInfo.Other).isEnemyHurtbox == false)
            {
                takeDamage(collisionInfo);
            }
        }
        else if (collisionInfo.Other is Prop || collisionInfo.Other is CollisionObject)
        {
            Bounds.Position -= collisionInfo.PenetrationVector;
            Position -= collisionInfo.PenetrationVector;

        }
    }
}

