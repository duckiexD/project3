-- Добавление брендов
INSERT INTO brands (brand_name, description, founded_year) VALUES
('Nike', 'Just Do It', 1964),
('Adidas', 'Impossible is Nothing', 1949),
('Zara', 'Spanish fast fashion', 1975),
('H&M', 'Swedish multinational clothing-retail company', 1947);

-- Добавление категорий
INSERT INTO categories (category_name, description) VALUES
('Мужская одежда', 'Одежда для мужчин'),
('Женская одежда', 'Одежда для женщин'),
('Обувь', 'Обувь для всех'),
('Аксессуары', 'Модные аксессуары');

-- Добавление подкатегорий
INSERT INTO categories (category_name, parent_category_id, description) VALUES
('Футболки', 1, 'Мужские футболки'),
('Джинсы', 1, 'Мужские джинсы'),
('Платья', 2, 'Женские платья'),
('Кроссовки', 3, 'Спортивная обувь');

-- Добавление продуктов
INSERT INTO products (product_name, description, price, stock_quantity, brand_id) VALUES
('Nike Air Max', 'Кроссовки Nike Air Max', 120.00, 50, 1),
('Adidas Superstar', 'Классические кроссовки', 100.00, 40, 2),
('Zara Джинсы Slim', 'Стройнящие джинсы', 59.99, 30, 3),
('H&M Платье', 'Летнее платье', 39.99, 25, 4),
('Nike Футболка', 'Хлопковая футболка', 29.99, 60, 1);

-- Связи продуктов и категорий
INSERT INTO product_categories (product_id, category_id) VALUES
(1, 7), -- Nike Air Max -> Кроссовки
(2, 7), -- Adidas Superstar -> Кроссовки
(3, 5), -- Zara Джинсы -> Джинсы
(4, 6), -- H&M Платье -> Платья
(5, 4); -- Nike Футболка -> Футболки

-- Добавление пользователя
INSERT INTO users (username, email, password_hash) VALUES
('testuser', 'user@example.com', 'hashed_password_123');

-- Добавление профиля пользователя
INSERT INTO user_profiles (user_id, first_name, last_name, phone, address) VALUES
(1, 'Иван', 'Иванов', '+79991234567', 'ул. Примерная, д. 1, кв. 2');
