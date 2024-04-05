// Phần language table
document.addEventListener("DOMContentLoaded", function() {
    var languageBtn = document.querySelector('.language');
    var languageName = document.querySelector('.language_name');
    var tableVN = document.querySelector('.table_language-VN');
    var tableEN = document.querySelector('.table_language-EN');

    languageBtn.addEventListener('click', function(event) {
        event.preventDefault();
        var tableLanguage = document.querySelector('.table_language');
        if (tableLanguage.style.display === 'none') {
            tableLanguage.style.display = 'block';
        } else {
            tableLanguage.style.display = 'none';
        }
    });
    tableEN.addEventListener('click', function(event) {
        event.preventDefault();
        languageName.textContent = "Tiếng Anh";
        var tableLanguage = document.querySelector('.table_language');
        tableLanguage.style.display = 'none';
    });
    tableVN.addEventListener('click', function(event) {
        event.preventDefault();
        languageName.textContent = "Tiếng Việt";
        var tableLanguage = document.querySelector('.table_language');
        tableLanguage.style.display = 'none';
    });
});

// Phần user table
document.addEventListener("DOMContentLoaded", function() {
    var userBtn = document.querySelector('.user');
    var userTable = document.querySelector('.user_table');

    userBtn.addEventListener('click', function(event) {
        event.preventDefault();
        if (userTable.style.display === 'none') {
            userTable.style.display = 'block';
        } else {
            userTable.style.display = 'none';
        }
    });
});


document.addEventListener('DOMContentLoaded', function() {
    var numbers = document.querySelectorAll('.discount_pagination-numbers li');
    
    numbers.forEach(function(number) {
        number.addEventListener('click', function() {
            // Bỏ đi thuộc tính đã áp dụng trước đó
            var selected = document.querySelector('.pagination-selected');
            if (selected) {
                selected.classList.remove('pagination-selected');
            }
            // Thêm thuộc tính cho số đã chọn
            this.classList.add('pagination-selected');
        });
    });
});

