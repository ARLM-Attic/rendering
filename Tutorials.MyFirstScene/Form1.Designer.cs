namespace Tutorials.MyFirstScene
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.renderedControl1 = new System.Rendering.Forms.RenderedControl();
            this.SuspendLayout();
            // 
            // renderedControl1
            // 
            this.renderedControl1.DisplayInfo = false;
            this.renderedControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renderedControl1.IncludeRayPicker = false;
            this.renderedControl1.Location = new System.Drawing.Point(0, 0);
            this.renderedControl1.Name = "renderedControl1";
            this.renderedControl1.Render = null;
            this.renderedControl1.Size = new System.Drawing.Size(736, 519);
            this.renderedControl1.TabIndex = 0;
            this.renderedControl1.Text = "renderedControl1";
            this.renderedControl1.InitializeRender += new System.EventHandler<System.Rendering.Forms.RenderEventArgs>(this.renderedControl1_InitializeRender);
            this.renderedControl1.Rendered += new System.EventHandler<System.Rendering.Forms.RenderEventArgs>(this.renderedControl1_Rendered);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 519);
            this.Controls.Add(this.renderedControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Rendering.Forms.RenderedControl renderedControl1;
    }
}

