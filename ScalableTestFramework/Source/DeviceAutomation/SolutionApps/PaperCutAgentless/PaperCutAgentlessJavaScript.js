function getDocumentCount() {
    var elements = document.getElementsByTagName("div");
    var count = 0;

    for (i = 0; i < elements.length; i++) {
        if (elements[i].id.indexOf("job-button-") !== -1) {
            count++
        }
    }
    return count;
}

function getDocumentIds() {
    var checklist = document.getElementsByTagName("div");
    var ids = "";
    var idStart = "job-button-";

    for (i = 0; i < checklist.length; i++) {
        if (checklist[i].id.length > 0 && checklist[i].id.substring(0, idStart.length) == idStart) {
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

function getDocumentCheckboxIds() {
    var checklist = document.getElementsByTagName("input");
    var ids = "";
    var idStart = "checkbox-";

    for (i = 0; i < checklist.length; i++) {
        if (checklist[i].id.length > 0 && checklist[i].id.substring(0, idStart.length) == idStart) {
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

function getSingleJobOptionsIdbyDocumentIds(id) {
    var checklist = document.getElementById(id).childNodes;
    var idStart = "button-";

    for (i = 0; i < checklist.length; i++) {
        if (checklist[i].id && checklist[i].id.substring(0, idStart.length) == idStart) {
            return checklist[i].id;
        }
    }
}