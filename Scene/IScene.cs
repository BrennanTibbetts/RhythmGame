using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public interface IScene
{
    void LoadObjects();
    void ReloadScene();
    void UpdateScene(GameTime gameTime);
    void DrawScene(SpriteBatch spriteBatch, GameTime gameTime);
}