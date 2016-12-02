using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exchange.Dependencies
{
	public enum ScreenOrientation
	{
		Landscape,
		Portrait,
		Sensor,
	}

    public interface IPlatform
    {
		void ChangeScreenOrientation(ScreenOrientation screenOrientation);
    }
}

