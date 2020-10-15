//
// app.js - View
//

// ===== Control panel page =====
$('#welcome').html(selectCookie('username'));

// ===== Debunking news page =====
if (window.location.href.indexOf("debunking-news") > -1) {
	getAllNewsSource();
}

$('#addnewsmodal').on('hidden.bs.modal', function () {
	location.reload();
})

// ===== Button for add news source ===== 
$(document).on('click', '#btn-addnews', function(event) {
	   	event.preventDefault();
	   	const uri = $('#uri-input').val();
	   	const classification = $('#class-input').val();
	   	addNewsSource(uri, classification);
	}
);

// ===== Button for remove news source ===== 
$(document).on('click', '#btn-removenews', function(event) {
	   	event.preventDefault();
	   	const uri = $('#uriselected').html();
	   	const classification = $('#classificationselected').html();
	   	removeNewsSource(uri,classification)
	}
);

// ===== Selector news source =====
$('#table').on('check.bs.table', function(e, row, element) {
	$('#uriselected').html(row.uri);
	$('#classificationselected').html(row.classification);
})


// ===== Utility =====
function selectCookie(value){
	const c = document.cookie
	if (c.includes(value)){
		const d = c.split('; ')
		.find(row => row.startsWith(value))
		.split('=')[1];
		return d;
	} else { 
		alert("Expired Session");
		window.location.href="/";
	}
}