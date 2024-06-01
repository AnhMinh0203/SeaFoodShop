
// Khai báo các biến
const sidebar = document.querySelector(".sidebar");
const closeBtn = document.querySelector("#close-btn");
const searchBtn = document.querySelector(".bx-search");
const homeSection = document.querySelector(".home-section");
const links = document.querySelectorAll(".sidebar a");
const sideMenu = document.querySelector('aside');
const menuBtn = document.getElementById('menu-btn');
const darkMode = document.querySelector('.dark-mode');
var pageIndex = 1
var pageSize = 5
// Xử lý load page với axios
document.addEventListener("DOMContentLoaded", () => {
    // Load page mặc định
    loadPageContent("./dashboard.html", homeSection);

    // Thêm sự kiện click cho từng link trong sidebar
    links.forEach(link => {
        link.addEventListener("click", (e) => {
            e.preventDefault();
            const href = link.getAttribute("href");

            loadPageContent(href, homeSection);
        });
    });

    // Thêm sự kiện click cho nút đóng/mở sidebar
    if (closeBtn) {
        closeBtn.addEventListener("click", toggleSidebar);
    }

    if (searchBtn) {
        searchBtn.addEventListener("click", toggleSidebar);
    }

    function toggleSidebar() {
        sidebar.classList.toggle("open");
        menuBtnChange();
        moveHomeSection();
    }

    function menuBtnChange() {
        if (sidebar.classList.contains("open")) {
            closeBtn.classList.replace("bx-menu", "bx-menu-alt-right");
        } else {
            closeBtn.classList.replace("bx-menu-alt-right", "bx-menu");
        }
    }

    function moveHomeSection() {
        if (sidebar.classList.contains("open")) {
            homeSection.style.marginLeft = "0";
        }
    }

    // Load page
    function loadPageContent(href, container) {
        if (href.includes("log-in-sign-up.html")) {
            window.location.href = '../../user_side/pages/log-in-sign-up.html';
            return;
        }
        axios.get(href)
            .then(response => {
                container.innerHTML = response.data;

                if (href.includes("users.html")) {
                    handleUsersPage(pageIndex, pageSize);
                }
                else if (href.includes("products.html")) {
                    loadParagraphEdit() // Load trình edit văn bản
                    handleProductsPage(pageIndex, pageSize)

                }
                else if (href.includes("dashboard.html")) {
                    handleAdminPage();
                }
                else if(href.includes("analysis.html")){
                    initChart();
                }
                else if(href.includes("blog.html")){
                    loadParagraphEdit() // Load trình edit văn bản
                }
                moveHomeSection();
            })
            .catch(error => console.error("Error:", error));
    }
});

// Xử lý load page với ajax
// document.addEventListener("DOMContentLoaded", () => {
//     const sidebar = document.querySelector(".sidebar");
//     const closeBtn = document.querySelector("#close-btn");
//     const searchBtn = document.querySelector(".bx-search");
//     const homeSection = document.querySelector(".home-section");
//     const links = document.querySelectorAll(".sidebar a");

//     links.forEach(link => {
//         link.addEventListener("click", (e) => {
//             e.preventDefault();
//             const href = link.getAttribute("href");

//             fetchPage(href);
//         });
//     });

//     if (closeBtn) {
//         closeBtn.addEventListener("click", toggleSidebar);
//     }

//     if (searchBtn) {
//         searchBtn.addEventListener("click", toggleSidebar);
//     }

//     function toggleSidebar() {
//         sidebar.classList.toggle("open");
//         menuBtnChange();
//         moveHomeSection();
//     }

//     function menuBtnChange() {
//         if (sidebar.classList.contains("open")) {
//             closeBtn.classList.replace("bx-menu", "bx-menu-alt-right");
//         } else {
//             closeBtn.classList.replace("bx-menu-alt-right", "bx-menu");
//         }
//     }

//     function moveHomeSection() {
//         if (sidebar.classList.contains("open")) {
//             homeSection.style.marginLeft = "0";
//         } else {
//             homeSection.style.marginLeft = ""; // Reset margin để tránh lỗi khi sidebar đóng
//         }
//     }

//     async function fetchPage(url) {
//         try {
//             const response = await fetch(url);
//             const data = await response.text();
//             homeSection.innerHTML = data;
//             moveHomeSection();
//         } catch (error) {
//             console.error("Error:", error);
//         }
//     }
// });

// Xử lý load page với js thuần
// document.addEventListener("DOMContentLoaded", () => {
//     // Khai báo các biến
//     const sidebar = document.querySelector(".sidebar");
//     const closeBtn = document.querySelector("#close-btn"); // Đổi "#btn" thành "#close-btn"
//     const searchBtn = document.querySelector(".bx-search");
//     const homeSection = document.querySelector(".home-section");
//     const links = document.querySelectorAll(".sidebar a");

//     // Thêm sự kiện click cho từng link trong sidebar
//     links.forEach(link => {
//         link.addEventListener("click", (e) => {
//             e.preventDefault();
//             const href = link.getAttribute("href");

//             fetch(href)
//                 .then(response => response.text())
//                 .then(data => {
//                     homeSection.innerHTML = data;
//                     moveHomeSection();
//                 })
//                 .catch(error => console.error("Error:", error));
//         });
//     });

//     // Thêm sự kiện click cho nút đóng/mở sidebar
//     if (closeBtn) {
//         closeBtn.addEventListener("click", toggleSidebar);
//     }

//     if (searchBtn) { // Kiểm tra xem searchBtn có tồn tại không
//         searchBtn.addEventListener("click", toggleSidebar);
//     }

//     function toggleSidebar() {
//         sidebar.classList.toggle("open");
//         menuBtnChange();
//         moveHomeSection();
//     }

//     function menuBtnChange() {
//         if (sidebar.classList.contains("open")) {
//             closeBtn.classList.replace("bx-menu", "bx-menu-alt-right");
//         } else {
//             closeBtn.classList.replace("bx-menu-alt-right", "bx-menu");
//         }
//     }
//     function moveHomeSection() {
//         if (sidebar.classList.contains("open")) {
//             homeSection.style.marginLeft = "0"; // Độ rộng tương ứng với độ rộng của sidebar
//         } 
//         // else {
//         //     homeSection.style.marginLeft = "250px";
//         // }
//     }
// });


// ---------------------


// Hiển  thị menu mobile
menuBtn.addEventListener('click', () => {
    sideMenu.style.display = 'block';
});

closeBtn.addEventListener('click', () => {
    sideMenu.style.display = 'none';
});

// Xử lý dark mode
darkMode.addEventListener('click', () => {
    document.body.classList.toggle('dark-mode-variables');
    darkMode.querySelector('span:nth-child(1)').classList.toggle('active');
    darkMode.querySelector('span:nth-child(2)').classList.toggle('active');
})

// Xử lý click slidebar
document.addEventListener("DOMContentLoaded", function () {

    const sidebarLinks = document.querySelectorAll(".sidebar a");

    sidebarLinks.forEach(link => {
        link.addEventListener("click", function (event) {
            sidebarLinks.forEach(link => {
                link.classList.remove("active");
            });
            this.classList.add("active");
        });
    });
});

// Xử lý click chọn ảnh cho sản phẩm khi tạo sản phẩm mới
function handleImageUpload(event, idName) {
    var selectedImage = document.getElementById(idName);
    var file = event.target.files[0];
    var reader = new FileReader();

    // Đọc file ảnh và hiển thị nó
    reader.onload = function (e) {
        selectedImage.src = e.target.result;
        selectedImage.classList.add('border-color-primary');
    };

    if (file) {
        reader.readAsDataURL(file);
        selectedImage.dataset.filename = file.name;
    }
}


// Hiển thị form thêm sản phẩm
function toggleAddProductForm() {
    var form = document.querySelector(".add_product--form");
    var overlay = document.querySelector(".overlay");
    var addBtn = document.querySelector("#addProductBtn");
    var updateBtn = document.querySelector("#updateProductBtn");
    if (addBtn.classList.contains("d-none")) {
        addBtn.classList.remove('d-none');
    }
    updateBtn.classList.add('d-none');
    addBtn.setAttribute('onclick', 'addProduct()');
    if (form.classList.contains("d-none")) {
        overlay.classList.remove("d-none")
        form.classList.remove("d-none");
    } else {
        overlay.classList.add("d-none")
        form.classList.add("d-none");
    }
}

function toggleUpdateProductForm(idProduct) {
    var form = document.querySelector(".add_product--form");
    var overlay = document.querySelector(".overlay");
    var addBtn = document.querySelector("#addProductBtn");
    var updateBtn = document.querySelector("#updateProductBtn");
    if (updateBtn.classList.contains("d-none")) {
        updateBtn.classList.remove('d-none');
    }
    addBtn.classList.add('d-none');
    updateBtn.setAttribute('onclick', `updateProduct(${idProduct})`);

    // Kiểm tra nếu form đang hiển thị, thì ẩn nó đi; ngược lại, hiển thị nó lên
    if (form.classList.contains("d-none")) {
        overlay.classList.remove("d-none")
        form.classList.remove("d-none");
    } else {
        overlay.classList.add("d-none")
        form.classList.add("d-none");
    }
}

// Hiển thị form tạo blog
// Hiển thị form thêm sản phẩm
function toggleAddBlogForm() {
    var form = document.querySelector(".add_blog--form");
    var overlay = document.querySelector(".overlay");
    var addBtn = document.querySelector("#addBlogBtn");
    var updateBtn = document.querySelector("#updateBlogBtn");
    if (addBtn.classList.contains("d-none")) {
        addBtn.classList.remove('d-none');
    }
    updateBtn.classList.add('d-none');
    addBtn.setAttribute('onclick', 'addBlog()');
    if (form.classList.contains("d-none")) {
        overlay.classList.remove("d-none")
        form.classList.remove("d-none");
    } else {
        overlay.classList.add("d-none")
        form.classList.add("d-none");
    }
}

function toggleUpdateBlogForm(idBlog) {
    var form = document.querySelector(".add_blog--form");
    var overlay = document.querySelector(".overlay");
    var addBtn = document.querySelector("#addBlogBtn");
    var updateBtn = document.querySelector("#updateBlogBtn");
    if (updateBtn.classList.contains("d-none")) {
        updateBtn.classList.remove('d-none');
    }
    addBtn.classList.add('d-none');
    updateBtn.setAttribute('onclick', `updateBlog(${idBlog})`);

    // Kiểm tra nếu form đang hiển thị, thì ẩn nó đi; ngược lại, hiển thị nó lên
    if (form.classList.contains("d-none")) {
        overlay.classList.remove("d-none")
        form.classList.remove("d-none");
    } else {
        overlay.classList.add("d-none")
        form.classList.add("d-none");
    }
}


// Xử lý chart
function initChart() {
    const ctx = document.getElementById('myChart');

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
            datasets: [{
                label: '# of Votes',
                data: [12, 19, 3, 5, 2, 3],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}

