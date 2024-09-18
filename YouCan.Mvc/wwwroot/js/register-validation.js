

document.addEventListener('DOMContentLoaded', function() {
    document.getElementById('file').addEventListener('change', function() {
        var fileInput = this;
        var filePath = fileInput.value;
        var allowedExtensions = /(\.jpg|\.jpeg|\.png)$/i;

        if (!allowedExtensions.exec(filePath)) {
            alert('Неверный тип файла. Выберите файл .png, .jpg или .jpeg.');
            fileInput.value = '';
            return false;
        }
    });
});

function validatePassword() {
    var password = document.querySelector('input[name=password]');
    var passwordError = document.getElementById('passwordError');
    var passwordValue = password.value;

    var lengthCheck = passwordValue.length >= 6;
    var digitCheck = /\d/.test(passwordValue);
    var uppercaseCheck = /[A-Z]/.test(passwordValue);
    var lowercaseCheck = /[a-z]/.test(passwordValue);

    var errorMessage = '';
    if (!lengthCheck) {
        errorMessage += 'Пароль должен быть не менее 6 символов.<br>';
    }
    if (!digitCheck) {
        errorMessage += 'Пароль должен содержать хотя бы одну цифру (\'0\'-\'9\').<br>';
    }
    if (!uppercaseCheck) {
        errorMessage += 'Пароль должен иметь хотя бы одну заглавную букву (\'A\'-\'Z\').<br>';
    }
    if (!lowercaseCheck) {
        errorMessage += 'Пароль должен иметь хотя бы одну строчную букву (\'a\'-\'z\').<br>';
    }

    if (errorMessage === '') {
        password.setCustomValidity('');
        passwordError.innerHTML = '';
    } else {
        password.setCustomValidity(errorMessage);
        passwordError.innerHTML = errorMessage;
    }
}

function checkPasswords() {
    var password = document.getElementById('pass').value;
    var confirm = document.getElementById('confPass').value;
    var passConfError = document.getElementById('passConfError');

    if (confirm === password) {
        document.getElementById('confPass').setCustomValidity('');
        passConfError.innerHTML = '';
    } else {
        document.getElementById('confPass').setCustomValidity('Пароли не совпадают');
        passConfError.innerHTML = 'Пароли не совпадают';
    }
}
