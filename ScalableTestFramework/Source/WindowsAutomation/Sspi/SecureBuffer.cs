using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace HP.ScalableTest.WindowsAutomation.Sspi
{
    /// <summary>
    /// Describes the purpose of a <see cref="SecureBuffer" />, i.e. the type of data it will store.
    /// </summary>
    internal enum SecureBufferType
    {
        Empty = 0,
        Data = 1,
        Token = 2,
        Padding = 9
    }

    /// <summary>
    /// Stores buffers to provide tokens and data to the native SSPI APIs.
    /// </summary>
    /// <remarks>
    /// This class should be used in conjunction with <see cref="SecureBufferAdapter" />,
    /// which manages marshaling array data to the managed API.
    /// </remarks>
    internal sealed class SecureBuffer
    {
        /// <summary>
        /// Gets the type of data this buffer will store.
        /// </summary>
        public SecureBufferType BufferType { get; }

        /// <summary>
        /// Gets the buffer.
        /// </summary>
        public byte[] Buffer { get; }

        /// <summary>
        /// Gets or sets the amount of the buffer actually in use,
        /// which may be less than the size of the allocated buffer.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureBuffer" /> class.
        /// </summary>
        /// <param name="buffer">The byte array to use as the buffer.</param>
        /// <param name="bufferType">A <see cref="SecureBufferType" /> indicating the type of data this buffer will store.</param>
        public SecureBuffer(byte[] buffer, SecureBufferType bufferType)
        {
            Buffer = buffer;
            BufferType = bufferType;
            Length = Buffer.Length;
        }
    }

    /// <summary>
    /// An adapter class that prepares <see cref="SecureBuffer" /> objects to be provided to native API calls.
    /// </summary>
    /// <remarks>
    /// SSPI requires custom structures to convey byte arrays along with their length and the amount of data
    /// that has actually been filled in.  This leads to a significant amount of marshaling to get the buffers
    /// back and forth to the native APIs.  All buffers must be pinned in memory, we must obtain IntPtr handles
    /// to each of the buffers, and we must provide a method for copying values back from the native structs
    /// to the "friendly" SecureBuffer class used by the rest of the library.
    /// 
    /// The typical flow is to take one or many buffers; create and fill the neccessary unmanaged structures;
    /// pin memory; acquire the IntPtr handles; let the caller access the top-level IntPtr representing
    /// the SecureBufferDescriptor, to provide to the native APIs; wait for the caller to invoke the native
    /// API; wait for the caller to invoke our Dispose; marshal back any data from the native structures
    /// (buffer write counts); release all GCHandles to unpin memory.
    /// </remarks>
    internal sealed class SecureBufferAdapter : IDisposable
    {
        private bool _disposed = false;
        private readonly List<SecureBuffer> _buffers;
        private readonly GCHandle _descriptorHandle;
        private readonly GCHandle _bufferCarrierHandle;
        private readonly GCHandle[] _bufferHandles;
        private readonly SecureBufferDescriptor _descriptor;
        private readonly SecureBufferInternal[] _bufferCarrier;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureBufferAdapter" /> class.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        public SecureBufferAdapter(SecureBuffer buffer)
            : this(new List<SecureBuffer> { buffer })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureBufferAdapter" /> class.
        /// </summary>
        /// <param name="buffers">The buffers.</param>
        public SecureBufferAdapter(List<SecureBuffer> buffers)
        {
            _buffers = buffers;

            _bufferHandles = new GCHandle[_buffers.Count];
            _bufferCarrier = new SecureBufferInternal[_buffers.Count];

            for (int i = 0; i < _buffers.Count; i++)
            {
                _bufferHandles[i] = GCHandle.Alloc(_buffers[i].Buffer, GCHandleType.Pinned);
                _bufferCarrier[i] = new SecureBufferInternal
                {
                    Type = _buffers[i].BufferType,
                    Count = _buffers[i].Buffer.Length,
                    Buffer = _bufferHandles[i].AddrOfPinnedObject()
                };
            }
            _bufferCarrierHandle = GCHandle.Alloc(_bufferCarrier, GCHandleType.Pinned);

            _descriptor = new SecureBufferDescriptor
            {
                Version = SecureBufferDescriptor.ApiVersion,
                NumBuffers = _buffers.Count,
                Buffers = _bufferCarrierHandle.AddrOfPinnedObject()
            };
            _descriptorHandle = GCHandle.Alloc(_descriptor, GCHandleType.Pinned);
        }

        /// <summary>
        /// Gets the handle to the secure buffer descriptor to pass to the native API.
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(nameof(SecureBufferAdapter));
                }
                return _descriptorHandle.AddrOfPinnedObject();
            }
        }

        /// <summary>
        /// Completes any buffer passing marshaling and releases all resources associated with the adapter.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                // Copy the potentially modified structure members back to the wrapped SecureBuffer
                for (int i = 0; i < _buffers.Count; i++)
                {
                    _buffers[i].Length = _bufferCarrier[i].Count;
                }

                for (int i = 0; i < _bufferHandles.Length; i++)
                {
                    if (_bufferHandles[i].IsAllocated)
                    {
                        _bufferHandles[i].Free();
                    }
                }

                if (_bufferCarrierHandle.IsAllocated)
                {
                    _bufferCarrierHandle.Free();
                }

                if (_descriptorHandle.IsAllocated)
                {
                    _descriptorHandle.Free();
                }

                _disposed = true;
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        private struct SecureBufferInternal
        {
            public int Count;
            public SecureBufferType Type;
            public IntPtr Buffer;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SecureBufferDescriptor
        {
            public int Version;
            public int NumBuffers;
            public IntPtr Buffers;
            public const int ApiVersion = 0;
        }
    }
}
