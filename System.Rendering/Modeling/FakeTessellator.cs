using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Rendering.Modeling
{
	public class FakeTessellator : ITessellator
	{
		public IRenderDevice Render
		{
			get { throw new NotImplementedException(); }
		}

		public void Draw<GP>(GP primitive) where GP : struct, IGraphicPrimitive
		{
			throw new NotImplementedException();
		}

		public void Draw<FVF, ResultFVF>(Action action, Func<FVF, ResultFVF> function)
			where FVF : struct
			where ResultFVF : struct
		{
			throw new NotImplementedException();
		}

		public void Draw(Action action, Maths.Matrix4x4 transform)
		{
			throw new NotImplementedException();
		}

		public bool IsSupported<GP>() where GP : struct, IGraphicPrimitive
		{
			return true;
		}
	}
}
