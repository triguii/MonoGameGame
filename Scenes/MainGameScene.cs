using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Scenes;
using PilotGame.GameObjects;

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

    public MainGameScene(GameWindow window)
    {
        _window = window;
    }

    public override void Initialize()
    {
        base.Initialize();

        BoxingViewportAdapter viewportAdapter = new BoxingViewportAdapter(_window, Core.GraphicsDevice, 690, 360);
        _player.Initialize(currentMap);

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
        _player.LoadContent();

        currentMap = new Map();
        currentMap.LoadContent();


    }

    public override void Update(GameTime gameTime)
    {
        _player.Update(gameTime);
        currentMap.Update(gameTime);

        _camera.LookAt(_player.Position);

    }

    public override void Draw(GameTime gameTime)
    {
        // Clear the back buffer.
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);

        // Get the camera's transformation matrix
        Matrix transformMatrix = _camera.GetViewMatrix();

        currentMap.Draw(gameTime, _camera);

        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix : transformMatrix);


        _player.Draw(gameTime);


        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();

    }

    public override void UnloadContent()
    {
        _player.UnloadContent();
    }



}

