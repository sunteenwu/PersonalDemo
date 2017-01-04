using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace CPathAnimation
{
    public static class PushpinAnimations
    {
        public static void AnimateY(UIElement pin, double fromY, double toY, int duration, EasingFunctionBase easingFunction)
        {
            pin.RenderTransform = new TranslateTransform();

            var sb = new Storyboard();
            var animation = new DoubleAnimation()
            {
                From = fromY,
                To = toY,
                Duration = new TimeSpan(0, 0, 0, 0, duration),
                EasingFunction = easingFunction
            };

            Storyboard.SetTargetProperty(animation, "(UIElement.RenderTransform).(TranslateTransform.Y)");
            Storyboard.SetTarget(animation, pin);

            sb.Children.Add(animation);
            sb.Begin();
        }

        public static void Drop(UIElement pin, double? height, int? duration)
        {
            height = (height.HasValue && height.Value > 0) ? height : 150;
            duration = (duration.HasValue && duration.Value > 0) ? duration : 150;

            var anchor = MapControl.GetLocation(pin).Position;
            //var anchor = new Point(50, 60);
            var from = anchor.Longitude + height.Value;

            AnimateY(pin, -from, -anchor.Longitude, duration.Value, new QuadraticEase()
            {
                EasingMode = EasingMode.EaseIn
            });
        }

        public static void Bounce(UIElement pin, double? height, int? duration)
        {
            height = (height.HasValue && height.Value > 0) ? height : 150;
            duration = (duration.HasValue && duration.Value > 0) ? duration : 1000;

            var anchor = MapControl.GetLocation(pin).Position;
            //var anchor = new Point(50, 60);
            var from = anchor.Longitude + height.Value;

            AnimateY(pin, -from, -anchor.Longitude, duration.Value, new BounceEase()
            {
                Bounces = 2,
                EasingMode = EasingMode.EaseOut,
                Bounciness = 2
            });
        }
    }
}
