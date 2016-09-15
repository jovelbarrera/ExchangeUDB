using System;
using System.Text.RegularExpressions;

namespace Exchange.Configs
{
	public static class Utils
	{
		private static Regex _alphaNumeric = new Regex(@"^[a-zA-Z0-9_.-ñáéíóúüÑÁÉÍÓÚÜ]*$");

		private const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
										  @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
		private const string urlRegex = @"/^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/";
		private const string ipadressRegex = @"/^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/";
		private const string numberRegex = @"[-+]?[0-9]*\.?[0-9]*";
		private const string digitRegex = @"[0-9]*";
		private const string nonNegativeRegex = @"[+]?[0-9]*\.?[0-9]*";
		private const string nameRegex = @"/^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$/u";
		private const string alphaRegex = @"^([\sA-Za-z]+)$";


		public static bool IsValid(this string input, string regex)
		{
			if (IsValidRegex(regex))
				return false;

			var match = Regex.Match(input, regex, RegexOptions.IgnoreCase);
			return match.Success;
		}

		public static bool IsEmail(this string input)
		{
			var match = Regex.Match(input, emailRegex, RegexOptions.IgnoreCase);
			return match.Success;
		}

		public static bool IsURL(this string input)
		{
			var match = Regex.Match(input, urlRegex, RegexOptions.IgnoreCase);
			return match.Success;
		}

		public static bool IsAddress(this string input)
		{
			var match = Regex.Match(input, ipadressRegex, RegexOptions.IgnoreCase);
			return match.Success;
		}

		public static bool IsNumber(this string input)
		{
			var match = Regex.Match(input, numberRegex, RegexOptions.IgnoreCase);
			return match.Success;
		}

		public static bool IsDigit(this string input)
		{
			var match = Regex.Match(input, digitRegex, RegexOptions.IgnoreCase);
			return match.Success;
		}

		public static bool IsNonNegative(this string input)
		{
			var match = Regex.Match(input, nonNegativeRegex, RegexOptions.IgnoreCase);
			return match.Success;
		}

		public static bool IsValidName(this string input)
		{
			var match = Regex.Match(input, nameRegex, RegexOptions.IgnoreCase);
			return match.Success;
		}

		public static bool IsAlpha(this string input)
		{
			var match = Regex.Match(input, alphaRegex, RegexOptions.IgnoreCase);
			return match.Success;
		}

		public static bool IsBetween(this DateTime input, DateTime? min, DateTime? max,
									  bool inclusive = true)
		{
			if (input == null || (min == null && max == null))
				return false;

			if (min == null)
				return inclusive ? input <= max : input < max;

			if (max == null)
				return inclusive ? input >= min : input > min;

			return inclusive ? input >= min && input <= max : input > min && input < max;
		}

		public static bool IsBetween(this int input, int min, int max,
									  bool inclusive = true)
		{
			if (input == null || (min == null && max == null))
				return false;

			if (min == null)
				return inclusive ? input <= max : input < max;

			if (max == null)
				return inclusive ? input >= min : input > min;

			return inclusive ? input >= min && input <= max : input > min && input < max;
		}

		public static bool IsBetween(this float input, float min, float max,
									  bool inclusive = true)
		{
			if (input == null || (min == null && max == null))
				return false;

			if (min == null)
				return inclusive ? input <= max : input < max;

			if (max == null)
				return inclusive ? input >= min : input > min;

			return inclusive ? input >= min && input <= max : input > min && input < max;
		}

		public static bool IsBetween(this double input, double min, double max,
									   bool inclusive = true)
		{
			if (input == null || (min == null && max == null))
				return false;

			if (min == null)
				return inclusive ? input <= max : input < max;

			if (max == null)
				return inclusive ? input >= min : input > min;

			return inclusive ? input >= min && input <= max : input > min && input < max;
		}

		private static bool IsValidRegex(string regex)
		{
			try
			{
				Regex _regex = new Regex(regex, RegexOptions.None);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public static string[] GetTags(string tagsString)
		{
			string rawTags = tagsString;
			string[] tags = rawTags.Split(',');
			return tags;
		}

		public static bool IsValidTag(string tag)
		{
			if (string.IsNullOrEmpty(tag))
				return false;
			return _alphaNumeric.Match(tag).Success;
		}
	}
}

