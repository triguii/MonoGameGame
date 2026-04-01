using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Graphics;
using MonoGameLibrary;
using PilotGame.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PilotGame.GameObjects.Player;

namespace PilotGame.GameObjects.Enemies;

public class Goblin : Enemy
{

    private TimeSpan _animationDuration = TimeSpan.FromSeconds(0.175);

    private new Vector2 hitboxOffset = new Vector2(-25, 0);
    private new SizeF hitboxSize = new SizeF(20, 20);


    public override void LoadContent()
    {
        base.LoadContent();

        //Set stats
        enemyHealth = 60f;
        enemyDamage = 10f;

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

        enemySpriteSheet.DefineAnimation("attack", builder =>
        {
            builder.IsLooping(false)
                   .AddFrame("enemy1034", _animationDuration)
                   .AddFrame("enemy1035", _animationDuration)
                   .AddFrame("enemy1036", _animationDuration)
                   .AddFrame("enemy1037", _animationDuration)
                   .AddFrame("enemy1038", _animationDuration)
                   .AddFrame("enemy1039", _animationDuration)
                   .AddFrame("enemy1040", _animationDuration)
                   .AddFrame("enemy1041", _animationDuration)
                   .AddFrame("enemy1042", _animationDuration)
                   .AddFrame("enemy1043", _animationDuration)
                   .AddFrame("enemy1044", _animationDuration)
                   .AddFrame("enemy1045", _animationDuration)
                   .AddFrame("enemy1046", _animationDuration)
                   .AddFrame("enemy1047", _animationDuration)
                   .AddFrame("enemy1048", _animationDuration)
                   .AddFrame("enemy1049", _animationDuration)
                   .AddFrame("enemy1050", _animationDuration);
        });

        enemySpriteSheet.DefineAnimation("hurt", builder =>
        {
            builder.IsLooping(false)
                   .AddFrame("enemy1068", _animationDuration)
                   .AddFrame("enemy1069", _animationDuration)
                   .AddFrame("enemy1070", _animationDuration)
                   .AddFrame("enemy1071", _animationDuration)
                   .AddFrame("enemy1072", _animationDuration)
                   .AddFrame("enemy1073", _animationDuration)
                   .AddFrame("enemy1074", _animationDuration)
                   .AddFrame("enemy1075", _animationDuration);
        });

        enemySpriteSheet.DefineAnimation("death", builder =>
        {
            builder.IsLooping(false)
                    .AddFrame("enemy1085", _animationDuration)
                    .AddFrame("enemy1086", _animationDuration)
                    .AddFrame("enemy1087", _animationDuration)
                    .AddFrame("enemy1088", _animationDuration)
                    .AddFrame("enemy1089", _animationDuration)
                    .AddFrame("enemy1090", _animationDuration)
                    .AddFrame("enemy1091", _animationDuration)
                    .AddFrame("enemy1092", _animationDuration)
                    .AddFrame("enemy1093", _animationDuration)
                    .AddFrame("enemy1094", _animationDuration)
                    .AddFrame("enemy1095", _animationDuration)
                    .AddFrame("enemy1096", _animationDuration)
                    .AddFrame("enemy1097", _animationDuration);
            ;
        });



        enemySprite = new AnimatedSprite(enemySpriteSheet, "idle");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (enemyHealth <= 0)
        {
            handleDeath();
        }

    }


}