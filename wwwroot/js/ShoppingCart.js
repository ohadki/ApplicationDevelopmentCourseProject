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
            //alert("Product has been added successfully to the cart.");
            Toast.show('Product has been added successfully to the cart','success');
            return true;
        },
        error: function (request, status, error) {
            alert(request.responseText);
        }
    });
}

function RemoveProductFromCart(productId) {
    $.ajax({
        type: "POST",
        url: "/Cart/Remove",
        data: { id: productId },
        success: function () {
            //alert("Product has been removed successfully from the cart.");
            Toast.show('Product has been removed successfully from the cart', 'success');

            return true;
        },
        error: function (request, status, error) {
            alert(request.responseText);
        }
    });
}