using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// Provides extension methods and utilities for working with <see cref="IPAddress" />.
    /// </summary>
    public static class IPAddressUtil
    {
        //Reference: http://en.wikipedia.org/wiki/Reserved_IP_addresses
        private static readonly Dictionary<IPAddress, IPAddress> _nonRoutableSubnets = new Dictionary<IPAddress, IPAddress>
        {
            { IPAddress.Parse("0.0.0.0"), IPAddress.Parse("255.0.0.0") },        // Reserved for broadcast
            { IPAddress.Parse("10.0.0.0"), IPAddress.Parse("255.0.0.0") },       // Class A private network
            { IPAddress.Parse("172.16.0.0"), IPAddress.Parse("255.240.0.0") },   // Class B private network
            { IPAddress.Parse("192.168.0.0"), IPAddress.Parse("255.255.0.0") },  // Class C private network
            { IPAddress.Parse("127.0.0.0"), IPAddress.Parse("255.0.0.0") }       // Loopback
        };

        /// <summary>
        /// Determines whether an address is in a subnet specified by an <see cref="IPAddress" /> and subnet mask.
        /// </summary>
        /// <param name="address">The <see cref="IPAddress" /> to test.</param>
        /// <param name="subnetAddress">An <see cref="IPAddress" /> that is in the desired subnet.</param>
        /// <param name="subnetMask">The subnet mask.</param>
        /// <returns><c>true</c> if the address is in the subnet indicated by the specified address and mask; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="address" /> is null.
        /// <para>or</para>
        /// <paramref name="subnetAddress" /> is null.
        /// <para>or</para>
        /// <paramref name="subnetMask" /> is null.
        /// </exception>
        public static bool IsInSubnet(this IPAddress address, IPAddress subnetAddress, IPAddress subnetMask)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (subnetAddress == null)
            {
                throw new ArgumentNullException(nameof(subnetAddress));
            }

            if (subnetMask == null)
            {
                throw new ArgumentNullException(nameof(subnetMask));
            }

            IPAddress network1 = subnetAddress.ApplySubnetMask(subnetMask);
            IPAddress network2 = address.ApplySubnetMask(subnetMask);
            return network1.Equals(network2);
        }

        /// <summary>
        /// Determines whether the specified address is routable.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns><c>true</c> if the specified address is routable; otherwise, <c>false</c>.</returns>
        public static bool IsRoutable(this IPAddress address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            return !_nonRoutableSubnets.Any(n => address.IsInSubnet(n.Key, n.Value));
        }

        /// <summary>
        /// Applies the specified subnet mask to the specified <see cref="IPAddress" />.
        /// </summary>
        /// <param name="address">The <see cref="IPAddress" />.</param>
        /// <param name="subnetMask">An <see cref="IPAddress" /> representing the subnet mask.</param>
        /// <returns>The result of applying the subnet mask.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="address" /> is null.
        /// <para>or</para>
        /// <paramref name="subnetMask" /> is null.
        /// </exception>
        public static IPAddress ApplySubnetMask(this IPAddress address, IPAddress subnetMask)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (subnetMask == null)
            {
                throw new ArgumentNullException(nameof(subnetMask));
            }

            return BitwiseOp(address, subnetMask, (addressByte, maskByte) => (byte)(addressByte & maskByte));
        }

        /// <summary>
        /// Gets the subnet broadcast address for the specified <see cref="IPAddress" /> and subnet mask.
        /// </summary>
        /// <param name="address">The <see cref="IPAddress" />.</param>
        /// <param name="subnetMask">An <see cref="IPAddress" /> representing the subnet mask.</param>
        /// <returns>The broadcast address for the subnet.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="address" /> is null.
        /// <para>or</para>
        /// <paramref name="subnetMask" /> is null.
        /// </exception>
        public static IPAddress GetBroadcast(this IPAddress address, IPAddress subnetMask)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (subnetMask == null)
            {
                throw new ArgumentNullException(nameof(subnetMask));
            }

            return BitwiseOp(address, subnetMask, (addressByte, maskByte) => (byte)(addressByte | ~maskByte));
        }

        private static IPAddress BitwiseOp(IPAddress address1, IPAddress address2, Func<byte, byte, byte> operation)
        {
            byte[] bytes1 = address1.GetAddressBytes();
            byte[] bytes2 = address2.GetAddressBytes();
            byte[] resultBytes = bytes1.Zip(bytes2, operation).ToArray();
            return new IPAddress(resultBytes);
        }

        /// <summary>
        /// Finds the next assignable IP address in the given subnet.
        /// </summary>
        /// <param name="address">The <see cref="IPAddress" /> to start from.</param>
        /// <param name="subnetMask">The subnet mask.</param>
        /// <returns>The next assignable IP address in the given subnet.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="address" /> is null.
        /// <para>or</para>
        /// <paramref name="subnetMask" /> is null.
        /// </exception>
        public static IPAddress NextAssignable(IPAddress address, IPAddress subnetMask)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (subnetMask == null)
            {
                throw new ArgumentNullException(nameof(subnetMask));
            }

            uint rawAddress = ToUint(address);
            uint rawSubnetMask = ToUint(subnetMask);
            do
            {
                rawAddress++;
            } while (!IsAssignable(rawAddress, rawSubnetMask));

            return FromUint(rawAddress);
        }

        private static bool IsAssignable(uint address, uint subnetMask)
        {
            uint networkAddress = address & subnetMask;
            uint networkBroadcast = address | ~subnetMask;

            // The IP Address is assignable as long as the address isn't the network address or broadcast address.
            return (networkAddress != address) && (networkBroadcast != address);
        }

        private static uint ToUint(this IPAddress address)
        {
            return BitConverter.ToUInt32(address.GetAddressBytes().Reverse().ToArray(), 0);
        }

        private static IPAddress FromUint(uint address)
        {
            return new IPAddress(BitConverter.GetBytes(address).Reverse().ToArray());
        }
    }
}
