using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Forms;
using System.Diagnostics;

namespace Testing.Common
{
	public abstract class Scene
	{
		protected Stopwatch appTime;
		protected Stopwatch renderStopwatch;
		protected int frames;

		public Scene()
		{
			frames = 0;

			appTime = new Stopwatch();
			renderStopwatch = new Stopwatch();
			///gjhdfghjfgj 
            ///
			appTime.Start();
		}

		public abstract void Initialize(IControlRenderDevice render);

		public abstract void Draw(IControlRenderDevice render);
	}
}
