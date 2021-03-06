﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Exchange.Services
{
	public class TimeService
	{
		private static TimeService _instance;
		public static TimeService Instance
		{
			get
			{
				if (_instance == null)
					_instance = new TimeService();
				return _instance;
			}
		}

		public async Task<DateTime> Now()
		{
			var httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
			HttpResponseMessage bar = await httpClient.GetAsync(new Uri("http://www.timeapi.org/utc/now"));

			DateTime currentTime;
			if (bar.IsSuccessStatusCode)
			{
				string d = await bar.Content.ReadAsStringAsync();
				currentTime = DateTime.Parse(d);
			}
			else
			{
				currentTime = DateTime.Now;
			}
			return currentTime;
		}
	}
}

