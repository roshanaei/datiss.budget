
function getStyle(el, cssprop) {
	if (el.currentStyle)
		return el.currentStyle[cssprop];	 // IE
	else if (document.defaultView && document.defaultView.getComputedStyle)
		return document.defaultView.getComputedStyle(el, "")[cssprop];	// Firefox
	else
		return el.style[cssprop]; //try and get inline style
}

function applyEdit(tabID, editables) {
	var tab = document.getElementById(tabID);
	if (tab) {
		var rows = tab.getElementsByTagName("tr");
		for(var r = 0; r < rows.length; r++) {
			var tds = rows[r].getElementsByTagName("td");
			for (var c = 0; c < tds.length; c++) {
				if (editables.indexOf(c) > -1)
					tds[c].onclick = function () { beginEdit(this); };
			}
		}
	}
}
var oldColor, oldText, padTop, padBottom = "";
function beginEdit(td) {

	if (td.firstChild && td.firstChild.tagName == "INPUT")
		return;

	oldText = td.innerHTML.trim();
	oldColor = getStyle(td, "backgroundColor");
	padTop = getStyle(td, "paddingTop");
	padBottom = getStyle(td, "paddingBottom");

	var input = document.createElement("input");
	input.value = oldText;

	//// ------- input style -------
	var left = getStyle(td, "paddingLeft").replace("px", "");
	var right = getStyle(td, "paddingRight").replace("px", "");
	input.style.width = td.offsetWidth - left - right - (td.clientLeft * 2) - 2 + "px";
	input.style.height = td.offsetHeight - (td.clientTop * 2) - 2 + "px";
	input.style.border = "0px";
	input.style.fontFamily = "inherit";
	input.style.fontSize = "inherit";
	input.style.textAlign = "inherit";
	input.style.backgroundColor = "LightGoldenRodYellow";

	input.onblur = function () { endEdit(this); };

	td.innerHTML = "";
	td.style.paddingTop = "0px";
	td.style.paddingBottom = "0px";
	td.style.backgroundColor = "LightGoldenRodYellow";
	td.insertBefore(input, td.firstChild);
	input.select();
}
function endEdit(input) {
	var td = input.parentNode;
	td.removeChild(td.firstChild);	//remove input
	td.innerHTML = input.value;
	if (oldText != input.value.trim() )
		td.style.color = "red";

	td.style.paddingTop = padTop;
	td.style.paddingBottom = padBottom;
	td.style.backgroundColor = oldColor;
}
applyEdit("tab1", [1, 2, 3, 4]);