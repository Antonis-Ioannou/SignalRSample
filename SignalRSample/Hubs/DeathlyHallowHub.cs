using Microsoft.AspNetCore.SignalR;

namespace SignalRSample.Hubs
{
	public class DeathlyHallowHub : Hub
	{

		public Dictionary <string,int> GetRaceStatus()
		{
			return SD.DeathlyHallowRace;
		}

	}
}
