using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using MonoGameLibrary;
using PilotGame.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotGame.GameObjects.Props;

public class Animated_Tree1 : Prop
{

    private TimeSpan _animationDuration = TimeSpan.FromSeconds(0.175);
    private AnimatedSprite _sprite;

    public override void LoadContent()
    {
        base.LoadContent();

        PropController.setAnimation("Animated_Tree1", builder =>
        {
            builder.IsLooping(true)
                   .AddFrame("Animated Tree1_frame1", _animationDuration)
                   .AddFrame("Animated Tree1_frame2", _animationDuration)
                   .AddFrame("Animated Tree1_frame3", _animationDuration)
                   .AddFrame("Animated Tree1_frame4", _animationDuration)
                   .AddFrame("Animated Tree1_frame5", _animationDuration)
                   .AddFrame("Animated Tree1_frame6", _animationDuration)
                   .AddFrame("Animated Tree1_frame7", _animationDuration)
                   .AddFrame("Animated Tree1_frame8", _animationDuration);
        });

        _sprite = new AnimatedSprite(PropController.propsSheet, "Animated_Tree1");
        _sprite.Origin = new Vector2(_sprite.Size.X / 1.5f, _sprite.Size.Y / 1.5f);
        Size = _sprite.Size;

    }

    public override void Update(GameTime gameTime)
    {
        _sprite.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, float depth)
    {
        _sprite.Depth = depth;
        Core.SpriteBatch.Draw(_sprite, Position, 0, new Vector2(1f));
    }


}

