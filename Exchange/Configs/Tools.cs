using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Reflection;
using System.Linq;

namespace Exchange.Configs
{
	public static class Tools
	{
		public static NavigationPage CreateNavigationPage(Page page)
		{
			NavigationPage navigationPage = new NavigationPage(page)
			{
				BarTextColor = Color.White,
				BarBackgroundColor = Styles.Colors.Primary,
			};
			return navigationPage;
		}

		public static void AddTapHandler(this View view, EventHandler tapped)
		{
			var tapGesture = new TapGestureRecognizer();
			tapGesture.Tapped += tapped;
			view.GestureRecognizers.Add(tapGesture);
		}

		public static string HumanDate(this DateTimeOffset dateTimeOffset)
		{
			var datetime = dateTimeOffset.DateTime;
			var now = DateTime.Now;
			var difference = now - datetime;

			if (difference.Seconds < 0)
				difference = now - now;

			if (difference.TotalSeconds < 60)
			{
				return "Hace un instante";
			}
			else if (difference.TotalSeconds < 60)
			{
				return string.Format("Hace {0} {1}",
									 difference.Seconds,
									 difference.Seconds > 1 ? "segundos" : "segundo");
			}
			else if (difference.TotalMinutes < 60)
			{
				return string.Format("Hace {0} {1}",
									 difference.Minutes,
									 difference.Minutes > 1 ? "minutos" : "minuto");
			}
			else if (difference.TotalHours < 24)
			{
				return string.Format("Hace {0} {1}",
									 difference.Hours,
									 difference.Hours > 1 ? "horas" : "hora");
			}
			else if (difference.TotalDays <= 1)
			{
				return string.Format("Ayer a {0} {1}",
									 datetime.Hour > 1 ? "las" : "la",
									 datetime.ToString("h:mm tt"));
			}
			else if (difference.TotalDays <= 7)
			{
				string weekday = string.Empty;
				switch ((int)datetime.DayOfWeek)
				{
					case 1:
						weekday = "lunes";
						break;
					case 2:
						weekday = "martes";
						break;
					case 3:
						weekday = "miércoles";
						break;
					case 4:
						weekday = "jueves";
						break;
					case 5:
						weekday = "viernes";
						break;
					case 6:
						weekday = "sábado";
						break;
					case 7:
						weekday = "domingo";
						break;
				}
				return string.Format("El {0} a {1} {2}",
									 weekday,
									 datetime.Hour > 1 ? "las" : "la",
									 datetime.ToString("h:mm tt"));
			}
			else
			{
				string month = string.Empty;
				switch (datetime.Month)
				{
					case 1:
						month = "Enero";
						break;
					case 2:
						month = "Febrero";
						break;
					case 3:
						month = "Marzo";
						break;
					case 4:
						month = "Abril";
						break;
					case 5:
						month = "Mayo";
						break;
					case 6:
						month = "Junio";
						break;
					case 7:
						month = "Julio";
						break;
					case 8:
						month = "Agosto";
						break;
					case 9:
						month = "Septiembre";
						break;
					case 10:
						month = "Octubre";
						break;
					case 11:
						month = "Noviembre";
						break;
					case 12:
						month = "Diciembre";
						break;
				}
				return string.Format("{0} de {1}{2}",
									 datetime.Day,
									 month,
									 datetime.Year != now.Year ? " del " + datetime.Year : string.Empty); ;
			}
		}

		public static string TruncateText(string text, int characters)
		{
			string truncatedText = string.Empty;
			if (text.Length > characters)
				truncatedText = text.Substring(0, characters) + "...";
			else
				truncatedText = text;
			return truncatedText;
		}


		public static T ToObject<T>(this IDictionary<string, object> source)
			where T : class, new()
		{
			T someObject = new T();
			Type someObjectType = someObject.GetType();

			foreach (KeyValuePair<string, object> item in source)
			{
				someObjectType.GetRuntimeProperty(item.Key).SetValue(someObject, item.Value, null);
			}

			return someObject;
		}

		public static Dictionary<string, object> AsDictionary(this object source)
		{
			return source.GetType().GetRuntimeProperties().ToDictionary
			(
				propInfo => propInfo.Name,
				propInfo => propInfo.GetValue(source, null)
			);
		}
	}
}

