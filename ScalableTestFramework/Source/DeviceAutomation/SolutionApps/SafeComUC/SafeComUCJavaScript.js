function selectFirstDocument(className) {
    //"checkbox"
    var checklist = document.getElementsByClassName(className);
    if (checklist.length > 0) {
        for (var i = 0; i < checklist.length; i++) {            
            checklist[i].onclick.apply(checklist[i]);
            break;            
        }
    }
}

function getDocumentCounts(className) {
    var checklist = document.getElementsByClassName(className);
    return checklist.length;
}

function getDocumentIds(className) {
    
    var checklist = document.getElementsByClassName(className), i, ids;
    ids = "";
    for (i = 0; i < checklist.length; i++) {
        if (checklist[i].id.length > 0) {
            ids = ids + checklist[i].id + ";";
        }
    }

    if (checklist.length === 0) {
        var html = document.documentElement.outerHTML;

        if (html.indexOf('Error') > 0) {
            ids = "-1";
        }
    }

    return ids;
}

function getDocumentNames(className) {    
    var checklist = document.getElementsByClassName(className), i, names;
    names = "";
    for (i = 0; i < checklist.length; i++) {
        if (checklist[i].childElementCount > 0) {
            names = names + checklist[i].firstChild.innerHTML + ";";
        }
    }

    if (checklist.length === 0) {
        var html = document.documentElement.outerHTML;

        if (html.indexOf('Error') > 0) {
            names = "-1";
        }
    }

    return names;
}

function isCheckBoxChecked(id) {

    var element = document.getElementById(id);

    if (element.className.indexOf('checkboxChecked') > 0) {
        return true;
    }

    return false;
}