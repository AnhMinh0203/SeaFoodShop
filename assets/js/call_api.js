function login(){
    const url = 'https://localhost:7018/api/Account/SignIn';

    var phoneNumber = document.getElementById('phoneNumberLogin').value;
    var password = document.getElementById('passwordLogin').value;
    
    const loginData = {
        phoneNumber: phoneNumber,
        password: password
    };

    fetch(uri)
    .then(response => response.json())
    .then(data => {
        console.log(data);  // In ra nội dung của data
        return data;
    })
    .catch(error => console.error('Unable to get items.', error));
}
