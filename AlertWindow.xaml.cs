using System;
using System.Windows;

namespace ExtraTheLockScreen
{
    public partial class AlertWindow : Window
    {
        private AlertWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public class Builder
        {
            private Action<object, RoutedEventArgs, AlertWindow> _onPositiveButtonClick;
            private Action<object, RoutedEventArgs, AlertWindow> _onNegativeButtonClick;
            private string _positiveButtonText;
            private string _negativeButtonText;
            private string _message;

            public Builder SetPositiveButton(string text, Action<object, RoutedEventArgs, AlertWindow> listener)
            {
                _positiveButtonText = text;
                _onPositiveButtonClick = listener;
                return this;
            }

            public Builder SetNegativeButton(string text, Action<object, RoutedEventArgs, AlertWindow> listener)
            {
                _negativeButtonText = text;
                _onNegativeButtonClick = listener;
                return this;
            }

            public Builder SetMessage(string message)
            {
                _message = message;
                return this;
            }

            public AlertWindow Create()
            {
                var window = new AlertWindow();
                if (!string.IsNullOrEmpty(_message))
                {
                    window.AlertMessage.Text = _message;
                }

                if (!string.IsNullOrEmpty(_positiveButtonText))
                {
                    window.PositiveButton.Content = _positiveButtonText;
                }

                if (!string.IsNullOrEmpty(_negativeButtonText))
                {
                    window.NegativeButton.Content = _negativeButtonText;
                }

                if (_onPositiveButtonClick != null)
                {
                    window.PositiveButton.Click += (obj, e) =>
                    {
                        _onPositiveButtonClick(obj, e, window);
                    };
                }

                if (_onNegativeButtonClick != null)
                {
                    window.NegativeButton.Click += (obj, e) =>
                    {
                        _onNegativeButtonClick(obj, e, window);
                    };
                }

                return window;
            }
        }
    }
}