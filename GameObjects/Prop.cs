using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Timers;
using MonoGameLibrary;
using PilotGame.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PilotGame.GameObjects;

public abstract class Prop : Entity
{


    public override void LoadContent()
    {
        
    }
    public override void Update(GameTime gameTime)
    {

    }
    public override void Draw(GameTime gameTime, float depth)
    {
        base.Draw(gameTime, depth);
    }
}