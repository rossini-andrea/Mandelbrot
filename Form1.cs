using System;
using System.Drawing;
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

			Mandel.LoadPalette();
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
			using (var p = new Pen(Color.Red, 2))
			{
				e.Graphics.DrawRectangle(
					p,
					new Rectangle(
						m_selrect.Width >= 0 ? m_selrect.X : m_selrect.X + m_selrect.Width,
						m_selrect.Height >= 0 ? m_selrect.Y : m_selrect.Y + m_selrect.Height,
						Math.Abs(m_selrect.Width)-1,
						Math.Abs(m_selrect.Height)-1
					)
				);
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
	}
}
