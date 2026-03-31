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
    }

}

