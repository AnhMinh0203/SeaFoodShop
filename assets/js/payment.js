
function displayname(nameClass) {
    var boxes = document.querySelectorAll('.content > div');
    boxes.forEach(function(box) {
        if (!box.classList.contains('d-none')) {
            box.classList.add('d-none');
        }
    });
    var targetBox = document.querySelector(nameClass);
    if (targetBox) {
        targetBox.classList.remove('d-none');
    } else {
        console.error('Element with the provided selector not found: ' + nameClass);
    }
}