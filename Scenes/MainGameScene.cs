using Gum.DataTypes;
using Gum.Managers;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Timers;
using MonoGame.Extended.ViewportAdapters;
using MonoGameGum;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Scenes;
using PilotGame.Controllers;
using PilotGame.GameObjects;
using PilotGame.GameObjects.Enemies;
using System;
using System.Collections.Generic;

namespace PilotGame.Scenes;

public class MainGameScene : Scene
{

    private enum GameState
    {
        Playing,
        Paused
    }

    private Player _player;
    private OrthographicCamera _camera;
    public Map currentMap { get; private set; }
    private GameWindow _window;

    //Entities
    private List<Entity> _entities;
    private List<Entity> _entitesToAdd;
    private List<Entity> _entitesToRemove;


    private CollisionComponent _collisionComponent;

    //UI Manager
    GumService GumUI => GumService.Default;
    GumProjectSave GumProject;

    public MainGameScene(GameWindow window)
    {
        
        _window = window;

    }

    public override void Initialize()
    {
        base.Initialize();

        currentMap.Initialize();

        BoxingViewportAdapter viewportAdapter = new BoxingViewportAdapter(_window, Core.GraphicsDevice, 690, 360);

        _player.Initialize(this);

        //Initialize UI
        GumProject = GumUI.Initialize(Core.GraphicsDevice, "GumUI/PilotGameProject.gumx");
        var screen = GumProject.Screens.Find(item => item.Name  == "mainGameUI").ToGraphicalUiElement();
        screen.AddToRoot();

        _camera = new OrthographicCamera(viewportAdapter);
        _camera.Zoom = 1.0f;
        _camera.EnableWorldBounds(currentMap.worldBounds);

        // Enable zoom clamping to prevent viewing beyond the world
        _camera.IsZoomClampedToWorldBounds = true;


    }

    public override void LoadContent()
    {
        base.LoadContent();

        _player = new Player();

        _entities = new List<Entity>();
        _entitesToAdd = new List<Entity>();
        _entitesToRemove = new List<Entity>();

        _entities.Add(_player);

        //Inicializar props del mapa
        _entities.AddRange(PropController.InitialitzeMap("maps/sampleMapProps", this));


        //Test enemy

        Enemy enemy = new Goblin();
        enemy.Initialize(new Vector2(380, 200), this);
        _entities.Add(enemy);



        currentMap = new Map();
        currentMap.LoadContent();

        //Add collisions from map to entities list
        _entities.AddRange(currentMap.SetCollisions());


        //Gestion collisions
        _collisionComponent = new CollisionComponent(new RectangleF(0, 0, currentMap.worldBounds.Width, currentMap.worldBounds.Height));

        //Load entities
        foreach (Entity entity in _entities)
        {

            entity.LoadContent();
            _collisionComponent.Insert(entity);

        }

    }

    public override void Update(GameTime gameTime)
    {

        //Update entities
        foreach (Entity entity in _entities)
        {

            entity.Update(gameTime);

        }

        //Check if there are any new entites to add or remove and update the main list accordingly
        if (_entitesToAdd.Count != 0)
        {   
            _entities.AddRange(_entitesToAdd);
            _entitesToAdd.Clear();
        }
        if (_entitesToRemove.Count != 0)
        {
            foreach (Entity entity in _entitesToRemove)
            {
                if (_entities.Contains(entity))
                {
                    _entities.Remove(entity);

                }
            }
            _entitesToRemove.Clear();
        }

        currentMap.Update(gameTime);
        _collisionComponent.Update(gameTime);

        _camera.LookAt(_player.Position);

        //Update UI
        GumUI.Update(gameTime);

    }

    public override void Draw(GameTime gameTime)
    {
        // Clear the back buffer.
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);

        // Get the camera's transformation matrix
        Matrix transformMatrix = _camera.GetViewMatrix();

        currentMap.Draw(gameTime, _camera);

        Core.SpriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp, transformMatrix : transformMatrix);

        
        //  Draw entities sorted by their Y position.

        foreach (Entity entity in _entities)
        {
            // Map the Y position to a 0.0 - 1.0 float. 
            // Clamp it just to be safe so it never exceeds 1.0f or drops below 0.0f.
            float depth = MathHelper.Clamp((entity.Position.Y) / currentMap.worldBounds.Size.Y, 0f, 1f);

            entity.Draw(gameTime, depth);
        }

        
        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();

        //Update UI
        GumUI.Draw();

    }

    public override void UnloadContent()
    {
        _player.UnloadContent();
        currentMap.UnloadContent();
    }

    public void AddEntity(Entity entity)
    {
        _entitesToAdd.Add(entity);
        entity.LoadContent();
        _collisionComponent.Insert(entity);
    }

    public void RemoveEntity(Entity entity)
    {
        _entitesToRemove.Add(entity);
        _collisionComponent.Remove(entity);
    }

}

