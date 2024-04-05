// Load user data 
function fetchUserData(token, status) {
    return axios.get(`https://localhost:7018/api/ManagerCustomer/GetAllCustomers?token=${token}&status=${status}`);
}
function populateUserTable(data) {
    const tableBody = document.getElementById('userTableBody');
    tableBody.innerHTML = '';  // Clear table body

    data.forEach((user, index) => {
        const row = `
            <tr>
                <th scope="row">${index + 1}</th>
                <td>${user.fullName}</td>
                <td>${user.phoneNumber}</td>
                <td>${new Date(user.dob).toLocaleDateString()}</td>
                <td>${user.gender === 0 ? 'Nam' : 'Nữ'}</td>
                <td>
                    <span class="material-icons-sharp">
                        more_horiz
                    </span>
                </td>
            </tr>
        `;
        tableBody.innerHTML += row;
    });
}

function handleUsersPage() {
    const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiJlZDJhNDE5Yi0yYjUyLTQwMzktOWE4MC1iY2M5YzNiYzZhYWIiLCJuYmYiOjE3MTIyNzc2ODQsImV4cCI6MTcxMjM2NDA4NCwiaWF0IjoxNzEyMjc3Njg0fQ.aGyISdPOvfLlEs5O8Rm5DYCWigemV_Hu06KZIpeBexk';
    const statusUser = '-1';

    fetchUserData(token, statusUser)
        .then(response => {
            populateUserTable(response.data);
        })
        .catch(error => console.error("Error:", error));
}




