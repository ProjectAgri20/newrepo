function getDocumentCount() {
    var elements = document.getElementsByTagName("span");
    var count = 0;
    for (var i = 0; i < elements.length; i++) {
        if (elements[i].className == "checkbox") {
            count++;
        }
    }
    return count;
}

function getDocumentIds() {
	var checklist = document.getElementsByClassName("checkbox"), i, ids;
	ids = "";
	for (i = 0; i < checklist.length; i++) {
		if (checklist[i].id.length > 0 && checklist[i].id.startsWith("job-list-item")) {
			ids = ids + checklist[i].getAttribute("onclick") + "$";
		}
	}

	if (checklist.length == 0) {
		var html = document.documentElement.outerHTML;

		if (html.indexOf('Error') > 0) {
			ids = "-1";
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
