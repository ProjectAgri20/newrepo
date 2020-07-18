function existclassName() {

	var bExist = false;

	if (document.querySelectorAll(".labelOXPd") != null) {
		bExist = true;
	}
	return bExist;
}

function ExistButtonId(buttonId) {
	var buttonElement = document.getElementById(buttonId), existBtn;
	existBtn = true;
	if (buttonElement == null) {
		existBtn = false;
	}
	return existBtn;
}

function getButtonIdByElementName(elementName) {
	var allElements = document.getElementsByName(elementName);
	var ids = "";
	for (i = 0; i < allElements.length; i++) {
		if (allElements[i].id.length > 0) {
			ids = ids + allElements[i].id + ";";
		}
	}
	return ids;
}

function getButtonIdByTextValue(textValue) {
	var allElements = document.querySelectorAll('.hp-listitem');
	var ids = "";
	Boolean isOCR = false;

	if (textValue.indexOf("(OCR)") > 0) {
		isOCR = true;
	}

	for (i = 0; i < allElements.length; i++) {
		var val = allElements[i].innerHTML;
		if (val.indexOf(textValue) >= 0) {
			if (!isOCR && val.indexOf("OCR") < 0) {
				ids = allElements[i].id;
			}
			else if(isOCR && val.indexOf("OCR") >= 0){
				ids = allElements[i].id;
			}
		}		
	}
	return ids;
}

function existWorkflow(workflow) {
	var bExist = false;
	var textValue = "nothing";

	textValue = document.querySelectorAll('.hp-list-title', '.lg')[0].innerHTML;
	if (textValue.indexOf(workflow) >= 0) {
		bExist = true;
	}
	
	return bExist;
}