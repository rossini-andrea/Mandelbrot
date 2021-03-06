﻿/*
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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Mandelbrot
{
	static class Mandel
	{
		private static int[] m_palette;

		private static int m_maxThreads = 6;
		private static int m_maxIters = 20000;

        public static void SetPalette(int[] value)
        {
            m_palette = value ?? throw new ArgumentNullException("value");
        }

        private static int[] GeneratePictureInternal(
			double xbase, double ybase, double scale,
			int w, int h,
			int firstY, int lastY,
			int maxiter, int[] scores, CancellationToken ct)
		{
			var hist = new int[maxiter + 1];

			if (lastY > h) lastY = h;

			for (int x = 0; x < w; x++)
			{
				if (ct.IsCancellationRequested)
				{
					ct.ThrowIfCancellationRequested();
				}

				for (int y = firstY; y < lastY; y++)
				{
					int i = 0;

					double zx = 0;
					double zy = 0;

					double scaledx = x * scale + xbase;
					double scaledy = y * scale + ybase;

					while (zx * zx + zy * zy < 4 && i < maxiter)
					{
						double zxt = (zx * zx - zy * zy) + scaledx;
						zy = 2 * zx * zy + scaledy;
						zx = zxt;
						++i;
					}

					++hist[i];
					scores[y * w + x] = i;
				}
			}

			return hist;
		}

		private static Bitmap RecomposeImage(int w, int h, int[] scores, int[] hist, int maxiter, CancellationToken ct)
		{
			var result = new Int32[w * h];
			int total = w * h;
			var remap = new double[maxiter + 1];

			for (int i = 0; i < scores.Length; ++i)
			{
				double color = 0;

				if (remap[scores[i]] == 0)
				{
					for (int j = 0; j <= scores[i]; ++j)
					{
						color += (double)hist[j];
					}

                    color /= total;

                    color = color < 0.000001 ? 0 :
                            color > 0.999999 ? 1 :
                            color;

                    remap[scores[i]] = color;
				}
				else
				{
					color = remap[scores[i]];
				}

				result[i] = m_palette[(int)(color * (m_palette.Length - 1))];
			}

			if (ct.IsCancellationRequested)
			{
				ct.ThrowIfCancellationRequested();
			}

			var bmp = new Bitmap(w, h);
			var data = bmp.LockBits(new Rectangle(0, 0, w, h), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
			Marshal.Copy(result, 0, data.Scan0, result.Length);
			bmp.UnlockBits(data);
			return bmp;
		}

		public static Task<Bitmap> GeneratePicture(double xbase, double ybase, double scale, int w, int h, CancellationToken ct)
		{
			var scores = new int[w * h];
			var tasks = new Task<int[]>[m_maxThreads];
			int rowsForThread = h / m_maxThreads + 1;

			for (int i = 0; i < tasks.Length; i++)
			{
				var startY = rowsForThread * i;
				var endY = startY + rowsForThread;
				tasks[i] = Task.Run(() => GeneratePictureInternal(xbase, ybase, scale, w, h, startY, endY, m_maxIters, scores, ct), ct);
			}

			var joinTask = Task.WhenAll(tasks)
				.ContinueWith(
					t =>
					{
						var hist = new int[m_maxIters + 1];

						foreach (var r in t.Result)
						{
							for (int i = 0; i < r.Length; i++)
							{
								hist[i] += r[i];
							}

							if (ct.IsCancellationRequested)
							{
								ct.ThrowIfCancellationRequested();
							}
						}

						return hist;
					},
					ct
				)
				.ContinueWith(
					t => RecomposeImage(w, h, scores, t.Result, m_maxIters, ct), ct
				);

			return joinTask;
		}
	}
}
