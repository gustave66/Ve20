using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueWaves
{
    class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int Speed { get; set; }
        private int currentFrame;
        private int totalFrames;
        private int slowDown = 1;

        public AnimatedSprite(Texture2D texture, int rows, int columns, int speed)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            Speed = speed;
            currentFrame = 0;
            totalFrames = Rows * Columns;
        }

        public void Update()
        {
            if (slowDown == Speed)
            {
                currentFrame++;
                slowDown = 1;
            }
            else
            {
                currentFrame++;
                slowDown += 1;
            }
            
            if (currentFrame == totalFrames)
                currentFrame = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Begin();
            //spriteBatch.Draw(Texture, location, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(Texture, location, destinationRectangle, sourceRectangle, new Vector2(0, 0), 0f, null, Color.White, SpriteEffects.None, 0f);
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }
    }
}
