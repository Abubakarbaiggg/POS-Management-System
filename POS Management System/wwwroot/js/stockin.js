document.addEventListener('DOMContentLoaded', function () {
    let products = [];

    const selectedBody = document.getElementById('selectedBody');
    const productsJson = document.getElementById('productsJson');
    const totalAmountInput = document.getElementById('totalAmount');
    const totalAmountHidden = document.getElementById('totalAmountHidden');
    const categoryButtons = document.querySelectorAll('.category-btn');

    function formatMoney(v) { return parseFloat(v || 0).toFixed(2); }

    function renderSelected() {
        selectedBody.innerHTML = '';
        products.forEach((p, idx) => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${p.name}</td>
                <td>
                    <input type="number" min="1" value="${p.qty}" data-index="${idx}" class="form-control qty-input" style="width:90px" />
                </td>
                <td>${formatMoney(p.price)}</td>
                <td class="item-total">${formatMoney(p.price * p.qty)}</td>
                <td><button type="button" data-index="${idx}" class="btn btn-sm btn-danger remove-btn">Remove</button></td>
            `;
            selectedBody.appendChild(tr);
        });

        const grand = products.reduce((s, it) => s + (it.price * it.qty), 0);
        if (totalAmountInput) totalAmountInput.value = formatMoney(grand);
        if (totalAmountHidden) totalAmountHidden.value = formatMoney(grand);

        if (productsJson) productsJson.value = JSON.stringify(products.map(p => ({ ProductId: p.id, ProductName: p.name, Quantity: p.qty, PurchasePrice: p.price, Total: parseFloat((p.price * p.qty).toFixed(2)) })));

        // no left-side highlighting: keep selected products only on the right
    }

    // category filter
    function applyCategoryFilter(catId) {
        document.querySelectorAll('#leftProducts .product-card').forEach(card => {
            const cid = parseInt(card.dataset.category || '0') || 0;
            const wrapper = card.closest('.col-12, .col-sm-6, .col-md-4');
            if (!wrapper) return;
            if (catId === 0 || cid === catId) wrapper.style.display = '';
            else wrapper.style.display = 'none';
        });
    }

    // Add product when clicking a left card
    document.querySelectorAll('.add-card').forEach(function (card) {
        card.addEventListener('click', function () {
            const productId = parseInt(this.dataset.id);
            const name = this.dataset.name || '';
            const price = parseFloat(this.dataset.price || 0);
            const image = this.dataset.image || '';

            if (!productId) return;

            const existing = products.find(p => p.id === productId);
            if (existing) existing.qty += 1; else products.push({ id: productId, name: name, price: price, qty: 1, image: image });

            renderSelected();

            // highlight the card on the left
            try {
                const leftCard = document.querySelector('.product-card[data-id="' + productId + '"]');
                if (leftCard) leftCard.classList.add('selected-card');
            } catch (ex) { console.error('highlight error', ex); }
        });
    });

    categoryButtons.forEach(function (btn) {
        btn.addEventListener('click', function () {
            categoryButtons.forEach(b => b.classList.remove('active'));
            this.classList.add('active');
            const id = parseInt(this.dataset.id || '0') || 0;
            applyCategoryFilter(id);
        });
    });

    // delegate qty change and remove
    document.addEventListener('input', function (e) {
        if (e.target && e.target.classList.contains('qty-input')) {
            const idx = parseInt(e.target.dataset.index);
            const val = parseInt(e.target.value) || 0;
            if (val <= 0) { e.target.value = 1; products[idx].qty = 1; } else { products[idx].qty = val; }
            renderSelected();
        }
    });

    document.addEventListener('click', function (e) {
        if (e.target && e.target.classList.contains('remove-btn')) {
            const idx = parseInt(e.target.dataset.index);
            const id = products[idx] && products[idx].id;
            products.splice(idx, 1);
            renderSelected();
            // remove highlight from left card
            if (id) {
                const leftCard = document.querySelector('.product-card[data-id="' + id + '"]');
                if (leftCard) leftCard.classList.remove('selected-card');
            }
        }
    });

    // on submit ensure productsJson set and there is at least one product
    const form = document.getElementById('stockInForm');
    if (form) {
        form.addEventListener('submit', function (e) {
            if (!products || products.length === 0) { e.preventDefault(); alert('Please add at least one product before saving.'); return false; }
            if (productsJson) productsJson.value = JSON.stringify(products.map(p => ({ ProductId: p.id, ProductName: p.name, Quantity: p.qty, PurchasePrice: p.price, Total: parseFloat((p.price * p.qty).toFixed(2)) })));
            if (totalAmountHidden) totalAmountHidden.value = totalAmountInput ? totalAmountInput.value : '';
        });
    }

    // initial state: show all and render
    applyCategoryFilter(0);
    renderSelected();
});
