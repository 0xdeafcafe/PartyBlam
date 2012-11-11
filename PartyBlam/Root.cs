using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PartyBlam
{
    public class Core
    {
        /// <summary>
        /// Enum for every Halo Game Supported.. These won't change (Apart from new ones added. ie; Halo 4) so don't feel afraid to use them.
        /// </summary>
        public enum Games
        {
            Halo3,
            Halo3ODST,
            HaloReach,
            HaloAnniversary,
            Halo4
        }
    }

    public class Info
    {
        /// <summary>
        /// Copyright infomation about BlamParty
        /// </summary>
        public static string Copyright = "This application is protected under GPL v3 licensing agreement. For more infomation on it, go to: http://www.gnu.org/licenses/quick-guide-gplv3.html.";

        /// <summary>
        /// About BlamParty
        /// </summary>
        public static string About =   "PartyBlam was coded by: Xerax (Alex Reed), via Valhalla Studios." + Environment.NewLine +
                                "Research Help from: Gabe_k, CLK, jfrankkiller, ThunderWaffle, AMD, DJ Shepard, ManBearPig and Gamecheat." + Environment.NewLine +
                                "Development help from kids that know stuff: Gabe_K, CLK, DJ Shepard and AMD." + Environment.NewLine +
                                Environment.NewLine +
                                "People that need to leave the scene, or just never come back.. Ever: PimpinTyler, Se7enSins Staff (mainly Azzid, Sumo, Trammel, and Deep3r). (ever expanding list...)";
    }
}
