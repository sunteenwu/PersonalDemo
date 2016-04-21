using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace CCustomTextblock
{
    public sealed class CustomTextBlock : Control
    {
        TextBlock text = null;
        public CustomTextBlock()
        {
            this.DefaultStyleKey = typeof(CustomTextBlock);
        }
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            VisualStateManager.GoToState(this, "unSelected", false);
            text = GetTemplateChild("txtpivot") as TextBlock;
            text.PointerPressed += Text_PointerPressed;
        }

        private void Text_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Selected", true);
        }
    }
}
