using System;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;

namespace HDT_BGhelper
{
	public class BGhelperPlugin : IPlugin
	{
		private BGhelper helper;

        public string Author
		{
			get { return "IBM5100"; }
		}

		public string ButtonText
		{
			get { return "No Settings"; }
		}

		public string Description
		{
			get { return "A battleground plugin makes shortcuts for refresh and freeze.\n" +
					"Right click = refresh, middle click = freeze"; }
		}

		public System.Windows.Controls.MenuItem MenuItem
		{
			get { return null; }
		}

		public string Name
		{
			get { return "HDT-BGhelper"; }
		}

		public void OnButtonPress()
		{
        }

        public void OnLoad()
		{
			helper = new BGhelper();
			GameEvents.OnGameStart.Add(helper.GameStart);
			GameEvents.OnInMenu.Add(helper.InMenu);
        }

        public void OnUnload()
		{
            helper.Disable();
        }

		public void OnUpdate()
		{
        }

		public Version Version
		{
			get { return new Version(1, 0); }
		}
	}
}