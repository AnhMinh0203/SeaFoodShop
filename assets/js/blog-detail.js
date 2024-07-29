document.addEventListener('DOMContentLoaded', function () {
    var collapseElement = document.getElementById('collapseExample');
    var toggleButton = document.getElementById('toggleButton');

    collapseElement.addEventListener('show.bs.collapse', function () {
      toggleButton.textContent = 'Thu gọn';
    });

    collapseElement.addEventListener('hide.bs.collapse', function () {
      toggleButton.textContent = 'Xem thêm';
    });
  });