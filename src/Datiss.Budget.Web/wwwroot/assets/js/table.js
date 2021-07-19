/* 
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
applyEdit("tab1", [1, 2, 3, 4]); */

$(document).ready(function () {
    var table;

    LoadTable();
});

function LoadTable() {
	var table = new Tabulator("#WasteInstallFeeTable", {
		height: "311px",
		layout: "fitColumns",
		placeholder: "No Data Set",
		ajaxURL: '@Url.Action("Index", "WasteInstallFee", new { Area = "" })',

		columns: [
			{ title: "Name", field: "name", sorter: "string", width: 200 },
			{ title: "Progress", field: "progress", sorter: "number", formatter: "progress" },
			{ title: "Gender", field: "gender", sorter: "string" },
			{ title: "Rating", field: "rating", formatter: "star", hozAlign: "center", width: 100 },
			{ title: "Favourite Color", field: "col", sorter: "string" },
			{ title: "Date Of Birth", field: "dob", sorter: "date", hozAlign: "center" },
			{ title: "Driver", field: "car", hozAlign: "center", formatter: "tickCross", sorter: "boolean" },
		],
	});
    
}

function ReloadTable() {
    table.ajax.reload();
}

//var tableData = [
//	{ Year: , Organization: "تست یک", DWasteType: "1994", Fee:"male"},
//    {id:1, name:"تست ۲", start_year:"1994", year_type:"male", end_year:'1445'},
//]
//var table = new Tabulator("#WasteInstallFeeTable", {
//    layout:"fitColumns",      //fit columns to width of table
//    responsiveLayout:"hide",  //hide columns that dont fit on the table
//    tooltips:true,            //show tool tips on cells
//    addRowPos:"top",          //when adding a new row, add it to the top of the table
//    history:true,             //allow undo and redo actions on the table
//    pagination:"local",       //paginate the data
//    paginationSize:7,         //allow 7 rows per page of data
//    movableColumns:true,      //allow column order to be changed
//    resizableRows:true,       //allow row order to be changed
//    initialSort:[             //set the initial sort order of the data
//        {column:"name", dir:"asc"},
//    ],
//	data:tableData,
//    columns:[                 //define the table columns
//		{ title: "سال", field: "Year", editor: "input" },

//        {title:"نام", field:"name", hozAlign:"center", editor:"input"},
//		{title:"نوع سال", field:"year_type", width:95, editor:"select", editorParams:{values:[{'male':'کبیسه','female':'معمولی'}]}},

//        {title:"سال شروع", field:"start_year", width:95, editor:"date", hozAlign:"center"},
//        {title:"سال پایان", field:"end_year", width:130, sorter:"date", hozAlign:"center"},
//    ],
//});