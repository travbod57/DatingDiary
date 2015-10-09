using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Interactivity;
using DatingDiary.Helpers;
using DatingDiary.Model;
using DatingDiary.Singletons;
using DatingDiary.ViewModels.People;

namespace DatingDiary.Interactivity
{
    public class DeleteItemBehaviour : Behavior<FrameworkElement>
    {
        public DeleteItemBehaviour() : base()
        {

        }

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.Tap += new EventHandler<GestureEventArgs>(AssociatedObject_Tap);
        }

        void AssociatedObject_Tap(object sender, GestureEventArgs e)
        {
             Storyboard s = new Storyboard();

             Image rect = GeneralMethods.FindChild<Image>(this.AssociatedObject, "ImageMask");
             Border b = (Border)sender;
             Photo item = b.DataContext as Photo;

             DoubleAnimation doubleAnimation = new DoubleAnimation();
             doubleAnimation.From = item.IsSelected ? 0 : 1;
             doubleAnimation.To = item.IsSelected ? 1 : 0;
             doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

             s.Children.Add(doubleAnimation);
             s.Duration = new Duration(TimeSpan.FromSeconds(1));

             Storyboard.SetTarget(doubleAnimation, rect);
             Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Rectangle.OpacityProperty));

             s.Begin();
            
             
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();


        }
    }
}
