import axios from "axios";

class AuthHandler {
  	login(username, email, password) {
    	return axios
      		.post("/api/identity/login", {
      		  	username,
      		  	email,
      		  	password
      		})

      		.then(response => {
        		if (response.data.result.jwtToken) {
        		  localStorage.setItem("user", JSON.stringify(response.data.result));
        		}
        		return response.data;
      		})

       		.catch(error => {
       		    switch(error.response.status) {
       		      case 500:
       		        throw error.response.data.error;
       		      case 400:
       		       	throw Object.entries(error.response.data.errors)[0][1][0];
       		    default:
       		        throw error.response.data;
       		    } 
       		})
  	}

  	register(username, email, password) {
  	  	return axios
  	  		.post("/api/identity/sign-up", {
  	  	  		username,
  	  	  		email,
  	  	  		password
  	  		})

  	  		.then(response => {
        		return response.data;
      		})

      		.catch(error => {
      			const str = JSON.stringify(Object.entries(error.response.data)[0][1])
      			throw str
      					.replace(/[\]}[{"]/g,'')
      					.replace(/[,]/g,'\n')
      		})
  	}

  	logout() {
  	  	localStorage.removeItem("user");
  	}

  	getCurrentUser() {
  		const user = JSON.parse(localStorage.getItem('user'))

	    if (user && new Date(user.jwtToken.expirationDate).getTime() < Date.now()) {
	    	localStorage.removeItem("user")
	  		return null
	    }
	  	
	    else if (!user) {
	    	return null
	    }

	    else {
	    	return user
	    }
	}

	forgotPwd(username, email) {
		return axios
  	  		.post("/api/identity/forgot-password", {
  	  	  		username,
  	  	  		email
  	  		})

  	  		.then(response => {
        		return response.data;
      		})

      		.catch(error => {
      			throw error.response.data.error;
			})
    }
}

export default new AuthHandler();