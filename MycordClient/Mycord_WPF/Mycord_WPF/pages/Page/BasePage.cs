using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;
using System;

namespace Mycord_WPF
{
    public class BasePage : Page
    {
        private object viewModel;

        #region public 애니메이션 멤버

        public EPageAnimation PageLoadAnimation { get; set; } = EPageAnimation.SildeAndFadeInFromRight;

        public EPageAnimation PageUnloadAnimation { get; set; } = EPageAnimation.SildeAndFadeOutToLeft;

        public float FadeInSeconds { get; set; } = 0.8f;
        public float FadeOutSeconds { get; set; } = 0.5f;

        public object ViewModel
        {
            get
            {
                return viewModel;
            }
            set
            {
                viewModel = value;
                VMFactory.PushViewModel(value.GetType(), value);
            }
        }

        public bool ShouldAnimateOut { get; set; }

        #endregion

        public BasePage()
        {
            if (this.PageLoadAnimation != EPageAnimation.None)
            {
                this.Visibility = Visibility.Collapsed;
            }

            this.Loaded += BasePage_Loaded;
        }

        private async void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (ShouldAnimateOut)
            {
                await AnimateOut();
            }             
            else
            {
                await AnimateIn();
            }
        }

        /*
        private async void BasePage_Unloaded(object sender, RoutedEventArgs e)
        {
            await AnimateOut();
        }
        */

        public async Task AnimateIn()
        {
            if (this.PageLoadAnimation == EPageAnimation.None)
            {
                return;
            }

            switch (this.PageLoadAnimation)
            {
                case EPageAnimation.SildeAndFadeInFromRight:
                    {
                        var storyboard = new Storyboard();

                        var slideAnimation = new ThicknessAnimation
                        {
                            Duration = new Duration(TimeSpan.FromSeconds(this.FadeInSeconds)),
                            From = new Thickness(this.WindowWidth, 0, -this.WindowWidth, 0),
                            To = new Thickness(0),
                            DecelerationRatio = 0.9f
                        };

                        var opacityAnimation = new DoubleAnimation
                        {
                            Duration = new Duration(TimeSpan.FromSeconds(0.4f)),
                            From = 0,
                            To = 1
                        };

                        Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("Margin"));
                        Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));

                        storyboard.Children.Add(slideAnimation);
                        storyboard.Children.Add(opacityAnimation);

                        storyboard.Begin(this);

                        this.Visibility = Visibility.Visible;

                        await Task.Delay((int)(this.FadeInSeconds * 1000));
                    }
                    break;


            }
        }
        public async Task AnimateOut()
        {
            if (this.PageUnloadAnimation == EPageAnimation.None)
            {
                return;
            }

            switch (this.PageUnloadAnimation)
            {
                case EPageAnimation.SildeAndFadeOutToLeft:
                    {
                        var storyboard = new Storyboard();

                        var opacityAnimation = new DoubleAnimation
                        {
                            Duration = new Duration(TimeSpan.FromSeconds(0.2f)),
                            From = 1,
                            To = 0
                        };

                        Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));

                        storyboard.Children.Add(opacityAnimation);

                        storyboard.Begin(this);

                        await Task.Delay((int)(this.FadeOutSeconds * 1000));
                        Task.WaitAll();
                        this.Visibility = Visibility.Collapsed;
                    }
                    break;
            }
        }
    }
}
