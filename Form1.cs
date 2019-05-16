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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mandelbrot
{
	public partial class Form1 : Form
	{
		private double m_scale;
		private double m_xbase;
		private double m_ybase;
		private double m_xcenter;
		private double m_ycenter;
		private Bitmap m_currentpicture;
		private Task m_task;
		private CancellationTokenSource m_cancel;

		private Rectangle m_selrect;
		private bool m_selecting;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			m_scale = 0.01;
			m_xcenter = -1;
			m_ycenter = 0;
			m_xbase = -1 - ClientSize.Width * m_scale / 2;
			m_ybase = 0 - ClientSize.Height * m_scale / 2;

            paletteCombo.DataSource = new BindingSource(PaletteGenerator.Palettes, null);
            paletteCombo.DisplayMember = "Key";
            paletteCombo.ValueMember = "Value";
            Mandel.SetPalette(PaletteGenerator.Palettes["Default"]());
            RunRedraw();
		}

		// RunRedraw - Sempre in th GUI
		private void RunRedraw()
		{
			if (m_task != null)
			{
				m_cancel.Cancel();
				try { m_task.Wait(); } catch { } finally { m_cancel.Dispose(); }
			}

			m_cancel = new CancellationTokenSource();
			m_task =
				Mandel.GeneratePicture(m_xbase, m_ybase, m_scale, ClientSize.Width, ClientSize.Height, m_cancel.Token)
				.ContinueWith(
					t => GenerateEnd(t),
					m_cancel.Token,
					TaskContinuationOptions.OnlyOnRanToCompletion,
					TaskScheduler.FromCurrentSynchronizationContext()
				);

			statusInfoLabel.Text =
				"center X: " + m_xcenter + "\n" +
				"center Y: " + m_ycenter + "\n" +
				"units/pixel: " + m_scale + "\n";
		}

		// RunRedraw - Pianificato sempre in th GUI
		private void GenerateEnd(Task<Bitmap> t)
		{
			try
			{
				m_currentpicture = t.Result;
			}
			finally
			{
				m_cancel.Dispose();
				m_task = null;
			}

			this.Invalidate();
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			if (m_currentpicture == null) return;

			e.Graphics.DrawImage(m_currentpicture, e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle, GraphicsUnit.Pixel);

			if (m_selecting)
            {
                using (var p = new Pen(Color.Red, 1))
			    {
                    var rect = new Rectangle(
                            m_selrect.Width >= 0 ? m_selrect.X : m_selrect.X + m_selrect.Width,
                            m_selrect.Height >= 0 ? m_selrect.Y : m_selrect.Y + m_selrect.Height,
                            Math.Abs(m_selrect.Width) - 1,
                            Math.Abs(m_selrect.Height) - 1
                        );
                    e.Graphics.DrawRectangle(p, rect);
			    }
            }
        }

		private void Form1_ResizeEnd(object sender, EventArgs e)
		{
			RunRedraw();
		}

		private void Form1_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				m_xcenter = m_xbase + (e.X * m_scale);
				m_ycenter = m_ybase + (e.Y * m_scale);

				m_scale *= 10;
			}
			else if (e.Button == MouseButtons.Left)
			{
				Capture = false;
				m_selecting = false;

				m_xcenter = m_xbase + ((m_selrect.X + m_selrect.Width / 2) * m_scale);
				m_ycenter = m_ybase + ((m_selrect.Y + m_selrect.Height / 2) * m_scale);

				m_scale = Math.Abs(m_selrect.Width) * m_scale / ClientSize.Width;
			}

			m_xbase = m_xcenter - ClientSize.Width * m_scale / 2;
			m_ybase = m_ycenter - ClientSize.Height * m_scale / 2;

			RunRedraw();
		}

		private void Form1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

			m_selecting = true;
			m_selrect = new Rectangle(e.X, e.Y, 0, 0);
			this.Capture = true;
		}

		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left && !m_selecting) return;

			Invalidate(new Rectangle(
				(m_selrect.Width >= 0 ? m_selrect.X : m_selrect.X + m_selrect.Width) - 10,
				(m_selrect.Height >= 0 ? m_selrect.Y : m_selrect.Y + m_selrect.Height) - 10,
				Math.Abs(m_selrect.Width) + 20,
				Math.Abs(m_selrect.Height) + 20
			));

			m_selrect.Width = e.X - m_selrect.X;
			m_selrect.Height = e.Y - m_selrect.Y;

			Invalidate(new Rectangle(
				(m_selrect.Width >= 0 ? m_selrect.X : m_selrect.X + m_selrect.Width) - 10,
				(m_selrect.Height >= 0 ? m_selrect.Y : m_selrect.Y + m_selrect.Height) - 10,
				Math.Abs(m_selrect.Width) + 20,
				Math.Abs(m_selrect.Height) + 20
			));
		}

		private void Form1_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				optionsPanel.Visible = !optionsPanel.Visible;
			}
		}

		private void homeButton_Click(object sender, EventArgs e)
		{
			m_scale = 0.01;
			m_xcenter = -1;
			m_ycenter = 0;
			m_xbase = -1 - ClientSize.Width * m_scale / 2;
			m_ybase = 0 - ClientSize.Height * m_scale / 2;

			RunRedraw();
		}

        private void paletteCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Mandel.SetPalette(((Func<int[]>)(paletteCombo.SelectedValue))());
            }
            catch { return; }
            optionsPanel.Visible = false;
            RunRedraw();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            m_currentpicture.Save("c:\\temp\\mandel.png", ImageFormat.Png);
        }
    }
}
