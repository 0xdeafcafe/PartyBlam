﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PartyBlam.IO;

namespace PartyBlam.Blam
{
    public class RawBLF
    {
        private EndianStream _blfStream;
        private IList<BLFChunk> _blfChunks;

        #region Public Access
        public Stream BLFStream
        {
            get { return _blfStream.BaseStream; }
        }
        public IList<BLFChunk> BLFChunks
        {
            get { return _blfChunks; }
            set { _blfChunks = value; }
        }
        #endregion

        #region Class Declaration
        public class BLFChunk
        {
            public string ChunkMagic { get; set; }
            public Int32 ChunkMagicIdent { get; set; }

            public Int32 ChunkLength { get; set; }

            public byte[] ChunkData { get; set; }
        }
        #endregion

        /// <summary>
        /// Initalize a new instance of a BLF container
        /// </summary>
        /// <param name="blfPath">Path to the BLF container.</param>
        public RawBLF(string blfPath) { Initalize(new FileStream(blfPath, FileMode.OpenOrCreate, FileAccess.ReadWrite)); }
        /// <summary>
        /// Initalize a new instance of a BLF container
        /// </summary>
        /// <param name="blfStream">Stream of the BLF container.</param>
        public RawBLF(Stream blfStream) { Initalize(blfStream); }
        private void Initalize(Stream blfStream)
        {
            _blfStream = new EndianStream(blfStream, Endian.BigEndian);

            if (!isValidBLF())
            {
                Close();
                throw new Exception("Invalid BLF Container!");
            }

            LoadChunkTable();
        }

        #region Loading Code
        /// <summary>
        /// Load the chunks in the BLF Table
        /// </summary>
        public void LoadChunkTable()
        {
            _blfChunks = new List<BLFChunk>();
            _blfStream.SeekTo(0x00);

            bool continueLoading = true;
            while (continueLoading)
            {
                if (_blfStream.Position >= _blfStream.Length)
                    continueLoading = false;
                else
                {
                    BLFChunk chunk = new BLFChunk();

                    chunk.ChunkMagic = _blfStream.ReadAscii(0x04);
                    _blfStream.SeekTo(_blfStream.Position - 0x04);
                    chunk.ChunkMagicIdent = _blfStream.ReadInt32();

                    chunk.ChunkLength = _blfStream.ReadInt32();

                    chunk.ChunkData = _blfStream.ReadBlock(chunk.ChunkLength - 0x08);

                    _blfChunks.Add(chunk);
                }
            }
        }
        #endregion

        #region RefreshCode
        /// <summary>
        /// Refresh the relative settings. This is called before a Table Update anyway.
        /// </summary>
        public void RefreshRelativeChunkData()
        {
            foreach (BLFChunk chunk in _blfChunks)
                chunk.ChunkLength = chunk.ChunkData.Length + 0x08;
        }
        #endregion

        #region Update Code
        /// <summary>
        /// Update the chunks in the BLF Table
        /// </summary>
        public void UpdateChunkTable()
        {
            RefreshRelativeChunkData();

            int totalLength = 0;
            foreach (BLFChunk chunk in _blfChunks)
                totalLength += chunk.ChunkLength;

            _blfStream.BaseStream.SetLength(totalLength);
            _blfStream.SeekTo(0x00);
            foreach (BLFChunk chunk in _blfChunks)
            {
                _blfStream.WriteAscii(chunk.ChunkMagic);
                _blfStream.WriteInt32(chunk.ChunkLength);
                _blfStream.WriteBlock(chunk.ChunkData);
            }
        }
        #endregion

        /// <summary>
        /// Add new a BLF chunk to the chunk table
        /// </summary>
        /// <param name="magic">The magic (has to be 4 chars long)</param>
        /// <param name="content">The content of the BLF Chunk</param>
        /// <param name="chunkToInsertAfter">The BLF chunk to insert the new chunk after</param>
        public void AddBLFChunk(string magic, byte[] content, BLFChunk chunkToInsertAfter)
        {
            BLFChunk chunk = new BLFChunk();
            chunk.ChunkMagic = magic;
            chunk.ChunkData = content;
            chunk.ChunkLength = content.Length + 0x08;
            chunk.ChunkMagicIdent = Convert.ToInt32(magic);

            // Checks
            if (chunk.ChunkMagic.Length != 4)
                throw new Exception("Chunk Magic has to be 4 chars long");

            if (_blfChunks[_blfChunks.Count] == chunkToInsertAfter)
                throw new Exception("You can't insert a chunk after the header, you nub.");

            int index = 1;
            foreach (BLFChunk chunkk in _blfChunks)
                if (chunkk == chunkToInsertAfter)
                    break;
                else
                    index++;

            // Add chunk
            _blfChunks.Insert(index, chunk);

            RefreshRelativeChunkData();
        }
        /// <summary>
        /// Add new a BLF chunk to the chunk table
        /// </summary>
        /// <param name="magic">The magic (has to be 4 chars long)</param>
        /// <param name="content">The content of the BLF Chunk</param>
        /// <param name="chunkIndex">The index of the chunk to insert the new chunk behind</param>
        public void AddBLFChunk(string magic, byte[] content, int chunkIndex)
        {
            try
            {
                BLFChunk chunk = _blfChunks[chunkIndex];

                AddBLFChunk(magic, content, chunk);
            }
            catch
            {
                throw new Exception("Chunk doesn't exist bro.");
            }
        }
        
        private bool isValidBLF()
        {
            _blfStream.SeekTo(0x00);
            string magic = _blfStream.ReadAscii(0x04);

            if (magic == "_blf")
                return true;
            else
                return false;
        }
        public void Close()
        {
            _blfStream.Close();
        }
    }
}
