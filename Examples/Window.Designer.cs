namespace CodeFull.CarveSharp.Examples
{
    partial class Window
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblHelp = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lstMeshes = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.viewport = new CodeFull.Controls.GLViewport3D();
            this.btnCut = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnCut);
            this.groupBox1.Controls.Add(this.lblHelp);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.lstMeshes);
            this.groupBox1.Location = new System.Drawing.Point(657, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(115, 537);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Controls";
            // 
            // lblHelp
            // 
            this.lblHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHelp.Location = new System.Drawing.Point(3, 455);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Size = new System.Drawing.Size(106, 79);
            this.lblHelp.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 133);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(103, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Load mesh";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lstMeshes
            // 
            this.lstMeshes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstMeshes.FormattingEnabled = true;
            this.lstMeshes.Location = new System.Drawing.Point(6, 19);
            this.lstMeshes.Name = "lstMeshes";
            this.lstMeshes.Size = new System.Drawing.Size(103, 108);
            this.lstMeshes.TabIndex = 0;
            this.lstMeshes.SelectedValueChanged += new System.EventHandler(this.lstMeshes_SelectedValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.viewport);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(639, 537);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "OpenGL";
            // 
            // viewport
            // 
            this.viewport.ArcballSensitivity = 0.01D;
            this.viewport.BackColor = System.Drawing.Color.Black;
            this.viewport.ClearColor = System.Drawing.Color.White;
            this.viewport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewport.FarClipping = 64D;
            this.viewport.FieldOfView = 45D;
            this.viewport.Location = new System.Drawing.Point(3, 16);
            this.viewport.Name = "viewport";
            this.viewport.NearClipping = 0.1D;
            this.viewport.Size = new System.Drawing.Size(633, 518);
            this.viewport.TabIndex = 0;
            this.viewport.VSync = false;
            this.viewport.SelectionChanged += new System.EventHandler(this.viewport_SelectionChanged);
            // 
            // btnCut
            // 
            this.btnCut.Location = new System.Drawing.Point(6, 162);
            this.btnCut.Name = "btnCut";
            this.btnCut.Size = new System.Drawing.Size(103, 23);
            this.btnCut.TabIndex = 3;
            this.btnCut.Text = "Cut 1st using 2nd";
            this.btnCut.UseVisualStyleBackColor = true;
            this.btnCut.Click += new System.EventHandler(this.btnCut_Click);
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Window";
            this.Text = "Example";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private CodeFull.Controls.GLViewport3D viewport;
        private System.Windows.Forms.ListBox lstMeshes;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblHelp;
        private System.Windows.Forms.Button btnCut;
    }
}

