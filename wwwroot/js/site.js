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