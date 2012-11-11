using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using PartyBlam.IO;

namespace PartyBlam.Blam.Halo3
{
    public class Usermap
    {
        private EndianStream _forgeStream;
        private Header _forgeHeader;
        private IList<ItemPlacementChunk> _forgePlacementChunks;
        private Tag _forgeMapTags;
        private IList<TagEntry> _forgeTagEntries;
        
        #region Public Access
        public Stream ForgeStream
        {
            get { return _forgeStream.BaseStream; }
        }
        public Header ForgeHeader
        {
            get { return _forgeHeader; }
            set { _forgeHeader = value; }
        }
        public IList<ItemPlacementChunk> ForgePlacementChunks
        {
            get { return _forgePlacementChunks; }
            set { _forgePlacementChunks = value; }
        }
        public Tag ForgeMapTags
        {
            get { return _forgeMapTags; }
        }
        public IList<TagEntry> ForgeTagEntries
        {
            get { return _forgeTagEntries; }
            set { _forgeTagEntries = value; }
        }
        #endregion

        #region Class Declaration
        public class Header
        {
            public Int32 CreationDate { get; set; }
            public string CreationVarientName { get; set; }
            public string CreationVarientDescription { get; set; }
            public string CreationVarientAuthor { get; set; }

            public Int32 ModificationDate { get; set; }
            public string VarientName { get; set; }
            public string VarientDescription { get; set; }
            public string VarientAuthor { get; set; }

            public Int32 MapID { get; set; }
            public Int16 SpawnedObjectCount { get; set; }

            public float WorldBoundsXMin { get; set; }
            public float WorldBoundsXMax { get; set; }
            public float WorldBoundsYMin { get; set; }
            public float WorldBoundsYMax { get; set; }
            public float WorldBoundsZMin { get; set; }
            public float WorldBoundsZMax { get; set; }

            public float MaximiumBudget { get; set; }
            public float CurrentBudget { get; set; }
        }

        public class Tag
        {
            public string MapName { get; set; }
            public Int32 MapID { get; set; }
            public List<MapTag> Tags { get; set; }

            public MapTag SearchTags(Int32 datumIndex)
            {
                foreach (MapTag tag in Tags)
                    if (tag.DatumIndex == datumIndex)
                        return tag;

                return null;
            }
        }
        #region MapTag Innards
        public class MapTag
        {
            public string TagClass { get; set; }
            public string TagPath { get; set; }
            public int TagIndex { get; set; }
            public Int32 DatumIndex { get; set; }
        }
        #endregion

        public class ItemPlacementChunk
        {
            public ItemPlacementChunk(EndianStream stream)
            {
                Offset = stream.Position;
                ChunkType = (ItemChunkType)stream.ReadInt16();
                stream.SeekTo(stream.Position + 0x0A);
                TagIndex = stream.ReadInt32();
                SpawnCoords = new ItemSpawnCoords()
                {
                    X = stream.ReadFloat(),
                    Y = stream.ReadFloat(),
                    Z = stream.ReadFloat(),
                    Yaw = stream.ReadFloat(),
                    Pitch = stream.ReadFloat(),
                    Roll = stream.ReadFloat()
                };
                stream.SeekTo(stream.Position + 0x16);
                stream.ReadByte();
                Team = stream.ReadByte();
                SpareClips = stream.ReadByte();
                RespawnTime = stream.ReadByte();
                stream.SeekTo(stream.Position + 0x12);
            }

            public void Update(EndianStream stream)
            {
                stream.SeekTo(Offset);
                stream.WriteInt16((Int16)ChunkType);
                stream.SeekTo(stream.Position + 0x0A);
                stream.WriteInt32(TagIndex);
                stream.WriteFloat(SpawnCoords.X);
                stream.WriteFloat(SpawnCoords.Y);
                stream.WriteFloat(SpawnCoords.Z);
                stream.WriteFloat(SpawnCoords.Pitch);
                stream.WriteFloat(SpawnCoords.Yaw);
                stream.WriteFloat(SpawnCoords.Roll);
                stream.SeekTo(stream.Position + 0x16);
                stream.Skip(0x01);
                stream.WriteByte(Team);
                stream.WriteByte(SpareClips);
                stream.WriteByte(RespawnTime);
                stream.SeekTo(stream.Position + 0x12);
            }

            public long Offset { get; set; }
            public ItemChunkType ChunkType { get; set; }
            public int TagIndex { get; set; }
            public ItemSpawnCoords SpawnCoords { get; set; }
            public byte Flags { get; set; }
            public byte Team { get; set; }
            public byte SpareClips { get; set; }
            public byte RespawnTime { get; set; }
            public bool[] Bitmask { get; set; }
            public TagEntry Entry { get; set; }

            public enum ItemChunkType : short
            {
                NoIdea = 0x00,
                Added = 0x03,
                PlayerSpawn = 0x09,
                Reserved = 0x29,
                Original = 0x89,
                Edited = 0x8B,
            }
            public class ItemSpawnCoords
            {
                public float X { get; set; }
                public float Y { get; set; }
                public float Z { get; set; }
                public float Yaw { get; set; }
                public float Pitch { get; set; }
                public float Roll { get; set; }
            }
        }

        public class TagEntry
        {
            public TagEntry(EndianStream stream, Tag mapTags)
            {
                PlacedItems = new List<ItemPlacementChunk>();
                Offset = stream.Position;
                Ident = stream.ReadInt32();
                Tag = mapTags.SearchTags(Ident);
                RunTimeMinimium = stream.ReadByte();
                RunTimeMaximium = stream.ReadByte();
                CountOnMap = stream.ReadByte();
                DesignTimeMaximium = stream.ReadByte();
                Cost = stream.ReadFloat();
            }
            public void Update(EndianStream stream)
            {
                stream.SeekTo(Offset);
                stream.WriteInt32(Ident);
                stream.WriteByte(RunTimeMinimium);
                stream.WriteByte(RunTimeMaximium);
                stream.WriteByte(CountOnMap);
                stream.WriteByte(DesignTimeMaximium);
                stream.WriteFloat(Cost);
            }

            public List<ItemPlacementChunk> PlacedItems { get; set; }
            public long Offset { get; set; }
            public Int32 Ident { get; set; }
            public MapTag Tag { get; set; }
            public byte RunTimeMinimium { get; set; }
            public byte RunTimeMaximium { get; set; }
            public byte CountOnMap { get; set; }
            public byte DesignTimeMaximium { get; set; }
            public float Cost { get; set; }
        }
        #endregion

        /// <summary>
        /// Initalize new instance of the Halo 3 Usermap
        /// </summary>
        /// <param name="fileLocation">Path to the Halo 3 'sandbox.map' extracted from a Container file.</param>
        public Usermap(string fileLocation) { Initalize(new MemoryStream(File.ReadAllBytes(fileLocation))); }
        /// <summary>
        /// Initalize new instance of the Halo 3 Usermap
        /// </summary>
        /// <param name="fileStream">Stream of a Halo 3 'sandbox.map' extracted from a Container file.</param>
        public Usermap(Stream fileStream) { Initalize(fileStream); }
        private void Initalize(Stream finalStream)
        {
            _forgeStream = new EndianStream(finalStream, Endian.BigEndian);

            if (!IsValidUsermap())
            {
                Close();
                throw new Exception("Invalid Halo 3 Usermap!");
            }

            LoadHeader();
            LoadTags();
            LoadItemPlacementChunks();
            LoadTagEntry();
            ApplyTagIndexes();
        }

        #region Loading Code
        /// <summary>
        /// Load the usermap's Header
        /// </summary>
        public void LoadHeader()
        {
            _forgeHeader = new Header();

            _forgeStream.SeekTo(0x42);
            _forgeHeader.CreationDate = _forgeStream.ReadInt32();
            _forgeStream.SeekTo(0x48);
            _forgeHeader.CreationVarientName = _forgeStream.ReadUTF16(0x1F);
            _forgeStream.SeekTo(0x68);
            _forgeHeader.CreationVarientDescription = _forgeStream.ReadAscii(0x80);
            _forgeStream.SeekTo(0xE8);
            _forgeHeader.CreationVarientAuthor = _forgeStream.ReadAscii(0x13);

            _forgeStream.SeekTo(0x114);
            _forgeHeader.ModificationDate = _forgeStream.ReadInt32();
            _forgeStream.SeekTo(0x150);
            _forgeHeader.VarientName = _forgeStream.ReadUTF16(0x1F);
            _forgeStream.SeekTo(0x170);
            _forgeHeader.VarientDescription = _forgeStream.ReadAscii(0x80);
            _forgeStream.SeekTo(0x1F0);
            _forgeHeader.VarientAuthor = _forgeStream.ReadAscii(0x13);

            _forgeStream.SeekTo(0x228);
            _forgeHeader.MapID = _forgeStream.ReadInt32();

            _forgeStream.SeekTo(0x246);
            _forgeHeader.SpawnedObjectCount = _forgeStream.ReadInt16();

            _forgeStream.SeekTo(0x24C);
            _forgeHeader.WorldBoundsXMin = _forgeStream.ReadFloat();
            _forgeHeader.WorldBoundsXMax = _forgeStream.ReadFloat();
            _forgeHeader.WorldBoundsYMin = _forgeStream.ReadFloat();
            _forgeHeader.WorldBoundsYMax = _forgeStream.ReadFloat();
            _forgeHeader.WorldBoundsZMin = _forgeStream.ReadFloat();
            _forgeHeader.WorldBoundsZMax = _forgeStream.ReadFloat();

            _forgeStream.SeekTo(0x268);
            _forgeHeader.MaximiumBudget = _forgeStream.ReadFloat();
            _forgeHeader.CurrentBudget = _forgeStream.ReadFloat();
        }

        /// <summary>
        /// Load the usermap's Tags
        /// </summary>
        public void LoadTags()
        {
            // Load taglist byte[]
            byte[] taglist = RandomFunctions.ReadResource("BlamCon.Resources.h3_" + _forgeHeader.MapID.ToString() + "");

            // Decompress Taglist
            taglist = RandomFunctions.GZip.Decompress(taglist);

            // Deseralize Taglist
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string json = RandomFunctions.ByteArrayToString(taglist, RandomFunctions.EncodingType.ASCII);
            _forgeMapTags = jss.Deserialize<Tag>(json);
        }

        /// <summary>
        /// Load the usermap's Placement Chunks
        /// </summary>
        public void LoadItemPlacementChunks()
        {
            _forgeStream.SeekTo(0x279);
            _forgePlacementChunks = new List<ItemPlacementChunk>();
            for (int chunk = 0; chunk < 640; chunk++)
            {
                ItemPlacementChunk placedChunk = new ItemPlacementChunk(_forgeStream);
                _forgePlacementChunks.Add(placedChunk);
            }
        }

        /// <summary>
        /// Load the usermap's Tag Entries
        /// </summary>
        public void LoadTagEntry()
        {
            _forgeTagEntries = new List<TagEntry>();
            _forgeStream.SeekTo(0xD494);
            for (int entry = 0; entry < 0x100; entry++)
            {
                TagEntry tagEntry = new TagEntry(_forgeStream, _forgeMapTags);
                if (tagEntry.Tag != null)
                {
                    tagEntry.Tag.TagIndex = entry;
                }
                _forgeTagEntries.Add(tagEntry);
            }
        }

        /// <summary>
        /// Load the usermap's Tag Indexes
        /// </summary>
        public void ApplyTagIndexes()
        {
            foreach (TagEntry tagEntry in _forgeTagEntries)
                if (tagEntry.CountOnMap > 0)
                    foreach (ItemPlacementChunk placedChunk in _forgePlacementChunks)
                        if (placedChunk.TagIndex == tagEntry.Tag.TagIndex)
                            tagEntry.PlacedItems.Add(placedChunk);

            for (int num = 0; num < 0x100; num++)
                for (int i = 0; i < 640; i++)
                    if (_forgePlacementChunks[i].TagIndex == num)
                        _forgePlacementChunks[i].Entry = _forgeTagEntries[num];
        }
        #endregion

        #region Update Code
        /// <summary>
        /// Update the usermap's Header, PlacementChunks and Tag Entries
        /// </summary>
        public void Update()
        {
            UpdateHeader();
            UpdateItemPlacementChunks();
            UpdateTagEntries();
        }

        /// <summary>
        /// Update the usermap's Header
        /// </summary>
        public void UpdateHeader()
        {
            _forgeStream.SeekTo(0x42);
            _forgeStream.WriteInt32(_forgeHeader.CreationDate);
            _forgeStream.SeekTo(0x48);
            _forgeStream.WriteUTF16(_forgeHeader.CreationVarientName);
            _forgeStream.SeekTo(0x68);
            _forgeStream.WriteAscii(_forgeHeader.CreationVarientDescription);
            _forgeStream.SeekTo(0xE8);
            _forgeStream.WriteAscii(_forgeHeader.CreationVarientAuthor);

            _forgeStream.SeekTo(0x114);
            _forgeStream.WriteInt32(_forgeHeader.ModificationDate);
            _forgeStream.SeekTo(0x150);
            _forgeStream.WriteUTF16(_forgeHeader.VarientName);
            _forgeStream.SeekTo(0x170);
            _forgeStream.WriteAscii(_forgeHeader.VarientDescription);
            _forgeStream.SeekTo(0x1F0);
            _forgeStream.WriteAscii(_forgeHeader.VarientAuthor);

            _forgeStream.SeekTo(0x228);
            _forgeStream.WriteInt32(_forgeHeader.MapID);

            _forgeStream.SeekTo(0x246);
            _forgeStream.WriteInt16(_forgeHeader.SpawnedObjectCount);

            _forgeStream.SeekTo(0x24C);
            _forgeStream.WriteFloat(_forgeHeader.WorldBoundsXMin);
            _forgeStream.WriteFloat(_forgeHeader.WorldBoundsXMax);
            _forgeStream.WriteFloat(_forgeHeader.WorldBoundsYMin);
            _forgeStream.WriteFloat(_forgeHeader.WorldBoundsYMax);
            _forgeStream.WriteFloat(_forgeHeader.WorldBoundsZMin);
            _forgeStream.WriteFloat(_forgeHeader.WorldBoundsZMax);

            _forgeStream.SeekTo(0x268);
            _forgeStream.WriteFloat(_forgeHeader.MaximiumBudget);
            _forgeStream.WriteFloat(_forgeHeader.CurrentBudget);
        }

        /// <summary>
        /// Update the usermap's ItemPlacementChunk
        /// </summary>
        public void UpdateItemPlacementChunks()
        {
            _forgeStream.SeekTo(0x279);
            foreach (ItemPlacementChunk chunk in _forgePlacementChunks)
                chunk.Update(_forgeStream);
        }

        /// <summary>
        /// Update the usermap's Tag Entries
        /// </summary>
        public void UpdateTagEntries()
        {
            _forgeStream.SeekTo(0xD494);
            foreach (TagEntry tagEntryChunk in _forgeTagEntries)
                tagEntryChunk.Update(_forgeStream);
        }
        #endregion

        public bool IsValidUsermap()
        {
            _forgeStream.SeekTo(0x138);
            string mapV = _forgeStream.ReadAscii(0x04);

            if (mapV != "mapv")
                return false;
            else 
                return true;
        }
        public void Close()
        {
            _forgeStream.Close();
        }
    }
}