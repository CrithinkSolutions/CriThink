//
// api.js - Fetch
//

// ============================= Login

// ===== Button for login request ===== 
$(document).on('click', '#btn-login', function(event) {
	   	event.preventDefault();
	   	const username = $('#inputemail').val();
	   	const password = $('#inputpwd').val();
	   	username.includes('@')? loginAPI('',username,password) : loginAPI(username,'',password);
	}
);

// ===== Login request =====
function loginAPI(username,email,password) {
	$('#btn-login').html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>');
	fetch('/api/identity/login', {
		method: 'POST',
		headers: {
    		'Content-Type': 'application/json',
  		},
  		body: JSON.stringify({
  			'email': email,
  			'username': username,
  			'password': password
  		}),
	})
	.then(response => response.json())
	.then(data => {
		if(data.statusCode == 200) {
			const expires = new Date(data.result.token.expirationDate);
			document.cookie = "username=" + data.result.username + "; expires=" + expires.toUTCString();
			document.cookie = "token=" + data.result.token.token + "; expires=" + expires.toUTCString();
			$('#errordiv').addClass('d-none');
			window.location.href="/control-panel";
		}
  		else {
  			$('#errordiv').removeClass('d-none');
  		}
	})
	.then(() => {
		$('#btn-login').html('Login');
	});
}

// ============================= Debunking News

// ===== Get News Source =====
function getAllNewsSource(){
	fetch('/api/news-source/all')
		.then(response => response.json())
		.then(data => $('#table').bootstrapTable({data: data}));
}

// ===== Add News Source (JWT) =====
function addNewsSource(uri, classification) {
	fetch('/api/news-source', {
		method: 'POST',
		headers: {
    		'Content-Type': 'application/json',
    		'Authorization': 'Bearer '+selectCookie('token')
  		},
  		body: JSON.stringify({
  			'uri': uri,
  			'classification': classification
  		}),
	})
	.then(response => {
		if(response.status == 204) {
			alert("Success")
		}
	})
}

// ===== Remove News Source (JWT) =====
function removeNewsSource(uri, classification) {
	let apiUri = ''
	switch (classification) {
		case 'Reliable':
		case 'Satirical':
			apiUri = '/api/news-source/whitelist';
			break;
		case 'Conspiracist':
		case 'FakeNews':
			apiUri = '/api/news-source/blacklist';
			break;
	}
	fetch(apiUri, {
		method: 'DELETE',
		headers: {
    		'Content-Type': 'application/json',
    		'Authorization': 'Bearer '+selectCookie('token')
  		},
  		body: JSON.stringify({
  			'uri': 'http://'+uri,
  			'classification': classification
  		}),
	})
	.then(response => {
		if(response.status == 204) {
			alert("Success")
			location.reload();
		}
	})
}

// ============================= User Management

// ===== Get all users (JWT) =====
function getAllUsers(){
	fetch('/api/admin/user/all?PageSize=30&PageIndex=1', {
		method: 'GET',
		headers: {
    		'Content-Type': 'application/json',
    		'Authorization': 'Bearer '+selectCookie('token')
  		},
	})
		.then(response => response.json())
		.then(data => $('#tableUser').bootstrapTable({
			data: data.result,
			sortStable: true
		}));
}

// ===== Add User (JWT) =====
function addUser(username, email, password, role) {
	let roleUri = ''
	switch (role) {
		case 'No role':
			roleUri = '/api/identity/sign-up';
			break;
		case 'Admin':
			roleUri = '/api/admin/sign-up';
			break;
	}
	fetch(roleUri, {
		method: 'POST',
		headers: {
    		'Content-Type': 'application/json',
    		'Authorization': 'Bearer '+selectCookie('token')
  		},
  		body: JSON.stringify({
  			'username': username,
  			'email': email,
  			'password': password
  		}),
	})
	.then(response => {
		if(response.status == 200) {
			alert("Success")
		}
	})
}

// ===== Remove User (JWT) =====
function removeUser(userId, mode) {
	let deleteMethod = '';
	switch (mode) {
		case 'hard':
			deleteMethod = 'DELETE';
			break;
		case 'soft':
			deleteMethod = 'PATCH';
			break;
	}
	fetch('/api/admin/user', {
		method: deleteMethod,
		headers: {
    		'Content-Type': 'application/json',
    		'Authorization': 'Bearer '+selectCookie('token')
  		},
  		body: JSON.stringify({
  			'userId': userId
  		}),
	})
	.then(response => {
		if(response.status == 204) {
			alert("Success")
			location.reload();
		}
	})
}

// ===== Info User (JWT) =====
async function infoUser(userId) {
	const res = await fetch('/api/admin/user?' + new URLSearchParams({
    	UserId: userId,
    	}), {
		method: 'GET',
		headers: {
    		'Content-Type': 'application/json',
    		'Authorization': 'Bearer '+selectCookie('token')
  		}
	});
	return res.json();
}

// ===== Edit User (JWT) =====
async function editUser(userId, emailconfirmed, lockoutenabled, lockoutenddate, role) {
	let body = {
		'userId': userId,
		'isEmailConfirmed': emailconfirmed,
		'isLockoutEnabled': lockoutenabled,
		'lockoutEnd': lockoutenddate
	} 
	await fetch('/api/admin/user', {
		method: 'PUT',
		headers: {
    		'Content-Type': 'application/json',
    		'Authorization': 'Bearer '+selectCookie('token')
  		},
  		body: JSON.stringify(Object.entries(body).reduce((a,[k,v]) => (v === '' ? a : (a[k]=v, a)), {})),
	})
	.then(response => {
		if(response.status == 204) {
			alert("Success")
		}
	})
	await fetch('/api/admin/user/role', {
		method: 'PATCH',
		headers: {
    		'Content-Type': 'application/json',
    		'Authorization': 'Bearer '+selectCookie('token')
  		},
  		body: JSON.stringify({
  			'userId': userId,
			'role': role
  		}),
	})
	.then(response => {
		if(response.status == 204) {
			alert("Role Success")
		}
	})
}

// ===== Get all roles (JWT) =====
function getAllRoles(){
	fetch('/api/admin/role', {
		method: 'GET',
		headers: {
    		'Content-Type': 'application/json',
    		'Authorization': 'Bearer '+selectCookie('token')
  		},
	})
		.then(response => response.json())
		.then(data => $('#tableRole').bootstrapTable({data: data.result}))
		.then(() => $('#tableRole').parents('.bootstrap-table').hide());
}

// ===== Add role (JWT) =====
function addRole(name) {
	fetch('/api/admin/role', {
		method: 'POST',
		headers: {
    		'Content-Type': 'application/json',
    		'Authorization': 'Bearer '+selectCookie('token')
  		},
  		body: JSON.stringify({
  			'name': name
  		}),
	})
	.then(response => {
		if(response.status == 204) {
			alert("Success")
		}
	})
}

// ===== Delete role (JWT) =====
function deleteRole(name) {
	fetch('/api/admin/role', {
		method: 'DELETE',
		headers: {
    		'Content-Type': 'application/json',
    		'Authorization': 'Bearer '+selectCookie('token')
  		},
  		body: JSON.stringify({
  			'name': name
  		}),
	})
	.then(response => {
		if(response.status == 204) {
			alert("Success")
			location.reload();
		}
	})
}

// ===== Rename role (JWT) =====
function renameRole(oldname, name) {
	fetch('/api/admin/role', {
		method: 'PATCH',
		headers: {
    		'Content-Type': 'application/json',
    		'Authorization': 'Bearer '+selectCookie('token')
  		},
  		body: JSON.stringify({
  			'oldName': oldname,
  			'newName': name
  		}),
	})
	.then(response => {
		if(response.status == 204) {
			alert("Success")
			location.reload();
		}
	})
}