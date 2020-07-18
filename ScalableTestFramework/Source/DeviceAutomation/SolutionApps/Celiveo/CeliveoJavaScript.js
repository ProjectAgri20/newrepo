function ExistButtonId(buttonId) {
	var buttonElement = document.getElementById(buttonId), existBtn;
	existBtn = true;
	if (buttonElement == null) {
		existBtn = false;
	}
	return existBtn;
}

function getDocumentCount() {
    var elements = document.getElementsByTagName("hp-listitem");
    var count = 0;
    var idStart = "bxD-";

    for (var i = 0; i < elements.length; i++) {
        if (elements[i].id && (elements[i].id.substring(0, idStart.length) == idStart)) {
            count++;
        }
    }
    return count;
}

function getDocumentIds() {
    var checklist = document.getElementsByTagName("hp-listitem"), i, ids;
    ids = "";
    var idStart = "bxD-";

    for (i = 0; i < checklist.length; i++) {
        if (checklist[i].id && (checklist[i].id.substring(0, idStart.length) == idStart)) {
            ids = ids + checklist[i].id + "$";
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