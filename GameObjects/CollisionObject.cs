using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Shapes;
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

public class CollisionObject : Entity
{

    //Rectangle constructor
    public CollisionObject(RectangleF boundingRectangle)
    {
        Bounds = boundingRectangle;

    }

    //Ellipse constructor
    public CollisionObject(CircleF boundingCircle)
    {
        Bounds = boundingCircle;

    }
}