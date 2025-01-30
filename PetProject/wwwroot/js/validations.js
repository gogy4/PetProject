window.onload = function () {
    var validationSummary = document.querySelector('.validation-summary');
    if (validationSummary && validationSummary.textContent.trim() !== '') {
        validationSummary.style.display = 'block'; // Показываем элемент, если ошибки есть
    }
};

$(document).ready(function() {
    // Отображаем сводку ошибок, если есть ошибки
    if ($(".validation-summary").children().length > 0) {
        $(".validation-summary").addClass("show");
    }

    // При изменении полей формы удаляем класс ошибки, если он был
    $("input").on("input", function() {
        $(this).removeClass("error");
    });
});

