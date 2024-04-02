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
// Work Page script to add functionality to the add to cart buttons and the floating cart
// Code based off: https://phppot.com/javascript/javascript-shopping-cart/#:~:text=How%20to%20build%20a%20JavaScript%20shopping%20cart%20using,Empty%20the%20cart%20by%20unsetting%20the%20sessionStorage%20instance.
document.addEventListener('DOMContentLoaded', () => {
    // Selecting the 'Add to Cart' buttons
    const addToCartButtons = document.querySelectorAll('.btn-add-to-cart');
    const cartItemsList = document.querySelector('.cart-items');
    const cartTotalAmount = document.getElementById('cart-total-amount');
    const cartItemCount = document.getElementById('cart-item-count');
    const cartOverlay = document.getElementById('cart-overlay');
    const closeCartBtn = document.querySelector('.close-cart');
    const floatingCart = document.getElementById('floating-cart');

    // Cart array to store the items
    let cart = [];

    // Function to add an item to the cart array
    function addToCart(name, price) {
        const existingItem = cart.find(item => item.name === name);
        if (existingItem) {
            existingItem.quantity++;
        } else {
            cart.push({ name, price, quantity: 1 });
        }
        updateCart();
    }
    // Function to update the cart display in the floating cart
    function updateCart() {
        cartItemsList.innerHTML = '';
        let totalAmount = 0;
        let itemCount = 0;
        cart.forEach(item => {
            const li = document.createElement('li');
            li.classList.add('cart-item');
            li.innerHTML = `<span>${item.name} x ${item.quantity}</span><span>R${(item.price * item.quantity).toFixed(2)}</span>`;
            cartItemsList.appendChild(li);
            totalAmount += item.price * item.quantity;
            itemCount += item.quantity;
        });
        cartTotalAmount.textContent = totalAmount.toFixed(2);
        cartItemCount.textContent = itemCount;
    }
    addToCartButtons.forEach(button => {
        button.addEventListener('click', () => {
            const name = button.getAttribute('data-name');
            const price = parseFloat(button.getAttribute('data-price'));
            addToCart(name, price);
        });
    });
    floatingCart.addEventListener('click', () => {
        cartOverlay.style.display = 'flex';
    });
    closeCartBtn.addEventListener('click', () => {
        cartOverlay.style.display = 'none';
    });
    // Simulate a purchase by emptying the cart and displaying a thank you message using a simple alert
    const buyNowButton = document.querySelector('.btn-buy-now');
    buyNowButton.addEventListener('click', () => {
        alert('Thank you for your purchase!');
        cart = [];
        updateCart();
        cartOverlay.style.display = 'none';
    });
});
//<--------------------------------------END---------------------------------------------->