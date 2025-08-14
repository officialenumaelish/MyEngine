using EnumaElish.Core.Content.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace EnumaElish.Core;

public class Game1 : MonoGameLibrary.Core
{

    
    public Game1() : base("Enuma Elish", 1920, 1080, true)
    {
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {

        ChangeScene(new TitleScene());
        // TODO: use this.Content to load your game content here
    }

}
