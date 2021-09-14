const Toast = {
    init() {
        this.hideTimeout = null;

        this.el = document.createElement("div");
        this.el.className = "toast";
        document.body.appendChild(this.el);
    },

    show(message, state) {
        clearTimeout(this.hideTimeout);

        this.el.textContent = message;
        this.el.className = "toast toast--visible";

        if (state) {
            this.el.classList.add(`toast--${state}`);
        }

        this.hideTimeout = setTimeout(() => {
            this.el.classList.remove("toast--visible");
        }, 1500);
    }
};

document.addEventListener("DOMContentLoaded", () => Toast.init());

function AddProductToCart(productId, fromShoppingCartPage) {
    $.ajax({
        type: "POST",
        url: "/Cart/Buy",
        data: { id: productId, fromShoppingCartPage: fromShoppingCartPage },
        success: function () {
            Toast.show('Product has been added successfully to the cart','success');
            if (fromShoppingCartPage) {
                //update product's quantity
                var quantityId = getAttributeId(productId, "quantity");
                var quantity = parseInt(document.getElementById(quantityId).textContent);

                quantity += 1;
                document.getElementById(quantityId).textContent = quantity.toString();

                //update total
                var priceId = getAttributeId(productId, "price");
                var price = parseInt(document.getElementById(priceId).textContent);

                var total = parseInt(document.getElementById("shopping-cart-total").textContent);
                total += price;
                document.getElementById("shopping-cart-total").textContent = total.toString() + "₪";
            }

            return true;
            
        },
        error: function (request, status, error) {
            if (request.status == 401) {
                window.location.href = "Users/Login";
            }
            else {
                alert(error);
            }
        }
    });
}

function RemoveProductFromCart(productId) {
    $.ajax({
        type: "POST",
        url: "/Cart/Remove",
        data: { id: productId },
        success: function () {
            Toast.show('Product has been removed successfully from the cart', 'success');
            // update total
            var priceId = getAttributeId(productId, "price");
            var price = parseInt(document.getElementById(priceId).textContent);

            var total = parseInt(document.getElementById("shopping-cart-total").textContent);
            total -= price;
            if (total == 0) {
                // if it was the last product in cart, we will refresh page in order to show the client empty cart page
                location.reload();
                return true;
            }
            else {
                document.getElementById("shopping-cart-total").textContent = total.toString() + "₪";
            }

            //update quantity
            var quantityId = getAttributeId(productId, "quantity");
            var quantity = parseInt(document.getElementById(quantityId).textContent);

            quantity -= 1;
            if (quantity == 0) {
                // remove product from cart list
                var containerId = getAttributeId(productId, "container");
                var container = document.getElementById(containerId);
                container.remove();
            }
            else {
                document.getElementById(quantityId).textContent = quantity.toString();
            }

            return true;
        },
        error: function (request, status, error) {
            alert(error);
        }
    });
}

function getAttributeId(productId, attribute) {
    return "shopping-cart-item-".concat(productId.toString(), "-", attribute);
}