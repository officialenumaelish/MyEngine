using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Entities;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.Events
{
    /// <summary>
    /// Gestiona dibujado, animación, posición y colisiones de sprites y animated sprites.
    /// </summary>
    public class SpriteManager
    {

        private readonly Dictionary<Sprite, Vector2> _sprites = [];
        private readonly Dictionary<AnimatedSprite, Vector2> _animatedSprites = [];
        private readonly Dictionary<int, Rectangle> _colisionesSpites = new Dictionary<int, Rectangle>();
        private readonly List<int> _hiddenSpriteIds = [];

        private Player _player;
        private Rectangle _playerColition = new Rectangle();
        public List<Sprite> colisionesJugador = new List<Sprite>();
        public SpriteManager()
        {
            
        }

        #region Añadir Sprites

        public void AddPlayer(Player player)
        {
            _player = player;

            _playerColition = new Rectangle
            {
                Height = (int)player.GetAnimation().Height,
                Width = (int)player.GetAnimation().Width,
                X = (int)player.Position().X,
                Y = (int)player.Position().Y
            };
        }

        // También corrige el método Add para no modificar Origin
        public void Add(Sprite sprite, Vector2 posicion)
        {
            sprite.SpriteId = (_sprites.Count + _animatedSprites.Count) + 1;
            _sprites.Add(sprite, posicion);

            //almacenar la colision internamente de dicho sprite
            Rectangle spriteColision = new Rectangle
            {
                X = (int)posicion.X,
                Y = (int)posicion.Y,
                Width = (int)sprite.Width,
                Height = (int)sprite.Height,
            };
            _colisionesSpites.Add(sprite.SpriteId, spriteColision);
            // NO modifiques sprite.Origin aquí
        }

        public void Add(AnimatedSprite sprite, Vector2 posicion)
        {
            sprite.SpriteId = (_sprites.Count + _animatedSprites.Count) + 1;
            _animatedSprites.Add(sprite, posicion);

            //almacenar la colision internamente de dicho sprite
            Rectangle spriteColision = new Rectangle
            {
                X = (int)posicion.X,
                Y = (int)posicion.Y,
                Width = (int)sprite.Width,
                Height = (int)sprite.Height,
            };
            _colisionesSpites.Add(sprite.SpriteId, spriteColision);
            // NO modifiques sprite.Origin aquí
        }
        #endregion

        #region Gestionar Visibilidad Sprites
        public void OcultarSprites(Sprite sprite)
        {
            _hiddenSpriteIds.Add(sprite.SpriteId);
        }
        public void MostrarSprite(Sprite sprite)
        {
            _hiddenSpriteIds.Remove(sprite.SpriteId);
        }
        #endregion

        #region actualizar Posicion

        public bool SetPlayerPosition(Vector2 NewPosition)
        {
            Rectangle colisionPropia = new Rectangle(
               (int)NewPosition.X,
               (int)NewPosition.Y,
               (int)_player.GetAnimation().Width,
               (int)_player.GetAnimation().Height
           );

            bool colision = false;

            foreach (var s in _sprites)
            {
                if (colision == true) continue;
                if (!colisionesJugador.Any(a => a.SpriteId == s.Key.SpriteId)) continue;

                _colisionesSpites.TryGetValue(s.Key.SpriteId, out Rectangle colisionOtro);
                colision = colisionPropia.Intersects(colisionOtro);
            }

            foreach (var s in _animatedSprites)
            {
                if (colision == true) continue;
                if (!colisionesJugador.Any(a => a.SpriteId == s.Key.SpriteId)) continue;

                _colisionesSpites.TryGetValue(s.Key.SpriteId, out Rectangle colisionOtro);
                colision = colisionPropia.Intersects(colisionOtro);
            }

            if (!colision)
            {
                _player.Position(NewPosition);
                _playerColition = colisionPropia;
            }

            return colision;

        }
        public bool SetPosition(Sprite sprite, Vector2 NewPosition, HashSet<int> colisionar)
        {
            // Crear Rectangle con la NUEVA posición para verificar colisiones
            Rectangle colisionPropia = new Rectangle(
                (int)NewPosition.X,
                (int)NewPosition.Y,
                (int)sprite.Width,
                (int)sprite.Height
            );

            bool colision = false;

            foreach (var s in _sprites)
            {
                if (s.Key.SpriteId == sprite.SpriteId || colision == true) continue;
                if (!colisionar.Contains(s.Key.SpriteId)) continue;

                _colisionesSpites.TryGetValue(s.Key.SpriteId, out Rectangle colisionOtro);
                colision = colisionPropia.Intersects(colisionOtro);
            }

            foreach (var s in _animatedSprites)
            {
                if (s.Key.SpriteId == sprite.SpriteId || colision == true) continue;
                if (!colisionar.Contains(s.Key.SpriteId)) continue;

                _colisionesSpites.TryGetValue(s.Key.SpriteId, out Rectangle colisionOtro);
                colision = colisionPropia.Intersects(colisionOtro);
            }

            // SIEMPRE actualizar la posición, sin importar si hay colisión o no
            // Solo prevenir el movimiento si hay colisión
            if (!colision)
            {
                // Actualizar TODAS las referencias de posición
                _sprites[sprite] = NewPosition;
                _colisionesSpites[sprite.SpriteId] = colisionPropia;
                // NO cambies sprite.Origin aquí - puede interferir con el dibujado
            }

            return colision;
        }

        public bool SetPosition(AnimatedSprite sprite, Vector2 NewPosition, HashSet<int> colisionar)
        {
            // Crear Rectangle con la NUEVA posición para verificar colisiones
            Rectangle colisionPropia = new Rectangle(
                (int)NewPosition.X,
                (int)NewPosition.Y,
                (int)sprite.Width,
                (int)sprite.Height
            );

            bool colision = false;

            foreach (var s in _sprites)
            {
                if (s.Key.SpriteId == sprite.SpriteId || colision == true) continue;
                if (!colisionar.Contains(s.Key.SpriteId)) continue;

                _colisionesSpites.TryGetValue(s.Key.SpriteId, out Rectangle colisionOtro);
                colision = colisionPropia.Intersects(colisionOtro);
            }

            foreach (var s in _animatedSprites)
            {
                if (s.Key.SpriteId == sprite.SpriteId || colision == true) continue;
                if (!colisionar.Contains(s.Key.SpriteId)) continue;

                _colisionesSpites.TryGetValue(s.Key.SpriteId, out Rectangle colisionOtro);
                colision = colisionPropia.Intersects(colisionOtro);
            }

            // SIEMPRE actualizar la posición, sin importar si hay colisión o no
            if (!colision)
            {
                // Actualizar TODAS las referencias de posición
                _animatedSprites[sprite] = NewPosition;
                _colisionesSpites[sprite.SpriteId] = colisionPropia;
                // NO cambies sprite.Origin aquí - puede interferir con el dibujado
            }

            return colision;
        }
        #endregion

        #region Dibujar

        public void Draw(SpriteBatch batch)
        {

            _player.Draw(batch);

            foreach (var kvp in _sprites)
            {
                if (!_hiddenSpriteIds.Contains(kvp.Key.SpriteId))
                    kvp.Key.Draw(batch, kvp.Value);
            }

            foreach (var kvp in _animatedSprites)
            {
                if (!_hiddenSpriteIds.Contains(kvp.Key.SpriteId))
                    kvp.Key.Draw(batch, kvp.Value);
            }

        }


        public void UpdateAnimated(GameTime gameTime)
        {
            _player.Update(gameTime);
            foreach(var kvp in _animatedSprites)
            {
                kvp.Key.Update(gameTime);
            }
        }

        #endregion

        #region remover
        public void Remove(AnimatedSprite sprite)
        {
            _animatedSprites.Remove(sprite);
            _colisionesSpites.Remove(sprite.SpriteId);
            _hiddenSpriteIds.Remove(sprite.SpriteId);
        }

        public void RemoveAll() 
        {
            _sprites.Clear();
            _animatedSprites.Clear();
            _colisionesSpites.Clear();
            _hiddenSpriteIds.Clear();
        }
        #endregion

    }
}
