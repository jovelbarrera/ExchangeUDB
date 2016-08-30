using System;
using System.Collections.Generic;
using BranchXamarinSDK;
using Exchange.Interfaces;
using Xamarin.Forms;

namespace Exchange.BranchService
{
	public enum LaunchPage
	{
		Main,
		Navigation,
		Modal
	}

	public class BranchService<T, I> : DeepLinkService<BranchService<T, I>, T>
		where T : IModel
		where I : Page, IDeepLinkPage<T>, new()
	{
		public IBranchUrlInterface BranchUrl;

		public LaunchPage LaunchPage { get; set; }

		public override void ReadLink(Dictionary<string, object> data)
		{
			T model = GetModel(data);
			var page = new I();
			page.OnDeeplink(model);
			App.Current.MainPage = page;
		}

		public override void GenerateLink(Dictionary<string, object> data, IDeepLinkMetadata deepLinkMetadata)
		{
			var universalObject = new BranchUniversalObject();
			//universalObject.canonicalIdentifier = "item/12345";
			universalObject.title = deepLinkMetadata.Title;
			universalObject.contentDescription = deepLinkMetadata.Description;
			universalObject.imageUrl = deepLinkMetadata.ImageUrl;
			universalObject.contentIndexMode = 0; // 1 for private

			foreach (var parameter in data)
			{
				universalObject.metadata.Add(parameter.Key, parameter.Value.ToString());
			}

			var linkProperties = new BranchLinkProperties();
			linkProperties.feature = "sharing";
			linkProperties.channel = "facebook";
			linkProperties.controlParams.Add("$desktop_url", "http://example.com/home");
			linkProperties.controlParams.Add("$ios_url", "http://example.com/ios");

			if (BranchUrl == null)
				throw new Exception("Deep link cannot be created witout define a IBranchUrlInterface");
			Branch.GetInstance().GetShortURL(BranchUrl,
							  universalObject,
							  linkProperties);
		}
	}
}

