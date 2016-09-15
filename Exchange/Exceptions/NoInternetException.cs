using System;

namespace Exchange.Exceptions
{
	public class NoInternetException : Exception
	{
		public override string Message { get { return "No internet connection"; } }
	}
}

