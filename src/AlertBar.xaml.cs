using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AlertBarWpf.Annotations;
using MahApps.Metro.IconPacks;


namespace AlertBarWpf
{



    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class AlertBar : INotifyPropertyChanged
    {
        public AlertBar()
        {
            InitializeComponent();
            AlertBarManager.AddAlertBar(Tag as string, this);
            // grdWrapper.DataContext = this;
        }




        public PackIconMaterialKind IconKind
        {
            get { return (PackIconMaterialKind)GetValue(IconKindProperty); }
            set { SetValue(IconKindProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconKindProperty =
            DependencyProperty.Register("IconKind", typeof(PackIconMaterialKind), typeof(AlertBar), new PropertyMetadata(PackIconMaterialKind.Information));






        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(AlertBar), new PropertyMetadata(string.Empty));




        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(AlertBar), new PropertyMetadata(string.Empty));



        public string ActionButtonContent
        {
            get { return (string)GetValue(ActionButtonContentProperty); }
            set { SetValue(ActionButtonContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActionButtonContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionButtonContentProperty =
            DependencyProperty.Register("ActionButtonContent", typeof(string), typeof(AlertBar), new PropertyMetadata(string.Empty));




        public Brush BackgroundBrush
        {
            get { return (Brush)GetValue(BackgroundBrushProperty); }
            set { SetValue(BackgroundBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundBrushProperty =
            DependencyProperty.Register("BackgroundBrush", typeof(Brush), typeof(AlertBar), new PropertyMetadata(Brushes.DarkBlue));






        public bool CanClose
        {
            get { return (bool)GetValue(CanCloseProperty); }
            set { SetValue(CanCloseProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanClose.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanCloseProperty =
            DependencyProperty.Register("CanClose", typeof(bool), typeof(AlertBar), new PropertyMetadata(true));

        public bool IsClosedByAction { get; set; } = false;


        internal class SimpleCommand : ICommand
        {
            public Action Action { get; set; }

            public SimpleCommand(Action action)
            {
                Action = action;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                Action();
                if (parameter is AlertBar {IsClosedByAction: true} b)
                {
                    b.Clear();
                } 
            }

            public event EventHandler CanExecuteChanged;
        }

        public Action Action
        {
            get => _action;
            set
            {
                _action = value;
                ActionButtonCommand = new SimpleCommand(Action);
            }
        }

        public ICommand ActionButtonCommand
        {
            get => _actionButtonCommand;
            set
            {
                if (Equals(value, _actionButtonCommand)) return;
                _actionButtonCommand = value;
                OnPropertyChanged();
            }
        }


        public static readonly RoutedEvent ShowEvent = EventManager.RegisterRoutedEvent("Show", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AlertBar));

        public event RoutedEventHandler Show
        {
            add => AddHandler(ShowEvent, value);
            remove => RemoveHandler(ShowEvent, value);
        }

        private void RaiseShowEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(ShowEvent);
            RaiseEvent(newEventArgs);
        }

        internal void TransformStage(double seconds)
        {
            var secs = (int) seconds;
            

            DefaultStackPanel.Visibility = Visibility.Visible;

            //if (_iconVisibility == false)
            //{
            //    statusIcon.Visibility = Visibility.Collapsed;
            //    parentGrid?.ColumnDefinitions.RemoveAt(0);
            //    messageTextBlock?.SetValue(Grid.ColumnProperty, 0);
            //    closeIcon.SetValue(Grid.ColumnProperty, 1);
            //    if (messageTextBlock != null)
            //    {
            //        messageTextBlock.Margin = new Thickness(10, 4, 0, 4);
            //        messageTextBlock.Height = 16;
            //    }
            //}
            //else
            //{
            //    statusIcon.Source = new BitmapImage(new Uri("/AlertBarWpf;component/Resources/" + iconSource, UriKind.Relative));
            //}


            //if (messageTextBlock != null) messageTextBlock.Text = msg;
            WrapperGrid.Visibility = Visibility.Visible;
            key1.KeyTime = new TimeSpan(0, 0, (secs == 0 ? 0 : secs - 1));
            key2.KeyTime = new TimeSpan(0, 0, secs);
            RaiseShowEvent();
        }



        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield break;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T dependencyObject)
                {
                    yield return dependencyObject;
                }

                foreach (var childOfChild in FindVisualChildren<T>(child))
                {
                    yield return childOfChild;
                }
            }
        }


        /// <summary>
        /// Shows a Danger Alert
        /// </summary>
        /// <param name="message">The message for the alert</param>
        /// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        //public void SetDangerAlert(string message, int timeoutInSeconds = 0)
        //{
        //    string color = "#D9534F";
        //    string icon = "danger_16.png";
        //    //TransformStage(message, timeoutInSeconds, color, icon);
        //}

        ///// <summary>
        ///// Shows a warning Alert
        ///// </summary>
        ///// <param name="message">The message for the alert</param>
        ///// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        //public void SetWarningAlert(string message, int timeoutInSeconds = 0)
        //{
        //    string color = "#F0AD4E";
        //    string icon = "warning_16.png";

        //    TransformStage(message, timeoutInSeconds, color, icon);
        //}

        ///// <summary>
        ///// Shows a Success Alert
        ///// </summary>
        ///// <param name="message">The message for the alert</param>
        ///// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        //public void SetSuccessAlert(string message, int timeoutInSeconds = 0)
        //{
        //    string color = "#5CB85C";
        //    string icon = "success_16.png";
        //    TransformStage(message, timeoutInSeconds, color, icon);
        //}


        ///// <summary>
        ///// Shows an Information Alert
        ///// </summary>
        ///// <param name="message">The message for the alert</param>
        ///// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        //public void SetInformationAlert(string message, int timeoutInSeconds = 0)
        //{
        //    string color = "#5BC0DE";
        //    string icon = "information_16.png";
        //    TransformStage(message, timeoutInSeconds, color, icon);
        //}


     
        private bool _iconVisibility = true;
        private Action _action;
        private ICommand _actionButtonCommand;

        /// <summary>
        /// Hide or show icons in the messages.
        /// </summary>
        public bool? IconVisibility
        {
            set
            {
                if (value == null)
                {
                    return;
                }
                _iconVisibility = value.Value;
            }
            get => _iconVisibility;
        }



      


        /// <summary>
        /// Remove a message if one is currently being shown.
        /// </summary>
        public void Clear()
        {
            WrapperGrid.Visibility = Visibility.Collapsed;
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Clear();

        }

        private void AnimationObject_Completed(object sender, EventArgs e)
        {
            if (WrapperGrid.Opacity == 0)
            {
                //If you call msgbar.setErrorMessage("Whateva") in MainWindow() of your WPF the window is not rendered yet.  So opacity is 0.  If you have a timeout of 0 then it would call this immediately
                if (key1.KeyTime.TimeSpan.Seconds > 0)
                {
                    Clear();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
