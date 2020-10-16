//
// api.js - Fetch
//

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