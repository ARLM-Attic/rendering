#define FLOAT

#if DECIMAL
using FLOATINGTYPE = System.Decimal;
#endif
#if FLOATINGTYPE
using FLOATINGTYPE = System.FLOATINGTYPE;
#endif
#if FLOAT
using FLOATINGTYPE = System.Single;
#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.Rendering.Modeling;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Maths;
using System.Rendering.RenderStates;
using OpenTK.Graphics.OpenGL;

namespace System.Rendering.OpenGL
{
    partial class OpenGLRender
    {
        protected class OpenGLModelling : TessellatorBase,
            ITessellatorOf<Basic>
        {
            public OpenGLModelling(OpenGLRender parent) : base(parent) { }

            #region IModelling<Text3D> Members

            //Dictionary<string, int> fontBaseList = new Dictionary<string, int>();

            //void ITessellatorOf<Text3D>.Draw(Text3D model)
            //{
            //    ((OpenGLRender)render).SyncExecute(() =>
            //    {
            //        if (!fontBaseList.ContainsKey(model.FontFamily))
            //        {
            //            int index = 0 + fontBaseList.Count * 512;
            //            IntPtr hdc = ((OpenGLRender)render).deviceContext;

            //            GL. Gdi.SelectObject(hdc, new Font(model.FontFamily, 14).ToHfont());

            //            Tao.Platform.Windows.Gdi.GLYPHMETRICSFLOAT[] gmf = new Tao.Platform.Windows.Gdi.GLYPHMETRICSFLOAT[256];
            //            Wgl.wglUseFontOutlinesW(hdc, 0, 255, index, 0, 0, Wgl.WGL_FONT_POLYGONS, gmf);
            //            Wgl.wglUseFontOutlinesW(hdc, 0, 255, 256 + index, 0, 1, Wgl.WGL_FONT_POLYGONS, gmf);

            //            fontBaseList.Add(model.FontFamily, index);
            //        }

            //        GL.MatrixMode(GL._MODELVIEW);
            //        GL.PushMatrix();
            //        GL.ListBase(fontBaseList[model.FontFamily] + ((true) ? 256 : 0));
            //        GL.CallLists(model.Text.Length, GL._UNSIGNED_BYTE, model.Text);
            //        GL.PopMatrix();
            //    });
            //}

            #endregion

            void ITessellatorOf<Basic>.Draw(Basic primitive)
            {

                var primitiveType = OpenGLTools.Convert(primitive.Type);

                VertexBuffer finalVertexBuffer;
                IndexBuffer finalIndexBuffer;

                if (primitive.VertexBuffer.Render != this.render)
                    finalVertexBuffer = primitive.VertexBuffer.Clone(this.render) as VertexBuffer; // allocates temporaly the vertex buffer at render.
                else
                    finalVertexBuffer = primitive.VertexBuffer;

                if (primitive.Indexes != null)
                {
                    if (primitive.Indexes.Render != this.render)
                        finalIndexBuffer = primitive.Indexes.Clone(this.render) as IndexBuffer; // allocates temporaly the index buffer at render.
                    else
                        finalIndexBuffer = primitive.Indexes;
                }
                else
                    finalIndexBuffer = null;

                ((OpenGLRender)render).SyncExecute(() =>
                {

                    if (finalIndexBuffer == null) // Draw primitive
                    {
                        var vb = ((OpenGLResourcesManager.VertexBufferResourceOnDeviceManager)this.Resources.GetManagerFor<VertexBuffer>(finalVertexBuffer)).VertexBuffer;

                        vb.Activate();
                        vb.SetupPointers();
                        ((OpenGLRender.OpenGLRenderStates)((OpenGLRender)render).RenderStates).effectManager.UpdateAndApplyEffect(vb.Descriptor);

                        GL.DrawArrays(OpenGLTools.Convert(primitive.Type), primitive.StartIndex, primitive.Count);
                        vb.Deactivate();
                    }
                    else // Draw indexed primitive
                    {
                        var vb = ((OpenGLResourcesManager.VertexBufferResourceOnDeviceManager)this.Resources.GetManagerFor<VertexBuffer>(finalVertexBuffer)).VertexBuffer;
                        var ib = ((OpenGLResourcesManager.IndexBufferResourceOnDeviceManager)this.Resources.GetManagerFor<IndexBuffer>(finalIndexBuffer)).IndexBuffer;

                        vb.Activate();
                        vb.SetupPointers();
                        ib.Activate();
                        ib.SetupPointers();
                        ((OpenGLRender.OpenGLRenderStates)((OpenGLRender)render).RenderStates).effectManager.UpdateAndApplyEffect(vb.Descriptor);
                        GL.DrawRangeElements(OpenGLTools.Convert(primitive.Type), primitive.StartIndex, primitive.VertexBuffer.Length, primitive.Count, (DrawElementsType)OpenGLTools.GetBaseType(primitive.Indexes.InnerElementType), IntPtr.Zero);

                        //GL.DrawRangeElements((int)OpenGLTools.Convert(primitive.Type), 0, primitive.Indexes.Length-1, primitive.Indexes.Length,
                        //    OpenGLTools.GetBaseType (primitive.Indexes.InnerElementType),
                        //    IntPtr.Zero);
                        vb.Deactivate();
                        ib.Deactivate();
                    }
                });

                if (primitive.VertexBuffer != finalVertexBuffer)
                    finalVertexBuffer.Dispose();
                if (primitive.Indexes != finalIndexBuffer)
                    finalIndexBuffer.Dispose();
            }
        }
    }
}
