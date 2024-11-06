document.addEventListener('DOMContentLoaded', function () {
    autoLogin(); // Sayfa yüklendiğinde otomatik login yap

    // Buton için olay dinleyicilerini ekleyin
    document.getElementById('addProduct').addEventListener('click', addProduct);
    document.getElementById('updateProduct').addEventListener('click', updateProduct);
});

// Otomatik Login
async function autoLogin() {
    const response = await fetch('/api/Login/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            username: 'superadmin', // Kullanıcı adı
            password: 'superadmin'  // Şifre
        })
    });

    if (response.ok) {
        const data = await response.json();
        localStorage.setItem('token', data.token); // Token'ı local storage'a kaydet
        console.log('Login successful!'); // Konsola başarılı giriş mesajı
        loadProducts(); // Ürünleri yükle
    } else {
        alert('Auto login failed!');
        console.error('Login error:', await response.text()); // Hata durumunu konsola yazdır
    }
}

// Ürünleri yükle
async function loadProducts() {
    const token = localStorage.getItem('token'); // Token'ı alıyoruz
    const response = await fetch('/api/Product/get-products', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}` // Token'ı ekleyin
        }
    });

    if (response.ok) {
        const products = await response.json();
        const tableBody = document.querySelector('#productTable tbody');
        tableBody.innerHTML = ''; // Mevcut satırları temizle

        // Yanıtın doğru formatta olup olmadığını kontrol et
        if (Array.isArray(products)) {
            products.forEach(product => {
                const row = document.createElement('tr');
                row.innerHTML = `
                    <td>${product.name}</td>
                    <td>${product.productCode}</td>
                    <td>${product.price}</td>
                    <td><img src="${product.productImage}" alt="${product.name}" style="width: 50px; height: auto;"></td>
                    <td>
                        <button class="btn btn-edit" onclick="editProduct('${product.id}', '${product.name}', '${product.productCode}', ${product.price}, '${product.productImage}')">Edit</button>
                        <button class="btn btn-delete" onclick="deleteProduct('${product.id}')">Delete</button>
                    </td>
                `;
                tableBody.appendChild(row);
            });
        } else {
            console.error('Unexpected response format:', products); // Beklenmeyen yanıt formatını konsola yazdır
        }
    } else {
        console.error('Error loading products:', await response.text()); // Hata durumunu konsola yazdır
    }
}


// Ürün düzenle
function editProduct(id, name, productCode, price, productImage) {
    const modal = document.getElementById('editProductModal');
    modal.style.display = "block"; // Modal'ı aç

    document.getElementById('editProductId').value = id;
    document.getElementById('editName').value = name;
    document.getElementById('editProductCode').value = productCode;
    document.getElementById('editPrice').value = price;
    document.getElementById('editProductImage').value = productImage;
}

// Modal'ı kapat
document.getElementById('closeModal').addEventListener('click', function() {
    const modal = document.getElementById('editProductModal');
    modal.style.display = "none"; // Modal'ı gizle
});

// Modal dışında tıklayınca kapat
window.onclick = function(event) {
    const modal = document.getElementById('editProductModal');
    if (event.target === modal) {
        modal.style.display = "none"; // Modal'ı gizle
    }
};

// Ürün ekle
async function addProduct() {
    const product = {
        name: document.getElementById('name').value,
        productCode: document.getElementById('productCode').value,
        price: parseFloat(document.getElementById('price').value),
        productImage: document.getElementById('productImage').value // Kullanıcının girdiği resim URL'si
    };

    // Boş alan kontrolü
    if (!product.name || !product.productCode || isNaN(product.price)) {
        alert('Please fill in all required fields.');
        return;
    }

    const token = localStorage.getItem('token'); // Token'ı alıyoruz

    const response = await fetch('/api/Product/create-product', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}` // Token'ı ekliyoruz
        },
        body: JSON.stringify(product)
    });

    if (response.ok) {
        loadProducts(); // Ürünler güncellensin
        // Form alanlarını temizle
        document.getElementById('name').value = '';
        document.getElementById('productCode').value = '';
        document.getElementById('price').value = '';
        document.getElementById('productImage').value = '';
    } else {
        const errorData = await response.json();
        alert(`Failed to add product: ${errorData.message}`); // Hata mesajını göster
        console.error('Error:', errorData); // Hata bilgilerini konsola yazdır
    }
}

// Ürün güncelle
async function updateProduct() {
    const id = document.getElementById('editProductId').value;
    const product = {
        name: document.getElementById('editName').value,
        productCode: document.getElementById('editProductCode').value,
        price: parseFloat(document.getElementById('editPrice').value),
        productImage: document.getElementById('editProductImage').value
    };

    const token = localStorage.getItem('token'); // Token'ı alıyoruz

    const response = await fetch(`/api/Product/${id}/update-product`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}` // Token'ı ekliyoruz
        },
        body: JSON.stringify(product)
    });

    if (response.ok) {
        loadProducts(); // Ürünler güncellensin
        alert('Product updated successfully!'); // Başarılı güncelleme mesajı
        // Modal'ı kapat
        document.getElementById('editProductModal').style.display = "none";
    } else {
        const errorData = await response.json();
        alert(`Failed to update product: ${errorData.message}`); // Hata mesajını göster
        console.error('Error:', errorData); // Hata bilgilerini konsola yazdır
    }
}

// Ürün sil
async function deleteProduct(id) {
    const token = localStorage.getItem('token'); // Token'ı alıyoruz

    const response = await fetch(`/api/Product/${id}/delete-product`, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${token}` // Token'ı ekliyoruz
        }
    });

    if (response.ok) {
        loadProducts(); // Ürünler güncellensin
    } else {
        console.error('Error deleting product:', await response.text()); // Hata durumunu konsola yazdır
    }
}
