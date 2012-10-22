using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Modelling;
using System.Rendering.RenderStates;
using Microsoft.Xna.Framework.Graphics;
using System.Maths;
using System.Rendering.Effects;

namespace System.Rendering.Xna
{
  partial class Direct3DRender
  {
    public class RenderStatesManager : RenderStatesManagerBase,
      IRenderStateSetterOf<FrameBufferState>,
      IRenderStateSetterOf<DepthBufferState>,
      IRenderStateSetterOf<StencilBufferState>,
      IRenderStateSetterOf<FillModeState>,
      IRenderStateSetterOf<CullModeState>,
      IRenderStateSetterOf<BlendOperationState>,
      IRenderStateSetterOf<MaterialState>,
      IRenderStateSetterOf<LightingState>,
      IRenderStateSetterOf<WorldState>,
      IRenderStateSetterOf<ViewState>,
      IRenderStateSetterOf<ProjectionState>
    {
      internal BasicEffect basicEffect;
      private DepthStencilState depthStencilState;
      private RasterizerState rasterizerState;
      private BlendState blendState;

      public RenderStatesManager(Direct3DRender render)
        : base(render)
      {
        render.Created += new EventHandler(render_Created);
      }

      private void render_Created(object sender, EventArgs e)
      {
        basicEffect = new BasicEffect(Device);
        depthStencilState = DepthStencilState.None.Clone();
        rasterizerState = RasterizerState.CullNone.Clone();
        blendState = BlendState.Opaque.Clone();

        Create<FrameBufferState>(FrameBufferState.Default);
        Create<DepthBufferState>(DepthBufferState.Default);
        Create<StencilBufferState>(StencilBufferState.Default);
        Create<FillModeState>(FillModeState.Default);
        Create<CullModeState>(CullModeState.Default);
        Create<BlendOperationState>(BlendOperationState.Default);
        Create<MaterialState>(MaterialState.Default);
        Create<LightingState>(LightingState.Default);
        Create<WorldState>(WorldState.Default);
        Create<ViewState>(ViewState.Default);
        Create<ProjectionState>(ProjectionState.Default);
      }

      private GraphicsDevice Device
      {
        get { return ((Direct3DRender)render).device; }
      }

      #region [ IRenderStateSetterOf<FrameBufferState> ]

      FrameBufferState IRenderStateSetterOf<FrameBufferState>.State
      {
        set
        {
          int red = Convert.ToInt32((value.Mask & ColorMask.Red) != 0);
          int green = Convert.ToInt32((value.Mask & ColorMask.Green) != 0);
          int blue = Convert.ToInt32((value.Mask & ColorMask.Blue) != 0);
          int alpha = Convert.ToInt32((value.Mask & ColorMask.Alpha) != 0);

          if (value.ClearOnSet)
            Device.Clear(ClearOptions.Target, Direct3DTools.ToXnaVector(value.DefaultValue), 0, 0);
        }
      }

      #endregion

      #region [ IRenderStateSetterOf<DepthBufferState> ]

      DepthBufferState IRenderStateSetterOf<DepthBufferState>.State
      {
        set
        {
          depthStencilState.DepthBufferEnable = value.EnableTest;
          depthStencilState.DepthBufferWriteEnable = value.WriteEnable;
          depthStencilState.DepthBufferFunction = Direct3DTools.ToCompareFunction(value.Function);

          if (value.ClearOnSet)
            Device.Clear(ClearOptions.DepthBuffer, Direct3DTools.ToXnaVector(Vectors.Black), (float)value.DefaultValue, 0);

          Device.DepthStencilState = depthStencilState.Clone();
        }
      }

      #endregion

      #region [ IRenderStateSetterOf<StencilBufferState> ]

      StencilBufferState IRenderStateSetterOf<StencilBufferState>.State
      {
        set
        {
          //depthStencilState.StencilEnable = value.TestEnable;
          //depthStencilState.StencilWriteMask = Convert.ToInt32(value.WriteMask);
          //depthStencilState.StencilMask = Convert.ToInt32(value.CompareMask);
          //depthStencilState.StencilPass = Direct3DTools.ToStencilOperation(value.Pass);
          //depthStencilState.StencilFail = Direct3DTools.ToStencilOperation(value.StencilFails);
          //depthStencilState.StencilFunction = Direct3DTools.ToCompareFunction(value.Function);
          //depthStencilState.StencilDepthBufferFail = Direct3DTools.ToStencilOperation(value.DepthFails);
          //depthStencilState.ReferenceStencil = value.Reference;

          //if (value.ClearOnSet)
          //  Device.Clear(ClearOptions.Stencil, Direct3DTools.ToXnaVector(Vectors.Black), 0, 0);

          //Device.DepthStencilState = depthStencilState.Clone();
        }
      }

      #endregion

      #region [ IRenderStateSetterOf<FillModeState> ]

      FillModeState IRenderStateSetterOf<FillModeState>.State
      {
        set
        {
          rasterizerState.FillMode = Direct3DTools.ToXnaFillMode(value);

          Device.RasterizerState = rasterizerState.Clone();
        }
      }

      #endregion

      #region [ IRenderStateSetterOf<CullModeState> ]

      CullModeState IRenderStateSetterOf<CullModeState>.State
      {
        set
        {
          rasterizerState.CullMode = Direct3DTools.ToXnaCullMode(value);

          Device.RasterizerState = rasterizerState.Clone();
        }
      }

      #endregion

      #region [ IRenderStateSetterOf<BlendOperationState> ]

      BlendOperationState IRenderStateSetterOf<BlendOperationState>.State
      {
        set
        {
          blendState.ColorSourceBlend = Direct3DTools.ToXnaBlend(value.Source);
          blendState.ColorDestinationBlend = Direct3DTools.ToXnaBlend(value.Destination);
          
          Device.BlendState = blendState.Clone();
        }
      }

      #endregion

      #region [ IRenderStateSetterOf<MaterialState> ]

      MaterialState IRenderStateSetterOf<MaterialState>.State
      {
        set
        {
          basicEffect.Alpha = value.Diffuse.W;
          basicEffect.DiffuseColor = new Microsoft.Xna.Framework.Vector3(value.Diffuse.X, value.Diffuse.Y, value.Diffuse.Z);
          basicEffect.SpecularColor = new Microsoft.Xna.Framework.Vector3(value.Specular.X, value.Specular.Y, value.Specular.Z);
          basicEffect.SpecularPower = Convert.ToSingle(value.SpecularSharpness);
          basicEffect.EmissiveColor = new Microsoft.Xna.Framework.Vector3(value.Emissive.X, value.Emissive.Y, value.Emissive.Z);
        }
      }

      #endregion

      #region [ IRenderStateSetterOf<LightingState> ]

      LightingState IRenderStateSetterOf<LightingState>.State
      {
        set
        {
          basicEffect.LightingEnabled = value.Enable;
          if (basicEffect.LightingEnabled)
          {
            basicEffect.AmbientLightColor = new Microsoft.Xna.Framework.Vector3(value.Ambient.X, value.Ambient.Y, value.Ambient.Z);
            var lights = value.Lights.OfType<DirectionalLightSource>().ToArray();
            if (lights.Length > 0)
            {
              basicEffect.DirectionalLight0.Enabled = true;
              basicEffect.DirectionalLight0.Direction = Direct3DTools.ToXnaVector(lights[0].Direction);
              basicEffect.DirectionalLight0.DiffuseColor = Direct3DTools.ToXnaVector((Vector3)lights[0].Diffuse);
              basicEffect.DirectionalLight0.SpecularColor = Direct3DTools.ToXnaVector((Vector3)lights[0].Specular);
            }
            if (lights.Length > 1)
            {
              basicEffect.DirectionalLight1.Enabled = true;
              basicEffect.DirectionalLight1.Direction = Direct3DTools.ToXnaVector(lights[1].Direction);
              basicEffect.DirectionalLight1.DiffuseColor = Direct3DTools.ToXnaVector((Vector3)lights[1].Diffuse);
              basicEffect.DirectionalLight1.SpecularColor = Direct3DTools.ToXnaVector((Vector3)lights[1].Specular);
            }
            if (lights.Length > 2)
            {
              basicEffect.DirectionalLight2.Enabled = true;
              basicEffect.DirectionalLight2.Direction = Direct3DTools.ToXnaVector((Vector3)lights[2].Direction);
              basicEffect.DirectionalLight2.DiffuseColor = Direct3DTools.ToXnaVector((Vector3)lights[2].Diffuse);
              basicEffect.DirectionalLight2.SpecularColor = Direct3DTools.ToXnaVector((Vector3)lights[2].Specular);
            }
          }
        }
      }

      #endregion

      #region [ IRenderStateSetterOf<WorldState> ]

      WorldState IRenderStateSetterOf<WorldState>.State
      {
        set
        {
          basicEffect.World = Direct3DTools.ToXnaMatrix(value.Matrix);
        }
      }

      #endregion

      #region [ IRenderStateSetterOf<ViewState> ]

      ViewState IRenderStateSetterOf<ViewState>.State
      {
        set
        {
          basicEffect.View = Direct3DTools.ToXnaMatrix(value.Matrix);
        }
      }

      #endregion

      #region [ IRenderStateSetterOf<ProjectionState> ]

      ProjectionState IRenderStateSetterOf<ProjectionState>.State
      {
        set
        {
          basicEffect.Projection = Direct3DTools.ToXnaMatrix(value.Matrix);
        }
      }

      #endregion

    }
  }
}
