using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace System.Rendering.Services
{
    public struct LoaderService <T> : IRenderDeviceService where T:IGraphicResource
    {
        public LoaderService(IRenderDevice render)
        {
            this._Render = render;
            this.loaders = new Stack<ILoader<T>>();
        }

        private IRenderDevice _Render;
        public IRenderDevice Render { get { return _Render; } }

        private Stack<ILoader<T>> loaders;

        public bool IsSupported { get { return loaders != null; } }

        public void Register(ILoader<T> loader)
        {
            this.loaders.Push(loader);
        }

        public T Load(Stream stream)
        {
            if (loaders == null) throw new NotSupportedException("This Render doesnt support texture loading from streams.");

            T resource;
            foreach (var loader in loaders)
            {
                var p = stream.Position;
                if (loader.Load(stream, out resource))
                    return resource;
                else
                    stream.Position = p;
            }
            return default (T);
        }

        public T Load(string path)
        {
            var s = new FileStream(path, FileMode.Open);
            var t = Load(s);
            s.Close();
            return t;
        }

        public IEnumerable<string> Formats
        {
            get
            {
                if (loaders == null) yield break;
                foreach (var loader in loaders)
                    foreach (var f in loader.Formats)
                        yield return f;
                ;
            }
        }

        public void Save(T resource, Stream stream, string format)
        {
            if (loaders == null) throw new NotSupportedException("This Render doesnt support resource loading from streams.");

            foreach (var loader in loaders)
                if (loader.Save(stream, resource, format))
                    return;

            throw new NotSupportedException("Format " + format + " is not supported by this render.");
        }

        public void Save(T resource, string path)
        {
            var resourceFormat = Path.GetExtension(path).ToLower();

            var s = new FileStream(path, FileMode.Create);
            Save(resource, s, resourceFormat);
            s.Close();
        }
    }

    public interface ILoader<T> where T : IGraphicResource
    {
        IEnumerable<string> Formats { get; }

        bool Load(Stream stream, out T resource);

        bool Save(Stream stream, T resource, string format);
    }
}
