using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Print.Drivers;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// This is a container class for <see cref="PrintDeviceDriver"/> instances.  It 
    /// provides some general <seealso cref="T:System.Collections.Generic.IList`1"/> behaviors
    /// to help manage the list of drivers.
    /// </summary>
    [DataContract]
    public class PrintDeviceDriverCollection : IList<PrintDeviceDriver>
    {
        [DataMember]
        private Collection<PrintDeviceDriver> _driverProperties = new Collection<PrintDeviceDriver>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDeviceDriverCollection"/> class.
        /// </summary>
        public PrintDeviceDriverCollection()
        {
            Version = string.Empty;
            Id = SequentialGuid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDeviceDriverCollection"/> class.
        /// </summary>
        /// <param name="list">The list.</param>
        public PrintDeviceDriverCollection(IList<PrintDeviceDriver> list)
            : this()
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            foreach (PrintDeviceDriver driver in list)
            {
                if (!_driverProperties.Contains(driver))
                {
                    _driverProperties.Add(driver);
                }
            }
        }

        /// <summary>
        /// Sorts this instance by Name, Architecture and then Version.
        /// </summary>
        public void Sort()
        {
            _driverProperties = new Collection<PrintDeviceDriver>
                (
                    _driverProperties.OrderBy(e => e.Name)
                    .ThenBy(e => e.Architecture)
                    .ThenBy(e => e.Version)
                    .ToList()
                );
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/>.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the end of the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/>.</param>
        public void Add(PrintDeviceDriverCollection collection)
        {
            AddRange(collection);
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/>.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the end of this collection.</param>
        public void AddRange(IEnumerable<PrintDeviceDriver> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var item in collection)
            {
                _driverProperties.Add(item);
            }
        }

        /// <summary>
        /// Gets or sets the version for this print driver set.
        /// </summary>
        [DataMember]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets a unique ID associated with this print driver set.
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Removes all elements from the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/> and resets the properties to default values
        /// </summary>
        public void Clear()
        {
            _driverProperties.Clear();
            Version = string.Empty;
            Id = SequentialGuid.NewGuid();
        }

        /// <summary>
        /// Gets the number of elements actually contained in the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/>.
        /// </summary>
        public int Count
        {
            get { return _driverProperties.Count; }
        }

        /// <summary>
        /// Adds the specified element to the end of the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/>.
        /// </summary>
        /// <param name="item">The item to be added to the end of the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/>.</param>
        public void Add(PrintDeviceDriver item)
        {
            _driverProperties.Add(item);
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/>.</param>
        /// <returns>
        ///   <c>true</c> if the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/> contains this element; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(PrintDeviceDriver item)
        {
            return _driverProperties.Contains(item);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            PrintDeviceDriverCollection drivers = obj as PrintDeviceDriverCollection;

            if (drivers == null)
            {
                return false;
            }

            var setA = (from d in _driverProperties select d.InfPath);
            var setB = (from d in drivers select d.InfPath);

            // These packages are considered the same if their version and INF file locations are the same.            
            return drivers.Version.Equals(Version, StringComparison.OrdinalIgnoreCase)
                && new HashSet<string>(setA).SetEquals(setB);
        }

        /// <summary>
        /// Returns a hash code for the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/>.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            StringBuilder builder = new StringBuilder(Version);
            return builder.ToString().GetHashCode();
        }

        /// <summary>
        /// Determines the index the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/> based on criteria within the provided item.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/>.</param>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        /// <remarks>
        /// To determine what the index value is, the Name, Architecture, Version and INF file path are all considered in finding the location.
        /// </remarks>
        public int IndexOf(PrintDeviceDriver item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return IndexOf(item.Name, item.Architecture, item.Version, item.InfPath);
        }

        /// <summary>
        /// Gets the index of the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/> based on the provided criteria
        /// </summary>
        /// <param name="name">The name of the driver.</param>
        /// <param name="architecture">The applicable system architecture.</param>
        /// <param name="version">The version of the driver.</param>
        /// <param name="infFile">The INF file path for.</param>
        /// <returns></returns>
        public int IndexOf(string name, DriverArchitecture architecture, DriverVersion version, string infFile)
        {
            var enumeratedProperties = _driverProperties.Select((item, i) => new { Items = item, Index = i });

            var entry =
                (
                    from p in enumeratedProperties
                    where p.Items.Architecture == architecture
                        && p.Items.Name.EqualsIgnoreCase(name)
                        && p.Items.Version.Equals(version)
                        && string.IsNullOrEmpty(p.Items.InfPath) == false
                        && Path.GetFileName(p.Items.InfPath).EqualsIgnoreCase(Path.GetFileName(infFile))
                    select p
                ).FirstOrDefault();

            return entry != null ? entry.Index : -1;
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.
        ///   </exception>
        ///   
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
        ///   </exception>
        public void Insert(int index, PrintDeviceDriver item)
        {
            _driverProperties.Insert(index, item);
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.
        ///   </exception>
        ///   
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
        ///   </exception>
        public void RemoveAt(int index)
        {
            _driverProperties.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the element at the specified index in the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/>.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        ///   </returns>
        ///   
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.
        ///   </exception>
        ///   
        /// <exception cref="T:System.NotSupportedException">
        /// The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
        ///   </exception>
        public PrintDeviceDriver this[int index]
        {
            get
            {
                return _driverProperties[index];
            }
            set
            {
                _driverProperties[index] = value;
            }
        }

        /// <summary>
        /// Copies the entire <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/> to a compatible 
        /// one-dimensional <see cref="System.Array"/> starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the 
        /// elements copied from the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/>.  
        /// The <see cref="System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <see cref="System.Array"/> at which copying begins.</param>
        public void CopyTo(PrintDeviceDriver[] array, int arrayIndex)
        {
            _driverProperties.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/> is read-only; otherwise, false.
        ///   </returns>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/>; otherwise, false. 
        /// This method also returns false if <paramref name="item"/> is not found in the original <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/> is read-only.
        ///   </exception>
        public bool Remove(PrintDeviceDriver item)
        {
            return _driverProperties.Remove(item);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<PrintDeviceDriver> GetEnumerator()
        {
            return _driverProperties.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
