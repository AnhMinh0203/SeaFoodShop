async function login() {
  const url = 'https://localhost:7018/api/Account/SignIn';

  const phoneNumber = $('#phoneNumberLogin').val();
  const password = $('#passwordLogin').val();

  const loginData = {
      phoneNumber: phoneNumber,
      password: password
  };

  try {
      const response = await $.ajax({
          url: url,
          type: 'POST',
          contentType: 'application/json',
          data: JSON.stringify(loginData)
      });
      if (response && response.token) {
          window.location.href = 'home.html';
        console.log(response)
      }
  } catch (error) {
      console.error('Unable to get items.', error);
  }
}
