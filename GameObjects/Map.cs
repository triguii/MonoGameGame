using PilotGame.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Timers;
using MonoGameLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using MonoGame.Extended.Tilemaps;
using MonoGame.Extended.Tilemaps.Rendering;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace PilotGame.GameObjects;

public class Map
{
    private Tilemap _tilemap;
    private TilemapSpriteBatchRenderer _renderer;

    public Rectangle worldBounds { get; set; }


    public void Initialize()
    {

    }

    public void LoadContent()
    {

        _tilemap = Core.Content.Load<Tilemap>("maps/Sample Map");

        _renderer = new TilemapSpriteBatchRenderer();
        _renderer.LoadTilemap(_tilemap);

        worldBounds = _tilemap.WorldBounds; 

    }

    public void Update(GameTime gameTime)
    {
        _renderer.Update(gameTime);
    }

    public void Draw(GameTime gameTime, OrthographicCamera camera)
    {

        _renderer.Draw(Core.SpriteBatch, camera);

    }




}

