﻿// Author:
//       Gabriel Reiser <gabe@reisergames.com>
//
// Copyright (c) 2010-2016 Reiser Games, LLC.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using Reactor.Geometry;
using Reactor.Math;
using Reactor.Platform.OpenGL;
using Reactor.Types.States;

namespace Reactor.Types
{
    /// <summary>
    ///     The RMeshBuilder provides basic geometry building methods and represents a Mesh in it's basic form.  A vertex
    ///     buffer, and index buffer, and a shader.
    /// </summary>
    public class RMeshBuilder : RRenderNode, IDisposable
    {
        #region Members

        internal RMaterial _material;

        internal RIndexBuffer _index;

        internal int vertCount;

        #endregion

        #region Properties

        public RMaterial Material
        {
            get => _material;
            set => _material = value;
        }

        public RPrimitiveType PrimitiveType { get; set; }

        #endregion

        #region Methods

        public RMeshBuilder()
        {
            Scale = Vector3.One;
            Rotation = Quaternion.Identity;
            Position = Vector3.Zero;
            _material = RMaterial.defaultMaterial;
            IsDrawable = true;
            CullEnable = true;
            CullMode = RCullMode.CullClockwiseFace;
            DepthWrite = true;
            BlendEnable = true;
            PrimitiveType = RPrimitiveType.Triangles;
        }


        public void CreateBox(Vector3 Center, Vector3 Size, bool FlipNormals)
        {
            var vertices = new RVertexData[36];


            // Calculate the position of the vertices on the top face.
            var topLeftFront = Position + new Vector3(-1.0f, 1.0f, -1.0f) * Size;
            var topLeftBack = Position + new Vector3(-1.0f, 1.0f, 1.0f) * Size;
            var topRightFront = Position + new Vector3(1.0f, 1.0f, -1.0f) * Size;
            var topRightBack = Position + new Vector3(1.0f, 1.0f, 1.0f) * Size;

            // Calculate the position of the vertices on the bottom face.
            var btmLeftFront = Position + new Vector3(-1.0f, -1.0f, -1.0f) * Size;
            var btmLeftBack = Position + new Vector3(-1.0f, -1.0f, 1.0f) * Size;
            var btmRightFront = Position + new Vector3(1.0f, -1.0f, -1.0f) * Size;
            var btmRightBack = Position + new Vector3(1.0f, -1.0f, 1.0f) * Size;

            // Normal vectors for each face (needed for lighting / display)
            var normalFront = new Vector3(0.0f, 0.0f, -1.0f) * Size;
            var normalBack = new Vector3(0.0f, 0.0f, 1.0f) * Size;
            var normalTop = new Vector3(0.0f, 1.0f, 0.0f) * Size;
            var normalBottom = new Vector3(0.0f, -1.0f, 0.0f) * Size;
            var normalLeft = new Vector3(1.0f, 0.0f, 0.0f) * Size;
            var normalRight = new Vector3(-1.0f, 0.0f, 0.0f) * Size;

            // UV texture coordinates
            var textureTopLeft = new Vector2(1.0f * Size.X, 0.0f * Size.Y);
            var textureTopRight = new Vector2(0.0f * Size.X, 0.0f * Size.Y);
            var textureBottomLeft = new Vector2(1.0f * Size.X, 1.0f * Size.Y);
            var textureBottomRight = new Vector2(0.0f * Size.X, 1.0f * Size.Y);

            // Add the vertices for the FRONT face.
            vertices[0] = new RVertexData(topLeftFront, normalFront, Vector3.Zero, Vector3.Zero, textureTopLeft);
            vertices[1] = new RVertexData(btmLeftFront, normalFront, Vector3.Zero, Vector3.Zero, textureBottomLeft);
            vertices[2] = new RVertexData(topRightFront, normalFront, Vector3.Zero, Vector3.Zero, textureTopRight);
            vertices[3] = new RVertexData(btmLeftFront, normalFront, Vector3.Zero, Vector3.Zero, textureBottomLeft);
            vertices[4] = new RVertexData(btmRightFront, normalFront, Vector3.Zero, Vector3.Zero, textureBottomRight);
            vertices[5] = new RVertexData(topRightFront, normalFront, Vector3.Zero, Vector3.Zero, textureTopRight);

            // Add the vertices for the BACK face.
            vertices[6] = new RVertexData(topLeftBack, normalBack, Vector3.Zero, Vector3.Zero, textureTopRight);
            vertices[7] = new RVertexData(topRightBack, normalBack, Vector3.Zero, Vector3.Zero, textureTopLeft);
            vertices[8] = new RVertexData(btmLeftBack, normalBack, Vector3.Zero, Vector3.Zero, textureBottomRight);
            vertices[9] = new RVertexData(btmLeftBack, normalBack, Vector3.Zero, Vector3.Zero, textureBottomRight);
            vertices[10] = new RVertexData(topRightBack, normalBack, Vector3.Zero, Vector3.Zero, textureTopLeft);
            vertices[11] = new RVertexData(btmRightBack, normalBack, Vector3.Zero, Vector3.Zero, textureBottomLeft);

            // Add the vertices for the TOP face.
            vertices[12] = new RVertexData(topLeftFront, normalTop, Vector3.Zero, Vector3.Zero, textureBottomLeft);
            vertices[13] = new RVertexData(topRightBack, normalTop, Vector3.Zero, Vector3.Zero, textureTopRight);
            vertices[14] = new RVertexData(topLeftBack, normalTop, Vector3.Zero, Vector3.Zero, textureTopLeft);
            vertices[15] = new RVertexData(topLeftFront, normalTop, Vector3.Zero, Vector3.Zero, textureBottomLeft);
            vertices[16] = new RVertexData(topRightFront, normalTop, Vector3.Zero, Vector3.Zero, textureBottomRight);
            vertices[17] = new RVertexData(topRightBack, normalTop, Vector3.Zero, Vector3.Zero, textureTopRight);

            // Add the vertices for the BOTTOM face. 
            vertices[18] = new RVertexData(btmLeftFront, normalBottom, Vector3.Zero, Vector3.Zero, textureTopLeft);
            vertices[19] = new RVertexData(btmLeftBack, normalBottom, Vector3.Zero, Vector3.Zero, textureBottomLeft);
            vertices[20] = new RVertexData(btmRightBack, normalBottom, Vector3.Zero, Vector3.Zero, textureBottomRight);
            vertices[21] = new RVertexData(btmLeftFront, normalBottom, Vector3.Zero, Vector3.Zero, textureTopLeft);
            vertices[22] = new RVertexData(btmRightBack, normalBottom, Vector3.Zero, Vector3.Zero, textureBottomRight);
            vertices[23] = new RVertexData(btmRightFront, normalBottom, Vector3.Zero, Vector3.Zero, textureTopRight);

            // Add the vertices for the LEFT face.
            vertices[24] = new RVertexData(topLeftFront, normalLeft, Vector3.Zero, Vector3.Zero, textureTopRight);
            vertices[25] = new RVertexData(btmLeftBack, normalLeft, Vector3.Zero, Vector3.Zero, textureBottomLeft);
            vertices[26] = new RVertexData(btmLeftFront, normalLeft, Vector3.Zero, Vector3.Zero, textureBottomRight);
            vertices[27] = new RVertexData(topLeftBack, normalLeft, Vector3.Zero, Vector3.Zero, textureTopLeft);
            vertices[28] = new RVertexData(btmLeftBack, normalLeft, Vector3.Zero, Vector3.Zero, textureBottomLeft);
            vertices[29] = new RVertexData(topLeftFront, normalLeft, Vector3.Zero, Vector3.Zero, textureTopRight);

            // Add the vertices for the RIGHT face. 
            vertices[30] = new RVertexData(topRightFront, normalRight, Vector3.Zero, Vector3.Zero, textureTopLeft);
            vertices[31] = new RVertexData(btmRightFront, normalRight, Vector3.Zero, Vector3.Zero, textureBottomLeft);
            vertices[32] = new RVertexData(btmRightBack, normalRight, Vector3.Zero, Vector3.Zero, textureBottomRight);
            vertices[33] = new RVertexData(topRightBack, normalRight, Vector3.Zero, Vector3.Zero, textureTopRight);
            vertices[34] = new RVertexData(topRightFront, normalRight, Vector3.Zero, Vector3.Zero, textureTopLeft);
            vertices[35] = new RVertexData(btmRightBack, normalRight, Vector3.Zero, Vector3.Zero, textureBottomRight);

            if (FlipNormals)
                for (var i = 0; i < 36; i++)
                    vertices[i].Normal *= -1.0f;
            VertexBuffer = new RVertexBuffer(typeof(RVertexData), vertices.Length,
                RBufferUsage.WriteOnly);

            VertexBuffer.SetData(vertices);
            vertices = null;
            vertCount = 36;
            Position = Center;
        }

        public void CreateFullscreenQuad()
        {
            var viewport = REngine.Instance._viewport;
            //CreateQuad(new Vector2(0, 0), new Vector2(1,1), true);
            CreateQuad(new Vector2(0, 0), new Vector2(viewport.Width, viewport.Height), true);
        }

        /// <summary>
        ///     Creates a quad from screen coordinates.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="size">Size.</param>
        /// <param name="deviceNormalized">If set to <c>true</c> the position and size are already device normalized.</param>
        public void CreateQuad(Vector2 position, Vector2 size, bool deviceNormalized)
        {
            var vertices = new RVertexData2D[4];
            var indices = new short[6] { 0, 1, 2, 0, 2, 3 };
            if (!deviceNormalized)
            {
                // Kinda like a 2d unproject,  screen coords to device normal coords...
                var viewport = REngine.Instance._viewport;
                var w = new Vector2(viewport.Width, viewport.Height);
                var l = size;
                var d = l / w;
                size.X = d.X;
                size.Y = d.Y;
                var p = position / w;
                position.X = p.X;
                position.Y = p.Y;
            }

            vertices[0] = new RVertexData2D(new Vector2(position.X, position.Y), new Vector2(0, 0));
            vertices[1] = new RVertexData2D(new Vector2(position.X + size.X, position.Y), new Vector2(1, 0));
            vertices[2] = new RVertexData2D(new Vector2(position.X + size.X, position.Y + size.Y), new Vector2(1, 1));
            vertices[3] = new RVertexData2D(new Vector2(position.X, position.Y + size.Y), new Vector2(0, 1));

            VertexBuffer = new RVertexBuffer(vertices[0].Declaration, 4, RBufferUsage.WriteOnly);
            VertexBuffer.SetData(vertices);

            _index = new RIndexBuffer(typeof(short), 6, RBufferUsage.WriteOnly);
            _index.SetData(indices);
        }

        public void CreateSphere(Vector3 Center, float Radius, int Tessellation)
        {
            var Stacks = Tessellation;
            var Slices = Tessellation * 2;

            var vertices = new List<RVertexData>();


            var dphi = MathHelper.Pi / Stacks;
            var dtheta = MathHelper.TwoPi / Slices;

            var index = 0;
            vertices.Add(new RVertexData(new Vector3(0, -1f, 0) * Radius, new Vector3(0, -1f, 0), Vector3.Zero,
                Vector3.Zero, Vector2.Zero));
            for (var i = 0; i < Stacks - 1; i++)
            {
                var latitude = (i + 1) * MathHelper.Pi / Stacks - MathHelper.PiOver2;

                var dy = (float)System.Math.Sin(latitude);
                var dxz = (float)System.Math.Cos(latitude);

                // Create a single ring of vertices at this latitude.
                for (var j = 0; j < Slices; j++)
                {
                    var longitude = j * MathHelper.TwoPi / Slices;

                    var dx = (float)System.Math.Cos(longitude) * dxz;
                    var dz = (float)System.Math.Sin(longitude) * dxz;


                    var normal = new Vector3(dx, dy, dz);
                    var position = normal * Radius;
                    var tex = new Vector2(1.0f - (j / (float)Slices - 1), 1.0f - i / (float)(Stacks - 1));
                    var tangent = Vector3.Cross(position, Vector3.UnitX);
                    var binormal = Vector3.Cross(position, tangent);
                    vertices.Add(new RVertexData(position, Vector3.Normalize(normal), binormal, tangent, tex));
                }
            }

            vertices.Add(new RVertexData(new Vector3(0, 1f, 0) * Radius, new Vector3(0, 1f, 0), Vector3.Zero,
                Vector3.Zero, Vector2.Zero));
            vertCount = vertices.Count;
            /*for (int x = 0; x < Stacks-1; x++)
                for (int y = 0; y < Slices-1; y++)
                {
                    //Vector3 normal = Vector3.Normalize(vertices[y * Stacks + x].position);
                    //Normal.Normalize();
                    //vertices[y * Stacks + x].texture = new Vector2(((float)x) / (float)Slices, ((float)y) / (float)Stacks);
                    // Tangent Data.
                    RVERTEXFORMAT v = vertices[y * Stacks + x];
                    if (x != 0 && x < Slices - 1)
                        v.tangent = vertices[y * Stacks + x - 1].position - vertices[y * Stacks + x + 1].position;
                    else
                        if (x == 0)
                            v.tangent = vertices[y * Stacks + x].position - vertices[y * Stacks + x+1].position;
                        else
                            v.tangent = vertices[y * Stacks + x - 1].position - vertices[y * Stacks + x].position;

                    // Bi Normal Data.
                    if (y != 0 && y < Stacks - 1)
                        v.binormal = vertices[(y - 1) * Stacks + x].position - vertices[(y + 1) * Stacks + x].position;
                    else
                        if (y == 0)
                            v.binormal = vertices[y * Stacks + x].position - vertices[(y + 1) * Stacks + x].position;
                        else
                            v.binormal = vertices[(y - 1) * Stacks + x].position - vertices[y * Stacks + x].position;

                    //vertices[y * Stacks + x].normal = normal;
                    //vertices[y * Stacks + x].normal.Normalize();
                    vertices[y * Stacks + x] = v;

                }*/
            var indices = new List<ushort>();
            for (var i = 0; i < Slices; i++)
            {
                indices.Add(0);
                indices.Add((ushort)(1 + (i + 1) % Slices));
                indices.Add((ushort)(1 + i));
            }

            // Fill the sphere body with triangles joining each pair of latitude rings.
            for (var i = 0; i < Stacks - 2; i++)
            for (var j = 0; j < Slices; j++)
            {
                var nextI = i + 1;
                var nextJ = (j + 1) % Slices;

                indices.Add((ushort)(1 + i * Slices + j));
                indices.Add((ushort)(1 + i * Slices + nextJ));
                indices.Add((ushort)(1 + nextI * Slices + j));

                indices.Add((ushort)(1 + i * Slices + nextJ));
                indices.Add((ushort)(1 + nextI * Slices + nextJ));
                indices.Add((ushort)(1 + nextI * Slices + j));
            }

            // Create a fan connecting the top vertex to the top latitude ring.
            for (var i = 0; i < Slices; i++)
            {
                indices.Add((ushort)(vertices.Count - 1));
                indices.Add((ushort)(vertices.Count - 2 - (i + 1) % Slices));
                indices.Add((ushort)(vertices.Count - 2 - i));
            }

            VertexBuffer = new RVertexBuffer(typeof(RVertexData), vertices.Count,
                RBufferUsage.None);

            VertexBuffer.SetData(vertices.ToArray());
            //vertCount = vertices.Length;
            vertices = null;

            _index = new RIndexBuffer(typeof(ushort), indices.Count, RBufferUsage.None, false);

            _index.SetData(indices.ToArray());
            indices = null;

            Position = Center;
        }

        public void CreatePoints(Vector3[] points, RColor[] colors)
        {
        }

        public override void Render()
        {
            if (IsDrawable)
            {
                ApplyState();
                base.Render();
                /*
                GL.Enable(EnableCap.CullFace);
                REngine.CheckGLError();
                GL.FrontFace(FrontFaceDirection.Ccw);
                REngine.CheckGLError();
                GL.CullFace(CullFaceMode.FrontAndBack);
                REngine.CheckGLError();
                */
                _material.Shader.Bind();
                _material.Apply();
                VertexBuffer.BindVertexArray();
                VertexBuffer.Bind();

                VertexBuffer.VertexDeclaration.Apply(_material.Shader, IntPtr.Zero);

                _material.Shader.BindSemantics(matrix, REngine.camera.View, REngine.camera.Projection);
                if (PrimitiveType == RPrimitiveType.Points)
                {
                    GL.Enable(EnableCap.PointSprite);
                    GL.Enable(EnableCap.ProgramPointSize);
                }

                if (_index != null)
                {
                    var shortIndices = _index.IndexElementSize == RIndexElementSize.SixteenBits;
                    var indexElementType = shortIndices ? DrawElementsType.UnsignedShort : DrawElementsType.UnsignedInt;
                    var indexElementSize = shortIndices ? 2 : 4;
                    var indexOffsetInBytes = (IntPtr)indexElementSize;
                    var indexElementCount = _index.GetElementCountArray(PrimitiveType, vertCount);
                    _index.Bind();
                    REngine.CheckGLError();
                    GL.DrawRangeElements((BeginMode)PrimitiveType, 0, 1, _index.IndexCount, indexElementType,
                        indexOffsetInBytes);
                    REngine.CheckGLError();
                    _index.Unbind();
                    REngine.CheckGLError();
                }
                else
                {
                    GL.DrawArrays((BeginMode)PrimitiveType, 0, VertexBuffer.VertexCount);
                    REngine.CheckGLError();
                }

                if (PrimitiveType == RPrimitiveType.Points)
                {
                    GL.Disable(EnableCap.PointSprite);
                    GL.Disable(EnableCap.ProgramPointSize);
                }

                _material.Shader.Unbind();

                VertexBuffer.Unbind();
                VertexBuffer.UnbindVertexArray();
            }
        }

        public void Dispose()
        {
            if (_index != null)
                _index.Dispose();
            VertexBuffer.Dispose();
        }

        #endregion
    }
}