using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void LoadContent()
    {
        base.LoadContent();

        _player = new Player();
        _player.LoadContent();

    }

    public override void Update(GameTime gameTime)
    {
        _player.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        // Clear the back buffer.
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);

        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);


        _player.Draw(gameTime);

        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();

    }



}

