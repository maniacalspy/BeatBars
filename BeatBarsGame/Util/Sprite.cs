using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatBarsGame
{
    class Sprite : DrawableGameComponent
    {
        public Vector2 Location, Direction, Origin;
        public float Speed;
        public float Rotation; //rotation in degrees
        public Color DrawColor;
        public SpriteEffects SpriteEffects;

        public Matrix spriteTransform;

        public Rectangle LocationRect { get { return locationRect; } set { locationRect = value; } }

        public Color[] SpriteTextureData;

        protected Texture2D spriteTexture;

        public Texture2D SpriteTexture
        {
            get { return spriteTexture; }

            set
            {
                spriteTexture = value;

                this.SpriteTextureData = new Color[this.spriteTexture.Width * this.spriteTexture.Height];
                this.spriteTexture.GetData(this.SpriteTextureData);

            }
        }

        protected float timeSinceLastUpdate;
        protected Rectangle locationRect;

        private Rectangle _rectangle;

        public Rectangle Rectangle { get { return this._rectangle; } set { this._rectangle = value; } }

        protected float scale;

        public float Scale
        {
            get { return this.scale; }

            set
            {
                if(value != this.scale)
                {
                    SetTranformAndRect();
                }
                this.scale = value;
            }
        }

        public Sprite(Game game) : base(game)
        {
            this.Scale = 1;
            _rectangle = new Rectangle();
        }

        public override void Initialize()
        {
            SpriteEffects = SpriteEffects.None;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.Origin = Vector2.Zero;

            this.DrawColor = Color.White;


            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            timeSinceLastUpdate = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            SetTranformAndRect();
            UpdateDrawingRect();
            base.Update(gameTime);
        }

        private void UpdateDrawingRect()
        {
            _rectangle.X = (int)Location.X;
            _rectangle.Y = (int)Location.Y;

            //_rectangle.Width = (int)(spriteTexture.Width * this.Scale);
            //_rectangle.Height = (int)(spriteTexture.Height * this.Scale);
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(spriteTexture,
                _rectangle,
                null,
                this.DrawColor,
                MathHelper.ToRadians(Rotation),
                this.Origin,
                SpriteEffects,
                0);
        }

        public static Rectangle CalculateBoundingRectangle(Rectangle rectangle,
                                                           Matrix transform)
        {
            // Get all four corners in local space
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return that as a rectangle
            return new Rectangle((int)min.X, (int)min.Y,
                                 (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        public virtual void SetTranformAndRect()
        {
            //The first time this is called the spritetexture may not be loaded
            //try and catch is too slow
            if (this.spriteTexture != null)
            {
                // Build the block's transform
                spriteTransform =
                    Matrix.CreateTranslation(new Vector3(this.Origin * -1, 0.0f)) *
                    Matrix.CreateScale(this.Scale) *
                    Matrix.CreateRotationZ(0.0f) *
                    Matrix.CreateTranslation(new Vector3(this.Location, 0.0f));

                // Calculate the bounding rectangle of this block in world space
                this.locationRect = CalculateBoundingRectangle(
                         new Rectangle(0, 0, (int)(this.spriteTexture.Width * Scale),
                             (int)(this.spriteTexture.Height * Scale)),
                         spriteTransform);
            }
        }
    }
}
