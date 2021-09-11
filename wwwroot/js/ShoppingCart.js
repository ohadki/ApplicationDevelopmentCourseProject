function AddProductToCart(productId, fromShoppingCartPage) {
    $.ajax({
        type: "POST",
        url: "/Cart/Buy",
        data: { id: productId, fromShoppingCartPage: fromShoppingCartPage },
        success: function () {
            alert("Product has been added successfully to the cart.");
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
            alert("Product has been added successfully to the cart.");
            return true;
        },
        error: function (request, status, error) {
            alert(request.responseText);
        }
    });
}