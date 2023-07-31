const wishlistArea = document.querySelector(".wishlist");

let products = JSON.parse(localStorage.getItem("wishlistpr"));

function DisplayProducts() {
  wishlistArea.innerHTML = "";
  products = JSON.parse(localStorage.getItem("wishlistpr"));
  products.forEach((product) => {
    let html = `
   <tr>
        <td>
            <div class="wishlistItem">
                <div class="wishlist_productImg">
                    <img src="${product.img}" alt="">
                </div>
                <div class="wishlist__content">
                    <h6 class="wishlist_name">${product.Name}</h6>
                </div>
            </div>
        </td>
        <td>
            <p>$260</p>
        </td>
        <td>
            <button type="button" data-id='${product.id}' class="delete-icon btn-delete"><i class="fa-regular fa-trash-can"></i>
            </button>
        </td>
        <td>
            <a href="cart.html" class="btn wishlist_btn">Add to Cart</a>
        </td>
    </tr>
    `;
    wishlistArea.insertAdjacentHTML("beforeend", html);
  });
  let deleteButtons = document.querySelectorAll(".btn-delete");
  deleteButtons.forEach((btn) => {
    btn.addEventListener("click", function (e) {
      products = products.filter((m) => m.id != e.target.dataset.id);
      localStorage.setItem("wishlistpr", JSON.stringify(products));
      DisplayProducts();
    });
  });
}
DisplayProducts();

// const btnProducts = document.querySelectorAll(".delete-icon");

// function DeleteProduct(btnprod) {
//   btnProducts.forEach((btnProduct) => {
//     btnProduct.addEventListener("click", function (e) {
//       products = products.filter((m) => m.id != e.target.dataset.id);
//       localStorage.setItem("wishlistpr", JSON.stringify(products));
//       DisplayProducts();
//     });
//   });
// }
