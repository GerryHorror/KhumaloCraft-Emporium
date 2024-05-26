// Navbar Hamburger Menu Toggle
document.addEventListener('DOMContentLoaded', function () {
    var navbarToggler = document.querySelector('.navbar-toggler');
    var navbarCollapse = document.querySelector('.navbar-collapse');
    navbarToggler.addEventListener('click', function () {
        if (navbarCollapse.classList.contains('show')) {
            navbarCollapse.classList.remove('show');
        } else {
            navbarCollapse.classList.add('show');
        }
    });
});
//<-------------------------------------------------------------------------------------->
// Contact Page
/* This script is used to simulate a successful form submission by hiding the form
and showing a success message after a delay*/
// Code based off: https://www.freecodecamp.org/news/how-to-submit-a-form-with-javascript/
document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('contact-form');
    const loadingSpinner = document.querySelector('.loading-spinner');
    const successMessage = document.querySelector('.success-message');
    form.addEventListener('submit', (e) => {
        e.preventDefault();
        form.style.opacity = 0;
        setTimeout(() => {
            form.style.display = 'none';
            loadingSpinner.style.display = 'flex';
            setTimeout(() => {
                loadingSpinner.style.display = 'none';
                successMessage.style.display = 'flex';
                setTimeout(() => {
                    successMessage.classList.add('show');
                }, 100);
            }, 2000);
        }, 500);
    });
});
//<-------------------------------------------------------------------------------------->
// Password Confirmation Validation and Password Toggle for Signup Page
// Code based off: https://www.w3schools.com/howto/howto_js_password_validation.asp
document.addEventListener("DOMContentLoaded", function () {
    var form = document.querySelector(".signup-form");
    if (form) {
        var password = document.getElementById("password");
        var confirmPassword = document.getElementById("confirm-password");
        var passwordToggle = document.getElementById("password-toggle");

        form.addEventListener("submit", function (event) {
            if (password.value !== confirmPassword.value) {
                event.preventDefault();
                document.getElementById("password-error").style.display = "inline";
                confirmPassword.focus();
            } else {
                document.getElementById("password-error").style.display = "none";
            }
        });

        passwordToggle.addEventListener("change", function () {
            var type = this.checked ? "text" : "password";
            password.type = type;
            confirmPassword.type = type;
        });
    }
});
//<-------------------------------------------------------------------------------------->
// Form Submission using AJAX for Transaction Page
// Code based off: https://www.w3schools.com/howto/howto_js_form_steps.asp
function submitForm(form) {
    var formData = new FormData(form);
    var request = new XMLHttpRequest();
    request.open("POST", form.action);
    request.onreadystatechange = function () {
        if (request.readyState === 4 && request.status === 200) {
            alert(request.responseText);
            location.reload();
        }
    };
    request.send(formData);
}
//<--------------------------------------END---------------------------------------------->