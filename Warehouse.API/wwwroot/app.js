const api = {
    products: "/api/Product",
    suppliers: "/api/Supplier",
    dashboard: "/api/Stock/dashboard",
    lowStock: "/api/Stock/low-stock",
    history: "/api/Stock/history",
    report: "/api/Stock/report",
    importStock: "/api/Stock/import",
    exportStock: "/api/Stock/export",
    search: "/api/Stock/search"
};

const state = {
    products: [],
    suppliers: [],
    currentView: "dashboard"
};

const qs = (selector) => document.querySelector(selector);
const qsa = (selector) => [...document.querySelectorAll(selector)];

function formatNumber(value) {
    return Number(value || 0).toLocaleString("vi-VN");
}

function formatMoney(value) {
    return formatNumber(value);
}

function showAlert(message, tone = "info") {
    const alert = qs("#alert");
    alert.textContent = cleanErrorMessage(message);
    alert.hidden = false;
    alert.dataset.tone = tone;
    window.clearTimeout(showAlert.timer);
    showAlert.timer = window.setTimeout(() => {
        alert.hidden = true;
    }, 4200);
}

async function request(url, options = {}) {
    const response = await fetch(url, {
        headers: {
            "Content-Type": "application/json",
            ...(options.headers || {})
        },
        ...options
    });

    if (!response.ok) {
        const message = await response.text();
        throw new Error(cleanErrorMessage(message));
    }

    const contentType = response.headers.get("content-type") || "";
    if (contentType.includes("application/json")) {
        return response.json();
    }

    return response.text();
}

function cleanErrorMessage(message) {
    if (!message) {
        return "Không thể xử lý yêu cầu";
    }

    if (message.includes("SqlException")
        || message.includes("Connection Timeout")
        || message.includes("Microsoft.Data.SqlClient")) {
        return "Không thể kết nối cơ sở dữ liệu. Kiểm tra SQL Server và chuỗi kết nối.";
    }

    return message.split("\n")[0].replace(/^\"|\"$/g, "");
}

function setApiStatus(isOnline) {
    qs("#apiStatus").textContent = isOnline ? "Đã kết nối" : "Mất kết nối";
    qs(".status-dot").style.background = isOnline ? "var(--success)" : "var(--danger)";
}

function renderRows(tbody, rows, emptyText) {
    tbody.innerHTML = rows.join("");
    if (!rows.length) {
        tbody.innerHTML = `<tr><td class="empty" colspan="8">${emptyText}</td></tr>`;
    }
}

function renderProducts(products = state.products) {
    const rows = products.map((product) => `
        <tr>
            <td>${product.sku || ""}</td>
            <td>${product.productName || ""}</td>
            <td>${formatNumber(product.quantity)}</td>
            <td>${formatMoney(product.price)}</td>
            <td>${formatNumber(product.minStock)}</td>
            <td>
                <div class="row-actions">
                    <button class="text-button" type="button" data-edit-product="${product.productId}">Sửa</button>
                    <button class="text-button danger" type="button" data-delete-product="${product.productId}">Xóa</button>
                </div>
            </td>
        </tr>
    `);

    renderRows(qs("#productsTable"), rows, "Chưa có sản phẩm");
}

function renderSuppliers() {
    const rows = state.suppliers.map((supplier) => `
        <tr>
            <td>${supplier.supplierCode || ""}</td>
            <td>${supplier.supplierName || ""}</td>
            <td>${supplier.phone || ""}</td>
            <td>${supplier.address || ""}</td>
            <td>
                <div class="row-actions">
                    <button class="text-button" type="button" data-edit-supplier="${supplier.supplierId}">Sửa</button>
                    <button class="text-button danger" type="button" data-delete-supplier="${supplier.supplierId}">Xóa</button>
                </div>
            </td>
        </tr>
    `);

    renderRows(qs("#suppliersTable"), rows, "Chưa có nhà cung cấp");
}

function renderSupplierOptions() {
    const supplierOptions = state.suppliers
        .map((supplier) => `<option value="${supplier.supplierId}">${supplier.supplierName}</option>`)
        .join("");

    qs("#productSupplier").innerHTML = supplierOptions || "<option value=''>Chưa có nhà cung cấp</option>";
}

function renderProductOptions() {
    const productOptions = state.products
        .map((product) => `<option value="${product.productId}">${product.sku} - ${product.productName} (${formatNumber(product.quantity)})</option>`)
        .join("");

    qs("#importProduct").innerHTML = productOptions || "<option value=''>Chưa có sản phẩm</option>";
    qs("#exportProduct").innerHTML = productOptions || "<option value=''>Chưa có sản phẩm</option>";
}

function renderLowStock(items) {
    const rows = items.map((product) => `
        <tr>
            <td>${product.sku || ""}</td>
            <td>${product.productName || ""}</td>
            <td>${formatNumber(product.quantity)}</td>
            <td>${formatNumber(product.minStock)}</td>
        </tr>
    `);

    renderRows(qs("#lowStockTable"), rows, "Không có sản phẩm tồn thấp");
}

function renderHistory(items) {
    const rows = items.slice(0, 8).map((item) => `
        <article class="activity-item">
            <span class="badge ${item.type === "EXPORT" ? "export" : ""}">${item.type}</span>
            <strong>${item.productName || ""} - ${formatNumber(item.quantity)}</strong>
            <small>${item.note || "Không có ghi chú"} • ${new Date(item.createdAt).toLocaleString("vi-VN")}</small>
        </article>
    `);

    qs("#historyList").innerHTML = rows.join("") || "<p class='empty'>Chưa có lịch sử kho</p>";
}

function renderReport(items) {
    const rows = items.map((item) => `
        <tr>
            <td>${item.productName || ""}</td>
            <td>${formatNumber(item.totalImport)}</td>
            <td>${formatNumber(item.totalExport)}</td>
        </tr>
    `);

    renderRows(qs("#reportTable"), rows, "Chưa có dữ liệu báo cáo");
}

async function loadDashboard() {
    const dashboard = await request(api.dashboard);

    qs("#totalProducts").textContent = formatNumber(dashboard.totalProducts);
    qs("#totalStock").textContent = formatNumber(dashboard.totalStock);
    qs("#lowStockProducts").textContent = formatNumber(dashboard.lowStockProducts);
    qs("#totalImport").textContent = formatNumber(dashboard.totalImport);
    qs("#totalExport").textContent = formatNumber(dashboard.totalExport);

    try {
        renderLowStock(await request(api.lowStock));
    } catch {
        renderLowStock([]);
    }

    try {
        renderHistory(await request(api.history));
    } catch {
        renderHistory([]);
    }

    try {
        renderReport(await request(api.report));
    } catch {
        renderReport([]);
    }
}

async function loadProducts() {
    state.products = await request(api.products);
    renderProducts();
    renderProductOptions();
}

async function loadSuppliers() {
    state.suppliers = await request(api.suppliers);
    renderSuppliers();
    renderSupplierOptions();
}

async function refreshAll() {
    try {
        await loadProducts();
        await loadSuppliers();
        await loadDashboard();
        setApiStatus(true);
    } catch (error) {
        setApiStatus(false);
        showAlert(error.message, "error");
    }
}

function switchView(viewName) {
    state.currentView = viewName;
    qsa(".nav-tab").forEach((tab) => tab.classList.toggle("active", tab.dataset.view === viewName));
    qsa(".view").forEach((view) => view.classList.toggle("active", view.id === `${viewName}View`));
    const activeView = qs(`#${viewName}View`);
    qs("#viewTitle").textContent = activeView.dataset.title;
}

function resetProductForm() {
    qs("#productForm").reset();
    qs("#productId").value = "";
    qs("#productFormTitle").textContent = "Thêm sản phẩm";
}

function resetSupplierForm() {
    qs("#supplierForm").reset();
    qs("#supplierId").value = "";
    qs("#supplierFormTitle").textContent = "Thêm nhà cung cấp";
}

function fillProductForm(product) {
    qs("#productId").value = product.productId;
    qs("#productSku").value = product.sku || "";
    qs("#productName").value = product.productName || "";
    qs("#productQuantity").value = product.quantity ?? 0;
    qs("#productPrice").value = product.price ?? 0;
    qs("#productMinStock").value = product.minStock ?? 0;
    qs("#productSupplier").value = product.supplierId || "";
    qs("#productFormTitle").textContent = "Sửa sản phẩm";
}

function fillSupplierForm(supplier) {
    qs("#supplierId").value = supplier.supplierId;
    qs("#supplierCode").value = supplier.supplierCode || "";
    qs("#supplierName").value = supplier.supplierName || "";
    qs("#supplierPhone").value = supplier.phone || "";
    qs("#supplierAddress").value = supplier.address || "";
    qs("#supplierFormTitle").textContent = "Sửa nhà cung cấp";
}

async function saveProduct(event) {
    event.preventDefault();
    const id = qs("#productId").value;
    const payload = {
        sku: qs("#productSku").value.trim(),
        productName: qs("#productName").value.trim(),
        quantity: Number(qs("#productQuantity").value),
        price: Number(qs("#productPrice").value),
        minStock: Number(qs("#productMinStock").value),
        supplierId: Number(qs("#productSupplier").value)
    };

    try {
        await request(id ? `${api.products}/${id}` : api.products, {
            method: id ? "PUT" : "POST",
            body: JSON.stringify(payload)
        });
        resetProductForm();
        await refreshAll();
        showAlert("Đã lưu sản phẩm");
    } catch (error) {
        showAlert(error.message, "error");
    }
}

async function saveSupplier(event) {
    event.preventDefault();
    const id = qs("#supplierId").value;
    const payload = {
        supplierCode: qs("#supplierCode").value.trim(),
        supplierName: qs("#supplierName").value.trim(),
        phone: qs("#supplierPhone").value.trim(),
        address: qs("#supplierAddress").value.trim()
    };

    try {
        await request(id ? `${api.suppliers}/${id}` : api.suppliers, {
            method: id ? "PUT" : "POST",
            body: JSON.stringify(payload)
        });
        resetSupplierForm();
        await refreshAll();
        showAlert("Đã lưu nhà cung cấp");
    } catch (error) {
        showAlert(error.message, "error");
    }
}

async function moveStock(event, type) {
    event.preventDefault();
    const isImport = type === "import";
    const payload = {
        productId: Number(qs(isImport ? "#importProduct" : "#exportProduct").value),
        quantity: Number(qs(isImport ? "#importQuantity" : "#exportQuantity").value),
        note: qs(isImport ? "#importNote" : "#exportNote").value.trim()
    };

    try {
        await request(isImport ? api.importStock : api.exportStock, {
            method: "POST",
            body: JSON.stringify(payload)
        });
        event.target.reset();
        await refreshAll();
        showAlert(isImport ? "Nhập kho thành công" : "Xuất kho thành công");
    } catch (error) {
        showAlert(error.message, "error");
    }
}

async function deleteItem(url, refreshMessage) {
    try {
        await request(url, { method: "DELETE" });
        await refreshAll();
        showAlert(refreshMessage);
    } catch (error) {
        showAlert(error.message, "error");
    }
}

qsa(".nav-tab").forEach((tab) => {
    tab.addEventListener("click", () => switchView(tab.dataset.view));
});

qs("#refreshBtn").addEventListener("click", refreshAll);
qs("#productForm").addEventListener("submit", saveProduct);
qs("#supplierForm").addEventListener("submit", saveSupplier);
qs("#importForm").addEventListener("submit", (event) => moveStock(event, "import"));
qs("#exportForm").addEventListener("submit", (event) => moveStock(event, "export"));
qs("#clearProductForm").addEventListener("click", resetProductForm);
qs("#clearSupplierForm").addEventListener("click", resetSupplierForm);

qs("#productSearch").addEventListener("input", async (event) => {
    const keyword = event.target.value.trim();
    if (!keyword) {
        renderProducts();
        return;
    }

    try {
        const result = await request(`${api.search}?keyword=${encodeURIComponent(keyword)}`);
        renderProducts(result);
    } catch (error) {
        showAlert(error.message, "error");
    }
});

document.addEventListener("click", (event) => {
    const editProductId = event.target.dataset.editProduct;
    const deleteProductId = event.target.dataset.deleteProduct;
    const editSupplierId = event.target.dataset.editSupplier;
    const deleteSupplierId = event.target.dataset.deleteSupplier;

    if (editProductId) {
        const product = state.products.find((item) => String(item.productId) === editProductId);
        if (product) fillProductForm(product);
    }

    if (deleteProductId) {
        deleteItem(`${api.products}/${deleteProductId}`, "Đã xóa sản phẩm");
    }

    if (editSupplierId) {
        const supplier = state.suppliers.find((item) => String(item.supplierId) === editSupplierId);
        if (supplier) fillSupplierForm(supplier);
    }

    if (deleteSupplierId) {
        deleteItem(`${api.suppliers}/${deleteSupplierId}`, "Đã xóa nhà cung cấp");
    }
});

refreshAll();
