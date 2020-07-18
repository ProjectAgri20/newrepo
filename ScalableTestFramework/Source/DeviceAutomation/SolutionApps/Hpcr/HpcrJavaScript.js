
    function ExistButtonId(buttonId) {
        var buttonElement = document.getElementById(buttonId), existBtn;
        existBtn = true;
        if (buttonElement == null) {
            existBtn = false;
        }        
        return existBtn;
    }

	function getOptionIds(){
	  var checklist = document.querySelectorAll('.hp-listitem', '.lg'), i, ids;
	  if(checklist.length > 0){
	  	  for(i = 0; i < checklist.length; i++){
		  	  if(checklist[i].id.length > 0){
			  	  ids = ids + checklist[i].id + "-" + checklist[i].innerText + ";";
			  }
		  }
	  }
	  return ids;
	}


function existclassName() {

	var bExist = false;

	if (document.querySelectorAll(".hp-listitem-text", ".lx") != null) {
		bExist = true;
	}
		return bExist;
}

function getDocumentCount() {
	var elements = document.getElementsByTagName(".hp-list.xl");
    var count = 0;
	var temp = "";
    for (var i = 0; i < elements.length; i++) {
        if (elements[i].length > 0) {
            count++;
			temp += elements[0];
        }
    }
	temp += count;
    return temp;
	}


