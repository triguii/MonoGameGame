using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using MonoGameLibrary;
using PilotGame.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotGame.GameObjects.Enemies;

public class Goblin : Enemy
{

    private TimeSpan _animationDuration = TimeSpan.FromSeconds(0.175);

    private new Vector2 hitboxOffset = new Vector2(-25, 0);
    private new SizeF hitboxSize = new SizeF(20, 20);


    public override void LoadContent()
    {
        base.LoadContent();

        enemySpriteAtlas = Core.Content.Load<Texture2DAtlas>("images/enemy1");
        enemySpriteSheet = new SpriteSheet("images/enemy1-texture", enemySpriteAtlas);

        setAnimations();

        //SetHitbox
        Bounds = new RectangleF(Position + hitboxOffset, hitboxSize);

        enemySprite.Origin = new Vector2(enemySprite.Size.X * 0.5f, enemySprite.Size.Y * 0.5f);


    }

    private void setAnimations()
    {
        enemySpriteSheet.DefineAnimation("idle", builder =>
        {
            builder.IsLooping(true)
                   .AddFrame("enemy1000", _animationDuration)
                   .AddFrame("enemy1001", _animationDuration)
                   .AddFrame("enemy1002", _animationDuration)
                   .AddFrame("enemy1003", _animationDuration)
                   .AddFrame("enemy1004", _animationDuration)
                   .AddFrame("enemy1005", _animationDuration)
                   .AddFrame("enemy1006", _animationDuration)
                   .AddFrame("enemy1007", _animationDuration); ;
        });

        enemySpriteSheet.DefineAnimation("walk", builder =>
        {
            builder.IsLooping(true)
                   .AddFrame("enemy1017", _animationDuration)
                   .AddFrame("enemy1018", _animationDuration)
                   .AddFrame("enemy1019", _animationDuration)
                   .AddFrame("enemy1020", _animationDuration)
                   .AddFrame("enemy1021", _animationDuration)
                   .AddFrame("enemy1022", _animationDuration)
                   .AddFrame("enemy1023", _animationDuration)
                   .AddFrame("enemy1024", _animationDuration); ;
        });

        enemySprite = new AnimatedSprite(enemySpriteSheet, "idle");
    }
}