using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Views;
using Android.Hardware;

using Android.Graphics;
using SkiaSharp.Views.Android;
using SkiaSharp;
using System.Collections.Generic;

namespace testOverlays
{
    [Activity(Label = "testOverlays", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, TextureView.ISurfaceTextureListener
    {
        private Android.Hardware.Camera _camera;
        private TextureView _textureView;
        private SurfaceView _surfaceView;
        private ISurfaceHolder holder;
        SKCanvasView canvasView;
        List<EllipseDrawingFigure> completedFigures = new List<EllipseDrawingFigure>();
        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Fill
        };
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _textureView = (TextureView)FindViewById(Resource.Id.textureView);
            _textureView.SurfaceTextureListener = this;
            Button addButton = FindViewById<Button>(Resource.Id.Add);
            canvasView = FindViewById<SKCanvasView>(Resource.Id.canvasView);
            canvasView.Touch += CanvasView_Touch; 

           

      //      canvasView.BringToFront();
            EllipseDrawingFigure figure = new EllipseDrawingFigure
            {
                Color = SKColors.Red,
                StartPoint = ConvertToPixel(new Point(131, 132)),
                EndPoint = ConvertToPixel(new Point(171, 171))
            };
            completedFigures.Add(figure);
       //     SKPoint pt = figure.StartPoint;
            canvasView.PaintSurface += CanvasView_PaintSurface;
            addButton.Click += AddButton_Click;
            
        }

        private void CanvasView_Touch(object sender, View.TouchEventArgs e)
        {
            var action = e.Event.Action;
            switch (action)
            {
                case MotionEventActions.Down:
                
                    {
                        // TODO use data
                        break;
                    }

                case MotionEventActions.Move:
                    {
                        // TODO use data
                        break;
                    }


            }

        }

        private void AddButton_Click(object sender, System.EventArgs e)
        {
            EllipseDrawingFigure figure = new EllipseDrawingFigure
            {
                Color = SKColors.Red,
                StartPoint = new SKPoint(131, 132),
                EndPoint = new SKPoint(171, 171)
            };
            completedFigures.Add(figure);
            canvasView.Invalidate();
        }

        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            //SKCanvas canvas = e.Surface.Canvas;
            //canvas.Clear();

            

            var surface = e.Surface;
            // then we get the canvas that we can draw on
            var canvas = surface.Canvas;
            // clear the canvas / view
            canvas.Clear(SKColors.Transparent);
            foreach (EllipseDrawingFigure figure in completedFigures)
            {
                paint.Color = figure.Color;
                canvas.DrawOval(figure.Rectangle, paint);
            }

            // DRAWING SHAPES

            // create the paint for the filled circle
          
        }

        private void _surfaceView_Touch(object sender, View.TouchEventArgs e)
        {
            //define the paintbrush
            Paint mpaint = new Paint();
            mpaint.Color = Color.Red;
            mpaint.SetStyle(Paint.Style.Stroke);
            mpaint.StrokeWidth = 2f;

            //draw
            Canvas canvas = holder.LockCanvas();
            //clear the paint of last time
            canvas.DrawColor(Color.Transparent, PorterDuff.Mode.Clear);
            //draw a new one, set your ball's position to the rect here
            var x = e.Event.GetX();
            var y = e.Event.GetY();
            Rect r = new Rect((int)x, (int)y, (int)x + 100, (int)y + 100);
            canvas.DrawRect(r, mpaint);
            holder.UnlockCanvasAndPost(canvas);
        }
     //   public void OnPainting(object sender, )
        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            _camera.StopPreview();
            _camera.Release();

            return true;
        }

        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            _camera = Android.Hardware.Camera.Open();

            try
            {
                _camera.SetPreviewTexture(surface);
                _camera.SetDisplayOrientation(90);
                _camera.StartPreview();
            }
            catch (Java.IO.IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
        }

        SKPoint ConvertToPixel(Point pt)
        {
            return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                               (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
        }
    }
}

