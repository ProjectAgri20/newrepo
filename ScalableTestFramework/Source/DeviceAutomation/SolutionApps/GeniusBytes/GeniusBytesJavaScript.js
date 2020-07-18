function getElementIdbyText(tagname, testtext) {
    var elements = document.querySelectorAll(tagname);
    var ids;
    testtext = testtext.replace(/>/g, '&gt;').replace(/&/g, '&amp;');
    var count = 0;

    for (i = 0; i < elements.length; i++) {
        if (elements[i].innerHTML && (elements[i].innerHTML == testtext)) {
            ids = elements[i].id;
            break;
        }
    }
    return ids;
}

function getElementLastIdbyText(tagname, testtext) {
    var elements = document.querySelectorAll(tagname);
    var ids;
    testtext = testtext.replace(/>/g, '&gt;').replace(/&/g, '&amp;');
    var count = 0;

    for (i = 0; i < elements.length; i++) {
        if (elements[i].innerHTML && (elements[i].innerHTML == testtext)) {
            ids = elements[i].id;
        }
    }
    return ids;
}

function getElementIdbyTextContains(tagname, testtext) {
    var elements = document.querySelectorAll(tagname);
    var ids;
    testtext = testtext.replace(/>/g, '&gt;').replace(/&/g, '&amp;');

    for (i = 0; i < elements.length; i++) {
        if (elements[i].innerHTML && (elements[i].innerHTML.indexOf(testtext) !== -1)) {
            ids = elements[i].id;
            break;
        }
    }
    return ids;
}

function getElementIDbyZIndex(tagname, zindex) {
    var elements = document.querySelectorAll(tagname);
    var ids;

    for (i = 0; i < elements.length; i++) {
        if (elements[i].style && elements[i].style.zIndex === zindex) {
            ids = elements[i].id;
            break;
        }
    }
    return ids;
}

function getElementIDbyImageName(tagname, imagename) {
    var elements = document.querySelectorAll(tagname);
    var ids;

    for (i = 0; i < elements.length; i++) {
        if (elements[i].style && elements[i].style.backgroundImage.indexOf(imagename) !== -1) {
            ids = elements[i].id;
            break;
        }
    }
    return ids;
}

function getInnerHTMLbyTextContains(tagname, testtext) {
    var elements = document.querySelectorAll(tagname);
    var text;

    for (i = 0; i < elements.length; i++) {
        if (elements[i].innerHTML && (elements[i].innerHTML.indexOf(testtext) !== -1)){
            text = elements[i].innerHTML;
            break;
        }
    }
    return text;
}

function getInnerHTMLbyTextContainsforDocumentCount(tagname, testtext) {
    var elements = document.querySelectorAll(tagname);
    var text;
	
    for (i = 0; i < elements.length; i++) {
        if (elements[i].innerHTML && (elements[i].innerHTML.indexOf(testtext) !== -1)) {
            text = elements[i].innerHTML;            
        }
    }
    return text;
}

function getElementIdbyTextandZIndex(tagname, testtext, zindex) {
    var elements = document.querySelectorAll(tagname);
    var ids;
    testtext = testtext.replace(/>/g, '&gt;').replace(/&/g, '&amp;');

    for (i = 0; i < elements.length; i++) {
        if (elements[i].innerHTML && (elements[i].innerHTML == testtext)) {
            var pElement = elements[i];

            while (pElement.parentElement) {
                pElement = pElement.parentElement;
                if (pElement.style.zIndex) {
                    if (pElement.style.zIndex === zindex) {
                        ids = elements[i].id
                        break;
                    }
                }
            }
        }
    }
    return ids;
}

function getElementIdbyTextContainsandZIndex(tagname, testtext, zindex) {
    var elements = document.querySelectorAll(tagname);
    var ids;
    testtext = testtext.replace(/>/g, '&gt;').replace(/&/g, '&amp;');

    for (i = 0; i < elements.length; i++) {
        if (elements[i].innerHTML && (elements[i].innerHTML.indexOf(testtext) !== -1)) {
            var pElement = elements[i];

            while (pElement.parentElement) {
                pElement = pElement.parentElement;

                if (pElement.zIndex) {
                    if (pElement.zIndex === zindex) {
                        ids = elements[i].id
                        break;
                    }
                }
            }
        }
    }
    return ids;
}

function getElementIdofFirstTimeFormat(tagname) {
    var elements = document.querySelectorAll(tagname);
    var regex = /([01]\d|2[0-3]):?([0-5]\d):?([0-5]\d)/;
    var ids;

    for (i = 0; i < elements.length; i++) {
        if (elements[i].innerHTML && regex.test(elements[i].innerHTML)) {
            {
                ids = elements[i].id;
                break;
            }
        }
    }
    return ids;
}

function getElementDisplayValue(elementId) {
	var value;
		value = document.getElementById(elementId).style.display;
	return value;
}

function ExistElementId(elementId) {
	var element = document.getElementById(elementId), existElement;
	existElement = true;
	if (element == null) {
		existElement = false;
	}
	return existElement;
}
