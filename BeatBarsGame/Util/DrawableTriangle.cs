using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BeatBarsGame
{

    class DrawableTriangle : DrawableGameComponent
    {
        Triangle triangle;
        VertexPositionColor[] _pcVerticies;
        BasicEffect basicEffect;
        Matrix world;
        Matrix view;
        Matrix projection;
        VertexBuffer vertexBuffer;


        public Color DrawColor
        {
            set
            {
                for(int i = 0; i < 3; i++)
                {
                    _pcVerticies[i].Color = value;
                }
            }
        }

        public override void Initialize()
        {
            basicEffect = new BasicEffect(GraphicsDevice);
            world = Matrix.CreateTranslation(0, 0, 0);
            view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), this.Game.GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f);

            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(_pcVerticies);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            basicEffect = new BasicEffect(GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            vertexBuffer.SetData<VertexPositionColor>(_pcVerticies);
            base.Update(gameTime);
        }

        public DrawableTriangle(Game game, Vector2[] basisVectors) : base(game)
        {

            VertexPositionColor[] temp = new VertexPositionColor[3];
            for (int i = 0; i < 3; i++)
            {
                temp[i].Position = new Vector3(basisVectors[i].X, -basisVectors[i].Y, 0);
            }
            _pcVerticies = temp;
        }

        public DrawableTriangle(Game game, VertexPositionColor[] vertexPositionColors) : base(game)
        {
            _pcVerticies = vertexPositionColors;
            Vector2[] convertedPositions = new Vector2[3];
            for (int i = 0; i < 3; i++)
            {
                convertedPositions[i] = new Vector2((vertexPositionColors[i].Position.X + 1) * (game.GraphicsDevice.Viewport.Width / 2), 
                    (vertexPositionColors[i].Position.Y + 1) * (game.GraphicsDevice.Viewport.Height / 2));
            }
            triangle = new Triangle(convertedPositions);
        }

        public DrawableTriangle(Game game, Triangle T): base(game)
        {
            triangle = T;
            VertexPositionColor[] temp = new VertexPositionColor[3];
            for (int i = 0; i < 3; i++)
            {
                temp[i].Position = new Vector3(T.Verticies[i].X / (game.GraphicsDevice.Viewport.Width / 2) - 1, T.Verticies[i].Y / (game.GraphicsDevice.Viewport.Height / 2) - 1, 0);
            }
            _pcVerticies = temp;
        }


        public override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.VertexColorEnabled = true;

            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            }

            base.Draw(gameTime);
        }


    }
}
