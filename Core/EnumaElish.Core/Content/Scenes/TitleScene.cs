using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Entities;
using MonoGameLibrary.Events;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Scenes;

namespace EnumaElish.Core.Content.Scenes
{
    public class TitleScene : Scene
    {
        private Button _startButton;
        private GameObject _panel;

        private Button _sound;
        private SpriteFont fuente;
        public TitleScene()
        {
        }

        public override void Initialize()
        {
            _startButton = new Button();
            SpriteManager = new SpriteManager();
            _sound = new Button();
            base.Initialize();
        }

        public override void LoadContent()
        {
            TextureAtlas textureAtlas = TextureAtlas.FromFile(Content, "Utilities/hoja_de_utilidades.xml");
            // fuente = Content.Load<SpriteFont>("Fonts/main");

            _startButton.AddSprite(ButtonState.normal, textureAtlas.CreateSprite("play_normal"));
            _startButton.AddSprite(ButtonState.hover, textureAtlas.CreateSprite("play_normal_hover"));
            _startButton.Position(new Vector2(1550, 830f));
            _startButton.Scale(new Vector2(0.7f));
            // _startButton.Font = fuente;

            // _startButton.Text = "Empezar";
            _startButton.Clicked += () => { Debug.WriteLine("Empezamos!!!"); };

            _sound.AddSprite(ButtonState.normal, textureAtlas.CreateSprite("SoundOff"));
            _sound.AddSprite(ButtonState.hover, textureAtlas.CreateSprite("SoundOn"));
            _sound.Clicked += () => { };
            _sound.Position(new Vector2(1780, 50));
            _sound.Scale(new Vector2(0.25f));

            _panel = new GameObject(textureAtlas.CreateSprite("frameBorder"));
            _panel.Position(new Vector2(30f));
            _panel._sprite.Scale = new Vector2(0.5f);
            SpriteManager.Add(_panel._sprite, _panel.Position());


            base.LoadContent();
            
        }

        public override void Update(GameTime gameTime)
        {
            _startButton.Update(gameTime);
            _sound.Update(gameTime);

            SpriteManager.UpdateAnimated(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            MonoGameLibrary.Core.GraphicsDevice.Clear(Color.RoyalBlue);
            var brocha = MonoGameLibrary.Core.SpriteBatch;
            brocha.Begin();
            //Dibujar Contenido
            SpriteManager.Draw(brocha);
            _startButton.Draw(brocha);
            _sound.Draw(brocha);

            MonoGameLibrary.Core.SpriteBatch.End();
            base.Draw(gameTime);
        }

    }
}