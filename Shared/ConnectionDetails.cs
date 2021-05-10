using static System.Environment;

namespace Shared
{
	public static class ConnectionDetails
	{
		const bool publish = false;
		public static readonly string connection = "Host=localhost;Username=app;Database=tournaments";
		public static readonly string host = "localhost";

		static ConnectionDetails()
		{
			if (!publish)
			{
				string password = GetEnvironmentVariable("mtganalysis_password");
				string ip = GetEnvironmentVariable("mtganalysis_ip");

				connection = "Host=" + ip + ";Username=app;Password=" + password + ";Database=tournaments";
				host = ip;
			}
		}
	}
}
