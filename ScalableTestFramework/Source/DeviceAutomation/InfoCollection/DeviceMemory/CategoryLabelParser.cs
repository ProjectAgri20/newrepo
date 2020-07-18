using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceMemory
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CategoryLabelParser
    {
        private readonly XDocument _memoryData;
        private string _desiredCategoryLabels = string.Empty;
        private List<MemoryPairs> _listMemoryPairs = new List<MemoryPairs>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryLabelParser"/> class.
        /// </summary>
        /// <param name="memoryData">The memory data.</param>
        /// <param name="desiredCategoryLabels">The desired category labels.</param>
        public CategoryLabelParser(XDocument memoryData, string desiredCategoryLabels)
        {
            _memoryData = memoryData;
            _desiredCategoryLabels = desiredCategoryLabels;
            GetDesiredPairs();
        }

        /// <summary>
        /// Processes the memory data.
        /// </summary>
        /// <param name="snapshotId">The snapshot identifier.</param>
        /// <returns></returns>
        public IEnumerable<DeviceMemoryCountLog> ProcessMemoryData(Guid snapshotId)
        {
            List<XElement> jediCounters = GetElementsSelf(_memoryData.Root, "Counter");
            List<DeviceMemoryCountLog> listDeviceMemoryCountLog = new List<DeviceMemoryCountLog>();
            //Process default memory counters
            foreach (XElement xe in jediCounters)
            {
                string categoryName = GetAttributeValue(xe, "Category");
                string dataLabel = GetAttributeValue(xe, "Name");
                string dataValue = GetAttributeValue(xe, "Value");
                long value = 0;
                long.TryParse(dataValue, out value);


                if (IsDesiredPair(categoryName, dataLabel))
                {
                    DeviceMemoryCountLog memoryCountLog = new DeviceMemoryCountLog(snapshotId, categoryName, dataLabel, value);
                    listDeviceMemoryCountLog.Add(memoryCountLog);
                }
            }
            //Process additional webkit memory counters
            List<XElement> webkitCounters = GetElementsSelf(_memoryData.Root, "Statistic");
            foreach (XElement xe in webkitCounters)
            {
                string categoryName = GetAttributeValue(xe.Parent, "Name");
                if (!string.IsNullOrEmpty(categoryName))
                {
                    categoryName = categoryName.Replace(' ', '_');

                    string dataLabel = GetAttributeValue(xe, "Type");
                    string bytesValue = GetAttributeValue(xe, "Bytes");
                    long value = 0;
                    if (string.IsNullOrEmpty(bytesValue))
                    {
                        float mbValue = 0.0f;
                        string megabytesValue = GetAttributeValue(xe, "MB");
                        float.TryParse(megabytesValue, out mbValue);
                        value = (long)(mbValue * 1024.0f);
                    }
                    else
                    {
                        long.TryParse(bytesValue, out value);
                    }
                    if (IsDesiredPair(categoryName, dataLabel))
                    {
                        DeviceMemoryCountLog memoryCountLog = new DeviceMemoryCountLog(snapshotId, categoryName, dataLabel, value);
                        listDeviceMemoryCountLog.Add(memoryCountLog);
                    }
                }
            }

            return listDeviceMemoryCountLog;
        }
        private bool IsDesiredPair(string categoryName, string dataLabel)
        {
            bool found = false;

            foreach (MemoryPairs mp in _listMemoryPairs)
            {
                if (mp.CategoryName.Equals(categoryName) && mp.DataLabel.Equals(dataLabel))
                {
                    found = true;
                    break;
                }
            }

            return found;
        }
        private static List<XElement> GetElementsSelf(XElement root, string localName)
        {
            var result = (
                        from el in root.DescendantsAndSelf().Where(x => x.Name.LocalName == localName)
                        select el
                        ).ToList();
            return result;
        }

        private static string GetAttributeValue(XElement node, string attrName)
        {
            string sValue = string.Empty;

            var attrValue = (from a in node.Attributes()
                             where a.Name.LocalName.Equals(attrName)
                             select a).FirstOrDefault();

            if (attrValue != null)
            {
                sValue = attrValue.Value;
            }

            return sValue;
        }
        private void GetDesiredPairs()
        {
            string[] values = _desiredCategoryLabels.Split(';');
            foreach (string pair in values)
            {
                string[] pairs = pair.Split(',');
                MemoryPairs mp = new MemoryPairs(pairs[0], pairs[1]);

                _listMemoryPairs.Add(mp);
            }

        }
        private class MemoryPairs
        {
            /// <summary>
            /// Gets or sets the name of the category.
            /// </summary>
            /// <value>
            /// The name of the category.
            /// </value>
            public string CategoryName { get; set; }
            /// <summary>
            /// Gets or sets the data label.
            /// </summary>
            /// <value>
            /// The data label.
            /// </value>
            public string DataLabel { get; set; }
            public MemoryPairs(string categoryName, string dataLable)
            {
                CategoryName = categoryName;
                DataLabel = dataLable;
            }
        }
    }
}
