using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LoadingSpinner.Src
{
    public partial class CircularProgressBar : UserControl
    {
        public CircularProgressBar()
        {
            InitializeComponent();
            Angle = (Percentage * 360) / 100;
            RenderArc();
        }

        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public Brush SegmentColor
        {
            get { return (Brush)GetValue(SegmentColorProperty); }
            set { SetValue(SegmentColorProperty, value); }
        }

        public int StrokeThickness
        {
            get { return (int)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public double Percentage
        {
            get { return (double)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        public double RotateAngle
        {
            get { return (double)GetValue(RotateAngleProperty); }
            set { SetValue(RotateAngleProperty, value); }
        }

        public bool IsShowPercentage
        {
            get { return (bool)GetValue(IsShowPercentageProperty); }
            set { SetValue(IsShowPercentageProperty, value); }
        }

        public Storyboard SpinStyle
        {
            get { return (Storyboard)GetValue(SpinStyleProperty); }
            set { SetValue(SpinStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Percentage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register("Percentage", typeof(double), typeof(CircularProgressBar), new PropertyMetadata(65d, new PropertyChangedCallback(OnPercentageChanged)));

        // Using a DependencyProperty as the backing store for StrokeThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(int), typeof(CircularProgressBar), new PropertyMetadata(5, new PropertyChangedCallback(OnThicknessChanged)));

        // Using a DependencyProperty as the backing store for SegmentColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SegmentColorProperty =
            DependencyProperty.Register("SegmentColor", typeof(Brush), typeof(CircularProgressBar), new PropertyMetadata(new SolidColorBrush(Colors.Red), new PropertyChangedCallback(OnColorChanged)));

        // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(int), typeof(CircularProgressBar), new PropertyMetadata(25, new PropertyChangedCallback(OnPropertyChanged)));

        // Using a DependencyProperty as the backing store for Angle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(CircularProgressBar), new PropertyMetadata(120d, new PropertyChangedCallback(OnPropertyChanged)));

        public static readonly DependencyProperty RotateAngleProperty =
            DependencyProperty.Register("RotateAngle", typeof(double), typeof(CircularProgressBar), new PropertyMetadata(0d, new PropertyChangedCallback(OnPropertyChanged)));

        public static readonly DependencyProperty IsShowPercentageProperty =
            DependencyProperty.Register("IsShowPercentage", typeof(bool), typeof(CircularProgressBar), new PropertyMetadata(false, new PropertyChangedCallback(OnPropertyChanged)));

        public static readonly DependencyProperty SpinStyleProperty =
            DependencyProperty.Register("SpinStyle", typeof(Storyboard), typeof(CircularProgressBar), new PropertyMetadata(null, new PropertyChangedCallback(OnSpinStyleChanged)));

        private static void OnColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CircularProgressBar circle = sender as CircularProgressBar;
            circle.SetColor((SolidColorBrush)args.NewValue);
        }

        private static void OnThicknessChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CircularProgressBar circle = sender as CircularProgressBar;
            circle.SetTick((int)args.NewValue);
        }

        private static void OnPercentageChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CircularProgressBar circle = sender as CircularProgressBar;
            if (circle.Percentage > 100) circle.Percentage = 100;
            circle.Angle = (circle.Percentage * 360) / 100;
        }

        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CircularProgressBar circle = sender as CircularProgressBar;
            circle.RenderArc();
        }

        private static void OnSpinStyleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CircularProgressBar circle = sender as CircularProgressBar;

            if (circle.SpinStyle == null) return;

            string storyBoardName = "BeginNotificationStoryboard";
            var beginStoryboard = new BeginStoryboard();
            beginStoryboard.Name = storyBoardName;
            beginStoryboard.Storyboard = circle.SpinStyle;

            var eventTrigger = new EventTrigger(LoadedEvent);
            eventTrigger.Actions.Add(beginStoryboard);

            var enterSeekStoryboard = new SeekStoryboard
            {
                BeginStoryboardName = storyBoardName
            };
            var enterResumeStoryboard = new ResumeStoryboard
            {
                BeginStoryboardName = storyBoardName
            };

            // Actions for the exiting animation
            var exitSeekStoryboard = new SeekStoryboard
            {
                BeginStoryboardName = storyBoardName
            };
            var exitPauseStoryboard = new PauseStoryboard
            {
                BeginStoryboardName = storyBoardName
            };

            var isEnabledTrigger = new Trigger
            {
                Property = UIElement.IsEnabledProperty,
                Value = true
            };

            //isEnabledTrigger.EnterActions.Add(enterSeekStoryboard);
            isEnabledTrigger.EnterActions.Add(enterResumeStoryboard);
            isEnabledTrigger.ExitActions.Add(exitPauseStoryboard);
            //isEnabledTrigger.ExitActions.Add(exitSeekStoryboard);
            var style = new Style();

            // The name of the Storyboard must be registered so the actions can find it
            style.RegisterName(storyBoardName, beginStoryboard);

            // Add both the EventTrigger and the regular Trigger
            style.Triggers.Add(isEnabledTrigger);
            style.Triggers.Add(eventTrigger);

            circle.Style = style;
        }

        public void SetTick(int n)
        {
            pathRoot.StrokeThickness = n;
        }

        public void SetColor(SolidColorBrush n)
        {
            pathRoot.Stroke = n;
        }

        public void RenderArc()
        {
            Point startPoint = new Point(Radius, 0);
            Point endPoint = ComputeCartesianCoordinate(Angle, Radius);
            endPoint.X += Radius;
            endPoint.Y += Radius;

            pathRoot.Width = Radius * 2 + StrokeThickness;
            pathRoot.Height = Radius * 2 + StrokeThickness;
            pathRoot.Margin = new Thickness(StrokeThickness, StrokeThickness, 0, 0);
            pathRoot.RenderTransform = new RotateTransform(RotateAngle) { CenterX = Radius, CenterY = Radius };

            bool largeArc = Angle > 180.0;

            Size outerArcSize = new Size(Radius, Radius);

            pathFigure.StartPoint = startPoint;

            if (startPoint.X == Math.Round(endPoint.X) && startPoint.Y == Math.Round(endPoint.Y))
                endPoint.X -= 0.01;

            arcSegment.Point = endPoint;
            arcSegment.Size = outerArcSize;
            arcSegment.IsLargeArc = largeArc;
        }

        private Point ComputeCartesianCoordinate(double angle, double radius)
        {
            // convert to radians
            double angleRad = (Math.PI / 180.0) * (angle - 90);

            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);

            return new Point(x, y);
        }
    }
}
