//---------------------------------------------------------------------------- 
//
// <copyright file="ManagedIStream.cs" company="Microsoft">
//    Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright> 
//
// Description: 
//              Implements an IStream component initialized from an 
//              object of type System.IO.Stream.
// 
// History:
//  03/28/2004: [....]: Initial implementation
//---------------------------------------------------------------------------

using System;
using System.IO;
using System.Runtime.InteropServices;               // for Marshal, COMException, etc. 
using System.Runtime.InteropServices.ComTypes;      // for IStream
using System.Security;                              // for SecurityCritical

namespace MS.Internal.IO.Packaging
{
    // The class ManagedIStream is not COM-visible. Its purpose is to be able to invoke COM interfaces 
    // from managed code rather than the contrary.
    class ManagedIStream : IStream
    {
        private enum STGTY
        {
            STGTY_STORAGE = 1,
            STGTY_STREAM = 2,
            STGTY_LOCKBYTES = 3,
            STGTY_PROPERTY = 4
        }

        [Flags]
        private enum STGM
        {
            STGM_READ = 0x0,
            STGM_WRITE = 0x1,
            STGM_READWRITE = 0x2,
            STGM_SHARE_DENY_NONE = 0x40,
            STGM_SHARE_DENY_READ = 0x30,
            STGM_SHARE_DENY_WRITE = 0x20,
            STGM_SHARE_EXCLUSIVE = 0x10,
            STGM_PRIORITY = 0x40000,
            STGM_CREATE = 0x1000,
            STGM_CONVERT = 0x20000,
            STGM_FAILIFTHERE = 0x0,
            STGM_DIRECT = 0x0,
            STGM_TRANSACTED = 0x10000,
            STGM_NOSCRATCH = 0x100000,
            STGM_NOSNAPSHOT = 0x200000,
            STGM_SIMPLE = 0x8000000,
            STGM_DIRECT_SWMR = 0x400000,
            STGM_DELETEONRELEASE = 0x4000000
        }

        /// <summary> 
        /// Constructor
        /// </summary> 
        internal ManagedIStream(Stream ioStream)
        {
            if (ioStream == null)
            {
                throw new ArgumentNullException("ioStream");
            }
            _ioStream = ioStream;
        }

        /// <summary> 
        /// Read at most bufferSize bytes into buffer and return the effective
        /// number of bytes read in bytesReadPtr (unless null). 
        /// </summary>
        /// <remarks>
        /// mscorlib disassembly shows the following MarshalAs parameters
        /// void Read([Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] byte[] pv, int cb, IntPtr pcbRead); 
        /// This means marshaling code will have found the size of the array buffer in the parameter bufferSize.
        /// </remarks> 
        ///<securitynote> 
        ///     Critical: calls Marshal.WriteInt32 which LinkDemands, takes pointers as input
        ///</securitynote> 
        [SecurityCritical]
        void IStream.Read(Byte[] buffer, Int32 bufferSize, IntPtr bytesReadPtr)
        {
            Int32 bytesRead = _ioStream.Read(buffer, 0, (int)bufferSize);
            if (bytesReadPtr != IntPtr.Zero)
            {
                Marshal.WriteInt32(bytesReadPtr, bytesRead);
            }
        }

        /// <summary>
        /// Move the stream pointer to the specified position.
        /// </summary> 
        /// <remarks>
        /// System.IO.stream supports searching past the end of the stream, like 
        /// OLE streams. 
        /// newPositionPtr is not an out parameter because the method is required
        /// to accept NULL pointers. 
        /// </remarks>
        ///<securitynote>
        ///     Critical: calls Marshal.WriteInt64 which LinkDemands, takes pointers as input
        ///</securitynote> 
        [SecurityCritical]
        void IStream.Seek(Int64 offset, Int32 origin, IntPtr newPositionPtr)
        {
            // The operation will generally be I/O bound, so there is no point in
            // eliminating the following switch by playing on the fact that
            // System.IO uses the same integer values as IStream for SeekOrigin.
            long position = _ioStream.Seek(offset, (SeekOrigin)origin);

            // Dereference newPositionPtr and assign to the pointed location.
            if (newPositionPtr != IntPtr.Zero)
            {
                Marshal.WriteInt64(newPositionPtr, position);
            }
        }

        /// <summary> 
        /// Sets stream's size.
        /// </summary>
        void IStream.SetSize(Int64 libNewSize)
        {
            _ioStream.SetLength(libNewSize);
        }

        /// <summary>
        /// Obtain stream stats. 
        /// </summary>
        /// <remarks>
        /// STATSG has to be qualified because it is defined both in System.Runtime.InteropServices and
        /// System.Runtime.InteropServices.ComTypes. 
        /// The STATSTG structure is shared by streams, storages and byte arrays. Members irrelevant to streams
        /// or not available from System.IO.Stream are not returned, which leaves only cbSize and grfMode as 
        /// meaningful and available pieces of information. 
        /// grfStatFlag is used to indicate whether the stream name should be returned and is ignored because
        /// this information is unavailable. 
        /// </remarks>
        void IStream.Stat(out System.Runtime.InteropServices.ComTypes.STATSTG streamStats, int grfStatFlag)
        {
            streamStats = new System.Runtime.InteropServices.ComTypes.STATSTG();
            streamStats.type = (int)STGTY.STGTY_STREAM;
            streamStats.cbSize = _ioStream.Length;

            // Return access information in grfMode.
            streamStats.grfMode = 0; // default value for each flag will be false 
            if (_ioStream.CanRead && _ioStream.CanWrite)
            {
                streamStats.grfMode |= (int)STGM.STGM_READWRITE;
            }
            else if (_ioStream.CanRead)
            {
                streamStats.grfMode |= (int)STGM.STGM_READ;
            }
            else if (_ioStream.CanWrite)
            {
                streamStats.grfMode |= (int)STGM.STGM_WRITE;
            }
            else
            {
                // A stream that is neither readable nor writable is a closed stream. 
                // Note the use of an exception that is known to the interop marshaller 
                // (unlike ObjectDisposedException).
                throw new IOException();
            }
        }

        /// <summary> 
        /// Write at most bufferSize bytes from buffer.
        /// </summary> 
        ///<securitynote> 
        ///     Critical: calls Marshal.WriteInt32 which LinkDemands, takes pointers as input
        ///</securitynote> 
        [SecurityCritical]
        void IStream.Write(Byte[] buffer, Int32 bufferSize, IntPtr bytesWrittenPtr)
        {
            _ioStream.Write(buffer, 0, bufferSize);
            if (bytesWrittenPtr != IntPtr.Zero)
            {
                // If fewer than bufferSize bytes had been written, an exception would 
                // have been thrown, so it can be assumed we wrote bufferSize bytes.
                Marshal.WriteInt32(bytesWrittenPtr, bufferSize);
            }
        }

        #region Unimplemented methods 
        /// <summary>
        /// Create a clone. 
        /// </summary> 
        /// <remarks>
        /// Not implemented. 
        /// </remarks>
        void IStream.Clone(out IStream streamCopy)
        {
            streamCopy = null;
            throw new NotSupportedException();
        }

        /// <summary>
        /// Read at most bufferSize bytes from the receiver and write them to targetStream. 
        /// </summary>
        /// <remarks>
        /// Not implemented.
        /// </remarks> 
        void IStream.CopyTo(IStream targetStream, Int64 bufferSize, IntPtr buffer, IntPtr bytesWrittenPtr)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Commit changes.
        /// </summary>
        /// <remarks> 
        /// Only relevant to transacted streams.
        /// </remarks> 
        void IStream.Commit(Int32 flags)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Lock at most byteCount bytes starting at offset. 
        /// </summary>
        /// <remarks> 
        /// Not supported by System.IO.Stream. 
        /// </remarks>
        void IStream.LockRegion(Int64 offset, Int64 byteCount, Int32 lockType)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Undo writes performed since last Commit. 
        /// </summary> 
        /// <remarks>
        /// Relevant only to transacted streams. 
        /// </remarks>
        void IStream.Revert()
        {
            throw new NotSupportedException();
        }

        /// <summary> 
        /// Unlock the specified region.
        /// </summary> 
        /// <remarks>
        /// Not supported by System.IO.Stream.
        /// </remarks>
        void IStream.UnlockRegion(Int64 offset, Int64 byteCount, Int32 lockType)
        {
            throw new NotSupportedException();
        }
        #endregion Unimplemented methods

        #region Fields
        private Stream _ioStream;
        #endregion Fields
    }
}

// File provided for Reference Use Only by Microsoft Corporation (c) 2007.
// Copyright (c) Microsoft Corporation. All rights reserved.