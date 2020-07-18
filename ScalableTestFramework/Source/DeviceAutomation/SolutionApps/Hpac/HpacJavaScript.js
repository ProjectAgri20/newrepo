function getDocumentCount() {
    var elements = document.getElementsByTagName("input");
    var count = 0;
    for (var i = 0; i < elements.length; i++) {
        if (elements[i].type == "checkbox") {
            count++;
        }
	}

	if (count == 0) {
		var html = document.documentElement.outerHTML;

		if (html.indexOf('Error') > 0) {
			count = -1;
		}
	}

    return count;
}

function getDocumentIds(className) {
	var checklist = document.getElementsByClassName(className), i, ids; //hp-listitem - checkbox
    ids = "";
	var startsWith = "http";
    for (i = 0; i < checklist.length; i++) {
        if (checklist[i].id.length > 0 && (checklist[i].id.substring(0, 4) == startsWith)) {
            ids = ids + checklist[i].id + ";";
        }
    }
    return ids;
}

    function ExistButtonId(buttonId) {
        var buttonElement = document.getElementById(buttonId), existBtn;
        existBtn = true;
        if (buttonElement == null) {
            existBtn = false;
        }        
        return existBtn;
    }

