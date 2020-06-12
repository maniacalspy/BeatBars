using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BeatBarsGame
{

    class DrawableTriangle : DrawableGameComponent
    {

        enum colorState { Solid, Changing, Finished}
        colorState _state;

        float changeRate;
        double changeElapsedTime;

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

        public DrawableTriangle(Game game, Vector2[] vertexPositionVectors) : base(game)
        {
            changeRate = .1f;

            VertexPositionColor[] temp = new VertexPositionColor[3];
            for (int i = 0; i < 3; i++)
            {
                temp[i].Position = new Vector3(vertexPositionVectors[i].X, -vertexPositionVectors[i].Y, 0);
            }
            _pcVerticies = temp;
        }

        public override void Initialize()
        {
            world = Matrix.CreateTranslation(0, 0, 0);
            view = Matrix.CreateLookAt(new Vector3(0, 0, 1.5f), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), this.Game.GraphicsDevice.Viewport.AspectRatio, 0.01f, 100f);

            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(_pcVerticies);



            CalcTriangle();


            base.Initialize();
        }

        void CalcTriangle()
        {
            Vector2[] convertedPositions = new Vector2[3];
            for (int i = 0; i < 3; i++)
            {
                convertedPositions[i] = new Vector2((_pcVerticies[i].Position.X + 1) * (Game.GraphicsDevice.Viewport.Width / 2),
                    (_pcVerticies[i].Position.Y + 1) * (Game.GraphicsDevice.Viewport.Height / 2));
            }
            triangle = new Triangle(convertedPositions);
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            basicEffect = new BasicEffect(GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            StateUpdate(gameTime);
            vertexBuffer.SetData<VertexPositionColor>(_pcVerticies);
            base.Update(gameTime);
        }

        void StateUpdate(GameTime gameTime)
        {
            switch (_state)
            {
                case colorState.Changing:
                    ChangingUpdate(gameTime);
                    break;
                case colorState.Finished:
                    for (int i = 1; i < 3; i++)
                    {
                        _pcVerticies[i].Color = _pcVerticies[0].Color;
                    }
                    changeElapsedTime = 0;
                    _state = colorState.Solid;
                    break;
            }
        }

        void ChangingUpdate(GameTime gameTime)
        {
            changeElapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            for(int i = 1; i < 3; i++)
            {
                _pcVerticies[i].Color = Color.Lerp(_pcVerticies[i].Color, _pcVerticies[0].Color, (float)changeElapsedTime);
            }
            if (changeElapsedTime >= changeRate) _state = colorState.Finished;
        }

        protected void GradualColorChange(Color newColor)
        {
            _state = colorState.Changing;
            _pcVerticies[0].Color = newColor;
        }

        public override void Draw(GameTime gameTime)
        {
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
