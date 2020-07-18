function getDocumentCount() {
    var docIds = document.getElementsByClassName("checkbox"), i;
    var count = 0;
    for (i = 0; i < docIds.length; i++) {
        if (docIds[i].id.length > 0){
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

function getDocumentNameById(documentId) {
    var docElement = document.getElementById(documentId), docName;
    docName = "No Element";
    if (docElement != null) {
        docName = docElement.innerText;
    }
    return docName;
}

function selectAllDocuments(className) {
    //"checkbox"
    var checklist = document.getElementsByClassName(className), i;
    for (i = 0; i < checklist.length; i++) {
        if (checklist[i].id.length > 0) {
            checklist[i].onclick.apply(checklist[i]);
        }
    }
}

function selectFirstDocument(className) {
    //"checkbox"
    var checklist = document.getElementsByClassName(className);
    if (checklist.length > 0) {
        for (var i = 0; i < checklist.length; i++) {
            if (checklist[i].id.length > 0) {
                checklist[i].onclick.apply(checklist[i]);
                break;
            }
        }
    }
}

function getDocumentIds(className) {
    //"hp-listitem"
    var checklist = document.getElementsByClassName(className), i, ids;
    ids = "";
        for (i = 0; i < checklist.length; i++) {
            if (checklist[i].id.length > 0) {
                ids = ids + checklist[i].id + ";";
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