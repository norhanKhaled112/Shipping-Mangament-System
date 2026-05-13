create database shipping_company 
use shipping_company 
CREATE TABLE CUSTOMER (
    customer_id INT PRIMARY KEY,
    name        VARCHAR(100),
    phone       VARCHAR(20),
    address     VARCHAR(255)
);


CREATE TABLE MERCHANT (
    merchant_id  INT PRIMARY KEY,
    name         VARCHAR(100),
    phone        VARCHAR(20),
    address      VARCHAR(255),
    bank_account VARCHAR(50)
);


CREATE TABLE DRIVER (
    driver_id INT PRIMARY KEY,
    name      VARCHAR(100),
    phone     VARCHAR(20)
);


CREATE TABLE EMPLOYEE (
    employee_id INT PRIMARY KEY,
    name        VARCHAR(100),
    role        VARCHAR(50),
    phone       VARCHAR(20)
);
CREATE TABLE SHIPMENT (
    shipment_id       INT PRIMARY KEY,
    customer_id       INT,
    merchant_id       INT,
    driver_id         INT,
    employee_id       INT,
    status            VARCHAR(50),
    created_at        DATE,
    pickup_address    VARCHAR(255),
    delivery_address  VARCHAR(255),
    cod_amount        FLOAT,
    merchant_amount   FLOAT,
    shipping_fee      FLOAT,

    FOREIGN KEY (customer_id)  REFERENCES CUSTOMER(customer_id),
    FOREIGN KEY (merchant_id)  REFERENCES MERCHANT(merchant_id),
    FOREIGN KEY (driver_id)    REFERENCES DRIVER(driver_id),
    FOREIGN KEY (employee_id)  REFERENCES EMPLOYEE(employee_id)
);
CREATE TABLE PACKAGE (
    package_id  INT PRIMARY KEY,
    shipment_id INT,
    status      VARCHAR(50),

    FOREIGN KEY (shipment_id) REFERENCES SHIPMENT(shipment_id)
);
