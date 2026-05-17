-- =============================================
-- V-Clothes Database Schema for Supabase
-- Run this SQL in Supabase SQL Editor
-- =============================================

-- 1. ROLES
CREATE TABLE roles (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    description VARCHAR(200)
);

-- 2. USERS
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(256) NOT NULL,
    display_name VARCHAR(100),
    is_active BOOLEAN DEFAULT TRUE,
    role_id INTEGER NOT NULL REFERENCES roles(id),
    created_at TIMESTAMP DEFAULT NOW(),
    last_login TIMESTAMP
);

-- 3. CATEGORIES
CREATE TABLE categories (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    description VARCHAR(500),
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT NOW()
);

-- 4. SUPPLIERS
CREATE TABLE suppliers (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    address VARCHAR(500),
    phone VARCHAR(20),
    email VARCHAR(100),
    note VARCHAR(500),
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT NOW()
);

-- 5. EMPLOYEES
CREATE TABLE employees (
    id SERIAL PRIMARY KEY,
    employee_code VARCHAR(20) NOT NULL UNIQUE,
    full_name VARCHAR(100) NOT NULL,
    gender VARCHAR(10),
    date_of_birth DATE,
    phone VARCHAR(20),
    email VARCHAR(100),
    address VARCHAR(500),
    position VARCHAR(100),
    hire_date DATE DEFAULT CURRENT_DATE,
    is_active BOOLEAN DEFAULT TRUE,
    user_id INTEGER REFERENCES users(id)
);

-- 6. CUSTOMERS
CREATE TABLE customers (
    id SERIAL PRIMARY KEY,
    full_name VARCHAR(100) NOT NULL,
    phone VARCHAR(20),
    email VARCHAR(100),
    address VARCHAR(500),
    created_at TIMESTAMP DEFAULT NOW()
);

-- 7. PRODUCTS
CREATE TABLE products (
    id SERIAL PRIMARY KEY,
    product_code VARCHAR(50) NOT NULL UNIQUE,
    name VARCHAR(200) NOT NULL,
    description VARCHAR(1000),
    price DECIMAL(18,2) NOT NULL,
    cost_price DECIMAL(18,2) DEFAULT 0,
    stock_quantity INTEGER DEFAULT 0,
    size VARCHAR(50),
    color VARCHAR(50),
    material VARCHAR(50),
    image_path TEXT,
    category_id INTEGER NOT NULL REFERENCES categories(id),
    supplier_id INTEGER NOT NULL REFERENCES suppliers(id),
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT NOW()
);

-- 8. PURCHASE INVOICES
CREATE TABLE purchase_invoices (
    id SERIAL PRIMARY KEY,
    invoice_code VARCHAR(50) NOT NULL UNIQUE,
    invoice_date TIMESTAMP DEFAULT NOW(),
    supplier_id INTEGER NOT NULL REFERENCES suppliers(id),
    employee_id INTEGER NOT NULL REFERENCES employees(id),
    total_amount DECIMAL(18,2) DEFAULT 0,
    note VARCHAR(500)
);

-- 9. PURCHASE INVOICE DETAILS
CREATE TABLE purchase_invoice_details (
    id SERIAL PRIMARY KEY,
    purchase_invoice_id INTEGER NOT NULL REFERENCES purchase_invoices(id) ON DELETE CASCADE,
    product_id INTEGER NOT NULL REFERENCES products(id),
    quantity INTEGER NOT NULL,
    unit_price DECIMAL(18,2) NOT NULL,
    sub_total DECIMAL(18,2) NOT NULL
);

-- 10. SALES INVOICES
CREATE TABLE sales_invoices (
    id SERIAL PRIMARY KEY,
    invoice_code VARCHAR(50) NOT NULL UNIQUE,
    invoice_date TIMESTAMP DEFAULT NOW(),
    customer_id INTEGER REFERENCES customers(id),
    employee_id INTEGER NOT NULL REFERENCES employees(id),
    total_amount DECIMAL(18,2) DEFAULT 0,
    discount DECIMAL(18,2) DEFAULT 0,
    final_amount DECIMAL(18,2) DEFAULT 0,
    note VARCHAR(500)
);

-- 11. SALES INVOICE DETAILS
CREATE TABLE sales_invoice_details (
    id SERIAL PRIMARY KEY,
    sales_invoice_id INTEGER NOT NULL REFERENCES sales_invoices(id) ON DELETE CASCADE,
    product_id INTEGER NOT NULL REFERENCES products(id),
    quantity INTEGER NOT NULL,
    unit_price DECIMAL(18,2) NOT NULL,
    discount DECIMAL(18,2) DEFAULT 0,
    sub_total DECIMAL(18,2) NOT NULL
);

-- =============================================
-- SEED DATA
-- =============================================

-- Roles
INSERT INTO roles (id, name, description) VALUES
(1, 'Admin', 'Quản trị viên - Toàn quyền'),
(2, 'Manager', 'Quản lý - Quản lý cửa hàng'),
(3, 'Staff', 'Nhân viên - Bán hàng');

-- Users (password: 123456, MD5: e10adc3949ba59abbe56e057f20f883e)
INSERT INTO users (id, username, password_hash, display_name, is_active, role_id, created_at) VALUES
(1, 'admin', 'e10adc3949ba59abbe56e057f20f883e', 'Quản trị viên', TRUE, 1, '2024-01-01'),
(2, 'manager', 'e10adc3949ba59abbe56e057f20f883e', 'Quản lý', TRUE, 2, '2024-01-01'),
(3, 'staff', 'e10adc3949ba59abbe56e057f20f883e', 'Nhân viên', TRUE, 3, '2024-01-01');

-- Categories
INSERT INTO categories (id, name, description, is_active, created_at) VALUES
(1, 'Áo thun nam', 'Các loại áo thun dành cho nam', TRUE, '2024-01-01'),
(2, 'Áo thun nữ', 'Các loại áo thun dành cho nữ', TRUE, '2024-01-01'),
(3, 'Áo thun unisex', 'Áo thun cho cả nam và nữ', TRUE, '2024-01-01'),
(4, 'Áo thun trẻ em', 'Áo thun dành cho trẻ em', TRUE, '2024-01-01'),
(5, 'Áo polo', 'Áo polo các loại', TRUE, '2024-01-01');

-- Suppliers
INSERT INTO suppliers (id, name, address, phone, email, is_active, created_at) VALUES
(1, 'Công ty TNHH Dệt May Việt Tiến', '123 Nguyễn Trãi, Q.1, TP.HCM', '028-38123456', 'viettien@email.com', TRUE, '2024-01-01'),
(2, 'Công ty CP May Nhà Bè', '456 Lê Lợi, Q.7, TP.HCM', '028-38654321', 'nhabe@email.com', TRUE, '2024-01-01'),
(3, 'Xưởng may Đại Phát', '789 Trần Hưng Đạo, Q.5, TP.HCM', '028-38789012', 'daiphat@email.com', TRUE, '2024-01-01');

-- Employees
INSERT INTO employees (id, employee_code, full_name, gender, phone, email, position, is_active, hire_date, user_id) VALUES
(1, 'NV001', 'Nguyễn Văn An', 'Nam', '0901234567', 'an@vclothes.com', 'Quản lý', TRUE, '2024-01-01', 2),
(2, 'NV002', 'Trần Thị Bình', 'Nữ', '0912345678', 'binh@vclothes.com', 'Nhân viên bán hàng', TRUE, '2024-01-15', 3),
(3, 'NV003', 'Lê Hoàng Cường', 'Nam', '0923456789', 'cuong@vclothes.com', 'Nhân viên kho', TRUE, '2024-02-01', NULL),
(4, 'NV004', 'Phạm Thị Dung', 'Nữ', '0934567890', 'dung@vclothes.com', 'Nhân viên bán hàng', TRUE, '2024-03-01', NULL),
(5, 'NV005', 'Hoàng Minh Tuấn', 'Nam', '0945678901', 'tuan@vclothes.com', 'Nhân viên kho', FALSE, '2024-01-10', NULL);

-- Customers
INSERT INTO customers (id, full_name, phone, email, address, created_at) VALUES
(1, 'Phạm Minh Đức', '0934567890', 'duc@gmail.com', '12 Lý Thường Kiệt, Q.10, TP.HCM', '2024-01-01'),
(2, 'Hoàng Thị Em', '0945678901', 'em@gmail.com', '34 Hai Bà Trưng, Q.3, TP.HCM', '2024-01-01'),
(3, 'Võ Văn Phúc', '0956789012', 'phuc@gmail.com', '56 Pasteur, Q.1, TP.HCM', '2024-01-01'),
(4, 'Nguyễn Thị Hoa', '0967890123', 'hoa@gmail.com', '78 Nguyễn Huệ, Q.1, TP.HCM', '2024-02-01'),
(5, 'Trần Văn Khoa', '0978901234', 'khoa@gmail.com', '90 Điện Biên Phủ, Q.Bình Thạnh, TP.HCM', '2024-02-15');

-- Products
INSERT INTO products (id, product_code, name, price, cost_price, stock_quantity, size, color, material, category_id, supplier_id, is_active, created_at) VALUES
(1, 'SP001', 'Áo thun nam cổ tròn trắng', 199000, 120000, 50, 'M,L,XL', 'Trắng', 'Cotton 100%', 1, 1, TRUE, '2024-01-01'),
(2, 'SP002', 'Áo thun nam cổ tròn đen', 199000, 120000, 45, 'M,L,XL', 'Đen', 'Cotton 100%', 1, 1, TRUE, '2024-01-01'),
(3, 'SP003', 'Áo thun nữ form rộng', 179000, 100000, 60, 'S,M,L', 'Hồng', 'Cotton pha', 2, 2, TRUE, '2024-01-01'),
(4, 'SP004', 'Áo thun unisex oversize', 249000, 150000, 35, 'M,L,XL,XXL', 'Xám', 'Cotton 95%', 3, 2, TRUE, '2024-01-01'),
(5, 'SP005', 'Áo polo nam cao cấp', 349000, 200000, 25, 'M,L,XL', 'Xanh navy', 'Cotton Pique', 5, 1, TRUE, '2024-01-01'),
(6, 'SP006', 'Áo thun trẻ em hoạt hình', 129000, 70000, 80, 'S,M,L', 'Nhiều màu', 'Cotton', 4, 3, TRUE, '2024-01-01'),
(7, 'SP007', 'Áo thun nam cổ V', 219000, 130000, 40, 'M,L,XL', 'Xanh rêu', 'Cotton 100%', 1, 1, TRUE, '2024-02-01'),
(8, 'SP008', 'Áo thun nữ croptop', 159000, 85000, 55, 'S,M,L', 'Trắng', 'Cotton pha Spandex', 2, 2, TRUE, '2024-02-01'),
(9, 'SP009', 'Áo polo unisex', 299000, 170000, 30, 'M,L,XL', 'Đen', 'Cotton Pique', 5, 1, TRUE, '2024-02-15'),
(10, 'SP010', 'Áo thun nam in hình', 229000, 140000, 8, 'M,L,XL', 'Đen', 'Cotton 100%', 1, 3, TRUE, '2024-03-01');

-- Purchase Invoices
INSERT INTO purchase_invoices (id, invoice_code, invoice_date, supplier_id, employee_id, total_amount, note) VALUES
(1, 'PN001', '2024-01-05', 1, 1, 12000000, 'Nhập hàng đợt 1'),
(2, 'PN002', '2024-01-20', 2, 1, 7500000, 'Nhập hàng nữ'),
(3, 'PN003', '2024-02-10', 3, 3, 5600000, 'Nhập áo trẻ em'),
(4, 'PN004', '2024-03-01', 1, 1, 10000000, 'Nhập polo cao cấp');

-- Purchase Invoice Details
INSERT INTO purchase_invoice_details (id, purchase_invoice_id, product_id, quantity, unit_price, sub_total) VALUES
(1, 1, 1, 50, 120000, 6000000),
(2, 1, 2, 50, 120000, 6000000),
(3, 2, 3, 50, 100000, 5000000),
(4, 2, 4, 25, 100000, 2500000),
(5, 3, 6, 80, 70000, 5600000),
(6, 4, 5, 25, 200000, 5000000),
(7, 4, 9, 30, 170000, 5100000);

-- Sales Invoices
INSERT INTO sales_invoices (id, invoice_code, invoice_date, customer_id, employee_id, total_amount, discount, final_amount, note) VALUES
(1, 'HD001', '2024-02-01', 1, 2, 647000, 0, 647000, 'Khách mua lẻ'),
(2, 'HD002', '2024-02-05', 2, 2, 856000, 50000, 806000, 'Giảm giá khách quen'),
(3, 'HD003', '2024-02-14', 3, 4, 1047000, 100000, 947000, 'Valentine sale'),
(4, 'HD004', '2024-03-01', 4, 2, 498000, 0, 498000, NULL),
(5, 'HD005', '2024-03-10', 5, 4, 1296000, 100000, 1196000, 'Mua số lượng lớn'),
(6, 'HD006', '2024-03-15', 1, 2, 748000, 0, 748000, NULL);

-- Sales Invoice Details
INSERT INTO sales_invoice_details (id, sales_invoice_id, product_id, quantity, unit_price, discount, sub_total) VALUES
(1, 1, 1, 2, 199000, 0, 398000),
(2, 1, 4, 1, 249000, 0, 249000),
(3, 2, 3, 2, 179000, 0, 358000),
(4, 2, 4, 2, 249000, 0, 498000),
(5, 3, 5, 2, 349000, 0, 698000),
(6, 3, 5, 1, 349000, 0, 349000),
(7, 4, 2, 1, 199000, 0, 199000),
(8, 4, 9, 1, 299000, 0, 299000),
(9, 5, 1, 3, 199000, 0, 597000),
(10, 5, 5, 2, 349000, 0, 698000),
(11, 6, 4, 2, 249000, 0, 498000),
(12, 6, 4, 1, 249000, 0, 250000);

-- Reset sequences
SELECT setval('roles_id_seq', (SELECT MAX(id) FROM roles));
SELECT setval('users_id_seq', (SELECT MAX(id) FROM users));
SELECT setval('categories_id_seq', (SELECT MAX(id) FROM categories));
SELECT setval('suppliers_id_seq', (SELECT MAX(id) FROM suppliers));
SELECT setval('employees_id_seq', (SELECT MAX(id) FROM employees));
SELECT setval('customers_id_seq', (SELECT MAX(id) FROM customers));
SELECT setval('products_id_seq', (SELECT MAX(id) FROM products));
SELECT setval('purchase_invoices_id_seq', (SELECT MAX(id) FROM purchase_invoices));
SELECT setval('purchase_invoice_details_id_seq', (SELECT MAX(id) FROM purchase_invoice_details));
SELECT setval('sales_invoices_id_seq', (SELECT MAX(id) FROM sales_invoices));
SELECT setval('sales_invoice_details_id_seq', (SELECT MAX(id) FROM sales_invoice_details));
