function getDocumentCount() {
    var count = 0;
    var elements = document.getElementsByClassName("checkbox");
    for (var i = 1; i < elements.length; i++) {
        count++;
    }
    return count;
}


function SelectSingleDocument(className) {
    var elements = document.getElementsByClassName(className);
    if (elements.length > 1) {
        elements[1].className = "checkboxchecked";
    }
}


function SelectAllDocuments(elementId) {
    document.getElementById(elementId).click();
}



