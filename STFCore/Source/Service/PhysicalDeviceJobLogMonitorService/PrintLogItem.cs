using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.ScalableTest.Service.PhysicalDeviceJobLogMonitor
{
    /// <summary>
    /// <jasi:PrintInfo>
    ///  <jasi:PrintSettings>
    ///    <dd:PrintQuality>Normal</dd:PrintQuality>
    ///    <jasi:FirstPrintedSheet>
    ///      <dd:MediaSizeID>na_letter_8.5x11in</dd:MediaSizeID>
    ///      <dd:MediaTypeID>plain</dd:MediaTypeID>
    ///      <dd:MediaInputID>Tray2</dd:MediaInputID>
    ///      <dd:MediaOutputID>Face-down</dd:MediaOutputID>
    ///      <dd:Plex>Simplex</dd:Plex>
    ///    </jasi:FirstPrintedSheet>
    ///    <dd:AccentColorProThreshold>0</dd:AccentColorProThreshold>
    ///    <dd:AccentColorOfficeThreshold>0</dd:AccentColorOfficeThreshold>
    ///  </jasi:PrintSettings>
    ///  <jasi:PrintSummary>
    ///    <ct:CounterGroup>
    ///      <dd:CounterGroupName>A4EquivalentImpressions</dd:CounterGroupName>
    ///      <ct:Counter>
    ///        <dd:CounterName>MonochromeImpressions</dd:CounterName>
    ///        <dd:FixedPointNumber>
    ///          <dd:Significand>9</dd:Significand>
    ///          <dd:Exponent>0</dd:Exponent>
    ///        </dd:FixedPointNumber>
    ///      </ct:Counter>
    ///      <ct:Counter>
    ///        <dd:CounterName>ColorImpressions</dd:CounterName>
    ///        <dd:FixedPointNumber>
    ///          <dd:Significand>0</dd:Significand>
    ///          <dd:Exponent>0</dd:Exponent>
    ///        </dd:FixedPointNumber>
    ///      </ct:Counter>
    ///    </ct:CounterGroup>
    ///    <ct:CounterGroup>
    ///      <dd:CounterGroupName>A4EquivalentSheets</dd:CounterGroupName>
    ///      <ct:Counter>
    ///        <dd:CounterName>SimplexSheets</dd:CounterName>
    ///        <dd:FixedPointNumber>
    ///          <dd:Significand>9</dd:Significand>
    ///          <dd:Exponent>0</dd:Exponent>
    ///        </dd:FixedPointNumber>
    ///      </ct:Counter>
    ///      <ct:Counter>
    ///        <dd:CounterName>TotalSheets</dd:CounterName>
    ///        <dd:FixedPointNumber>
    ///          <dd:Significand>9</dd:Significand>
    ///          <dd:Exponent>0</dd:Exponent>
    ///        </dd:FixedPointNumber>
    ///      </ct:Counter>
    ///    </ct:CounterGroup>
    ///    <ct:Counter>
    ///      <dd:CounterName>TotalImpressions</dd:CounterName>
    ///      <dd:FixedPointNumber>
    ///        <dd:Significand>9</dd:Significand>
    ///        <dd:Exponent>0</dd:Exponent>
    ///      </dd:FixedPointNumber>
    ///    </ct:Counter>
    ///    <ct:Counter>
    ///      <dd:CounterName>MonochromeImpressions</dd:CounterName>
    ///      <dd:FixedPointNumber>
    ///        <dd:Significand>9</dd:Significand>
    ///        <dd:Exponent>0</dd:Exponent>
    ///      </dd:FixedPointNumber>
    ///    </ct:Counter>
    ///    <ct:Counter>
    ///      <dd:CounterName>SimplexSheets</dd:CounterName>
    ///      <dd:FixedPointNumber>
    ///        <dd:Significand>9</dd:Significand>
    ///        <dd:Exponent>0</dd:Exponent>
    ///      </dd:FixedPointNumber>
    ///    </ct:Counter>
    ///    <ct:Counter>
    ///      <dd:CounterName>ColorImpressions</dd:CounterName>
    ///      <dd:FixedPointNumber>
    ///        <dd:Significand>0</dd:Significand>
    ///        <dd:Exponent>0</dd:Exponent>
    ///      </dd:FixedPointNumber>
    ///    </ct:Counter>
    ///  </jasi:PrintSummary>
    ///  <sup:Agents>
    ///    <sup:Agent>
    ///      <ct:CounterGroup>
    ///        <dd:CounterGroupName>A4EquivalentImpressions</dd:CounterGroupName>
    ///        <ct:Counter>
    ///          <dd:CounterName>TotalImpressions</dd:CounterName>
    ///          <dd:FixedPointNumber>
    ///            <dd:Significand>9</dd:Significand>
    ///            <dd:Exponent>0</dd:Exponent>
    ///          </dd:FixedPointNumber>
    ///        </ct:Counter>
    ///      </ct:CounterGroup>
    ///      <ct:Counter>
    ///        <dd:CounterName>AgentUsed</dd:CounterName>
    ///        <dd:FixedPointNumber>
    ///          <dd:Significand>14303136</dd:Significand>
    ///          <dd:Exponent>0</dd:Exponent>
    ///        </dd:FixedPointNumber>
    ///      </ct:Counter>
    ///      <dd:ConsumableTypeEnum>tonerCartridge</dd:ConsumableTypeEnum>
    ///      <dd:Description>BlackCartridge1</dd:Description>
    ///      <dd:ProductNumber>CE255X</dd:ProductNumber>
    ///      <dd:SerialNumber>84092690</dd:SerialNumber>
    ///      <dd:MakeAndModel>HP LaserJet flow MFP M525</dd:MakeAndModel>
    ///      <dd:Manufacturer>
    ///        <dd:Name>HP    </dd:Name>
    ///        <dd:Date>2014-01-08</dd:Date>
    ///      </dd:Manufacturer>
    ///      <dd:ApproxPercentRemainingOnInstall>100</dd:ApproxPercentRemainingOnInstall>
    ///      <dd:ApproxPercentRemaining>100</dd:ApproxPercentRemaining>
    ///      <dd:MarkerColor>Black</dd:MarkerColor>
    ///      <sup:AgentID>9dc7fa5a-0740-5c3e-7ab0-79265675dfa1</sup:AgentID>
    ///      <dd:ConsumableContentType>markingAgent</dd:ConsumableContentType>
    ///      <dd:Capacity>
    ///        <dd:MaxCapacity>12500</dd:MaxCapacity>
    ///        <dd:Unit>impressions</dd:Unit>
    ///      </dd:Capacity>
    ///      <dd:Installation>
    ///        <dd:Date>2014-03-18</dd:Date>
    ///      </dd:Installation>
    ///      <dd:LastUse>2014-10-03-02:33</dd:LastUse>
    ///    </sup:Agent>
    ///  </sup:Agents>
    /// </jasi:PrintInfo>

    /// </summary>
    public class PrintLogItem
    {
        /// <summary>
        /// Impressions are the physical pages printed
        /// </summary>
        public string TotalImpressions { get; set; }
        public string MonochromeImpressions { get; set; }
        public string ColorImpressions { get; set; }
        public string SimplexSheets { get; set; }
        public string PrintQuality { get; set; }
        public string MediaSizeID { get; set; }
        public string MediaTypeID { get; set; }
        public string MediaInputID { get; set; }
        public string Plex { get; set; }

        public PrintLogItem()
        {
            TotalImpressions = string.Empty;
            MonochromeImpressions = string.Empty;
            ColorImpressions = string.Empty;
            SimplexSheets = string.Empty;
            PrintQuality = string.Empty;
            MediaSizeID = string.Empty;
            MediaTypeID = string.Empty;
            MediaInputID = string.Empty;
            Plex = string.Empty;
        }
        public override string ToString()
        {
            return "PageSize={0}, PagesPrinted={1}".FormatWith(MediaSizeID, TotalImpressions);
        }
    }
}
