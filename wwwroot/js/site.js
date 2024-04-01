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
//<-------------------------------------------------------------------------------------->
// Work Page
document.addEventListener('DOMContentLoaded', () => {
    const addToCartButtons = document.querySelectorAll('.btn-add-to-cart');
    const cartItemsList = document.querySelector('.cart-items');
    const cartTotalAmount = document.getElementById('cart-total-amount');
    const buyNowButton = document.querySelector('.btn-buy-now');
    const floatingCart = document.getElementById('floating-cart');
    const cartContainer = document.querySelector('.cart-container');
    const cartItemCount = document.getElementById('cart-item-count');
    const cartOverlay = document.getElementById('cart-overlay');
    const closeCartBtn = document.querySelector('.close-cart');
    floatingCart.addEventListener('click', () => {
        cartOverlay.style.display = 'flex';
    });
    closeCartBtn.addEventListener('click', () => {
        cartOverlay.style.display = 'none';
    });
    let cart = [];
    cartContainer.style.display = 'none';
    addToCartButtons.forEach(button => {
        button.addEventListener('click', () => {
            const name = button.getAttribute('data-name');
            const price = parseFloat(button.getAttribute('data-price'));
            addToCart(name, price);
        });
    });
    floatingCart.addEventListener('click', () => {
        cartContainer.style.display = cartContainer.style.display === 'none' ? 'block' : 'none';
    });
    function addToCart(name, price) {
        const existingItem = cart.find(item => item.name === name);
        if (existingItem) {
            existingItem.quantity++;
        } else {
            cart.push({ name, price, quantity: 1 });
        }
        updateCart();
    }
    function updateCart() {
        cartItemsList.innerHTML = '';
        let totalAmount = 0;
        let itemCount = 0;
        cart.forEach(item => {
            const li = document.createElement('li');
            li.classList.add('cart-item');
            li.innerHTML = `
                <span>${item.name} x ${item.quantity}</span>
                <span>R${(item.price * item.quantity).toFixed(2)}</span>
            `;
            cartItemsList.appendChild(li);
            totalAmount += item.price * item.quantity;
            itemCount += item.quantity;
        });
        cartTotalAmount.textContent = totalAmount.toFixed(2);
        cartItemCount.textContent = itemCount;
    }
    buyNowButton.addEventListener('click', () => {
        alert('Thank you for your purchase!');
        cart = [];
        updateCart();
        cartContainer.style.display = 'none';
    });
});
//<--------------------------------------END---------------------------------------------->