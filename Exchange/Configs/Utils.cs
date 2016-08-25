using System;
using System.Text.RegularExpressions;

namespace Exchange.Configs
{
	public class Utils
	{
		private static Regex _alphaNumeric = new Regex(@"^[a-zA-Z0-9_.-ñáéíóúüÑÁÉÍÓÚÜ]*$");

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

