using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace DatingDiary.Validation
{
    public class ListPickerValidationControl : Microsoft.Phone.Controls.ListPicker
    {
        public static readonly DependencyProperty IsValidProperty =
        DependencyProperty.Register("IsValid", typeof(bool), typeof(ListPickerValidationControl), new PropertyMetadata(true, new PropertyChangedCallback(ListPickerValidationControl.OnIsValidPropertyChanged)));

        public static readonly DependencyProperty ValidationRuleProperty =
        DependencyProperty.Register("ValidationRule", typeof(IValidationRule), typeof(ListPickerValidationControl), new PropertyMetadata(new PropertyChangedCallback(ListPickerValidationControl.OnValidationRulePropertyChanged)));

        public static readonly DependencyProperty ValidationContentProperty =
        DependencyProperty.Register("ValidationContent", typeof(object), typeof(ListPickerValidationControl), new PropertyMetadata("Incorrect Value", new PropertyChangedCallback(ListPickerValidationControl.OnValidationContentPropertyChanged)));

        //public static readonly DependencyProperty ValidationSymbolProperty =
        //DependencyProperty.Register("ValidationSymbol", typeof(object), typeof(ValidationControl), new PropertyMetadata("!", new PropertyChangedCallback(ValidationControl.OnValidationSymbolPropertyChanged)));

        public static readonly DependencyProperty ValidationContentStyleProperty =
        DependencyProperty.Register("ValidationContentStyle", typeof(Style), typeof(ListPickerValidationControl), null);

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            set { base.SetValue(IsValidProperty, value); }
        }

        public Style ValidationContentStyle
        {
            get { return base.GetValue(ValidationContentStyleProperty) as Style; }
            set { base.SetValue(ValidationContentStyleProperty, value); }
        }

        public object ValidationContent
        {
            get { return base.GetValue(ValidationContentProperty) as object; }
            set { base.SetValue(ValidationContentProperty, value); }
        }

        //public object ValidationSymbol
        //{
        //    get { return base.GetValue(ValidationSymbolProperty) as object; }
        //    set { base.SetValue(ValidationSymbolProperty, value); }
        //}

        public IValidationRule ValidationRule
        {
            get { return base.GetValue(ValidationRuleProperty) as IValidationRule; }
            set { base.SetValue(ValidationRuleProperty, value); }
        }

        public ListPickerValidationControl()
        {
            DefaultStyleKey = typeof(ListPickerValidationControl);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            bool isInputValid = true;
            IValidationRule validationRule = this.ValidationRule;
            if (validationRule != null)
            {
                isInputValid = validationRule.Validate("");
            }
            this.IsValid = isInputValid;

            base.OnLostFocus(e);
        }

        private void ChangeVisualState(bool useTransitions)
        {
            if (!this.IsValid)
            {
                VisualStateManager.GoToState(this, "InValid", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Valid", useTransitions);
            }
        }

        private static void OnIsValidPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListPickerValidationControl control = d as ListPickerValidationControl;
            bool newValue = (bool)e.NewValue;
            control.ChangeVisualState(false);
            //add some additional logic here.
        }

        private static void OnValidationSymbolPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //you can add some additional logic here.
        }

        private static void OnValidationContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //you can add some additional logic here.
        }

        private static void OnValidationRulePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //you can add some additional logic here.
        }
    }

}
