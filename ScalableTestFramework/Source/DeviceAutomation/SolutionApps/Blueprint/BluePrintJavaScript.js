function getDocumentCount() {
    var elements = document.getElementsByTagName("input");
    var count = 0;
    for (var i = 0; i < elements.length; i++) {
        if (elements[i].type == "checkbox") {
            count++;
        }
    }
    return count;
}


function getDocumentCountUsingImgTag() {
    var elements = document.getElementsByTagName("img");
    var count = 0;
    for (var i = 0; i < elements.length; i++) {
		if (elements[i].className.substring(0, 34) == "hp-listitem-icon hp-listitem-image") {
			if (elements[i].className.indexOf("collapse") < 1) {
				count++;
			}
        }
    }

    if (count == 0) {
        count = getDocumentCount();
	}

	if (count == 0) {
		var html = document.documentElement.outerHTML;

		if (html.indexOf('No Documents') > 0) {
			count = 0;
		}
		else if (html.indexOf('Error') > 0) {
			count = -1;
		}
	}
    return count;
}


function CheckElementUsingClassIndex(className) {
    var elementExists = document.getElementsByClassName(className);
    var length = elementExists.length;
    return length;
}


function ExistButtonId(buttonId) {
    var buttonElement = document.getElementById(buttonId), existBtn;
    existBtn = true;
    if (buttonElement == null) {
        existBtn = false;
    }
    return existBtn;
}

