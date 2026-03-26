using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.Tilemaps;
using MonoGame.Extended.Tilemaps.Rendering;
using MonoGame.Extended.Timers;
using MonoGame.Extended.ViewportAdapters;
using MonoGameLibrary;
using PilotGame.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;

namespace PilotGame.GameObjects;

public class Map
{
    private Tilemap _tilemap;
    private TilemapRenderer _renderer;

    private string[] _backgroundLayers;


    public Rectangle worldBounds { get; set; }


    public void Initialize()
    {

    }

    public void LoadContent()
    {

        _backgroundLayers = new string[] { "terrain0", "terrain1", "terrain2", "fences", "water",
                                           "grass_to_water_platform", "grass_platform","terrain3", "fences2" };

        _tilemap = Core.Content.Load<Tilemap>("maps/Sample Map");

        _renderer = new TilemapRenderer(Core.GraphicsDevice);
        _renderer.LoadTilemap(_tilemap);

        // Define layer groups for rendering order and optimization
        _renderer.DefineLayerGroup("Background", _backgroundLayers);


        worldBounds = _tilemap.WorldBounds; 


    }

    public void Update(GameTime gameTime)
    {
        _renderer.Update(gameTime);
    }

    public void Draw(GameTime gameTime, OrthographicCamera camera)
    {

        _renderer.BeginDraw(camera);

        _renderer.DrawLayerGroup("Background");

        _renderer.EndDraw();

    }

    public void UnloadContent()
    {
        _renderer?.Dispose();
    }

    public List<CollisionObject> SetCollisions()
    {
        // Set Collisions of the map based on collisions layer in Tiled
        List<CollisionObject> collisionsList = new List<CollisionObject>();


        TilemapObjectLayer collisionsLayer = _tilemap.Layers["collisions"] as TilemapObjectLayer;

        foreach (TilemapObject obj in collisionsLayer.Objects)
        {
            CollisionObject collisionObject;

            switch (obj)
            {
                case TilemapRectangleObject rect:
                    // Use rect.Width and rect.Height for collision bounds
                    collisionObject = new CollisionObject(new RectangleF(rect.Position, rect.Size));
                    collisionsList.Add(collisionObject);

                    break;

                case TilemapEllipseObject elli:

                    collisionObject =  new CollisionObject(new CircleF(elli.Center, elli.RadiusX));
                    collisionsList.Add(collisionObject);

                    break;



            }
        }

        return collisionsList;

    }




}

