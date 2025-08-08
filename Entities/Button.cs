using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;

namespace MonoGameLibrary.Entities
{
    public class Button
    {
        private Dictionary<ButtonState, Sprite> _button = new Dictionary<ButtonState, Sprite>();
        private ButtonState _StateActive = ButtonState.normal;
        private Vector2 position;
        private MouseState _previousMouseState;
        public event Action Clicked;
        public Button()
        {

            TouchPanel.EnabledGestures = GestureType.Tap;
        }

        public void AddSprite(ButtonState state, Sprite sprite)
        {
            _button.TryAdd(state, sprite);
        }

        public void AddFilter(Color color)
        {
            _button[_StateActive].Color = color;
        }
        public void Draw(SpriteBatch bash)
        {
            _button[_StateActive].Draw(bash, position);
        }

        public Vector2 Position()
        {
            return position;
        }

        public void Position(Vector2 vector)
        {
            position = vector;
        }

        public void Scale(Vector2 scale)
        {
            _button[ButtonState.normal].Scale = scale;
            _button[ButtonState.hover].Scale = scale;
        }

        public void Update(GameTime gameTime)
        {
            var sprite = _button[_StateActive];

            // Área del botón (rectángulo para colisión)
            Rectangle buttonBounds = new Rectangle(
                (int)Position().X,
                (int)Position().Y,
                (int)sprite.Width,
                (int)sprite.Height
            );

            // ===== DETECCIÓN MOUSE =====
            var mouseState = Mouse.GetState();
            if (buttonBounds.Contains(mouseState.Position))
                _StateActive = ButtonState.hover;
            else
                _StateActive = ButtonState.normal;

            if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                _previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
                buttonBounds.Contains(mouseState.Position))
            {
                Clicked?.Invoke();
            }
            _previousMouseState = mouseState;

            // ===== DETECCIÓN TOUCH =====
            TouchCollection touchState = TouchPanel.GetState();
            foreach (var touch in touchState)
            {
                if (touch.State == TouchLocationState.Pressed &&
                    buttonBounds.Contains(touch.Position))
                {
                    Clicked?.Invoke();
                }
            }
        }
    }
    public enum ButtonState
    {
        normal,
        hover
    }
}
