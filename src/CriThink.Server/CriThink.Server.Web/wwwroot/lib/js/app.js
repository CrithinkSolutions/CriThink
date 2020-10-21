//
// app.js - View
//

// ============================= Control Panel

$('#welcome').html(selectCookie('username'));

// ============================= Debunking News

if (window.location.href.indexOf("debunking-news") > -1) {
	$("#deletenewstoolbar").prop('disabled', true);
	getAllNewsSource();
}

$('#addnewsmodal').on('hidden.bs.modal', function () {
	location.reload();
})

// ===== Selector news source =====
$('#table').on('check.bs.table', function(e, row, element) {
	$("#deletenewstoolbar").prop('disabled', false);
	$('#uriselected').html(row.uri);
	$('#classificationselected').html(row.classification);
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

// ============================= User Management

if (window.location.href.indexOf("user-management") > -1) {
	$("#deleteusertoolbar").prop('disabled', true);
	getAllUsers()
}

// ===== Button for add user ===== 
$(document).on('click', '#btn-adduser', function(event) {
	   	event.preventDefault();
	   	const username = $('#username-input').val();
	   	const email = $('#email-input').val();
	   	const password = $('#password-input').val();
	   	const role = $('#role-input').val();
	   	addUser(username,email,password,role);
	}
);

$('#addusermodal').on('hidden.bs.modal', function () {
	location.reload();
})


// ============================= Utility
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