using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Maths;

namespace System.Rendering.Modeling
{
	public class TranformedModel : IModel
	{
		private IModel _model;

		public TranformedModel(IModel model)
			: this(model, Matrices.I)
		{
		}

		public TranformedModel(IModel model, Matrix4x4 tranform)
		{
			if (model == null)
				throw new ArgumentNullException("model");

			_model = model;
			Transform = tranform;
		}

		#region [ IModel members ]
		
		public bool IsSupported(IRenderDevice device)
		{
			return _model.IsSupported(device);
		}

		public void Tesselate(ITessellator tessellator)
		{
			tessellator.Draw(() => 
			{
				_model.Tesselate(tessellator);
			}, Transform);
		}

		public IRenderDevice Render
		{
			get { return _model.Render; }
		}

		public Location Location
		{
			get { return _model.Location; }
		}

		public Resourcing.IAllocateable Clone(IRenderDevice render)
		{
			return _model.Clone(render);
		}

		public void Dispose()
		{
			_model.Dispose();
		}
		
		#endregion

		public Matrix4x4 Transform { get; set; }
	}

	public static class TranformedExtensions
	{
		public static IModel Translated(this IModel model, Vector3 offset)
		{
			return new TranformedModel(model, Matrices.Translate(offset));
		}
	}
}
