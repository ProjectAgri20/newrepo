function getElementByText(parentID, tagname, controlValue) {
    var allElements = document.getElementById(parentID).getElementsByTagName(tagname);
    for (i = 0; i < allElements.length; i++) {
        if (allElements[i].innerHTML == controlValue) {
            break;
        }
    }
    allElements[i].scrollIntoView();
    return allElements[i];
}

function getStatusMessage() {
	var statusValue = "";

	statusValue = document.querySelectorAll('.statusString', '.ng-binding')[0].innerHTML;

	return statusValue;
}