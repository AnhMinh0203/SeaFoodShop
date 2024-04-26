// Khai báo
const token = localStorage.getItem('token');
const status_user = '0';
const status_admin = '1';
let pageSizeDefault = 2
localStorage.setItem("currentPage",1)
localStorage.setItem("lastPage",'False')


// Load user data 
function fetchUserData(token, status, pageIndex, pageSize) {
    return axios.get(`https://localhost:7018/api/ManagerCustomer/GetAllCustomers?token=${token}&status=${status}&pageIndex=${pageIndex}&pageSize=${pageSize}`);
}

function renderUserPage(data) {
    const totalPage =  Math.ceil( parseInt(data.totalRecord) / pageSizeDefault);
    const tableBody = document.getElementById('userTableBody');
    const paginationLoad = document.getElementById('pagination_user--nav');
    var currentPage = parseInt(localStorage.getItem("currentPage")) || 1;

    tableBody.innerHTML = '';  // Clear table body
    paginationLoad.innerHTML = '';

    data.listCustomerInfor.forEach((user, index) => {
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

    renderPagination(currentPage, totalPage, paginationLoad);

}

function renderPagination(currentPage, totalPage, paginationLoad){
    var lastPage = localStorage.getItem("lastPage") === "True";
    var updateColorPage = localStorage.getItem("updateColor");
    var residual = lastPage ? (totalPage % 4 > 0 ? totalPage % 4 - 1 : totalPage % 4 + 3) : 0;
    var htmlContent = '';

    htmlContent += `<li class="page-item">
                        <a class="page-link" href="#" onclick="firstPage(${1})">
                            <i class="fa-solid fa-angles-left"></i>
                        </a>
                    </li>
                    <li class="page-item">
                        <a class="page-link" href="#" onclick="periousPage(${lastPage ? currentPage - 4 - residual : currentPage - 4})">
                            <i class="fa fa-angle-left"></i>
                        </a>
                    </li>`;

    for (let page = lastPage ? totalPage - residual : currentPage; page <= totalPage && page < currentPage + 4; page++) {
        htmlContent += `<li class="page-item ${page == updateColorPage ? 'active' : ''}">
                            <a class="page-link" onclick="handleUsersPage(${page},${pageSizeDefault})" href="#" value="${page}">${page}</a>
                        </li>`;
    }

    htmlContent += `<li class="page-item">
                        <a class="page-link" href="#" onclick="nextPage(${currentPage + 4},${totalPage})">
                            <i class="fa fa-angle-right"></i>
                        </a>
                    </li>
                    <li class="page-item">
                        <a class="page-link" href="#" onclick="lastPage(${totalPage})">
                            <i class="fa-solid fa-angles-right"></i>
                        </a>
                    </li>`;

    paginationLoad.innerHTML += htmlContent;

    localStorage.setItem("lastPage", "False");
    localStorage.setItem("currentPage", lastPage ? currentPage - residual : currentPage);

    // if(localStorage.getItem("lastPage")=="True"){
    //     var residual = totalPage%4 > 0 ? totalPage%4 - 1 : totalPage%4 + 3

    //     paginationLoad.innerHTML += `<li class="page-item">
    //                             <a class="page-link" href="#" onclick="firstPage(${1})">
    //                                 <i class="fa-solid fa-angles-left"></i>
    //                             </a>
    //                         </li>
    //                         <li class="page-item">
    //                             <a class="page-link" href="#" onclick="periousPage(${currentPage-4-residual})">
    //                                 <i class="fa fa-angle-left"></i>
    //                             </a>
    //                         </li>`;

    //     for(let page = totalPage - residual; page <= totalPage && page < currentPage + 4; page++){ 
    //         paginationLoad.innerHTML += `<li class="page-item ${page == updateColorPage ? 'active' : ''}">
    //             <a class="page-link" onclick="handleUsersPage(${page},${pageSizeDefault})" href="#" value="${page}">${page}</a>
    //         </li>`;
    //     }
    //     paginationLoad.innerHTML += `<li class="page-item">
    //                             <a class="page-link" href="#" onclick="nextPage(${currentPage+4},${totalPage})">
    //                                 <i class="fa fa-angle-right"></i>
    //                             </a>
    //                         </li>
    //                         <li class="page-item">
    //                             <a class="page-link" href="#" onclick="lastPage(${totalPage})">
    //                                 <i class="fa-solid fa-angles-right"></i>
    //                             </a>
    //                         </li>`;
    //     localStorage.setItem("lastPage","False")
    //     localStorage.setItem("currentPage",currentPage-residual)
    // }
    // else{
    //     paginationLoad.innerHTML += `<li class="page-item">
    //             <a class="page-link" href="#" onclick="firstPage(${1})">
    //                 <i class="fa-solid fa-angles-left"></i>
    //             </a>
    //         </li>
    //         <li class="page-item">
    //             <a class="page-link" href="#" onclick="periousPage(${currentPage-4})">
    //                 <i class="fa fa-angle-left"></i>
    //             </a>
    //         </li>`;

    //     for(let page = currentPage ; page <= totalPage && page < currentPage + 4; page++){ 
    //     paginationLoad.innerHTML += `<li class="page-item ${page == updateColorPage ? 'active' : ''}">
    //     <a class="page-link" onclick="handleUsersPage(${page},${pageSizeDefault})" href="#" value="${page}">${page}</a>
    //     </li>`;
    //     }
    //     paginationLoad.innerHTML += `<li class="page-item">
    //             <a class="page-link" href="#" onclick="nextPage(${currentPage+4},${totalPage})">
    //                 <i class="fa fa-angle-right"></i>
    //             </a>
    //         </li>
    //         <li class="page-item">
    //             <a class="page-link" href="#" onclick="lastPage(${totalPage})">
    //                 <i class="fa-solid fa-angles-right"></i>
    //             </a>
    //         </li>`;
    // }
}

function handleUsersPage(pageIndex, pageSize) {
    localStorage.setItem("updateColor", pageIndex);
    fetchUserData(token, status_user, pageIndex, pageSize)
        .then(response => {
            renderUserPage(response.data);
        })
        .catch(error => console.error("Error:", error));
}

function nextPage(nextPage,totalPage) {
    if(nextPage > totalPage){
        return;
    }
    else{
        localStorage.setItem("currentPage",nextPage)
        handleUsersPage(nextPage,pageSizeDefault)
    }

}

function periousPage(periousPage){
    if(periousPage < 1){
        return;
    }
    else{
        localStorage.setItem("currentPage",periousPage)
        handleUsersPage(periousPage,pageSizeDefault)
    }
}

function lastPage(page){
    localStorage.setItem("lastPage","True")
    localStorage.setItem("currentPage",page)
    handleUsersPage(page,pageSizeDefault)
}

function firstPage(page){
    localStorage.setItem("currentPage",page)
    handleUsersPage(page,pageSizeDefault)
}

// Load dashboard data
function fetchAdminData(token, status,phoneNumber) {
    return axios.get(`https://localhost:7018/api/ManagerCustomer/SearchCustomers?token=${token}&phoneNumber=${phoneNumber}&status=${status}`);
}
function populateAdmin(data) {
    const tableBody = document.querySelector('.right-section .nav .profile');
    tableBody.innerHTML = '';  // Clear table body

    data.forEach((user) => {
        const row = `
            <div class="profile">
                <div class="info">
                    <b>${user.fullName}</b>
                </div>
                <div class="profile-photo">
                    <img src="../assets/images/${user.avatar}">
                </div>
            </div>
        `;
        tableBody.innerHTML += row;
    });
}

function handleAdminPage(){
    const phoneNumber = '0869819316';
    fetchAdminData(token, status_admin,phoneNumber)
        .then(response => {
            populateAdmin(response.data);
        })
        .catch(error => console.error("Error:", error));
}

