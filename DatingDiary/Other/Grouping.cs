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
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;

namespace DatingDiary.Other
{
	public class Grouping<TKey, TElement> : Object, IGrouping<TKey, TElement>
	{
		private readonly IGrouping<TKey, TElement> _group;
		public Grouping(IGrouping<TKey, TElement> group)
		{
			this._group = group;
            Debug.WriteLine("Person Created");
		}
		public TKey Key { get { return _group.Key; } }
		public IEnumerator<TElement> GetEnumerator()
		{
			return _group.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return _group.GetEnumerator();
		}

		public override bool Equals(object obj)
		{
			IGrouping<TKey, TElement> that = obj as IGrouping<TKey, TElement>;
			return (that != null) && (this.Key.Equals(that.Key));
		}
		public override int GetHashCode()
		{
			return Key.GetHashCode();
		}
	}
}
