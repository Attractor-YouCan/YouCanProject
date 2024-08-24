document.addEventListener('DOMContentLoaded', function() {
    // Handle the modal hidden event
    document.getElementById('aboutUsModal').addEventListener('hidden.bs.modal', function() {
        var iframe = document.getElementById('youtube-video');
        var src = iframe.getAttribute('src');
        iframe.setAttribute('src', '');
        iframe.setAttribute('src', src);
    });

    const pages = document.querySelectorAll('.form-page');
    const progressBar = document.getElementById('progress-bar');
    const nextButtons = document.querySelectorAll('.nextBtn');
    const backButtons = document.querySelectorAll('.backBtn');
    let currentPage = 0;

    function updateProgressBar() {
        const progress = ((currentPage + 1) / pages.length) * 100;
        progressBar.style.width = progress + '%';
    }

    function showPage(pageIndex) {
        pages.forEach((page, index) => {
            page.style.display = index === pageIndex ? 'block' : 'none';
        });
        updateProgressBar();
    }

    nextButtons.forEach(button => {
        button.addEventListener('click', function() {
            if (currentPage < pages.length - 1) {
                currentPage++;
                showPage(currentPage);
            }
        });
    });

    backButtons.forEach(button => {
        button.addEventListener('click', function() {
            if (currentPage > 0) {
                currentPage--;
                showPage(currentPage);
            }
        });
    });

    showPage(currentPage);

    let userEmail;

    document.getElementById('registerForm').addEventListener('submit', function(event) {
        event.preventDefault();
        var formData = new FormData(this);

        document.getElementById('errorContainer').innerHTML = '';

        showLoader();

        fetch('Register/', {
            method: 'POST',
            body: formData,
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    document.getElementById('userEmailAddress').innerText = data.email;
                    userEmail = data.email;
                    startTimer(60);
                    currentPage++;
                    showPage(currentPage);
                } else {
                    console.log("error" + data.errors);
                    displayValidationErrors(data.errors);
                }
            })
            .catch(error => console.error('Error:', error))
            .finally(() => hideLoader());
    });

    const resendBtn = document.getElementById("resend-btn");

    function resendCode() {
        resendBtn.disabled = true;

        fetch('ResendCode/', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ "email": userEmail })
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    startTimer(60);
                } else {
                    alert('Ошибка, повторный код не отправился!');
                }
            })
            .catch(error => console.error('Error:', error));
    }

    resendBtn.addEventListener("click", function() {
        resendCode();
    });

    const confirmBtn = document.getElementById('confirm-button');
    const errorContainer = document.getElementById('errorContainer2');

    confirmBtn.addEventListener('click', function() {
        const verificationCode = Array.from(document.querySelectorAll('.code-input')).map(input => input.value).join('');
        const email = document.getElementById('email').value;
        errorContainer.innerHTML = '';

        showLoader();

        fetch('ConfirmCode/', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ email: email, code: verificationCode })
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    window.location.href = `/Account/Profile/${data.userId}`;
                } else {
                    displayValidationErrors2(data.errors || [data.message]);
                }
            })
            .catch(error => console.log('Error:', error))
            .finally(() => hideLoader());
    });

    function showLoader() {
        document.querySelector('.button-text').style.display = 'none';
        document.querySelector('.loginBtn').style.background = '#5B7CB2';
        document.querySelector('.loader').style.display = 'block';
    }

    function hideLoader() {
        document.querySelector('.button-text').style.display = 'block';
        document.querySelector('.loginBtn').style.background = '#58CC02';
        document.querySelector('.loader').style.display = 'none';
    }

    document.querySelectorAll('.code-input').forEach((input, index, inputs) => {
        input.addEventListener('input', function() {
            if (input.value.length === 1 && index < inputs.length - 1) {
                inputs[index + 1].focus();
            }
        });

        input.addEventListener('keydown', function(event) {
            if (event.key === 'Backspace' && index > 0 && input.value === '') {
                inputs[index - 1].focus();
            }
        });
    });

    function startTimer(duration) {
        const timerSpan = document.getElementById('timer');
        let time = duration, minutes, seconds;

        resendBtn.disabled = true;
        resendBtn.style.background = "#5B7CB2";

        const timer = setInterval(function() {
            minutes = parseInt(time / 60, 10);
            seconds = parseInt(time % 60, 10);

            minutes = minutes < 10 ? "0" + minutes : minutes;
            seconds = seconds < 10 ? "0" + seconds : seconds;

            timerSpan.textContent = `${minutes}:${seconds}`;

            if (--time < 0) {
                clearInterval(timer);
                timerSpan.textContent = '';
                resendBtn.disabled = false;
                resendBtn.style.background = "#c4960b";
            }
        }, 1000);
    }

    function displayValidationErrors(errors) {
        const errorContainer = document.getElementById('errorContainer');
        errorContainer.innerHTML = '';
        errors.forEach(error => {
            const errorDiv = document.createElement('div');
            errorDiv.className = 'error';
            errorDiv.innerText = error;
            errorContainer.appendChild(errorDiv);
        });
    }

    function displayValidationErrors2(errors) {
        const errorContainer = document.getElementById('errorContainer2');
        errorContainer.innerHTML = '';
        errors.forEach(error => {
            const errorDiv = document.createElement('div');
            errorDiv.className = 'error';
            errorDiv.innerText = error;
            errorContainer.appendChild(errorDiv);
        });
    }
});
