/*
    Mandelbrot.exe - A Mandelbrot Set generator.
    Copyright (C) 2017 Andrea Rossini

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

namespace Mandelbrot
{
	partial class Form1
	{
		/// <summary>
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Pulire le risorse in uso.
		/// </summary>
		/// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Codice generato da Progettazione Windows Form

		/// <summary>
		/// Metodo necessario per il supporto della finestra di progettazione. Non modificare
		/// il contenuto del metodo con l'editor di codice.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.optionsPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.statusInfoLabel = new System.Windows.Forms.Label();
			this.homeButton = new System.Windows.Forms.Button();
			this.optionsPanel.SuspendLayout();
			this.SuspendLayout();
			//
			// optionsPanel
			//
			this.optionsPanel.BackColor = System.Drawing.Color.White;
			this.optionsPanel.Controls.Add(this.statusInfoLabel);
			this.optionsPanel.Controls.Add(this.homeButton);
			this.optionsPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.optionsPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.optionsPanel.Location = new System.Drawing.Point(0, 0);
			this.optionsPanel.Margin = new System.Windows.Forms.Padding(0);
			this.optionsPanel.Name = "optionsPanel";
			this.optionsPanel.Padding = new System.Windows.Forms.Padding(23, 13, 12, 13);
			this.optionsPanel.Size = new System.Drawing.Size(210, 493);
			this.optionsPanel.TabIndex = 0;
			this.optionsPanel.Visible = false;
			//
			// statusInfoLabel
			//
			this.statusInfoLabel.AutoSize = true;
			this.statusInfoLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.statusInfoLabel.Location = new System.Drawing.Point(26, 13);
			this.statusInfoLabel.MaximumSize = new System.Drawing.Size(170, 0);
			this.statusInfoLabel.Name = "statusInfoLabel";
			this.statusInfoLabel.Size = new System.Drawing.Size(43, 17);
			this.statusInfoLabel.TabIndex = 1;
			this.statusInfoLabel.Text = "label1";
			//
			// homeButton
			//
			this.homeButton.FlatAppearance.BorderSize = 0;
			this.homeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
			this.homeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.homeButton.Location = new System.Drawing.Point(26, 34);
			this.homeButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.homeButton.Name = "homeButton";
			this.homeButton.Size = new System.Drawing.Size(170, 30);
			this.homeButton.TabIndex = 0;
			this.homeButton.Text = "Home";
			this.homeButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.homeButton.UseVisualStyleBackColor = true;
			this.homeButton.Click += new System.EventHandler(this.homeButton_Click);
			//
			// Form1
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(671, 493);
			this.Controls.Add(this.optionsPanel);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "Form1";
			this.Text = "Mandelbrot set";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
			this.optionsPanel.ResumeLayout(false);
			this.optionsPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel optionsPanel;
		private System.Windows.Forms.Button homeButton;
		private System.Windows.Forms.Label statusInfoLabel;
	}
}

