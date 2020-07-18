function getDocumentCount() {
	var documents = document.getElementById("JobsDynamicCheckList").getElementsByClassName("checkbox");

	var count = documents.length;

	if (count == 0) {
		var html = document.documentElement.outerHTML;

		if (html.indexOf('Error') > 0) {
			count = -1;
		}		
	}

	return count;
}

function selectAllDocuments() {
    var checklist = document.getElementById("JobsDynamicCheckList").getElementsByClassName("checkbox");
    for (var i = 0; i < checklist.length; i++) {
        checklist[i].onclick.apply(checklist[i]);
    }
}

function selectFirstDocument() {
    var checklist = document.getElementById("JobsDynamicCheckList").getElementsByClassName("checkbox");
    if (checklist.length > 0) {
        checklist[0].onclick.apply(checklist[0]);
    }
}