using System;
using System.Net.Http;
using System.Threading.Tasks;
using Plugin.Connectivity;

namespace Exchange.Services
{
	public class RequestHandler<T>
	{
		public Action<Exception> ExceptionHandler;
		public Task<T> ServiceRequest { get; private set; }

		public RequestHandler(Task<T> serviceRequest)
		{
			ServiceRequest = serviceRequest;
		}

		public static RequestHandler<T> Factory(Task<T> serviceRequest)
		{
			return new RequestHandler<T>(serviceRequest);
		}

		public async Task<T> Execute()
		{
			try
			{
				if (ServiceRequest == null)
					throw new NullReferenceException();
				else if (!CrossConnectivity.Current.IsConnected)
					throw new NoInternetException();
				await ServiceRequest.ConfigureAwait(true);
				return ServiceRequest.Result;

			}
			catch (Exception ex)
			{
				ExceptionHandler(ex);
			}
			return default(T);
		}
	}


	//catch (HttpRequestException ex)
	//{
	//	//if (ex.Message == "401 (Unauthorized)")
	//	//	await App.Current.MainPage.DisplayAlert("Error", "No tienes permisos para completar esta acción", "ACEPTAR");
	//	//else if (ex.Message == "400 (Bad Request)")
	//	//	await App.Current.MainPage.DisplayAlert("Error", "Un error ocurrió al intentar realizar esta acción", "ACEPTAR");
	//	//else
	//	//	await App.Current.MainPage.DisplayAlert("Error", ex.Message, "ACEPTAR");
	//	System.Diagnostics.Debug.WriteLine("**********************" + ex.Message);
	//}
	//catch (NoInternetException ex)
	//{
	//	System.Diagnostics.Debug.WriteLine("**********************" + ex.Message);
	//}
	public class NoInternetException : Exception
	{
		public override string Message { get { return "No internet connection"; } }
	}
}

