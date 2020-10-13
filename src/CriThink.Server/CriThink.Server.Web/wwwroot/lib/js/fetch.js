function loginAPI(username,email,password,errorEl,clickEl) {
	$('#'+clickEl).html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>');
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
			console.log('Success:', data);
			$(errorEl).addClass('d-none');
		}
  		else {
  			$(errorEl).removeClass('d-none');
  		}
	})
	.then(() => {
		$('#'+clickEl).html('Login');
	});
}

$(document).on('click', '#btn-login', function(event) {
	   	event.preventDefault();
	   	const username = $('#inputemail').val();
	   	const password = $('#inputpwd').val();
	   	username.includes('@')? loginAPI('',username,password,'#errordiv', event.target.id) : loginAPI(username,'',password,'#errordiv', event.target.id);
	}
);