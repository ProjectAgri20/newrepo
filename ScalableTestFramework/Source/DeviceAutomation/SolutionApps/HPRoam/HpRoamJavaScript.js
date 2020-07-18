function getSelectedDocumentCount() {
    var documents = document.getElementById("job-queue-list").getElementsByClassName("selected");
    var count = documents.length;

    return count;
}

function getDocumentCount() {
    var documents = document.getElementById("job-queue-list").getElementsByTagName("li");
    var count = documents.length;

    return count;
}

function getJobProcessingCount() {
    var printing = document.getElementById("job-queue-list").getElementsByClassName("job-processing");
    var count = printing.length;

    return count;
}

function ExistButtonId(buttonId) {
    var buttonElement = document.getElementById(buttonId), existBtn;
    existBtn = true;
    if (buttonElement == null) {
        existBtn = false;
    }
    return existBtn;
}

function isElementSelected(elementId) {
    var element = document.getElementById(elementId);
    var selected = element.className.indexOf("selected");

    if (selected >= 0) {
        return true;
    }

    return false;
}

function getRoamAlertText() {
    var element = document.getElementsByClassName("modal-content-label").item(0);

    return element.innerHTML;
}
