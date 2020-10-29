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
	$("#deletetoolbar, #infotoolbar, #edittoolbar").prop('disabled', true);
	getAllUsers(30);
	getAllRoles();	
}

// ===== Selector user =====
$('#tableUser').on('check.bs.table', function(e, row, element) {
	$('#deletetoolbar, #infotoolbar, #edittoolbar').prop('disabled', false);
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
	   	let mode = $('input[name=modeDelete]:checked').val();
	   	removeUser(userId, mode);
	}
);

// ===== Button for info user ===== 
$(document).on('click', '#infotoolbar', function(event) {
	   	event.preventDefault();
		infoUser(userId).then(data => {
			$('#infoname').html(data.username);
			$('#infobody').html(JSON.stringify(data, null, 2));
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

// ===== Button for update settings user ===== 
$(document).on('click', '#btn-settinguser', function(event) {
	   	event.preventDefault();
	   	let num = $('#displayuser-input').val()
	   	getAllUsers(num);
	}
);

// ===== Tab Users/Roles handler ===== 
$(document).on('click', '#users-tab', function(event) {
	$('#tableUser').parents('.bootstrap-table').show();
	$('#tableRole').parents('.bootstrap-table').hide();
	$('#addtoolbar').attr('data-target', '#addusermodal');
	$('#deletetoolbar').attr('data-target', '#removeusermodal');
	$('#edittoolbar').attr('data-target', '#editusermodal');
});

$(document).on('click', '#roles-tab', function(event) {
	$('#tableUser').parents('.bootstrap-table').hide();
	$('#tableRole').parents('.bootstrap-table').show();
	$('#deletetoolbar, #edittoolbar, #infotoolbar').prop('disabled', true);
	$('#addtoolbar').attr('data-target', '#addrolemodal');
	$('#deletetoolbar').attr('data-target', '#removerolemodal');
	$('#edittoolbar').attr('data-target', '#editrolemodal');
});

// ============================= Role Management

// ===== Selector role =====
$('#tableRole').on('check.bs.table', function(e, row, element) {
	$("#deletetoolbar, #edittoolbar").prop('disabled', false);
	$('.rolename').html(row.name);
})

$('#addrolemodal, #editrolemodal').on('hidden.bs.modal', function () {
	location.reload();
})

// ===== Button for add role ===== 
$(document).on('click', '#btn-addrole', function(event) {
	   	event.preventDefault();
	   	let name = $('#addrole-input').val();
	   	addRole(name);
	}
);

// ===== Button for remove role ===== 
$(document).on('click', '#btn-removerole', function(event) {
	   	event.preventDefault();
	   	let name = $('.rolename').html();
	   	deleteRole(name);
	}
);

// ===== Button for edit role ===== 
$(document).on('click', '#btn-editrole', function(event) {
	   	event.preventDefault();
	   	let oldname = $('.rolename').html();
	   	let name = $('#newrolename-input').val();
	   	renameRole(oldname,name);
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

function loadingTemplate(message) {
    return '<div class="spinner-grow" role="status"><span class="sr-only">Loading...</span></div>'
}