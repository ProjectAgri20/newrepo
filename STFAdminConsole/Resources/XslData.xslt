<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="html" indent="yes" version="4.0"/>

  <xsl:template match="/">
    <html>
      <head>
        <style type="text/css">
          #table-layout
          {
          font-family: "Lucida Sans Unicode", "Lucida Grande", Sans-Serif;
          font-size: 11px;
          width: 500px;
          margin: 20px;
          border-collapse: collapse;
          text-align: left;          
          }
          #name-value-layout
          {
          font-family: "Lucida Sans Unicode", "Lucida Grande", Sans-Serif;
          font-size: 12px;
          margin: 20px;
          text-align: left;
          color: #039;
          }
          #table-layout th
          {
          font-size: 12px;
          font-weight: normal;
          color: #039;
          padding: 10px 8px;
          border-bottom: 2px solid #6678b1;
          }
          #table-layout-header
          {
          font-family: "Lucida Sans Unicode", "Lucida Grande", Sans-Serif;
          margin: 20px;
          font-size: 14px;
          font-weight: bold;
          color: #039;
          }
          #table-layout-subheader
          {
          font-family: "Lucida Sans Unicode", "Lucida Grande", Sans-Serif;
          margin-left: 20px;
          margin-bottom: 0px;
          font-size: 12px;
          font-weight: bold;
          color: #039;
          }
          #table-layout td
          {
          color: #669;
          padding: 6px 8px;
          }
          #table-layout tbody tr:hover td
          {
          color: #009;
          }
        </style>
      </head>
      <body style="background-color: #f0f0ea">
        <xsl:apply-templates select="DispatcherConfiguration" />
      </body>
    </html>
  </xsl:template>

  <xsl:template match="DispatcherConfiguration">
    <div id="table-layout-header">STF Scenario Configuration</div>
    <table id="name-value-layout">
      <tr>
        <td>Start Time:</td>
        <td style="font-size:12px;color:#009;">
          <xsl:apply-templates select="StartTime"/>
        </td>
      </tr>
    </table>
    <xsl:if test="count(RequestedVMs/item) > 0">
      <div id="table-layout-subheader">Virtual Machine Selection</div>
      <table id="table-layout">
        <tr>
          <th valign="top">
            Selection Mode
          </th>
          <th>
            Virtual Machine Hosts
          </th>
        </tr>
        <xsl:for-each select="RequestedVMs/item">
          <tr>
            <td style="valign:top">
              <xsl:value-of select="key/string"/>
            </td>
            <td>
              <xsl:apply-templates select="value/ArrayOfString/string"/>
            </td>
          </tr>
        </xsl:for-each>
      </table>
    </xsl:if>
    <xsl:if test="count(SessionAssets/SessionAsset) > 0">
      <div id="table-layout-subheader">Session Assets</div>
      <table id="table-layout">
        <thead>
          <tr>
            <th>AssetId</th>
            <th>Availabile Until</th>
            <th>Address</th>
            <th>Product</th>
            <th>Port</th>
            <th>Snmp</th>
            <th>Use CRC</th>
            <th>Description</th>
          </tr>
        </thead>
        <xsl:for-each select="SessionAssets/SessionAsset">
          <tr>
            <td>
              <nobr>
                <xsl:value-of select="AssetId"/>
              </nobr>
            </td>
            <td>
              <nobr>
                <xsl:apply-templates select="AvailabilityEndTime"/>
              </nobr>
            </td>
            <td>
              <xsl:value-of select="Address"/>
            </td>
            <td>
              <xsl:value-of select="Product"/>
            </td>
            <td>
              <xsl:value-of select="PortNumber"/>
            </td>
            <td>
              <xsl:value-of select="SnmpEnabled"/>
            </td>
            <td>
              <xsl:value-of select="UseCrc"/>
            </td>
            <td>
              <xsl:value-of select="Description"/>
            </td>
          </tr>
        </xsl:for-each>
      </table>
    </xsl:if>
  </xsl:template>

  <xsl:template match="value/ArrayOfString/string">
    <xsl:value-of select="."/>
    <br/>
  </xsl:template>

  <xsl:template match="SessionAssets/SessionAssset">
    <xsl:value-of select="."/>
    <br/>
  </xsl:template>

  <xsl:template match="StartTime">
    <xsl:element name="date">
      <xsl:call-template name="formatdate">
        <xsl:with-param name="datestr" select="."/>
      </xsl:call-template>
    </xsl:element>
  </xsl:template>

  <xsl:template match="AvailabilityEndTime">
    <xsl:element name="date">
      <xsl:call-template name="formatdate">
        <xsl:with-param name="datestr" select="."/>
      </xsl:call-template>
    </xsl:element>
  </xsl:template>

  <xsl:template name="formatdate">
    <xsl:param name="datestr" />
    <!-- input format mmddyyyy -->
    <!-- output format mm/dd/yyyy -->

    <xsl:variable name="dateValue">
      <xsl:value-of select="substring($datestr,1,10)" />
    </xsl:variable>

    <xsl:variable name="timeValue">
      <xsl:value-of select="substring($datestr,12,8)" />
    </xsl:variable>

    <!--<xsl:value-of select="$datestr" />-->
    <xsl:value-of select="$dateValue" />
    <xsl:text> </xsl:text>
    <xsl:value-of select="$timeValue" />
  </xsl:template>
</xsl:stylesheet>
