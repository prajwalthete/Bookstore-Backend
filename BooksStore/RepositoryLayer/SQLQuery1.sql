CREATE TABLE Customer (
    customer_id INT PRIMARY KEY IDENTITY,
    name NVARCHAR(255) NOT NULL,
    email NVARCHAR(255) NOT NULL,
    phone NVARCHAR(20) NOT NULL,
    password NVARCHAR(255) NOT NULL,
    role NVARCHAR(50) NOT NULL
);

CREATE TABLE Book (
    book_id INT PRIMARY KEY IDENTITY,
    title VARCHAR(255),
    author VARCHAR(255),
    genre VARCHAR(100),
    price DECIMAL(10, 2),
    ImagePath VARCHAR(255) -- Add this column to store the path of the image
);


-- Creating Order table with address attribute
CREATE TABLE [Order] (
    order_id INT PRIMARY KEY IDENTITY,
    customer_id INT,
    order_date DATETIME,
    address VARCHAR(255),
    FOREIGN KEY (customer_id) REFERENCES Customer(customer_id)
);

