﻿using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Threading;


namespace Bitsmith
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Bitsmith"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Bitsmith;assembly=Bitsmith"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:WindowBase/>
    ///
    /// </summary>
    public class WindowBase : Window
    {
        private Control _customFrame;

        private Point _ptCursorOffset;

        private FrameworkElement _sizingBorderLeft;
        private FrameworkElement _sizingBorderTopLeft;
        private FrameworkElement _sizingBorderTop;
        private FrameworkElement _sizingBorderTopRight;
        private FrameworkElement _sizingBorderRight;
        private FrameworkElement _sizingBorderBottomRight;
        private FrameworkElement _sizingBorderBottom;
        private FrameworkElement _sizingBorderBottomLeft;

        static WindowBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowBase), new FrameworkPropertyMetadata(typeof(WindowBase)));
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Handle TitleBar button commands (min/restore/close)
            CommandBindings.Add(new CommandBinding(WindowCommands.MinimizeWindow, OnCommand));
            CommandBindings.Add(new CommandBinding(WindowCommands.MaximizeWindow, OnCommand));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, OnCommand));

            _customFrame = (Control)Template.FindName("PART_CustomFrame", this);

            if (_customFrame != null)
                UpdateFrameSettings();
        }


        private void UpdateFrameSettings()
        {

            if (ApplyFrameStyle(WindowState == WindowState.Maximized ? "Max" : "Norm"))
            {
                // Make ASYNCHRONOUS call to method that can react to new style
                // (new settings are NOT detected when you call the method directly)

                if (WindowState == WindowState.Normal)
                    new Thread(() => { UpdateFrameBehavior(); }).Start();
            }
        }


        private bool ApplyFrameStyle(string strStyleId)
        {
            // Get frame style (note that using "GetType()" instead of "typeof" may cause find operation to FAIL!)
            Style style = (Style)TryFindResource(new ComponentResourceKey(typeof(WindowBase), strStyleId));

            if (style != null)
                _customFrame.Style = style;

            return style != null;
        }


        private void UpdateFrameBehavior()
        {
            // Attach handlers to events in updated template (CaptionBar dragging, sizing borders...)
            // (because we were called from a different thread, we must also marshall back to the UI thread)

            Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, (ThreadStart)delegate
            {
                FrameworkElement titleBar = (FrameworkElement)_customFrame.Template.FindName("PART_TitleBar", _customFrame);

                // Support window dragging via caption bar...
                if (titleBar != null)
                    titleBar.MouseLeftButtonDown += (sender, args) => { DragMove(); };

                _sizingBorderLeft = GetSizableBorder("PART_ResizeBorderLeft");
                _sizingBorderTopLeft = GetSizableBorder("PART_ResizeBorderTopLeft");
                _sizingBorderTop = GetSizableBorder("PART_ResizeBorderTop");
                _sizingBorderTopRight = GetSizableBorder("PART_ResizeBorderTopRight");
                _sizingBorderRight = GetSizableBorder("PART_ResizeBorderRight");
                _sizingBorderBottomRight = GetSizableBorder("PART_ResizeBorderBottomRight");
                _sizingBorderBottom = GetSizableBorder("PART_ResizeBorderBottom");
                _sizingBorderBottomLeft = GetSizableBorder("PART_ResizeBorderBottomLeft");

            });
        }


        // Handle TitleBar button commands (min/restore/close)

        private void OnCommand(object sender, ExecutedRoutedEventArgs args)
        {
            if (args.Command == WindowCommands.MinimizeWindow)
                WindowState = WindowState.Minimized;

            else if (args.Command == WindowCommands.MaximizeWindow)
            {
                if (WindowState == WindowState.Maximized)
                    WindowState = WindowState.Normal;
                else
                    WindowState = WindowState.Maximized;

                UpdateFrameSettings();
            }

            else if (args.Command == ApplicationCommands.Close)
                Close();
        }


        private Path GetSizableBorder(string borderSegmentID)
        {
            Path sizingBorderSegment = (Path)_customFrame.Template.FindName(borderSegmentID, _customFrame);

            if (sizingBorderSegment != null)
            {
                sizingBorderSegment.MouseLeftButtonDown += (sender, args) =>
                {
                    if (WindowState != WindowState.Maximized)
                    {
                        Path borderSegment = (Path)sender;
                        _ptCursorOffset = GetCursorOffset(args.GetPosition(this), borderSegment);
                        borderSegment.CaptureMouse();
                    }
                };

                sizingBorderSegment.MouseLeftButtonUp += (sender, args) =>
                {
                    Path borderSegment = (Path)sender;
                    borderSegment.ReleaseMouseCapture();
                };

                sizingBorderSegment.MouseMove += (sender, args) =>
                {
                    Path borderSegment = (Path)sender;

                    if (borderSegment.IsMouseCaptured)
                    {
                        PerformResize(args.GetPosition(this), borderSegment);
                    }
                };
            }

            return sizingBorderSegment;
        }


        private Point GetCursorOffset(Point ptMousePosition, Path borderSegment)
        {
            Point ptOffset = new Point(0, 0);

            if (borderSegment == _sizingBorderLeft)
                ptOffset.X = ptMousePosition.X;

            else if (borderSegment == _sizingBorderTopLeft)
            {
                ptOffset.Y = ptMousePosition.Y;
                ptOffset.X = ptMousePosition.X;
            }

            else if (borderSegment == _sizingBorderTop)
                ptOffset.Y = ptMousePosition.Y;

            else if (borderSegment == _sizingBorderTopRight)
            {
                ptOffset.Y = ptMousePosition.Y;
                ptOffset.X = (Width - ptMousePosition.X);
            }

            else if (borderSegment == _sizingBorderRight)
                ptOffset.X = (Width - ptMousePosition.X);

            else if (borderSegment == _sizingBorderBottomRight)
            {
                ptOffset.X = (Width - ptMousePosition.X);
                ptOffset.Y = (Height - ptMousePosition.Y);
            }

            else if (borderSegment == _sizingBorderBottom)
                ptOffset.Y = Height - ptMousePosition.Y;

            else if (borderSegment == _sizingBorderBottomLeft)
            {
                ptOffset.Y = (Height - ptMousePosition.Y);
                ptOffset.X = ptMousePosition.X;
            }

            return ptOffset;
        }


        private void PerformResize(Point ptMousePosition, Path borderSegment)
        {
            if (borderSegment == _sizingBorderLeft)
            {
                Left += (ptMousePosition.X - _ptCursorOffset.X);
                Width -= (ptMousePosition.X - _ptCursorOffset.X);
            }

            else if (borderSegment == _sizingBorderTopLeft)
            {
                Left += (ptMousePosition.X - _ptCursorOffset.X);
                Width -= (ptMousePosition.X - _ptCursorOffset.X);
                Top += (ptMousePosition.Y - _ptCursorOffset.Y);
                Height -= (ptMousePosition.Y - _ptCursorOffset.Y);
            }

            else if (borderSegment == _sizingBorderTop)
            {
                Top += (ptMousePosition.Y - _ptCursorOffset.Y);
                Height -= (ptMousePosition.Y - _ptCursorOffset.Y);
            }

            else if (borderSegment == _sizingBorderTopRight)
            {
                Width = ptMousePosition.X + _ptCursorOffset.X;
                Top += (ptMousePosition.Y - _ptCursorOffset.Y);
                Height -= (ptMousePosition.Y - _ptCursorOffset.Y);
            }

            else if (borderSegment == _sizingBorderRight)
                Width = ptMousePosition.X + _ptCursorOffset.X;

            else if (borderSegment == _sizingBorderBottomRight)
            {
                Width = ptMousePosition.X + _ptCursorOffset.X;
                Height = ptMousePosition.Y + _ptCursorOffset.Y;
            }

            else if (borderSegment == _sizingBorderBottom)
                Height = ptMousePosition.Y + _ptCursorOffset.Y;

            else if (borderSegment == _sizingBorderBottomLeft)
            {
                Left += (ptMousePosition.X - _ptCursorOffset.X);
                Width -= (ptMousePosition.X - _ptCursorOffset.X);
                Height = ptMousePosition.Y + _ptCursorOffset.Y;
            }
        }
  
    }
}
