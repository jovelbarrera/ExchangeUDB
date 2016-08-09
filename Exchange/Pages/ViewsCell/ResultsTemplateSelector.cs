using System;
using Exchange.Models;
using Xamarin.Forms;

namespace Exchange.ViewCells
{
	public class ResultsTemplateSelector : DataTemplateSelector
	{
		private readonly DataTemplate _askDataTemplate;
		private readonly DataTemplate _exchangeDataTemplate;

		public ResultsTemplateSelector()
		{
			_askDataTemplate = new DataTemplate(typeof(AskViewCell));
			_exchangeDataTemplate = new DataTemplate(typeof(ExchangeViewCell));
		}

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			if (item == null)
				return null;

			Type itemType = item.GetType();

			if (itemType == typeof(Ask))
				return _askDataTemplate;
			else if (itemType == typeof(Models.Exchange))
				return _exchangeDataTemplate;
			else
				return null;
		}
	}
}

