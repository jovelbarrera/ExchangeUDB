﻿using System;
using System.Threading.Tasks;

namespace Exchange.Dependencies.Facebook
{
	public interface IFacebook
	{
		Task<bool> ExistFacebookAccount ();
		Task<bool> CreateFacebookAccount ();
		Task<bool> PostOnFacebook (string message);
		bool FacebookPosting (string message, string tokenString = null, string fbId = null);
	}
}

