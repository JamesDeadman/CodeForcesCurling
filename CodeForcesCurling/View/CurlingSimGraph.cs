using CurlingSim;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace CodeForcesCurling.View
{
    /// <summary>
    /// A custom element/control to draw the results from any given solver
    /// </summary>
    public class CurlingSimGraph : FrameworkElement
    {
        public Solver Solver { get; set; }

        private readonly Typeface DefaultTypeFace = new Typeface("default");
        public float FontSize { get; private set; } = 12;

        public float GraphMarginLeft { get; set; } = 50;
        public float GraphMarginRight { get; set; } = 20;
        public float GraphMarginTop { get; set; } = 10;
        public float GraphMarginBottom { get; set; } = 50;

        public float GraphPaddingTop { get; set; } = 10;
        public float GraphPaddingRight { get; set; } = 10;

        public float AxisTickSize { get; set; } = 5;
        public float AxisTextSpacing { get; set; } = 2;
        public Pen DiskPen { get; set; } = new Pen(Brushes.Navy, 2.0f);
        public Brush DiskNameBrush { get; set; } = Brushes.Black;

        public Pen GridLinePen { get; set; } = new Pen(Brushes.Black, 0.5f);


        public static FrameworkPropertyMetadataOptions SolverPropertyOptions = FrameworkPropertyMetadataOptions.BindsTwoWayByDefault & FrameworkPropertyMetadataOptions.AffectsRender;

        public static readonly DependencyProperty SolverProperty =
            DependencyProperty.Register(nameof(Solver), typeof(Solver), typeof(CurlingSimGraph), 
                new FrameworkPropertyMetadata(null, SolverPropertyOptions, new PropertyChangedCallback(SolverPropertyChanged), new CoerceValueCallback(SolverPropertyCoerceChanged)));

        public CurlingSimGraph()
        {
        }

        public static void SolverPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CurlingSimGraph graph = (CurlingSimGraph)obj;

            graph.Solver = (Solver)e.NewValue;
            graph.UpdateLayout();
            graph.InvalidateVisual();

        }

        public static object SolverPropertyCoerceChanged(DependencyObject obj, object value)
        {
            CurlingSimGraph graph = (CurlingSimGraph)obj;

            graph.Solver = (Solver)value;
            graph.UpdateLayout();
            graph.InvalidateVisual();
            
            return value;
        }

        protected override void OnRender(DrawingContext context)
        {
            DpiScale dpiScale = System.Windows.Media.VisualTreeHelper.GetDpi(this);
            double pixelsPerDip = dpiScale.PixelsPerDip;
            
            if (Solver != null)
            {
                /* Draw a border around the control */
                Rect drawingArea = new Rect(0, 0, ActualWidth, ActualHeight);
                context.DrawRectangle(Brushes.LightGray, new Pen(Brushes.Black, 1), drawingArea);

                double graphWidth = ActualWidth - GraphMarginLeft - GraphMarginRight;
                double graphHeight = ActualHeight - GraphMarginTop - GraphMarginBottom;

                if(graphWidth <= GraphPaddingRight || graphHeight <= GraphPaddingTop)
                {
                    return; /* The control is too small, don't try to draw the graph */
                }

                Rect graphArea = new Rect(GraphMarginLeft, GraphMarginTop, graphWidth, graphHeight);

                /* Calculate the graph scale */
                double topRangeX = Solver.Disks.Max(d => d.XLocation) + Solver.Disks.Max(d => d.Radius);
                double topRangeY = Solver.Disks.Max(d => d.YLocation) + Solver.Disks.Max(d => d.Radius);
                double graphScaleX = (graphArea.Width - GraphPaddingRight) / topRangeX;
                double graphScaleY = (graphArea.Height - GraphPaddingTop) / topRangeY;
                double graphScale = Math.Min(graphScaleX, graphScaleY);

                /* Draw the graph area */
                context.DrawRectangle(Brushes.WhiteSmoke, new Pen(Brushes.Black, 0), graphArea);

                /* Draw Y-Axis Gridlines*/
                int verticalGridSpacing = Math.Max((int)Math.Ceiling(topRangeY) / 5, 1);
                for (int y = 0; y * graphScale < graphArea.Height; y += verticalGridSpacing)
                {
                    double x1 = graphArea.X - AxisTickSize;
                    double y1 = graphArea.Y + graphArea.Height - y * graphScale;
                    double x2 = graphArea.X + graphArea.Width;
                    context.DrawLine(GridLinePen, new Point(x1, y1), new Point(x2, y1));

                    FormattedText text = new FormattedText(y.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, DefaultTypeFace, FontSize, DiskNameBrush, pixelsPerDip);
                    context.DrawText(text, new Point(x1 - text.WidthIncludingTrailingWhitespace - AxisTickSize - AxisTextSpacing, y1 - text.Height / 2));
                }

                /* Draw X-Axis Gridlines*/
                int horizontalGridSpacing = Math.Max((int)Math.Floor(graphArea.Width / graphScale / 10), 2);
                for (int x = 0; x * graphScale < graphArea.Width; x += horizontalGridSpacing)
                {
                    double x1 = graphArea.X + x * graphScale;
                    double y1 = graphArea.Y;
                    double y2 = graphArea.Y + graphArea.Height + AxisTickSize;
                    context.DrawLine(GridLinePen, new Point(x1, y1), new Point(x1, y2));

                    FormattedText text = new FormattedText(x.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, DefaultTypeFace, FontSize, DiskNameBrush, pixelsPerDip);
                    context.DrawText(text, new Point(x1 - text.WidthIncludingTrailingWhitespace / 2, y2 + AxisTickSize + AxisTextSpacing));
                }

                /* Draw disks */
                foreach (Disk disk in Solver.Disks)
                {
                    double diskX = disk.XLocation * graphScale + graphArea.X;
                    double diskY = graphArea.Y + graphArea.Height - disk.YLocation * graphScale;
                    double diskR = disk.Radius * graphScale;
                    context.DrawGeometry(null, DiskPen, new EllipseGeometry(new Point(diskX , diskY), diskR, diskR));

                    FormattedText text = new FormattedText(disk.Name, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, DefaultTypeFace, FontSize, DiskNameBrush, pixelsPerDip);
                    context.DrawText(text, new Point(diskX - text.WidthIncludingTrailingWhitespace / 2, diskY - text.Height / 2));
                }
            }
        }
    }
}
