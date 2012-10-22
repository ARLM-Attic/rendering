using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;

namespace System.Rendering.Xna
{
  internal class XnaCustomProcessorContext : ContentProcessorContext
  {
    OpaqueDataDictionary parameters = new OpaqueDataDictionary();
    ContentBuildLogger logger = new XnaCustomLogger();

    public override TargetPlatform TargetPlatform
    {
      get { return TargetPlatform.Windows; }
    }
    public override GraphicsProfile TargetProfile
    {
      get { return GraphicsProfile.Reach; }
    }
    public override string BuildConfiguration
    {
      get { return string.Empty; }
    }
    public override string IntermediateDirectory
    {
      get { return string.Empty; }
    }
    public override string OutputDirectory
    {
      get { return string.Empty; }
    }
    public override string OutputFilename
    {
      get { return string.Empty; }
    }
    public override OpaqueDataDictionary Parameters
    {
      get { return parameters; }
    }
    public override ContentBuildLogger Logger
    {
      get { return logger; }
    }

    public override void AddDependency(string filename) { }
    public override void AddOutputFile(string filename) { }

    public override TOutput Convert<TInput, TOutput>(TInput input, string processorName, OpaqueDataDictionary processorParameters)
    {
      throw new NotImplementedException();
    }
    public override TOutput BuildAndLoadAsset<TInput, TOutput>(ExternalReference<TInput> sourceAsset, string processorName, OpaqueDataDictionary processorParameters, string importerName)
    {
      throw new NotImplementedException();
    }
    public override ExternalReference<TOutput> BuildAsset<TInput, TOutput>(ExternalReference<TInput> sourceAsset, string processorName, OpaqueDataDictionary processorParameters, string importerName, string assetName)
    {
      throw new NotImplementedException();
    }

    class XnaCustomLogger : ContentBuildLogger
    {
      public override void LogMessage(string message, params object[] messageArgs)
      {
      }
      public override void LogImportantMessage(string message, params object[] messageArgs)
      {
      }
      public override void LogWarning(string helpLink, ContentIdentity contentIdentity, string message, params object[] messageArgs)
      {
      }
    }
  }
}
