using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGameLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PilotGame.GameObjects;

public class Entity
{
    public Vector2 Position { get; set; }
    public Point Size { get; set; }

    public Entity()
    {
        Position = Vector2.Zero;
    }

    public virtual void Initialize(Vector2 position) { 

        Position = position;
    
    }

    public virtual void LoadContent() {

    }

    public virtual void Draw(GameTime gameTime, float depth) { 
    
    }

    public virtual void Update(GameTime gameTime)
    {

    }


}

