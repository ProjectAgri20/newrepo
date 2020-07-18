<?xml version="1.0" encoding="utf-8"?>
<tcx:Context xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
             xmlns:tcx="http://sherman.americas.hpqcorp.net/topcat/schemas/framework"
             xmlns:dib="http://sherman.americas.hpqcorp.net/topcat/schemas/group/dibble">
  <tcx:Annotation>
    <tcx:Description>
      TCXRunner Demo Test List
    </tcx:Description>
    <tcx:Created tcx:Author="jenkinga" tcx:Date="2011-03-16" />
    <tcx:LastUpdated tcx:Author="jenkinga" tcx:Date="2011-03-16" />
  </tcx:Annotation>

    <tcx:Properties>
		<tcx:Property tcx:ID="Name">
		  <tcx:Value>JetAdvantage</tcx:Value>
		  <tcx:Description>
			...
		  </tcx:Description>
		</tcx:Property>
		<tcx:Property tcx:ID="Setup">
		  <tcx:Value></tcx:Value>
		  <tcx:Description>
			default setup tcx
		  </tcx:Description>
		</tcx:Property>
		<tcx:Property tcx:ID="Cleanup">
		  <tcx:Value></tcx:Value>
		  <tcx:Description>
			default cleanup tcx
		  </tcx:Description>
		</tcx:Property>
		<tcx:Property tcx:ID="Publish">
		  <tcx:Value></tcx:Value>
		  <tcx:Description>
			default publish tcx
		  </tcx:Description>
		</tcx:Property>
		<tcx:Property tcx:ID="PTSetup">
		  <tcx:Value></tcx:Value>
		  <tcx:Description>
			default per test setup tcx
		  </tcx:Description>
		</tcx:Property>
		<tcx:Property tcx:ID="PTCleanup">
		  <tcx:Value></tcx:Value>
		  <tcx:Description>
			default per test cleanup tcx
		  </tcx:Description>
		</tcx:Property>
    </tcx:Properties>

  <tcx:Dibbles/>
  <tcx:Imports/>
  <tcx:Resources/>

  <tcx:Lists>
    <tcx:List tcx:ID="TestList">
      <tcx:ListItems>

        <tcx:Item>Products\ePrint20XPS\testdefs\InstallDutCancelRegNoPrinter.tcc</tcx:Item>
        <!--tcx:Item>print\PrinterPreferences.tcx</tcx:Item-->
        <tcx:Item>Working\JetAdvantage.tcx</tcx:Item>
        <!--tcx:Item>Products\guiop.tcx /property:GuiOperation,OKPrint.tcx</tcx:Item-->
      </tcx:ListItems>
    </tcx:List>
  </tcx:Lists>
  
</tcx:Context>
