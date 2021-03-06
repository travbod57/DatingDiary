﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DatingDiary.Other
{
    public class DoubleLoopingDataSource : LoopingDataSourceBase
    {
        private double minValue;
        private double maxValue;
        private double increment;

        public DoubleLoopingDataSource()
        {
            this.MaxValue = 5;
            this.MinValue = 0;
            this.Increment = 0.5;
            this.SelectedItem = 0;
        }

        public double MinValue
        {
            get
            {
                return this.minValue;
            }
            set
            {
                if (value >= this.MaxValue)
                {
                    throw new ArgumentOutOfRangeException("MinValue", "MinValue cannot be equal or greater than MaxValue");
                }
                this.minValue = value;
            }
        }

        public double MaxValue
        {
            get
            {
                return this.maxValue;
            }
            set
            {
                if (value <= this.MinValue)
                {
                    throw new ArgumentOutOfRangeException("MaxValue", "MaxValue cannot be equal or lower than MinValue");
                }
                this.maxValue = value;
            }
        }

        public double Increment
        {
            get
            {
                return this.increment;
            }
            set
            {
                if (value < 0.5)
                {
                    throw new ArgumentOutOfRangeException("Increment", "Increment cannot be less than or equal to zero");
                }
                this.increment = value;
            }
        }

        public override object GetNext(object relativeTo)
        {
            double nextValue = (double)relativeTo + this.Increment;
            if (nextValue > this.MaxValue)
            {
                nextValue = this.MinValue;
            }
            return nextValue;
        }

        public override object GetPrevious(object relativeTo)
        {
            double prevValue = (double)relativeTo - this.Increment;
            if (prevValue < this.MinValue)
            {
                prevValue = this.MaxValue;
            }
            return prevValue;
        }
    }
}
