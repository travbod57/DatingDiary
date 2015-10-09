using DatingDiary.ViewModels;
using System;

namespace DatingDiary.Views
{
    public partial class TestPage : BasePage
    {
        public TestPage()
        {
            InitializeComponent();
            AnimationContext = LayoutRoot;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.DataContext = new TestPageViewModel(this);
        }

    }
}