[Code]

// ############################################################################################
procedure UpdateAppConfig(ConfigFile, Hostname: String);
var
  XMLDoc, RootNode, SystemsNode, SystemChildNode, Node: Variant;
  ConfigFilename, Key: String;
  i: integer;

begin

  try
      XMLDoc := CreateOleObject('Msxml2.DOMDocument.6.0');
  except
    RaiseException('MSXML is required to complete the post-installation process.'#13#13'(Error ''' + GetExceptionMessage + ''' occurred)');
  end;  

  XMLDoc.async := False;
  XMLDoc.resolveExternals := False;
  XMLDoc.load(ConfigFile);
  if XMLDoc.parseError.errorCode <> 0 then
    RaiseException('Error on line ' + IntToStr(XMLDoc.parseError.line) + ', position ' + IntToStr(XMLDoc.parseError.linepos) + ': ' + XMLDoc.parseError.reason);

  RootNode := XMLDoc.documentElement;
  SystemsNode := RootNode.selectSingleNode('//configuration/Systems');

  SystemChildNode := SystemsNode.childNodes.Item[0];
  SystemChildNode.setAttribute('key', 'STBServer');
  SystemChildNode.setAttribute('value', Hostname);

  SystemsNode.selectNodes('add').removeAll();
  //RootNode.selectNodes('//configuration/Systems/add').removeAll();  
  SystemsNode.appendChild(SystemChildNode);

  XMLDoc.Save('C:\temp\App-NEW.config'); 
end;


// ############################################################################################
procedure UpdateAppConfigxx(ConfigFile, Hostname: String);
var
  XMLNode: Variant;
  XMLDocument: Variant;  
  FirstChild: Variant;

begin
Log('1');
  XMLDocument := CreateOleObject('Msxml2.DOMDocument.6.0');
Log('2');
  
  try
    XMLDocument.async := False;
    XMLDocument.Load(ConfigFile);
Log('3');

    if (XMLDocument.parseError.errorCode <> 0) then
      begin
        MsgBox('The XML file could not be parsed. ' + XMLDocument.parseError.reason, mbError, MB_OK);
        Exit;
      end
    else
      begin
Log('4');

        XMLDocument.setProperty('SelectionLanguage', 'XPath');
Log('5');
        XMLNode := XMLDocument.selectSingleNode('//Systems');
Log('5a');
        FirstChild := XMLNode.FirstChild();
Log('5b');
        //XMLNode.RemoveAll();
Log('5c');
        FirstChild.text := 'asdfasdf';
Log('5d');
        //XMLNode.AppendChild(FirstChild);
Log('6');
        //XmlNode.ParentNode.RemoveChild(XmlNode);
Log('7');
        XMLDocument.save('c:\temp\App-NEW.config');
Log('8');
        //SaveStringToFile('c:\temp\App-NEW1.config', XMLDocument.innerxml, False);
Log('9');
      end;
  except
    MsgBox('An error occured!' + #13#10 + GetExceptionMessage, mbError, MB_OK);
  end;
end;


// ############################################################################################
function LoadValueFromXML(const AFileName, APath: string): string;
var
  XMLNode: Variant;
  XMLDocument: Variant;  
begin
  Result := '';
  XMLDocument := CreateOleObject('Msxml2.DOMDocument.6.0');
  try
    XMLDocument.async := False;
    XMLDocument.load(AFileName);
    if (XMLDocument.parseError.errorCode <> 0) then
      MsgBox('The XML file could not be parsed. ' + 
        XMLDocument.parseError.reason, mbError, MB_OK)
    else
    begin
      XMLDocument.setProperty('SelectionLanguage', 'XPath');
      XMLNode := XMLDocument.selectSingleNode(APath);
      Result := XMLNode.text;
    end;
  except
    MsgBox('An error occured!' + #13#10 + GetExceptionMessage, mbError, MB_OK);
  end;
end;


// ############################################################################################
procedure SaveValueToXML(const AFileName, APath, AValue: string);
var
  XMLNode: Variant;
  XMLDocument: Variant;  
begin
  XMLDocument := CreateOleObject('Msxml2.DOMDocument.6.0');
  try
    XMLDocument.async := False;
    XMLDocument.load(AFileName);
    if (XMLDocument.parseError.errorCode <> 0) then
      MsgBox('The XML file could not be parsed. ' + 
        XMLDocument.parseError.reason, mbError, MB_OK)
    else
    begin
      XMLDocument.setProperty('SelectionLanguage', 'XPath');
      XMLNode := XMLDocument.selectSingleNode(APath);
      XMLNode.text := AValue;
      XMLDocument.save(AFileName);
    end;
  except
    MsgBox('An error occured!' + #13#10 + GetExceptionMessage, mbError, MB_OK);
  end;
end;