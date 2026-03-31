using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PilotGame.GameObjects;

public abstract class Entity : ICollisionActor
{
    public Vector2 Position { get; set; }
    public Point Size { get; set; }
    public IShapeF Bounds { get; set; }

    public Vector2 hitboxOffset = new Vector2(0, 0);
    public SizeF hitboxSize = new SizeF(0, 0);

    public Scene currentScene;


    public Entity()
    {
        Position = Vector2.Zero;
    }

    public virtual void Initialize(Vector2 position, Scene scene) { 

        Position = position;
        currentScene = scene;
    
    }

    public virtual void LoadContent() {

    }

    public virtual void Draw(GameTime gameTime, float depth) {

        //Draw collision bounds for debugging purposes

        Core.SpriteBatch.DrawRectangle((RectangleF)Bounds, Color.Red, 1, depth + 0.01f);

    }

    public virtual void Update(GameTime gameTime)
    {

    }

    public virtual void OnCollision(CollisionEventArgs collisionInfo)
    {

    }


}

