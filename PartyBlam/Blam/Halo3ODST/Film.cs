using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using PartyBlam.IO;

namespace PartyBlam.Blam.Halo3ODST
{
    public class Film
    {
        private EndianStream _filmStream;
        private PlayerTable _filmPlayerTableData = new PlayerTable();
        private Header _filmHeader;
        private IList<PlayerChunk> _filmPlayers;

        #region Public Access
        public Stream FilmStream
        {
            get { return _filmStream.BaseStream; }
        }
        public Header FilmHeader
        {
            get { return _filmHeader; }
            set { _filmHeader = value;}
        }
        public IList<PlayerChunk> FilmPlayers
        {
            get { return _filmPlayers; }
            set { _filmPlayers = value; }
        }
        #endregion

        #region Class Declaration
        public class PlayerTable
        {
            public long PlayerTableStartPosition = 0xE8F1;
            public long PlayerTableEntryLength = 0x260;
        }

        public class Header
        {
            public string FilmName { get; set; }
            public string FilmDescription { get; set; }
            public string FilmAuthor { get; set; }

            public string EngineBuildString1 { get; set; }
            public string EngineBuildString2 { get; set; }
            public string InfoString { get; set; }
            public string MapName { get; set; }
        }

        public class PlayerChunk
        {
            public long EntryStartLocation { get; set; }
            public long EntryIndex { get; set; }

            public string Gamertag { get; set; }
            public string ServiceTag { get; set; }

            public Character Character { get; set; }
            public HelmetToggle HelmetOn { get; set; }

            public Visuals PlayerVisuals { get; set; }
        }
        public class Visuals
        {
            public EmblemForeground EmblemForeground { get; set; }
            public EmblemBackground EmblemBackground { get; set; }
            public EmblemForegroundToggle EmblemForegroundToggle { get; set; }

            public Colours EmblemForegroundColour { get; set; }
            public Colours EmblemSecondaryColour { get; set; }
            public Colours EmblemBackgroundColour { get; set; }

            public Colours ArmourPrimary { get; set; }
            public Colours ArmourDetail { get; set; }
        }
        #endregion

        #region Enum Declareation
        public enum HelmetToggle
        {
            Off,
            On
        }
        public enum Character
        {
            Rookie,
            Mickey,
            Dutch,
            Romeo,
            Buck,
            Johnson,
            Dare
        }

        public enum Colours
        {
            Black,
            Grey,
            Snow,
            Maroon,
            Brick,
            Rose,
            Brown,
            Woodland,
            Cocoa,
            Tan,
            Khaki,
            Desert,
            Coral,
            Gold,
            Sand,
            Sage,
            Olive,
            Drab,
            Forest,
            Green,
            SeaFoam,
            Teal,
            Aqua,
            Cyan,
            Blue,
            Cobalt,
            Ice,
            Violet,
            Lavender,
            Pink
        }

        public enum EmblemForeground
        {
            SeventhColumn,
            Bullseye,
            Vortex,
            Halt,
            Spartan,
            DaBomb,
            Trinity,
            Delta,
            Rampancy,
            Stuck,
            Phoenix,
            Champion,
            JollyRoger,
            Marathon,
            Cube,
            Radioactive,
            Smiley,
            Frowney,
            Spearhead,
            Sol,
            DoubleCrescent,
            YinYang,
            Helmet,
            Triad,
            GruntSymbol,
            Surf,
            THor,
            SkullKing,
            Triplicate,
            Subnova,
            FlamingNinja,
            Pirate,
            Spades,
            Clubs,
            Diamonds,
            Hearts,
            Wasp,
            MarkOfShame,
            Snake,
            Hawk,
            Lips,
            Capsule,
            Tsantsa,
            GasMask,
            Grenade,
            SpartanHelmet,
            Cancel,
            Valkyrie,
            Drone,
            Grunt,
            GruntHead,
            Runes,
            Trident,
            CrossedSwords,
            Unicorn,
            Wolf,
            BubbleShield,
            TripMine,
            Daisho,
            Daisy,
            Crosshairs,
            TheLibrarian,
            TomCat,
            Supernova,
            FleurDeLis,
            BearClaw,
            FlamingHorns,
            BlackWidow,
            Peaceafist,
            Number1,
            Number2,
            Number3,
            Number4,
            Number5,
            Number6,
            Number7,
            Number8,
            Number9,
            Number0,
            KeepItClean,
            SuperIntendent,
            NewMombasa,
            ONI,
            Optican,
            Planet,
            Tycho,
            Chaos,
            Leo,
            Webmaster,
            Chicken,
            Bulltrue,
            Elephant,
            Zebra,
            SpartanLeague,
            Infected
        }
        public enum EmblemBackground
        {
            Blank,
            Circle,
            Diamond,
            Plus,
            Square,
            Triangle,
            VerticleStripe,
            HorizontalStripe,
            Cleft,
            CrissCross,
            Chalice,
            PointedStar8,
            Star,
            CowboyHat,
            ThickStar,
            Banner,
            Diamonds4,
            Sun,
            Hexagon,
            VerticalHexagon,
            Octagon,
            Pentagon,
            InvertedPentagon,
            RacingStripes,
            HorizontalStripes,
            Gradient,
            HorizontalGradient,
            Oval,
            VerticalOval,
            BluntDiamond,
            BluntDiamond2,
            SharpDiamond,
            SharpDiamond2,
            Aero,
            DeltaWing,
            Asterisk,
            Blam,
            Blam2,
            Shield,
            Display,
            Dreamcatcher,
            Buzzsaw,
            FourPlots,
            BallOFire,
            Cog,
            Sprocket,
            FaceMask,
            FaceMask2,
            SmallCircle,
            Cancel,
            Crown,
            SnappyHat
        }
        public enum EmblemForegroundToggle
        {
            Toggled,
            Untoggled
        }
        #endregion

        /// <summary>
        /// Initalize new instance of the Halo 3: ODST Film
        /// </summary>
        /// <param name="filmPath">Path to the Halo 3: ODST 'feature.film' extracted from a Container file.</param>
        public Film(string filmPath) { Initalize(new MemoryStream(File.ReadAllBytes(filmPath))); }
        /// <summary>
        /// Initalize new instance of the Halo 3: ODST Film
        /// </summary>
        /// <param name="filmStream">Stream of a Halo 3: ODST 'feature.film' extracted from a Container file.</param>
        public Film(Stream filmStream) { Initalize(filmStream); }
        private void Initalize(Stream filmStream)
        {
            _filmStream = new EndianStream(filmStream, Endian.BigEndian);

            if (!isValidFilm())
            {
                Close();
                throw new Exception("Invalid Halo 3: ODST film!");
            }

            // Load Film Parts
            LoadHeader();
            LoadPlayerTable();
        }

        #region Loading Code
        public void LoadHeader()
        {
            _filmHeader = new Header();

            _filmStream.SeekTo(0x48);
            _filmHeader.FilmName = _filmStream.ReadUTF16(0x1F);
            _filmStream.SeekTo(0x67);
            _filmHeader.FilmDescription = _filmStream.ReadAscii(0x80);
            _filmStream.SeekTo(0xE7);
            _filmHeader.FilmAuthor = _filmStream.ReadAscii(0x80);

            _filmStream.SeekTo(0x15);
            _filmHeader.FilmAuthor = _filmStream.ReadAscii(0x2E);
            _filmStream.SeekTo(0x198);
            _filmHeader.FilmAuthor = _filmStream.ReadAscii(0x23);

            _filmStream.SeekTo(0x220);
            _filmHeader.FilmAuthor = _filmStream.ReadAscii(0x26);
            _filmStream.SeekTo(0x2C4);
            _filmHeader.FilmAuthor = _filmStream.ReadAscii(0x2A);
        }

        public void LoadPlayerTable()
        {
            _filmPlayers = new List<PlayerChunk>();

            _filmStream.SeekTo(_filmPlayerTableData.PlayerTableStartPosition);
            bool contReadingTable = true;
            int playerIndex = 0;
            while (contReadingTable)
            {
                long currentEntryOffset = 0xE8F1 + (_filmPlayerTableData.PlayerTableEntryLength * playerIndex);

                _filmStream.SeekTo(currentEntryOffset + 0xF);
                string tmpGamertag = _filmStream.ReadUTF16(0x5);

                if (string.IsNullOrWhiteSpace(tmpGamertag))
                    contReadingTable = false;
                else
                {
                    PlayerChunk player = new PlayerChunk();
                    player.PlayerVisuals = new Visuals();

                    player.EntryIndex = playerIndex;
                    player.EntryStartLocation = currentEntryOffset;

                    _filmStream.SeekTo(currentEntryOffset + 0xF);
                    player.Gamertag = _filmStream.ReadUTF16();

                    _filmStream.SeekTo(currentEntryOffset + 0x45);
                    player.ServiceTag = _filmStream.ReadUTF16();

                    _filmStream.SeekTo(currentEntryOffset + 0x40);
                    player.Character = (Character)_filmStream.ReadByte();
                    player.HelmetOn = (HelmetToggle)_filmStream.ReadByte();

                    _filmStream.SeekTo(currentEntryOffset + 0x30);
                    player.PlayerVisuals.ArmourPrimary = (Colours)_filmStream.ReadByte();
                    player.PlayerVisuals.ArmourDetail = (Colours)_filmStream.ReadByte();
                    _filmStream.Skip(0x03);
                    player.PlayerVisuals.EmblemForeground = (EmblemForeground)_filmStream.ReadByte();
                    player.PlayerVisuals.EmblemBackground = (EmblemBackground)_filmStream.ReadByte();
                    player.PlayerVisuals.EmblemForegroundToggle = (EmblemForegroundToggle)_filmStream.ReadByte();
                    player.PlayerVisuals.EmblemForegroundColour = (Colours)_filmStream.ReadByte();
                    player.PlayerVisuals.EmblemSecondaryColour = (Colours)_filmStream.ReadByte();
                    player.PlayerVisuals.EmblemBackgroundColour = (Colours)_filmStream.ReadByte();

                    _filmPlayers.Add(player);
                }

                playerIndex++;
            }
        }
        #endregion

        #region Update Code
        /// <summary>
        /// Update the film's Header, and PlayerTable
        /// </summary>
        public void Update()
        {
            UpdateHeader();
            UpdatePlayerTable();

            Update();
        }

        /// <summary>
        /// Update the film's Header
        /// </summary>
        public void UpdateHeader()
        {
            _filmStream.SeekTo(0x48);
            _filmStream.WriteUTF16(_filmHeader.FilmName);
            _filmStream.SeekTo(0x67);
            _filmStream.WriteAscii(_filmHeader.FilmDescription);
            _filmStream.SeekTo(0xE7);
            _filmStream.WriteAscii(_filmHeader.FilmAuthor);

            _filmStream.SeekTo(0x15);
            _filmStream.WriteAscii(_filmHeader.FilmAuthor);
            _filmStream.SeekTo(0x198);
            _filmStream.WriteAscii(_filmHeader.FilmAuthor);

            _filmStream.SeekTo(0x220);
            _filmStream.WriteAscii(_filmHeader.FilmAuthor);
            _filmStream.SeekTo(0x2C4);
            _filmStream.WriteAscii(_filmHeader.FilmAuthor);
        }

        /// <summary>
        /// Update the film's Player Table
        /// </summary>
        public void UpdatePlayerTable()
        {
            foreach (PlayerChunk player in _filmPlayers)
            {
                long currentEntryOffset = player.EntryStartLocation;

                _filmStream.SeekTo(currentEntryOffset + 0x0F);
                _filmStream.WriteUTF16(player.Gamertag);

                _filmStream.SeekTo(currentEntryOffset + 0x45);
                _filmStream.WriteUTF16(player.ServiceTag);

                _filmStream.SeekTo(currentEntryOffset + 0x40);
                _filmStream.WriteByte((byte)player.Character);
                _filmStream.WriteByte((byte)player.HelmetOn);

                _filmStream.SeekTo(currentEntryOffset + 0x30);
                _filmStream.WriteByte((byte)player.PlayerVisuals.ArmourPrimary);
                _filmStream.WriteByte((byte)player.PlayerVisuals.ArmourDetail);
                _filmStream.Skip(0x03);
                _filmStream.WriteByte((byte)player.PlayerVisuals.EmblemForeground);
                _filmStream.WriteByte((byte)player.PlayerVisuals.EmblemBackground);
                _filmStream.WriteByte((byte)player.PlayerVisuals.EmblemForegroundToggle);
                _filmStream.WriteByte((byte)player.PlayerVisuals.EmblemForegroundColour);
                _filmStream.WriteByte((byte)player.PlayerVisuals.EmblemSecondaryColour);
                _filmStream.WriteByte((byte)player.PlayerVisuals.EmblemBackgroundColour);

                _filmStream.SeekTo(currentEntryOffset + 0x20F);
                _filmStream.WriteUTF16(player.Gamertag);
            }
        }
        #endregion

        public void Close()
        {
            _filmStream.Close();
        }
        public bool isValidFilm()
        {
            _filmStream.SeekTo(0x0E);
            string blfType = _filmStream.ReadAscii(0x12);

            if (blfType.StartsWith("halo 3 saved film"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Extract a player entry from the film
        /// </summary>
        /// <param name="entryIndex">Entry Index (User-Friendly, Not Computer Science Specific. ie 1, not 0 for position 0)</param>
        /// <param name="filePath">Path to extract too, include file name</param>
        public void ExtractPlayerEntry(int entryIndex, string filePath)
        {
            entryIndex--;

            long currentEntryOffset = _filmPlayerTableData.PlayerTableStartPosition + (_filmPlayerTableData.PlayerTableEntryLength * entryIndex);

            _filmStream.SeekTo(currentEntryOffset + 0x0F);
            if (String.IsNullOrWhiteSpace(_filmStream.ReadUTF16()))
            {
                Close();
                throw new Exception("Player Entry Index Doesn't Exist");
            }
            else
            {
                byte[] tableEntry = new byte[_filmPlayerTableData.PlayerTableEntryLength];
                _filmStream.SeekTo(currentEntryOffset);
                _filmStream.ReadBlock(tableEntry, 0, (int)_filmPlayerTableData.PlayerTableEntryLength);

                File.WriteAllBytes(filePath.Replace("/", ""), tableEntry);

                Close();
            }
        }
    }
}
