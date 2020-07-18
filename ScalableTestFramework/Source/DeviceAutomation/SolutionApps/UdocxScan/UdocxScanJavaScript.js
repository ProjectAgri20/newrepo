function getDocumentCount() {
    var elem = document.getElementById("js-job-count").innerHTML;
    return elem;
}

function getDocumentIds() {
    var checklist = document.getElementsByClassName("tf-list-item"), i, ids;
    ids = "";
    var idStart = "job-";

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

function getElementByText(tagname, controlValue) {
    var allElements = document.getElementsByClassName(tagname);
    for (i = 0; i < allElements.length; i++) {
        if (allElements[i].innerHTML.indexOf(controlValue) !== -1) {
            break;
        }
    }
    allElements[i].scrollIntoView();
    return allElements[i];
}