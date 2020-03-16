using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Mycord_WPF
{
    public class BaseUserControl : UserControl
    {
        public EUserControlAnimation ControlLoadAnimation { get; set; } = EUserControlAnimation.SildeAndFadeInFromRight;

        public float FadeSeconds { get; set; } = 0.5f;

        public BaseUserControl()
        {
            if (this.ControlLoadAnimation != EUserControlAnimation.None)
            {
                this.Visibility = Visibility.Collapsed;
            }

            this.Loaded += BaseUserControl_Loaded;
        }

        private async void BaseUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await AnimateIn();
        }

        public async Task AnimateIn()
        {
            if (this.ControlLoadAnimation == EUserControlAnimation.None)
            {
                return;
            }

            switch (this.ControlLoadAnimation)
            {
                case EUserControlAnimation.SildeAndFadeInFromRight:
                    {
                        var storyboard = new Storyboard();

                        var opacityAnimation = new DoubleAnimation
                        {
                            Duration = new Duration(TimeSpan.FromSeconds(0.8f)),
                            From = 0,
                            To = 1
                        };

                        Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));

                        storyboard.Children.Add(opacityAnimation);

                        storyboard.Begin(this);

                        this.Visibility = Visibility.Visible;

                        await Task.Delay((int)(this.FadeSeconds * 1000));
                    }
                    break;
            }
        }
    }
}
