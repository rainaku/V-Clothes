-- =============================================
-- SEED THÔNG TIN
-- =============================================

INSERT INTO categories (name, description, is_active) VALUES
('Áo thun thể thao', 'Áo thun chuyên dụng cho hoạt động thể thao', TRUE),
('Áo thun dài tay', 'Áo thun tay dài cho mùa lạnh', TRUE),
('Áo tank top', 'Áo ba lỗ, tank top nam nữ', TRUE),
('Áo thun cặp đôi', 'Áo thun thiết kế cho cặp đôi', TRUE),
('Áo thun local brand', 'Áo thun thương hiệu Việt Nam', TRUE),
('Áo thun oversized', 'Áo thun form rộng phong cách streetwear', TRUE),
('Áo thun basic', 'Áo thun cơ bản, đơn giản, dễ phối đồ', TRUE),
('Áo thun in họa tiết', 'Áo thun in hình, họa tiết đa dạng', TRUE),
('Áo thun organic', 'Áo thun từ cotton hữu cơ, thân thiện môi trường', TRUE),
('Áo thun limited edition', 'Áo thun phiên bản giới hạn, số lượng có hạn', TRUE);

INSERT INTO suppliers (name, address, phone, email, is_active) VALUES
('Công ty TNHH Thời Trang Sài Gòn', '12 Nguyễn Thị Minh Khai, Q.3, TP.HCM', '028-39123456', 'saigonfashion@email.com', TRUE),
('Xưởng may Hoàng Gia', '45 Lạc Long Quân, Q.Tân Bình, TP.HCM', '028-38456789', 'hoanggia@email.com', TRUE),
('Công ty CP Dệt May Phong Phú', '78 Tôn Đức Thắng, Q.1, TP.HCM', '028-38567890', 'phongphu@email.com', TRUE),
('Nhà máy dệt Thành Công', '234 Lý Thường Kiệt, Q.Tân Bình, TP.HCM', '028-38678901', 'thanhcong@email.com', TRUE),
('Công ty TNHH May Mặc Đông Á', '56 Cách Mạng Tháng 8, Q.10, TP.HCM', '028-38789012', 'donga@email.com', TRUE),
('Xưởng in áo Minh Phát', '89 Trường Chinh, Q.12, TP.HCM', '028-38890123', 'minhphat@email.com', TRUE),
('Công ty TNHH Vải Sợi Tân Tiến', '123 Quốc lộ 1A, Q.Bình Tân, TP.HCM', '028-38901234', 'tantien@email.com', TRUE),
('Nhà cung cấp Hàn Quốc K-Fashion', '67 Phạm Ngọc Thạch, Q.3, TP.HCM', '028-39012345', 'kfashion@email.com', TRUE),
('Công ty May Xuất Khẩu Việt Hưng', '345 Nguyễn Văn Linh, Q.7, TP.HCM', '028-39123456', 'viethung@email.com', TRUE),
('Xưởng may Bình Dương Garment', '12 Đại lộ Bình Dương, TX.Thuận An, Bình Dương', '0274-3812345', 'bdgarment@email.com', TRUE);

INSERT INTO employees (employee_code, full_name, gender, phone, email, position, is_active, hire_date, date_of_birth) VALUES
('NV006', 'Nguyễn Thị Mai', 'Nữ', '0901111001', 'mai.nt@vclothes.com', 'Nhân viên bán hàng', TRUE, '2024-03-15', '1998-05-12'),
('NV007', 'Trần Quốc Bảo', 'Nam', '0901111002', 'bao.tq@vclothes.com', 'Nhân viên kho', TRUE, '2024-04-01', '1995-08-20'),
('NV008', 'Lê Thị Hương', 'Nữ', '0901111003', 'huong.lt@vclothes.com', 'Nhân viên bán hàng', TRUE, '2024-04-10', '1999-01-15'),
('NV009', 'Phạm Văn Đức', 'Nam', '0901111004', 'duc.pv@vclothes.com', 'Nhân viên giao hàng', TRUE, '2024-04-15', '1997-11-03'),
('NV010', 'Hoàng Thị Lan', 'Nữ', '0901111005', 'lan.ht@vclothes.com', 'Kế toán', TRUE, '2024-05-01', '1996-03-22'),
('NV011', 'Võ Minh Khang', 'Nam', '0901111006', 'khang.vm@vclothes.com', 'Nhân viên bán hàng', TRUE, '2024-05-10', '2000-07-08'),
('NV012', 'Đặng Thị Ngọc', 'Nữ', '0901111007', 'ngoc.dt@vclothes.com', 'Nhân viên marketing', TRUE, '2024-05-15', '1998-09-14'),
('NV013', 'Bùi Thanh Tùng', 'Nam', '0901111008', 'tung.bt@vclothes.com', 'Nhân viên kho', TRUE, '2024-06-01', '1994-12-25'),
('NV014', 'Ngô Thị Yến', 'Nữ', '0901111009', 'yen.nt@vclothes.com', 'Nhân viên bán hàng', TRUE, '2024-06-10', '1999-04-30'),
('NV015', 'Dương Văn Hải', 'Nam', '0901111010', 'hai.dv@vclothes.com', 'Trưởng ca', TRUE, '2024-06-15', '1993-06-18'),
('NV016', 'Trịnh Thị Thảo', 'Nữ', '0901111011', 'thao.tt@vclothes.com', 'Nhân viên bán hàng', TRUE, '2024-07-01', '2001-02-10'),
('NV017', 'Lý Hoàng Nam', 'Nam', '0901111012', 'nam.lh@vclothes.com', 'Nhân viên IT', TRUE, '2024-07-10', '1997-10-05'),
('NV018', 'Phan Thị Diệu', 'Nữ', '0901111013', 'dieu.pt@vclothes.com', 'Nhân viên bán hàng', TRUE, '2024-07-15', '2000-08-22'),
('NV019', 'Huỳnh Văn Long', 'Nam', '0901111014', 'long.hv@vclothes.com', 'Nhân viên giao hàng', TRUE, '2024-08-01', '1996-01-17'),
('NV020', 'Mai Thị Hồng', 'Nữ', '0901111015', 'hong.mt@vclothes.com', 'Nhân viên bán hàng', TRUE, '2024-08-10', '1999-11-28'),
('NV021', 'Tạ Quang Vinh', 'Nam', '0901111016', 'vinh.tq@vclothes.com', 'Phó quản lý', TRUE, '2024-08-15', '1992-04-09'),
('NV022', 'Châu Thị Kim', 'Nữ', '0901111017', 'kim.ct@vclothes.com', 'Nhân viên bán hàng', TRUE, '2024-09-01', '2001-07-14'),
('NV023', 'Đinh Công Minh', 'Nam', '0901111018', 'minh.dc@vclothes.com', 'Nhân viên kho', TRUE, '2024-09-10', '1998-03-06'),
('NV024', 'Vương Thị Tuyết', 'Nữ', '0901111019', 'tuyet.vt@vclothes.com', 'Nhân viên bán hàng', FALSE, '2024-09-15', '2000-12-01'),
('NV025', 'Lâm Đình Phúc', 'Nam', '0901111020', 'phuc.ld@vclothes.com', 'Bảo vệ', TRUE, '2024-10-01', '1990-05-25');

-
INSERT INTO products (product_code, name, price, cost_price, stock_quantity, size, color, material, category_id, supplier_id, is_active) VALUES
('SP011', 'Áo thun thể thao nam Dri-Fit', 289000, 160000, 45, 'M,L,XL', 'Đen', 'Polyester Dri-Fit', 6, 4, TRUE),
('SP012', 'Áo thun thể thao nữ thoáng khí', 269000, 145000, 38, 'S,M,L', 'Hồng', 'Polyester mesh', 6, 4, TRUE),
('SP013', 'Áo thun gym nam cơ bắp', 259000, 140000, 30, 'M,L,XL', 'Xám đậm', 'Cotton Spandex', 6, 5, TRUE),
('SP014', 'Áo thun chạy bộ phản quang', 319000, 180000, 25, 'M,L,XL', 'Xanh neon', 'Polyester nhẹ', 6, 4, TRUE),
('SP015', 'Áo thun yoga nữ co giãn', 279000, 150000, 40, 'S,M,L', 'Tím lavender', 'Nylon Spandex', 6, 5, TRUE),
('SP016', 'Áo thun dài tay nam basic', 259000, 145000, 50, 'M,L,XL', 'Đen', 'Cotton 100%', 7, 1, TRUE),
('SP017', 'Áo thun dài tay nữ cổ lọ', 279000, 155000, 35, 'S,M,L', 'Trắng kem', 'Cotton pha Wool', 7, 6, TRUE),
('SP018', 'Áo thun dài tay raglan', 249000, 135000, 42, 'M,L,XL', 'Xám/Đen', 'Cotton 95%', 7, 1, TRUE),
('SP019', 'Áo thun dài tay henley', 289000, 160000, 28, 'M,L,XL', 'Xanh navy', 'Cotton Jersey', 7, 7, TRUE),
('SP020', 'Áo thun dài tay oversized', 269000, 148000, 33, 'M,L,XL,XXL', 'Be', 'French Terry', 7, 6, TRUE),

('SP021', 'Áo tank top nam tập gym', 159000, 80000, 60, 'M,L,XL', 'Đen', 'Cotton Spandex', 8, 5, TRUE),
('SP022', 'Áo tank top nữ croptop', 149000, 75000, 55, 'S,M,L', 'Trắng', 'Ribbed Cotton', 8, 2, TRUE),
('SP023', 'Áo ba lỗ nam thể thao', 169000, 85000, 48, 'M,L,XL', 'Xám', 'Polyester mesh', 8, 4, TRUE),
('SP024', 'Áo tank top nữ hai dây', 139000, 70000, 65, 'S,M,L', 'Đen', 'Viscose', 8, 2, TRUE),
('SP025', 'Áo tank top unisex streetwear', 179000, 95000, 40, 'M,L,XL', 'Trắng', 'Cotton 100%', 8, 8, TRUE),

('SP026', 'Áo thun cặp đôi trái tim', 199000, 100000, 30, 'M,L,XL', 'Trắng/Đen', 'Cotton 100%', 9, 9, TRUE),
('SP027', 'Áo thun cặp King & Queen', 219000, 115000, 25, 'M,L,XL', 'Đen', 'Cotton Premium', 9, 9, TRUE),
('SP028', 'Áo thun cặp đôi hoạt hình', 209000, 110000, 28, 'M,L', 'Xám', 'Cotton pha', 9, 6, TRUE),
('SP029', 'Áo thun cặp đôi minimalist', 189000, 95000, 35, 'S,M,L,XL', 'Trắng', 'Cotton Compact', 9, 9, TRUE),
('SP030', 'Áo thun cặp đôi phong cảnh', 229000, 120000, 20, 'M,L,XL', 'Xanh dương', 'Cotton 100%', 9, 6, TRUE),

('SP031', 'Áo thun Saigon Soul', 349000, 190000, 20, 'M,L,XL', 'Đen', 'Cotton Supima', 10, 8, TRUE),
('SP032', 'Áo thun Việt Heritage', 329000, 180000, 22, 'M,L,XL', 'Trắng', 'Cotton Organic', 10, 8, TRUE),
('SP033', 'Áo thun Urban Viet', 299000, 165000, 25, 'S,M,L,XL', 'Xám', 'Cotton French Terry', 10, 10, TRUE),
('SP034', 'Áo thun Phố Cổ Collection', 379000, 210000, 15, 'M,L,XL', 'Nâu đất', 'Cotton Slub', 10, 10, TRUE),
('SP035', 'Áo thun Mekong Delta', 319000, 175000, 18, 'M,L,XL', 'Xanh lá', 'Cotton Bamboo', 10, 8, TRUE),

('SP036', 'Áo thun oversized wash acid', 289000, 155000, 30, 'M,L,XL', 'Đen wash', 'Cotton Heavyweight', 11, 10, TRUE),
('SP037', 'Áo thun oversized drop shoulder', 269000, 145000, 35, 'M,L,XL,XXL', 'Trắng', 'Cotton 240gsm', 11, 3, TRUE),
('SP038', 'Áo thun oversized back print', 299000, 165000, 28, 'M,L,XL', 'Đen', 'Cotton Premium', 11, 6, TRUE),
('SP039', 'Áo thun oversized pastel', 259000, 140000, 40, 'M,L,XL', 'Tím nhạt', 'Cotton Combed', 11, 3, TRUE),
('SP040', 'Áo thun oversized vintage', 309000, 170000, 22, 'M,L,XL', 'Xanh rêu', 'Cotton Garment Dye', 11, 10, TRUE),

('SP041', 'Áo thun basic cổ tròn nam', 149000, 75000, 100, 'S,M,L,XL,XXL', 'Đen', 'Cotton 100%', 12, 1, TRUE),
('SP042', 'Áo thun basic cổ tròn nữ', 139000, 70000, 95, 'XS,S,M,L', 'Trắng', 'Cotton 100%', 12, 1, TRUE),
('SP043', 'Áo thun basic cổ V nam', 159000, 80000, 85, 'M,L,XL', 'Xám', 'Cotton Compact', 12, 7, TRUE),
('SP044', 'Áo thun basic fitted nữ', 149000, 75000, 90, 'XS,S,M,L', 'Đen', 'Cotton Spandex', 12, 2, TRUE),
('SP045', 'Áo thun basic unisex', 139000, 68000, 120, 'S,M,L,XL', 'Navy', 'Cotton Combed 30s', 12, 7, TRUE),
('SP046', 'Áo thun basic pack 3 cái', 399000, 200000, 50, 'M,L,XL', 'Đen/Trắng/Xám', 'Cotton 100%', 12, 1, TRUE),
('SP047', 'Áo thun basic heavyweight', 189000, 100000, 70, 'M,L,XL', 'Trắng', 'Cotton 220gsm', 12, 7, TRUE),

('SP048', 'Áo thun in hoa văn tropical', 239000, 130000, 35, 'M,L,XL', 'Trắng', 'Cotton DTG Print', 13, 6, TRUE),
('SP049', 'Áo thun in typography', 219000, 120000, 40, 'M,L,XL', 'Đen', 'Cotton Silk Screen', 13, 6, TRUE),
('SP050', 'Áo thun in anime', 249000, 135000, 30, 'M,L,XL', 'Trắng', 'Cotton DTG Print', 13, 9, TRUE),
('SP051', 'Áo thun in phong cảnh Việt Nam', 259000, 140000, 25, 'M,L,XL', 'Xám', 'Cotton Premium Print', 13, 9, TRUE),
('SP052', 'Áo thun in abstract art', 229000, 125000, 32, 'S,M,L,XL', 'Đen', 'Cotton Reactive Print', 13, 6, TRUE),
('SP053', 'Áo thun in retro 90s', 239000, 130000, 28, 'M,L,XL', 'Vàng', 'Cotton Vintage Wash', 13, 10, TRUE),
('SP054', 'Áo thun in galaxy', 249000, 138000, 22, 'M,L,XL', 'Đen', 'Cotton Sublimation', 13, 6, TRUE),

('SP055', 'Áo thun organic nam natural', 329000, 185000, 25, 'M,L,XL', 'Be tự nhiên', 'Organic Cotton GOTS', 14, 11, TRUE),
('SP056', 'Áo thun organic nữ soft', 319000, 178000, 28, 'S,M,L', 'Trắng ngà', 'Organic Cotton', 14, 11, TRUE),
('SP057', 'Áo thun organic unisex earth', 339000, 190000, 20, 'M,L,XL', 'Xanh olive', 'Organic Cotton Blend', 14, 11, TRUE),
('SP058', 'Áo thun organic bamboo', 359000, 200000, 18, 'M,L,XL', 'Xám nhạt', 'Bamboo Organic', 14, 12, TRUE),
('SP059', 'Áo thun organic hemp', 379000, 215000, 15, 'M,L,XL', 'Nâu nhạt', 'Hemp Cotton Blend', 14, 12, TRUE),

('SP060', 'Áo thun limited Tết 2024', 499000, 280000, 10, 'M,L,XL', 'Đỏ', 'Cotton Premium', 15, 8, TRUE),
('SP061', 'Áo thun limited collab Artist', 599000, 340000, 8, 'M,L,XL', 'Đen', 'Cotton Heavyweight', 15, 10, TRUE),
('SP062', 'Áo thun limited Saigon Night', 549000, 310000, 12, 'M,L,XL', 'Đen/Vàng', 'Cotton Foil Print', 15, 10, TRUE),
('SP063', 'Áo thun limited Summer Vibes', 479000, 265000, 15, 'S,M,L,XL', 'Tie-dye', 'Cotton Tie Dye', 15, 8, TRUE),
('SP064', 'Áo thun limited Anniversary', 529000, 295000, 10, 'M,L,XL', 'Trắng/Gold', 'Cotton Embroidery', 15, 10, TRUE),

('SP065', 'Áo thun nam slim fit', 209000, 115000, 45, 'S,M,L,XL', 'Đen', 'Cotton Lycra', 1, 7, TRUE),
('SP066', 'Áo thun nam henley ngắn tay', 239000, 130000, 35, 'M,L,XL', 'Trắng', 'Cotton Slub', 1, 1, TRUE),
('SP067', 'Áo thun nữ babydoll', 189000, 100000, 50, 'XS,S,M,L', 'Hồng pastel', 'Cotton Rib', 2, 2, TRUE),
('SP068', 'Áo thun nữ peplum', 209000, 112000, 38, 'S,M,L', 'Đen', 'Cotton Spandex', 2, 2, TRUE),
('SP069', 'Áo thun unisex tie-dye', 269000, 148000, 30, 'M,L,XL', 'Multi', 'Cotton Tie Dye', 3, 3, TRUE),
('SP070', 'Áo thun unisex pocket tee', 219000, 120000, 42, 'M,L,XL', 'Xám melange', 'Cotton French Terry', 3, 7, TRUE),
('SP071', 'Áo thun trẻ em dinosaur', 139000, 72000, 70, 'S,M,L', 'Xanh dương', 'Cotton 100%', 4, 3, TRUE),
('SP072', 'Áo thun trẻ em unicorn', 139000, 72000, 65, 'S,M,L', 'Hồng', 'Cotton 100%', 4, 3, TRUE),
('SP073', 'Áo polo nam classic', 329000, 185000, 30, 'M,L,XL', 'Trắng', 'Cotton Pique', 5, 1, TRUE),
('SP074', 'Áo polo nam sport', 349000, 195000, 25, 'M,L,XL', 'Đỏ', 'Polyester Pique', 5, 4, TRUE),
('SP075', 'Áo thun nam graphic tee', 229000, 125000, 38, 'M,L,XL', 'Đen', 'Cotton Screen Print', 1, 6, TRUE),
('SP076', 'Áo thun nữ off-shoulder', 199000, 108000, 32, 'S,M,L', 'Trắng', 'Cotton Rib', 2, 2, TRUE),
('SP077', 'Áo thun trẻ em superhero', 149000, 78000, 60, 'S,M,L', 'Đỏ/Xanh', 'Cotton 100%', 4, 9, TRUE),
('SP078', 'Áo polo nữ thanh lịch', 319000, 178000, 28, 'S,M,L', 'Hồng nhạt', 'Cotton Pique', 5, 2, TRUE),
('SP079', 'Áo thun nam acid wash', 259000, 142000, 22, 'M,L,XL', 'Xám wash', 'Cotton Garment Dye', 1, 10, TRUE),
('SP080', 'Áo thun unisex minimalist', 199000, 108000, 55, 'S,M,L,XL', 'Đen', 'Cotton Combed 40s', 3, 7, TRUE);

-- CHỐNG CONFLICTS 
SELECT setval('categories_id_seq', (SELECT MAX(id) FROM categories));
SELECT setval('suppliers_id_seq', (SELECT MAX(id) FROM suppliers));
SELECT setval('employees_id_seq', (SELECT MAX(id) FROM employees));
SELECT setval('products_id_seq', (SELECT MAX(id) FROM products));
