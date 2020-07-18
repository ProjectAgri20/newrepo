using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.ScalableTest.Service.PhysicalDeviceJobLogMonitor
{
    /// <summary>
    /// <jasi:ScanInfo>
    ///  <ct:CounterGroup>
    ///    <dd:CounterGroupName>A4EquivalentSheets</dd:CounterGroupName>
    ///    <ct:Counter>
    ///      <dd:CounterName>TotalSheets</dd:CounterName>
    ///      <dd:FixedPointNumber>
    ///        <dd:Significand>1</dd:Significand>
    ///        <dd:Exponent>0</dd:Exponent>
    ///      </dd:FixedPointNumber>
    ///    </ct:Counter>
    ///    <ct:Counter>
    ///      <dd:CounterName>SimplexSheets</dd:CounterName>
    ///      <dd:FixedPointNumber>
    ///        <dd:Significand>1</dd:Significand>
    ///        <dd:Exponent>0</dd:Exponent>
    ///      </dd:FixedPointNumber>
    ///    </ct:Counter>
    ///    <ct:Counter>
    ///      <dd:CounterName>FlatbedSheets</dd:CounterName>
    ///      <dd:FixedPointNumber>
    ///        <dd:Significand>1</dd:Significand>
    ///        <dd:Exponent>0</dd:Exponent>
    ///      </dd:FixedPointNumber>
    ///    </ct:Counter>
    ///  </ct:CounterGroup>
    ///  <ct:CounterGroup>
    ///    <dd:CounterGroupName>A4EquivalentImages</dd:CounterGroupName>
    ///    <ct:Counter>
    ///      <dd:CounterName>TotalImages</dd:CounterName>
    ///      <dd:FixedPointNumber>
    ///        <dd:Significand>1</dd:Significand>
    ///        <dd:Exponent>0</dd:Exponent>
    ///      </dd:FixedPointNumber>
    ///    </ct:Counter>
    ///  </ct:CounterGroup>
    ///  <ct:Counter>
    ///    <dd:CounterName>TotalSheets</dd:CounterName>
    ///    <dd:FixedPointNumber>
    ///      <dd:Significand>1</dd:Significand>
    ///      <dd:Exponent>0</dd:Exponent>
    ///    </dd:FixedPointNumber>
    ///  </ct:Counter>
    ///  <ct:Counter>
    ///    <dd:CounterName>SimplexSheets</dd:CounterName>
    ///    <dd:FixedPointNumber>
    ///      <dd:Significand>1</dd:Significand>
    ///      <dd:Exponent>0</dd:Exponent>
    ///    </dd:FixedPointNumber>
    ///  </ct:Counter>
    ///  <ct:Counter>
    ///    <dd:CounterName>FlatbedSheets</dd:CounterName>
    ///    <dd:FixedPointNumber>
    ///      <dd:Significand>1</dd:Significand>
    ///      <dd:Exponent>0</dd:Exponent>
    ///    </dd:FixedPointNumber>
    ///  </ct:Counter>
    ///  <ct:Counter>
    ///    <dd:CounterName>FlatbedImages</dd:CounterName>
    ///    <dd:FixedPointNumber>
    ///      <dd:Significand>1</dd:Significand>
    ///      <dd:Exponent>0</dd:Exponent>
    ///    </dd:FixedPointNumber>
    ///  </ct:Counter>
    ///  <ct:Counter>
    ///    <dd:CounterName>TotalImages</dd:CounterName>
    ///    <dd:FixedPointNumber>
    ///      <dd:Significand>1</dd:Significand>
    ///      <dd:Exponent>0</dd:Exponent>
    ///    </dd:FixedPointNumber>
    ///  </ct:Counter>
    ///  <jasi:FirstScannedSheet>
    ///    <dd:MediaSizeID>na_letter_8.5x11in</dd:MediaSizeID>
    ///    <dd:MediaInputID>Flatbed</dd:MediaInputID>
    ///    <dd:Plex>Simplex</dd:Plex>
    ///  </jasi:FirstScannedSheet>
    ///</jasi:ScanInfo>
    /// </summary>
    public class ScanLogItem
    {
        // FirstScannedSheet
        public string MediaInputID { get; set; }
        public string MediaSizeID { get; set; }
        public string Plex { get; set; }

        // Following is retrieved from the Counter nodes
        public string ADFImages { get; set; }
        public string ADFSheets { get; set; }
        public string ADFSimplexImages { get; set; }
        public string TotalImages { get; set; }

        public string FlatbedSheets { get; set; }
        public string FlatbedImages { get; set; }
        public string  SimplexSheets { get; set; }
        public string TotalSheets { get; set; }
      
        public ScanLogItem()
        {
            MediaInputID = string.Empty;
            MediaSizeID = string.Empty;            
            Plex = string.Empty;

            ADFImages = string.Empty;
            ADFSheets = string.Empty;
            ADFSimplexImages = string.Empty;
            TotalImages = string.Empty;
            TotalSheets = string.Empty;
            FlatbedImages = string.Empty;
            FlatbedSheets = string.Empty;
            SimplexSheets = string.Empty;
            TotalSheets = string.Empty;
        }
        public override string ToString()
        {
            return "MediaInput={0}, Plex={1}".FormatWith(MediaInputID, Plex);
        }
    }
}
