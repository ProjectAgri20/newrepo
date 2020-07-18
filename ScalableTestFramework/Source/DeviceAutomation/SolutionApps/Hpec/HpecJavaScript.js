function getButtons(pClassName) {
    var buttons = document.getElementsByClassName(pClassName);
    return buttons;
}

function getButtonByInnerText(pText) {
    var buttons = getButtons("button");
    for (i = 0; i < buttons.length; i++) {
        var button = buttons[i];
        if (button.innerHTML.toLowerCase().indexOf(pText.toLowerCase()) > -1) {
            return button;
            break;
        }
    }
}

function pressScanButton(buttonText) {
    var button = getButtonByInnerText(buttonText);
    if (typeof button.onmousedown == "function") {
        button.onmousedown.apply(button);
    }
}

function getButtonByText(pText) {
    var buttons = getButtons("labelOXPd");
    for (i = 0; i < buttons.length; i++) {
        var button = buttons[i];
        if (button.innerHTML.toLowerCase().indexOf(pText.toLowerCase()) > -1) {
            return button.parentNode;
            break;
        }
    }
}

function pressWorkflowButton(buttonText) {
    var button = getButtonByText(buttonText);
    if (typeof button.onmousedown == "function") {
        button.onmousedown.apply(button);
    }
}