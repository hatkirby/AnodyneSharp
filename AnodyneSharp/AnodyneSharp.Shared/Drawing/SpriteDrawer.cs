﻿using AnodyneSharp.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnodyneSharp.Drawing
{
    public static class SpriteDrawer
    {
        public static Color BackColor;
        public static SamplerState SamplerState;

        private static GraphicsDevice _graphicsDevice;
        private static SpriteBatch _spriteBatch;
        private static SpriteBatch _guiSpriteBatch;

        private static float _uiScale;

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            BackColor = Color.Honeydew;
            SamplerState = SamplerState.PointWrap;

            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            _guiSpriteBatch = new SpriteBatch(_graphicsDevice);
        }

        public static void BeginDraw(Camera camera)
        {
            _graphicsDevice.Clear(BackColor);
            _spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, blendState: BlendState.AlphaBlend, samplerState: SamplerState, transformMatrix: camera.Transform);
        }

        public static void BeginGUIDraw(float uiScale)
        {
            _uiScale = uiScale;
            _guiSpriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, blendState: BlendState.AlphaBlend, samplerState: SamplerState);
        }



        public static void DrawSprite(Texture2D texture, Rectangle rect, Rectangle? sRect = null, Color? color = null, float rotation = 0, float Z = 0)
        {
            _spriteBatch.Draw(texture, rect,
                sRect, color ?? Color.White, rotation,
                new Vector2(rect.Width / 2, rect.Height / 2), SpriteEffects.None, Z);
        }

        public static void DrawGuiSprite(Texture2D texture, Vector2 pos, Rectangle? sRect = null, Color? color = null, float rotation = 0, float Z = 0)
        {
            _guiSpriteBatch.Draw(texture, pos * _uiScale,
                sRect, color ?? Color.White, rotation,
                new Vector2(0,0), _uiScale, SpriteEffects.None, Z);
        }

        public static void EndDraw()
        {
            _spriteBatch.End();
        }

        public static void EndGUIDraw()
        {
            _guiSpriteBatch.End();
        }
    }
}
