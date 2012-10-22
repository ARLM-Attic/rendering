using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Maths;
using System.Runtime.InteropServices;

namespace System.Rendering
{
  /// <summary>
  /// Defines a standard context for shaders.
  /// </summary>
    public interface IBasicContext
    {
        /// <summary>
        /// Gets the world transformation of the render
        /// </summary>
        Matrix4x4 World { get; }
        /// <summary>
        /// Gets the view transformation of the render
        /// </summary>
        Matrix4x4 View { get; }
        /// <summary>
        /// Gets the projection transformation of the render
        /// </summary>
        Matrix4x4 Projection { get; }

        /// <summary>
        /// Gets the material of the render
        /// </summary>
        MaterialContext Material { get; }

        int NumberOfLights { get; }

        /// <summary>
        /// Gets the light 0 of the render
        /// </summary>
        LightingContext Light0 { get; }

        /// <summary>
        /// Gets the light 1 of the render
        /// </summary>
        LightingContext Light1 { get; }

        /// <summary>
        /// Gets the light 2 of the render
        /// </summary>
        LightingContext Light2 { get; }

        /// <summary>
        /// Gets the ambient color of the scene
        /// </summary>
        Vector3 AmbientColor { get; }

        /// <summary>
        /// Gets whenever a sample will be used.
        /// </summary>
        bool HasSampling { get; }

        /// <summary>
        /// Gets the sampler 2d set in the pipeline.
        /// </summary>
        Sampler2D Sampler { get; }
    }

  /// <summary>
  /// Defines a material context stored in the render.
  /// </summary>
  public struct MaterialContext
  {
    /// <summary>
    /// Ambient color of the material.
    /// </summary>
    public Vector3 Ambient;
    /// <summary>
    /// Diffuse color of the material.
    /// </summary>
    public Vector3 Diffuse;
    /// <summary>
    /// Specular color of the material.
    /// </summary>
    public Vector3 Specular;
    /// <summary>
    /// Emissive color of the material.
    /// </summary>
    public Vector3 Emissive;
    /// <summary>
    /// Specular shininess of the material.
    /// </summary>
    public float Shininess;
      /// <summary>
      /// Opacity of this material. Used during alpha blending.
      /// </summary>
    public float Opacity;
  }

  /// <summary>
  /// Defines a lighting context stored in the render. Only supported directional and point lights.
  /// </summary>
  [StructLayout (LayoutKind.Sequential)]
  public struct LightingContext
  {
    /// <summary>
    /// Gets the diffuse color of the light.
    /// </summary>
    public Vector3 Diffuse;
    /// <summary>
    /// Gets the specular color of the light.
    /// </summary>
    public Vector3 Specular;
    /// <summary>
    /// Gets the position or direction of the light. If the W component is 1, it mean that is a position, if is 0 is a direction.
    /// </summary>
    public Vector4 Position;
  }
}
