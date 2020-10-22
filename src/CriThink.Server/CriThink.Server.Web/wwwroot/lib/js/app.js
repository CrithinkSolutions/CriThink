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
	$("#deleteusertoolbar, #infousertoolbar, #editusertoolbar").prop('disabled', true);
	getAllUsers();
}

// ===== Selector user =====
$('#tableUser').on('check.bs.table', function(e, row, element) {
	$("#deleteusertoolbar, #infousertoolbar, #editusertoolbar").prop('disabled', false);
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

$('#addusermodal, #editusermodal').on('hidden.bs.modal', function () {
	location.reload();
})

// ===== Button for remove user ===== 
$(document).on('click', '#btn-removeuser', function(event) {
	   	event.preventDefault();
	   	removeUser(userId);
	}
);

// ===== Button for info user ===== 
$(document).on('click', '#infousertoolbar', function(event) {
	   	event.preventDefault();
		infoUser(userId).then(data => {
			$('#infoname').html(data.result.username);
			$('#infobody').html(JSON.stringify(data.result, null, 2));
		});
	}
);

// ===== Get info for edit user ===== 
$(document).on('click', '#editusertoolbar', function(event) {
	   	event.preventDefault();
	   	infoUser(userId).then(data => {
	   		$('#LE-editinput').val(data.result.lockoutEnd);
	   		data.result.isEmailConfirmed? $('#radioEYes').prop('checked', true) : $('#radioENo').prop('checked', true);
	   		data.result.isLockoutEnabled? $('#radioLYes').prop('checked', true) : $('#radioLNo').prop('checked', true);
	   		$('#role-editinput').val(data.result.role[0]);
	   	});
	}
);

// ===== Button for edit user ===== 
$(document).on('click', '#btn-edituser', function(event) {
	   	event.preventDefault();
	   	let emailconfirmed = Boolean($('input[name=checkEmail]:checked').val());
	   	let lockoutenabled = Boolean($('input[name=checkLockout]:checked').val());
	   	let lockoutenddate = $('#LE-editinput').val();
	   	let role = $('#role-editinput').val();
	   	editUser(userId, emailconfirmed, lockoutenabled, lockoutenddate, role);
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