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

let userId = '';

if (window.location.href.indexOf("user-management") > -1) {
	$("#deleteusertoolbar, #infousertoolbar").prop('disabled', true);
	getAllUsers();
}

// ===== Selector user =====
$('#tableUser').on('check.bs.table', function(e, row, element) {
	$("#deleteusertoolbar, #infousertoolbar").prop('disabled', false);
	$('#userselected').html(row.username);
	$('#roleselected').html(row.role);
	userId = row.userId;
})

// ===== Button for add user ===== 
$(document).on('click', '#btn-adduser', function(event) {
	   	event.preventDefault();
	   	let username = $('#username-input').val();
	   	let email = $('#email-input').val();
	   	let password = $('#password-input').val();
	   	let role = $('#role-input').val();
	   	addUser(username,email,password,role);
	}
);

$('#addusermodal').on('hidden.bs.modal', function () {
	location.reload();
})

// ===== Button for remove user ===== 
$(document).on('click', '#btn-removeuser', function(event) {
	   	event.preventDefault();
	   	removeUser(userId);
	}
);

// ===== Button for remove user ===== 
$(document).on('click', '#infousertoolbar', function(event) {
	   	event.preventDefault();
	   	infoUser(userId);
	}
);


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