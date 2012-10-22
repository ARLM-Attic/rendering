using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Modeling;

namespace System.Rendering.Services
{
    public struct MeshService : IRenderDeviceService
    {
        public MeshService(IRenderDevice render)
        {
            this._Render = render;
            this._Factories = new Stack<IMeshFactory> ();
            this._Factories.Push(new DefaultMeshFactory());
        }

        Stack<IMeshFactory> _Factories;
        public IMeshFactory Factory
        {
            get
            {
                return _Factories.Peek();
            }
        }

        public void Register(IMeshFactory meshFactory)
        {
            this._Factories.Push(meshFactory);
        }

        IRenderDevice _Render;
        public IRenderDevice Render
        {
            get { return _Render; }
        }
    }

    public interface IMeshFactory
    {
        IMeshManager Create(VertexBuffer vertexes, IndexBuffer indices);
    }

    class DefaultMeshFactory : IMeshFactory
    {
        public IMeshManager Create(VertexBuffer vertexes, IndexBuffer indices)
        {
            return new DefaultMeshManager(vertexes, indices);
        }
    }
}
