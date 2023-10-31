using Microsoft.AspNetCore.Components.Web;
using SkiaSharp.Views.Blazor;
using SkiaSharp;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.ExceptionServices;

namespace BlazorAppTestBugSnapshot.Pages
{
	public partial class TestBugSnapshotComponent
	{
		SKCanvasView skiaView = null!;
		Boolean firstRun = true;
		int counterSnapshots = 0;
		SKImage? layerBackGround;
		SKPoint mouseLocation;


		public TestBugSnapshotComponent()
		{
		}

		private void OnPaintSurface(SKPaintSurfaceEventArgs e)
		{
			try
			{
				var canvas = e.Surface.Canvas; // 8000x2000
				SKRect rect1 = new SKRect(0, 0, e.Info.Rect.Width, e.Info.Rect.Height);

				if (firstRun)
				{
					layerBackGround = e.Surface.Snapshot();

					firstRun = false;
				}
				else
				{
					canvas.DrawImage(layerBackGround, rect1);

					// Draw something permanent

					layerBackGround = e.Surface.Snapshot(); // Unhandle  Exception

					// Draw something temporary
					using (SKPaint paint = new SKPaint { Style = SKPaintStyle.Stroke, Color = SKColors.Red })
					{
						canvas.DrawCircle(mouseLocation, 100, paint);
					}

					Console.WriteLine($"Restore Snapshot Number Restore {counterSnapshots++}");
				}
				e.Surface.Flush();
			}
			catch (Exception ex)
			{
				// Eat it
			}

		}

		private void OnMouseMove(MouseEventArgs e)
		{
			mouseLocation = new SKPoint((float)e.OffsetX, (float)e.OffsetY);

			skiaView.Invalidate();
		}

	}
}
