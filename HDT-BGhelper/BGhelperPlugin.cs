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
                    "Right click = refresh, middle click = freeze."; }
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
        }

        public void OnUnload()
        {
            helper.Deactivate();
        }

        public void OnUpdate()
        {
            if (Core.Game.IsBattlegroundsMatch && !Core.Game.IsInMenu)
                helper.Activate();
            else
                helper.Deactivate();
        }

        public Version Version
        {
            get { return new Version(1, 5); }
        }
    }
}