using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace DatingDiary.Validation
{
    public class ValidationControl : TextBox
    {
        public static readonly DependencyProperty IsValidProperty =
        DependencyProperty.Register("IsValid", typeof(bool), typeof(ValidationControl), new PropertyMetadata(true, new PropertyChangedCallback(ValidationControl.OnIsValidPropertyChanged)));

        public static readonly DependencyProperty ValidationRuleProperty =
        DependencyProperty.Register("ValidationRule", typeof(IValidationRule), typeof(ValidationControl), new PropertyMetadata(new PropertyChangedCallback(ValidationControl.OnValidationRulePropertyChanged)));

        public static readonly DependencyProperty ValidationContentProperty =
        DependencyProperty.Register("ValidationContent", typeof(object), typeof(ValidationControl), new PropertyMetadata("Incorrect Value", new PropertyChangedCallback(ValidationControl.OnValidationContentPropertyChanged)));

        //public static readonly DependencyProperty ValidationSymbolProperty =
        //DependencyProperty.Register("ValidationSymbol", typeof(object), typeof(ValidationControl), new PropertyMetadata("!", new PropertyChangedCallback(ValidationControl.OnValidationSymbolPropertyChanged)));

        public static readonly DependencyProperty ValidationContentStyleProperty =
        DependencyProperty.Register("ValidationContentStyle", typeof(Style), typeof(ValidationControl), null);

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

        public ValidationControl()
        {
            DefaultStyleKey = typeof(ValidationControl);
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
                isInputValid = validationRule.Validate(this.Text);
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
            ValidationControl control = d as ValidationControl;
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

    public interface IValidationRule
    {
        bool Validate(string input);
    }

    public class RequiredValidationRule : IValidationRule
    {
        public bool Validate(string input)
        {
            return !String.IsNullOrEmpty(input);
        }
    }

    public class FixedLenghtValidationRule : IValidationRule
    {
        public int Length
        {
            get;
            set;
        }

        public bool Validate(string input)
        {
            int inputLength = input != null ? input.Length : 0;
            return inputLength == this.Length;
        }
    }

    public class MinLengthValidationRule : IValidationRule
    {
        public int MinLength
        {
            get;
            set;
        }

        public bool Validate(string input)
        {
            int inputLength = input != null ? input.Length : 0;
            return inputLength >= this.MinLength;
        }
    }

    public class MaxLenghtValidationRule : IValidationRule
    {
        public int MaxLength
        {
            get;
            set;
        }

        public bool Validate(string input)
        {
            int inputLength = input != null ? input.Length : 0;
            return inputLength <= this.MaxLength;
        }
    }

    public class RegexValidationRule : IValidationRule
    {
        public RegexValidationRule(string pattern)
        {
            this.Pattern = pattern;
        }

        public string Pattern
        {
            get;
            private set;
        }

        public bool Validate(string input)
        {
            return Regex.IsMatch(input, this.Pattern);
        }
    }

    public class AndValidationRule : IValidationRule
    {
        public AndValidationRule()
        {
            this.Rules = new List<IValidationRule>();
        }

        public List<IValidationRule> Rules
        {
            get;
            set;
        }

        public bool Validate(string input)
        {
            foreach (IValidationRule rule in this.Rules)
            {
                if (rule != null && !rule.Validate(input))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class OrValidationRule : IValidationRule
    {
        public OrValidationRule()
        {
            this.Rules = new List<IValidationRule>();
        }

        public List<IValidationRule> Rules
        {
            get;
            set;
        }

        public bool Validate(string input)
        {
            foreach (IValidationRule rule in this.Rules)
            {
                if (rule != null && rule.Validate(input))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
