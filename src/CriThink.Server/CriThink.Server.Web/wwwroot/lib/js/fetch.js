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
			const expires = new Date(data.result.token.expirationDate);
			document.cookie = "username=" + data.result.username + "; expires=" + expires.toUTCString();
			document.cookie = "token=" + data.result.token.token + "; expires=" + expires.toUTCString();
			$(errorEl).addClass('d-none');
			window.location.href="/control-panel";
		}
  		else {
  			$(errorEl).removeClass('d-none');
  		}
	})
	.then(() => {
		$('#'+clickEl).html('Login');
	});
}

function getAllNewsSource(){
	fetch('/api/news-source/all')
		.then(response => response.json())
		.then(data => $('#table').bootstrapTable({data: data}))
}


function addNews(uri, classification) {
	fetch('/api/news-source', {
		method: 'POST',
		headers: {
    		'Content-Type': 'application/json',
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


$(document).on('click', '#btn-login', function(event) {
	   	event.preventDefault();
	   	const username = $('#inputemail').val();
	   	const password = $('#inputpwd').val();
	   	username.includes('@')? loginAPI('',username,password,'#errordiv', event.target.id) : loginAPI(username,'',password,'#errordiv', event.target.id);
	}
);

if (window.location.href.indexOf("debunking-news") > -1) {
	getAllNewsSource();
}

$(document).on('click', '#btn-addnews', function(event) {
	   	event.preventDefault();
	   	const uri = $('#uri-input').val();
	   	const classification = $('#class-input').val();
	   	addNews(uri, classification);
	}
);

$('#addnewsmodal').on('hidden.bs.modal', function () {
	location.reload();
})