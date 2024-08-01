document.addEventListener('DOMContentLoaded', () => {
    const togglePassword = document.querySelector('.toggle-password');
    const passwordInput = document.querySelector('#pass');
    const passwordIcon = document.querySelector('#toggle-password-icon');

    togglePassword.addEventListener('click', () => {
        const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
        passwordInput.setAttribute('type', type);
        if(type == 'password'){
            passwordIcon.setAttribute('src', '../authIcons/eyes-closed.png')
        }if(type == 'text'){
            passwordIcon.setAttribute('src', '../authIcons/eyes-open.png')
        }
    });
    const togglePassword2 = document.querySelector('.toggle-password2');
    const passwordInput2 = document.querySelector('#confPass');
    const passwordIcon2 = document.querySelector('#toggle-password-icon2');

    togglePassword2.addEventListener('click', () => {
        const type = passwordInput2.getAttribute('type') === 'password' ? 'text' : 'password';
        passwordInput2.setAttribute('type', type);
        if(type == 'password'){
            passwordIcon2.setAttribute('src', '../authIcons/eyes-closed.png')
        }if(type == 'text'){
            passwordIcon2.setAttribute('src', '../authIcons/eyes-open.png')
        }
    });
});
