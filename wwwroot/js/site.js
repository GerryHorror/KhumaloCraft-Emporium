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
//document.addEventListener("DOMContentLoaded", function () {
//    let cart = JSON.parse(localStorage.getItem("cart")) || [];

//    updateCartUI();

//    window.addToCart = function (productId, productName, productPrice, quantity) {
//        let existingItem = cart.find(item => item.productId === productId);
//        if (existingItem) {
//            existingItem.quantity += quantity;
//        } else {
//            cart.push({ productId, productName, productPrice, quantity });
//        }
//        localStorage.setItem("cart", JSON.stringify(cart));
//        updateCartUI();
//    };

//    function updateCartUI() {
//        let cartItems = document.getElementById("cartItems");
//        let cartTotal = document.getElementById("cartTotal");
//        cartItems.innerHTML = "";
//        let total = 0;
//        cart.forEach(item => {
//            total += item.productPrice * item.quantity;
//            let li = document.createElement("li");
//            li.textContent = `${item.productName} - ${item.quantity} x ${item.productPrice.toFixed(2)}`;
//            cartItems.appendChild(li);
//        });
//        cartTotal.textContent = total.toFixed(2);
//        document.getElementById("floatingCart").style.display = cart.length > 0 ? "block" : "none";
//    }

//    window.checkout = function () {
//        if (cart.length === 0) {
//            alert("Your cart is empty!");
//            return;
//        }

//        fetch('/Order/FinalizeOrder', {
//            method: 'POST',
//            headers: {
//                'Content-Type': 'application/json'
//            },
//            body: JSON.stringify(cart)
//        })
//            .then(response => response.json())
//            .then(data => {
//                if (data.success) {
//                    alert("Order placed successfully!");
//                    cart = [];
//                    localStorage.setItem("cart", JSON.stringify(cart));
//                    updateCartUI();
//                    window.location.href = "/Order/OrderConfirmation";
//                } else {
//                    alert("Error placing order: " + data.message);
//                }
//            })
//            .catch(error => {
//                console.error('Error:', error);
//                alert("An error occurred while placing your order.");
//            });
//    };
//});
//<--------------------------------------END---------------------------------------------->