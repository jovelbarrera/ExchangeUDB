using System;
using System.Threading.Tasks;
using Exchange.Interfaces;
using Xamarin.Forms;

namespace Exchange.BranchService
{
	public interface IDeepLinkPage<T>
	where T : IModel
	{
		Task OnDeeplink(T model);
	}
}

