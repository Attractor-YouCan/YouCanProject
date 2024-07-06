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
});
// $(document).ready(function() {
//     const togglePassword = $('.toggle-password');
//     const passwordInput = $('#password');
//     const passwordIcon = $('#toggle-password-icon');
//
//     togglePassword.on('click', function() {
//         const type = passwordInput.attr('type') === 'password' ? 'text' : 'password';
//         passwordInput.attr('type', type);
//         if(type == 'password'){
//             passwordIcon.attr('src', '../authIcons/eyes-closed.png');
//             passwordIcon.attr('src', '../authIcons/eyes-open.png');
//         }
//     });
// });