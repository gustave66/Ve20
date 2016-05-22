using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace RogueWaves
{
    internal class Player
    {
        public Texture2D texture;
        public Texture2D weaponTexture;
        public Vector2 position;
        public Rectangle boundingBox, weaponBoundingBox;
        public Vector2 velocity, origin, weaponPosition;
        public bool hasJumped, isSwinging, isAlive;
        public Color[,] playerColorArray, playerWeaponData;
        public float timer, weaponRotation;
        GraphicsDevice graphics;
        public Color[] textureData;

        //New animation technique
        private AnimatedSprite playerSprite;

        public Vector2 Position
        {
            get { return position; }
        }

        public Player()
        {
            position = new Vector2(600, 200);
            hasJumped = false;
            isSwinging = false;
            isAlive = true;
        }

        public void LoadContent(ContentManager Content, GraphicsDevice newGraphics)
        {
            texture = Content.Load<Texture2D>("stock_WalkF");
            playerSprite = new AnimatedSprite(texture, 1, 10, 1);

            weaponTexture = Content.Load<Texture2D>("dwraf");
            playerWeaponData = TextureTo2DArray(weaponTexture);
            graphics = newGraphics;
        }

        public void Update(GameTime gameTime)
        {
            position += velocity;
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            weaponBoundingBox = new Rectangle((int)weaponPosition.X, (int)weaponPosition.Y, weaponTexture.Width, weaponTexture.Height);
            origin = new Vector2(boundingBox.Width / 2, boundingBox.Height / 2);

            Input(gameTime);
            if (velocity.X != 0 && isSwinging == false)
            {
                playerSprite.Update();
            }
            else if (velocity.X == 0 && isSwinging == false)
            {

            }

            //if (velocity.Y < 10)
                //velocity.Y += 0.4f;

            //weaponPosition = drawWeaponHere(texture);
            //weaponRotation = rotateWeapon(texture);
        }

        private void Input(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A) && isSwinging == false)
                velocity.X = -(float)gameTime.ElapsedGameTime.TotalMilliseconds / 3;
            else if (Keyboard.GetState().IsKeyDown(Keys.D) && isSwinging == false)
                velocity.X = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 3;
            else velocity.X = 0f;

            if (Keyboard.GetState().IsKeyDown(Keys.W) && hasJumped == false)
            {
                //position.Y -= 5f;
                //velocity.Y = -9f;
                hasJumped = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && isSwinging == false)
            {
                isSwinging = true;
            }
        }

        public void Collision(Rectangle newRectangle, int xOffset, int yOffset)
        {
            if (boundingBox.TouchTopOf(newRectangle))
            {
                boundingBox.Y = newRectangle.Y - boundingBox.Height;
                velocity.Y = 0f;
                hasJumped = false;
            }

            if (boundingBox.TouchLeftOf(newRectangle))
            {
                position.X = newRectangle.X - boundingBox.Width - 2;
            }
            if (boundingBox.TouchRightOf(newRectangle))
            {
                position.X = newRectangle.X + newRectangle.Width + 2;
            }
            if (boundingBox.TouchBottomOf(newRectangle))
            {
                velocity.Y = 1f;
            }

            if (position.X < 0) position.X = 0;
            if (position.X > xOffset - boundingBox.Width) position.X = xOffset - boundingBox.Width;
            if (position.Y < 0) velocity.Y = 1f;
            if (position.Y > yOffset - boundingBox.Height) position.Y = yOffset - boundingBox.Height;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            playerSprite.Draw(spriteBatch, position);

            //spriteBatch.Draw(weaponTexture, weaponPosition, null, Color.White, weaponRotation, new Vector2(13, 120), 1f, flip, 0f);
            //spriteBatch.Draw(texture, position, null, Color.White, 0f, new Vector2(0, 0), 1f, flip, 0f);
        }

        public Color[,] TextureTo2DArray(Texture2D texture)
        {

            Color[] colors1D = new Color[texture.Width * texture.Height]; //The hard to read,1D array
            texture.GetData(colors1D); //Get the colors and add them to the array

            Color[,] colors2D = new Color[texture.Width, texture.Height]; //The new, easy to read 2D array
            for (int x = 0; x < texture.Width; x++) //Convert!
                for (int y = 0; y < texture.Height; y++)
                    colors2D[x, y] = colors1D[x + y * texture.Width];

            return colors2D;
        }

        public Vector2 drawWeaponHere(Texture2D texture)
        {
            playerColorArray = TextureTo2DArray(texture);
            Vector2 weapPos = new Vector2(3250, 550);

            for (int y = 0; y < texture.Height; y++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    Color color = playerColorArray[x, y];
                    Color weapColor = new Color(57, 43, 31, 255);
                    if (color == weapColor) { weapPos.X = x + position.X; weapPos.Y = y + position.Y; }
                }
            }

            return weapPos;
        }

        public float rotateWeapon(Texture2D texture)
        {
            playerColorArray = TextureTo2DArray(texture);
            double weapRot = new double();
            weapRot = 0;
            Vector2 pos1 = new Vector2(0, 0);
            Vector2 pos2 = new Vector2(0, 0);
            Vector2 pos3 = new Vector2(0, 0);
            Vector2 eyePos = new Vector2(0, 0);

            for (int y = 0; y < texture.Height; y++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    Color color = playerColorArray[x, y];

                    Color weapBottomColor = new Color(57, 44, 32, 255);
                    Color weapMidColor = new Color(57, 43, 31, 255);
                    Color weapTopColor = new Color(57, 44, 31, 255);
                    Color eyeColor = new Color(25, 84, 15, 255);

                    if (color == weapTopColor) { pos1.X = x + position.X; pos1.Y = y + position.Y; }
                    if (color == weapMidColor) { pos2.X = x + position.X; pos2.Y = y + position.Y; }
                    if (color == weapBottomColor) { pos3.X = x + position.X; pos3.Y = y + position.Y; }
                    if (color == eyeColor) { eyePos.X = x + position.X; eyePos.Y = y + position.Y; }

                    double opp = pos2.Y - eyePos.Y;
                    double adj = pos2.X - eyePos.X;

                    //Standing guy eye position is 75, 21.  Glove color pixel position is 113, 55
                    //When the hands are below the starting pos by 5 lets tweak the rotation down to mimic moving the wrist
                    //When the hands are above the eye pos by the height of the head start keeping the rotation at perpendicular
                    double correction = -0.9;
                    if (opp > 45) { correction = 0.5; }
                    if (opp < 0) { correction = -0.25; }
                    weapRot = (Math.Atan2(opp, adj)) + correction;

                    //1.57 is pointing to the right
                    //-1.57 is pointing to the left
                    //0.7854 is pointing NE
                    //0 is north
                    //3.142 is pointing south

                    if (opp < 0 && weapRot < -1.57) { weapRot = -1.57; }
                    //weapRot = (Math.Atan2(pos1.Y - pos2.Y, pos1.X - pos2.X)) - 1.5;
                }
            }

            return (float)weapRot;
        }
    }
}