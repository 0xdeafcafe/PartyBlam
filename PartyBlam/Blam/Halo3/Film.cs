using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PartyBlam.IO;

namespace PartyBlam.Blam.Halo3
{
    public class Film
    {
        private EndianStream _filmStream;
        private string _filmPath;
        private PlayerTable _filmPlayerTable = new PlayerTable();
        private MachineIDTable _filmMachineIDTable = new MachineIDTable();
        private Header _filmHeader;
        private IList<byte[]> _filmMachineIDs;
        private IList<Player> _filmPlayers;
        private Footer _filmFooter;

        #region Public Access
        public Stream FileStream
        {
            get { return _filmStream.BaseStream; }
        }
        public string FilmPath
        {
            get { return _filmPath; }
        }
        public PlayerTable FilmPlayerTable
        {
            get { return _filmPlayerTable; }
        }
        public MachineIDTable FilmMachineIDTable
        {
            get { return _filmMachineIDTable; }
        }
        public Header FilmHeader
        {
            get { return _filmHeader; }
            set { _filmHeader = value; }
        }
        public IList<byte[]> FilmMachineIDs
        {
            get { return _filmMachineIDs; }
            set { _filmMachineIDs = value; }
        }
        public IList<Player> FilmPlayers
        {
            get { return _filmPlayers; }
            set { _filmPlayers = value; }
        }
        public Footer FilmFoooter
        {
            get { return _filmFooter; }
            set { _filmFooter = value; }
        }
        #endregion

        #region Class Declaration
        public class PlayerTable
        {
            public long PlayerTableStartPosition = 0xE839;
            public long PlayerTableEntryLength = 0x128;
        }
        public class MachineIDTable
        {
            public long MachineIDTableStartPosition = 0xE7C4;
        }

        public class Header
        {
            public string FilmName { get; set; }
            public string FilmDescription { get; set; }
            public string CreationAuthor { get; set; }
            public Int32 CreationDate { get; set; }
            public string EngineBuildString1 { get; set; }
            public string EngineBuildString2 { get; set; }
            public string InfoString { get; set; }
            public string MapName { get; set; }
            public string MatchmakingPlaylistName { get; set; }
            public string GametypeName { get; set; }
            public string GametypeDescription { get; set; }
            public string GametypeAuthor { get; set; }
            public string UsermapName { get; set; }
            public string UsermapDescription { get; set; }
            public string UsermapAuthor { get; set; }
        }
        public class Player
        {
            public long EntryStartLocation { get; set; }
            public int EntryIndex { get; set; }

            public string Gamertag { get; set; }
            public string ServiceTag { get; set; }

            public Species Species { get; set; }
            public byte[] MachineID { get; set; }

            public SpartanProperties SpartanProperties { get; set; }
            public EliteProperties EliteProperties { get; set; }
            public Visuals PlayerVisuals { get; set; }
        }
        public class Footer
        {
            public long GametypeLocation = 0x4DD;
            public long GametypeSize = 0x24F;
            public byte[] Gametype { get; set; }

            public long UsermapLocation = 0x739;
            public long UsermapSize = 0xE087;
            public byte[] Usermap { get; set; }
        }

        public class EliteProperties
        {
            public EliteHelmet Helmet { get; set; }
            public EliteShoulders LeftShoulder { get; set; }
            public EliteShoulders RightShoulder { get; set; }
            public EliteChest Chest { get; set; }
        }
        public class SpartanProperties
        {
            public SpartanGender Gender { get; set; }
            public SpartanHelmet Helmet { get; set; }
            public SpartanShoulder LeftShoulder { get; set; }
            public SpartanShoulder RightShoulder { get; set; }
            public SpartanChest Chest { get; set; }
        }
        public class Visuals
        {
            public Teams Team { get; set; }
            public Colours ArmourColourPrimary { get; set; }
            public Colours ArmourColourSecondary { get; set; }
            public Colours ArmourColourDetail { get; set; }
            public EmblemForeground EmblemForeground { get; set; }
            public EmblemForegroundToggle EmblemForegroundToggle { get; set; }
            public EmblemBackground EmblemBackground { get; set; }
            public Colours EmblemColourForeground { get; set; }
            public Colours EmblemColourBackground { get; set; }
        }
        #endregion

        #region Enum Declareation
        /// <summary>
        /// Offset: Ox2F
        /// </summary>
        public enum SpartanGender
        {
            Male,
            Female
        }

        /// <summary>
        /// Offset: 0x33
        /// </summary>
        public enum Species
        {
            Spartan,
            Elite
        }

        /// <summary>
        /// Offset: 0x35
        /// </summary>
        public enum EmblemForeground
        {
            SeventhColoumn,
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
            ChampionJollyRoger,
            Marathon,
            Cube,
            Radioactive,
            Smiley,
            Frowney,
            Spearhead,
            Sol,
            DoubleCrescent,
            Helmet,
            Triad,
            GruntSymbol,
            Surf,
            Thor,
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
            BruteHead,
            Runes,
            Trident,
            CrossedSwords,
            Unicorn,
            Wolf,
            BubbleShield,
            TipMine,
            Daisho,
            Daisy,
            Crosshairs,
            TheLibrarian,
            Tomcat,
            Supernova,
            FleurDeLis,
            BearClaw,
            FlamingHorns,
            BlackWidow,
            Ohnoudnt,
            Number1,
            Number2,
            Number3,
            Number4,
            Number5,
            Number6,
            Number7,
            Number8,
            Number9,
            Number0
        }
        /// <summary>
        /// Offset: 0x36
        /// </summary>
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
            FaceMask2
        }
        /// <summary>
        /// Offset: 0x37
        /// </summary>
        public enum EmblemForegroundToggle
        {
            Toggled,
            Untoggled
        }

        /// <summary>
        /// Offset: 0x3D
        /// </summary>
        public enum SpartanHelmet
        {
            MarkVI,
            CQB,
            EVA,
            Recon,
            EOD,
            Hayabusa,
            Security,
            Scout,
            ODST,
            MarkV,
            Rogue
        }
        /// <summary>
        /// Offset: 0x3E/0x3F
        /// </summary>
        public enum SpartanShoulder
        {
            MarkVI,
            CQB,
            EVA,
            Recon,
            EOD,
            Hayabusa,
            Security,
            Scout
        }
        /// <summary>
        /// Offset: 0x40
        /// </summary>
        public enum SpartanChest
        {
            MarkVI,
            CQB,
            EVA,
            Recon,
            Hayabusa,
            EOD,
            Scout,
            Katana,
            Bungie
        }

        /// <summary>
        /// Offset: 0x41
        /// </summary>
        public enum EliteHelmet
        {
            Combat,
            Assault,
            FlightMark,
            Ascetic,
            Commando
        }
        /// <summary>
        /// Offset: 0x42/0x43
        /// </summary>
        public enum EliteShoulders
        {
            Combat,
            Assault,
            FlightMark,
            Ascetic,
            Commando
        }
        /// <summary>
        /// Offset: 0x44
        /// </summary>
        public enum EliteChest
        {
            Combat,
            Assault,
            FlightMark,
            Ascetic,
            Commando
        }

        public enum Colours
        {
            Steel,
            Silver,
            White,
            Red,
            Mauve,
            Salmon,
            Orange,
            Coral,
            Peach,
            Gold,
            Yellow,
            Pale, 
            Sage,
            Green,
            Olive,
            Teal,
            Aqua,
            Cyan,
            Blue,
            Cobalt,
            Sapphire,
            Violet,
            Orchid,
            Lavender,
            Crimson,
            Rubine,
            Pink,
            Brown,
            Tan,
            Khaki
        }
        
        /// <summary>
        /// Offset: 0xFA
        /// </summary>
        public enum Teams
        {
            Red,
            Blue,
            Green,
            Orange,
            Purple,
            Gold,
            Brown,
            Pink,
            Observers
        }
        #endregion

        /// <summary>
        /// Initalize new instance of the Halo 3 Film
        /// </summary>
        /// <param name="filmPath">Path to the Halo 3 'feature.film' extracted from a Container file.</param>
        public Film(string filmPath)
        {
            _filmPath = filmPath;
            Stream memStream = new MemoryStream(File.ReadAllBytes(filmPath));
            _filmStream = new EndianStream(memStream, Endian.BigEndian);

            if (!IsValidFilm())
            {
                Close();
                throw new Exception("Invalid Halo 3 Film!");
            }

            LoadHeader();
            LoadMachineIDTable();
            LoadPlayerTable();
            LoadFooter();
        }
        /// <summary>
        /// Initalize new instance of the Halo 3 Film
        /// </summary>
        /// <param name="filmStream">Stream of a Halo 3 'feature.film' extracted from a Container file.</param>
        public Film(Stream filmStream)
        {
            _filmPath = null;
            _filmStream = new EndianStream(filmStream, Endian.BigEndian);

            if (!IsValidFilm())
            {
                Close();
                throw new Exception("Invalid Halo 3 Film!");
            }

            LoadHeader();
            LoadMachineIDTable();
            LoadPlayerTable();
            LoadFooter();
        }

        #region Load Code
        /// <summary>
        /// Load the Film Header
        /// </summary>
        public void LoadHeader()
        {
            _filmHeader = new Header();

            #region Creation and Modification
            // Read Creation Date
            _filmStream.SeekTo(0x114);
            _filmHeader.CreationDate = _filmStream.ReadInt32();

            // Read Creation Author
            _filmStream.SeekTo(0xE8);
            _filmHeader.CreationAuthor = _filmStream.ReadAscii();
            #endregion
            #region Film Info
            // Read Film Name
            _filmStream.SeekTo(0x48);
            _filmHeader.FilmName = _filmStream.ReadUTF16();

            // Read Film Description
            _filmStream.SeekTo(0x67);
            _filmHeader.FilmDescription = _filmStream.ReadUTF16();
            
            // Read Film Engine Build String 1
            _filmStream.SeekTo(0x15C);
            _filmHeader.EngineBuildString1 = _filmStream.ReadAscii();

            // Read Film Engine Build String 2
            _filmStream.SeekTo(0x198);
            _filmHeader.EngineBuildString2 = _filmStream.ReadAscii();
            
            // Read Film InfoString
            _filmStream.SeekTo(0x220);
            _filmHeader.InfoString = _filmStream.ReadAscii();
            #endregion
            #region GametypeUsermapInfo
            // Read Gametype Name
            _filmStream.SeekTo(0x4DC);
            _filmHeader.GametypeName = _filmStream.ReadUTF16();

            // Read Gametype Description
            _filmStream.SeekTo(0x4FB);
            _filmHeader.GametypeDescription = _filmStream.ReadUTF16();

            // Read Gametype Author
            _filmStream.SeekTo(0x57B);
            _filmHeader.GametypeAuthor = _filmStream.ReadUTF16();

            // Read Usermap Name
            _filmStream.SeekTo(0x738);
            _filmHeader.GametypeAuthor = _filmStream.ReadUTF16();

            // Read Usermap Description
            _filmStream.SeekTo(0x757);
            _filmHeader.GametypeAuthor = _filmStream.ReadUTF16();

            // Read Usermap Author
            _filmStream.SeekTo(0x7D7);
            _filmHeader.GametypeAuthor = _filmStream.ReadUTF16();
            #endregion
            #region RawMapData
            // Read Map Name
            _filmStream.SeekTo(0x24C);
            _filmHeader.MapName = _filmStream.ReadAscii();
            #endregion
            #region Mathcmaking Playlist Info
            // Read Matchmaking Playlist Name
            _filmStream.SeekTo(0x472);
            _filmHeader.MatchmakingPlaylistName = _filmStream.ReadUTF16();
            #endregion
        }

        /// <summary>
        /// Load MachineID Table
        /// </summary>
        public void LoadMachineIDTable()
        {
            _filmMachineIDs = new List<byte[]>();

            _filmStream.SeekTo(0xE7C4);
            bool contReadingMachineIDTable = true;
            while (contReadingMachineIDTable)
            {
                byte[] machineID = new byte[0x06];
                _filmStream.ReadBlock(machineID, 0, 0x06);

                if (machineID[0] == 0x00 &&
                    machineID[1] == 0x00 &&
                    machineID[2] == 0x00)
                    contReadingMachineIDTable = false;
                else
                    _filmMachineIDs.Add(machineID);
            }
        }

        /// <summary>
        /// Load Player Table
        /// </summary>
        public void LoadPlayerTable()
        {
            _filmPlayers = new List<Player>();

            bool contReadingPlayerTable = true;
            int playerTableIndex = 0;
            while (contReadingPlayerTable)
            {
                long currentEntryOffset = _filmPlayerTable.PlayerTableStartPosition + (_filmPlayerTable.PlayerTableEntryLength * playerTableIndex);

                _filmStream.SeekTo(currentEntryOffset + 0x0F);
                if (String.IsNullOrWhiteSpace(_filmStream.ReadUTF16()))
                    contReadingPlayerTable = false;
                else
                {
                    Player player = new Player();
                    player.SpartanProperties = new SpartanProperties();
                    player.EliteProperties = new EliteProperties();
                    player.PlayerVisuals = new Visuals();

                    #region Player
                    player.EntryIndex = playerTableIndex;
                    player.EntryStartLocation = currentEntryOffset;

                    // Read Gamertag
                    _filmStream.SeekTo(currentEntryOffset + 0x0F);
                    player.Gamertag = _filmStream.ReadUTF16();

                    // Read Species
                    _filmStream.SeekTo(currentEntryOffset + 0x33);
                    player.Species = (Species)_filmStream.ReadByte();

                    // Read ServiceTag
                    _filmStream.SeekTo(currentEntryOffset + 0x45);
                    player.ServiceTag = _filmStream.ReadUTF16();

                    player.MachineID = _filmMachineIDs[playerTableIndex];
                    #endregion
                    #region Elite Properties
                    _filmStream.SeekTo(currentEntryOffset + 0x41);
                    player.EliteProperties.Helmet = (EliteHelmet)_filmStream.ReadByte();
                    player.EliteProperties.LeftShoulder = (EliteShoulders)_filmStream.ReadByte();
                    player.EliteProperties.RightShoulder = (EliteShoulders)_filmStream.ReadByte();
                    player.EliteProperties.Chest = (EliteChest)_filmStream.ReadByte();
                    #endregion
                    #region Spartan Properties
                    _filmStream.SeekTo(currentEntryOffset + 0x2F);
                    player.SpartanProperties.Gender = (SpartanGender)_filmStream.ReadByte();

                    _filmStream.SeekTo(currentEntryOffset + 0x3D);
                    player.SpartanProperties.Helmet = (SpartanHelmet)_filmStream.ReadByte();
                    player.SpartanProperties.LeftShoulder = (SpartanShoulder)_filmStream.ReadByte();
                    player.SpartanProperties.RightShoulder = (SpartanShoulder)_filmStream.ReadByte();
                    player.SpartanProperties.Chest = (SpartanChest)_filmStream.ReadByte();
                    #endregion
                    #region Visuals
                    _filmStream.SeekTo(currentEntryOffset + 0xFA);
                    player.PlayerVisuals.Team = (Teams)_filmStream.ReadByte();

                    _filmStream.SeekTo(currentEntryOffset + 0x30);
                    player.PlayerVisuals.ArmourColourPrimary = (Colours)_filmStream.ReadByte();
                    player.PlayerVisuals.ArmourColourSecondary = (Colours)_filmStream.ReadByte();
                    player.PlayerVisuals.ArmourColourDetail = (Colours)_filmStream.ReadByte();

                    _filmStream.SeekTo(currentEntryOffset + 0x35);
                    player.PlayerVisuals.EmblemForeground = (EmblemForeground)_filmStream.ReadByte();
                    player.PlayerVisuals.EmblemBackground = (EmblemBackground)_filmStream.ReadByte();
                    player.PlayerVisuals.EmblemForegroundToggle = (EmblemForegroundToggle)_filmStream.ReadByte();

                    player.PlayerVisuals.EmblemColourForeground = (Colours)_filmStream.ReadByte();
                    player.PlayerVisuals.EmblemColourBackground = (Colours)_filmStream.ReadByte();
                    #endregion

                    // Add Player to List
                    _filmPlayers.Add(player);

                    playerTableIndex++;
                }
            }
        }

        /// <summary>
        /// Load the film Footer
        /// </summary>
        public void LoadFooter()
        {
            _filmFooter = new Footer();

            #region Gamertype
            byte[] gametype = new byte[_filmFooter.GametypeSize];
            _filmStream.SeekTo(_filmFooter.GametypeLocation);
            _filmStream.ReadBlock(gametype, 0, (int)_filmFooter.GametypeSize);
            _filmFooter.Gametype = gametype;
            #endregion

            #region Usermap
            byte[] usermap = new byte[_filmFooter.UsermapSize];
            _filmStream.SeekTo(_filmFooter.UsermapLocation);
            _filmStream.ReadBlock(usermap, 0, (int)_filmFooter.UsermapSize);
            _filmFooter.Usermap = usermap;
            #endregion
        }
        #endregion

        #region Update Code
        /// <summary>
        /// Update the film's Header, MachineIDTable, PlayerTable and Footer
        /// </summary>
        public void Update()
        {
            UpdateHeader();
            UpdateMachineIDTable();
            UpdatePlayerTable();
            UpdateFooter();
        }

        /// <summary>
        /// Update the film's Header
        /// </summary>
        public void UpdateHeader()
        {
            #region Creation and Modification
            // Write Creation Date
            _filmStream.SeekTo(0x114);
            _filmStream.WriteInt32(_filmHeader.CreationDate);

            // Write Creation Author
            _filmStream.SeekTo(0xE8);
            _filmStream.WriteAscii(_filmHeader.CreationAuthor);
            #endregion
            #region Film Info
            // Write Film Name
            _filmStream.SeekTo(0x48);
            _filmStream.WriteUTF16(_filmHeader.FilmName);

            // Write Film Description
            _filmStream.SeekTo(0x67);
            _filmStream.WriteUTF16(_filmHeader.FilmDescription);

            // Write Film Engine Build String 1
            _filmStream.SeekTo(0x15C);
            _filmStream.WriteAscii(_filmHeader.EngineBuildString1);

            // Write Film Engine Build String 2
            _filmStream.SeekTo(0x198);
            _filmStream.WriteAscii(_filmHeader.EngineBuildString2);

            // Write Film InfoString
            _filmStream.SeekTo(0x220);
            _filmStream.WriteAscii(_filmHeader.InfoString);
            #endregion
            #region GametypeUsermapInfo
            // Write Gametype Name
            _filmStream.SeekTo(0x4DC);
            _filmStream.WriteUTF16(_filmHeader.GametypeName);

            // Write Gametype Description
            _filmStream.SeekTo(0x4FB);
            _filmStream.WriteUTF16(_filmHeader.GametypeDescription);

            // Write Gametype Author
            _filmStream.SeekTo(0x57B);
            _filmStream.WriteUTF16(_filmHeader.GametypeAuthor);

            // Write Usermap Name
            _filmStream.SeekTo(0x738);
            _filmStream.WriteUTF16(_filmHeader.GametypeAuthor);

            // Write Usermap Description
            _filmStream.SeekTo(0x757);
            _filmStream.WriteUTF16(_filmHeader.GametypeAuthor);

            // Write Usermap Author
            _filmStream.SeekTo(0x7D7);
            _filmStream.WriteUTF16(_filmHeader.GametypeAuthor);
            #endregion
            #region RawMapData
            // Write Map Name
            _filmStream.SeekTo(0x24C);
            _filmStream.WriteAscii(_filmHeader.MapName);
            #endregion
            #region Mathcmaking Playlist Info
            // Write Matchmaking Playlist Name
            _filmStream.SeekTo(0x472);
            _filmStream.WriteUTF16(_filmHeader.MatchmakingPlaylistName);
            #endregion
        }

        /// <summary>
        /// Update the film's MachineID Table
        /// </summary>
        public void UpdateMachineIDTable()
        {
            _filmStream.SeekTo(_filmMachineIDTable.MachineIDTableStartPosition);
            for (int i = 0; i < _filmMachineIDs.Count; i++)
                _filmStream.WriteBlock(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            _filmStream.WriteByte(0x00);

            _filmStream.SeekTo(_filmMachineIDTable.MachineIDTableStartPosition);
            foreach (byte[] machineID in _filmMachineIDs)
                _filmStream.WriteBlock(machineID);
        }

        /// <summary>
        /// Update the film's Player Table
        /// </summary>
        public void UpdatePlayerTable()
        {
            foreach (Player player in _filmPlayers)
            {
                long EntryStartLocation = player.EntryStartLocation;
                long EntryIndex = player.EntryIndex;

                #region Player
                _filmStream.SeekTo(EntryStartLocation + 0x0F);
                _filmStream.WriteUTF16(player.Gamertag);

                _filmStream.SeekTo(EntryStartLocation + 0x45);
                _filmStream.WriteUTF16(player.ServiceTag);

                _filmStream.SeekTo(EntryStartLocation + 0xD7);
                _filmStream.WriteUTF16(player.Gamertag);
                #endregion
                #region Elite Properties
                _filmStream.SeekTo(EntryStartLocation + 0x41);
                _filmStream.WriteByte((byte)player.EliteProperties.Helmet);
                _filmStream.WriteByte((byte)player.EliteProperties.LeftShoulder);
                _filmStream.WriteByte((byte)player.EliteProperties.RightShoulder);
                _filmStream.WriteByte((byte)player.EliteProperties.Chest);
                #endregion
                #region Spartan Properties
                _filmStream.SeekTo(EntryStartLocation + 0x2F);
                _filmStream.WriteByte((byte)player.SpartanProperties.Gender);

                _filmStream.SeekTo(EntryStartLocation + 0x3D);
                _filmStream.WriteByte((byte)player.SpartanProperties.Helmet);
                _filmStream.WriteByte((byte)player.SpartanProperties.LeftShoulder);
                _filmStream.WriteByte((byte)player.SpartanProperties.RightShoulder);
                _filmStream.WriteByte((byte)player.SpartanProperties.Chest);
                #endregion
                #region Visuals
                _filmStream.SeekTo(EntryStartLocation + 0xFA);
                _filmStream.WriteByte((byte)player.PlayerVisuals.Team);

                _filmStream.SeekTo(EntryStartLocation + 0x30);
                _filmStream.WriteByte((byte)player.PlayerVisuals.ArmourColourPrimary);
                _filmStream.WriteByte((byte)player.PlayerVisuals.ArmourColourSecondary);
                _filmStream.WriteByte((byte)player.PlayerVisuals.ArmourColourDetail);

                _filmStream.SeekTo(EntryStartLocation + 0x35);
                _filmStream.WriteByte((byte)player.PlayerVisuals.EmblemForeground);
                _filmStream.WriteByte((byte)player.PlayerVisuals.EmblemBackground);
                _filmStream.WriteByte((byte)player.PlayerVisuals.EmblemForegroundToggle);

                _filmStream.WriteByte((byte)player.PlayerVisuals.EmblemColourForeground);
                _filmStream.WriteByte((byte)player.PlayerVisuals.EmblemColourBackground);
                #endregion
            }
        }

        /// <summary>
        /// Update the film's Footer
        /// </summary>
        public void UpdateFooter()
        {
            if (_filmFooter.Gametype.Length > _filmFooter.GametypeSize)
                throw new Exception("Gamertype to inject is too long. Can only be 0x'" + _filmFooter.GametypeSize.ToString("X") + "' in length");
            
            if (_filmFooter.Usermap.Length > _filmFooter.UsermapSize)
                throw new Exception("Usermap to inject is too long. Can only be 0x'" + _filmFooter.UsermapSize.ToString("X") + "' in length");

            // Inject Gametype
            _filmStream.SeekTo(_filmFooter.GametypeLocation);
            _filmStream.WriteBlock(_filmFooter.Gametype);

            // Inject Usermap
            _filmStream.SeekTo(_filmFooter.UsermapLocation);
            _filmStream.WriteBlock(_filmFooter.Usermap);
        }
        #endregion

        /// <summary>
        /// Close the Stream
        /// </summary>
        public void Close()
        {
            _filmStream.Close();
        }
        /// <summary>
        /// Check if the Film is Valid
        /// </summary>
        /// <returns></returns>
        public bool IsValidFilm()
        {
            _filmStream.SeekTo(0x0E);
            string _blfName = _filmStream.ReadAscii();

            if (_blfName == "halo 3 saved film")
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

            long currentEntryOffset = _filmPlayerTable.PlayerTableStartPosition + (_filmPlayerTable.PlayerTableEntryLength * entryIndex);

            _filmStream.SeekTo(currentEntryOffset + 0x0F);
            if (String.IsNullOrWhiteSpace(_filmStream.ReadUTF16()))
            {
                Close();
                throw new Exception("Player Entry Index Doesn't Exist");
            }
            else
            {
                byte[] tableEntry = new byte[_filmPlayerTable.PlayerTableEntryLength];
                _filmStream.SeekTo(currentEntryOffset);
                _filmStream.ReadBlock(tableEntry, 0, (int)_filmPlayerTable.PlayerTableEntryLength);

                File.WriteAllBytes(filePath.Replace("/", ""), tableEntry);

                Close();
            }
        }
    }
}