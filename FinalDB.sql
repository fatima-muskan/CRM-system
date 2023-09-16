CREATE TABLE Status (
    id INT IDENTITY(1,1) PRIMARY KEY,
    category VARCHAR(50) NOT NULL,
    name VARCHAR(50) NOT NULL,
    CONSTRAINT UC_Status UNIQUE (category, name)
);
CREATE TABLE Address (
    id INT IDENTITY(1,1) PRIMARY KEY,
    country VARCHAR(50) NOT NULL,
    state VARCHAR(50) NOT NULL,
    streetName VARCHAR(255) NOT NULL,
    city VARCHAR(50) NOT NULL,
    CONSTRAINT UC_Address UNIQUE (country, state, streetName, city)
);
CREATE TABLE Source (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);
CREATE TABLE Response (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    comment TEXT NOT NULL
);
CREATE TABLE Role (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);
CREATE TABLE Person (
    id INT IDENTITY(1,1) PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(50) NOT NULL CHECK (LEN(password) >= 7 AND password LIKE '%[A-Z]%' AND password LIKE '%[0-9]%'),
    roleid INT NOT NULL,
    addressid INT NOT NULL,
    email VARCHAR(50) NOT NULL CHECK (email LIKE '%_@__%.%com'),
    mobileNumber VARCHAR(11) CHECK (LEN(mobileNumber) <= 11 AND mobileNumber LIKE '03%'),
    isDeleted BIT DEFAULT 0,
    FOREIGN KEY (roleid) REFERENCES Role(id) ON DELETE NO ACTION,
    FOREIGN KEY (addressid) REFERENCES Address(id) ON DELETE NO ACTION
);
CREATE TABLE Type (
    id INT IDENTITY(1,1) PRIMARY KEY,
    category VARCHAR(50) NOT NULL,
    name VARCHAR(50) NOT NULL,
    CONSTRAINT UC_Type UNIQUE (category, name)
);
CREATE TABLE Employee (
    id INT IDENTITY(1,1) PRIMARY KEY,
    personid INT,
    salary DECIMAL(10,2) CHECK (salary >= 0),
    FOREIGN KEY (personid) REFERENCES Person(id) ON DELETE NO ACTION,
	CONSTRAINT FK_Employee_Person UNIQUE (personid)
);
CREATE TABLE Property (
    id INT IDENTITY(1,1) PRIMARY KEY,
    price DECIMAL(10,2) CHECK (price >= 0),
    typeid INT,
    addressid INT,
    employeeid INT NOT NULL,
    statusid INT NOT NULL,
    image VARBINARY(MAX),
    FOREIGN KEY (typeid) REFERENCES Type(id) ON DELETE NO ACTION,
    FOREIGN KEY (addressid) REFERENCES Address(id) ON DELETE NO ACTION,
    FOREIGN KEY (employeeid) REFERENCES Employee(id) ON DELETE NO ACTION,
    FOREIGN KEY (statusid) REFERENCES Status(id) ON DELETE NO ACTION
);
CREATE TABLE Lead (
    id INT IDENTITY(1,1) PRIMARY KEY,
    personid INT,
    sourceid INT,
    dateAdded DATE NOT NULL,
    dateConnected DATE NOT NULL,
    FOREIGN KEY (personid) REFERENCES Person(id) ON DELETE CASCADE,
    FOREIGN KEY (sourceid) REFERENCES Source(id) ON DELETE NO ACTION,
	CONSTRAINT FK_Lead_Person UNIQUE (personid)
);
CREATE TABLE Buyer (
    id INT IDENTITY(1,1) PRIMARY KEY,
    personid INT,
    propertyid INT,
    FOREIGN KEY (personid) REFERENCES Person(id) ON DELETE NO ACTION,
    FOREIGN KEY (propertyid) REFERENCES Property(id) ON DELETE NO ACTION,
);
CREATE TABLE Owner (
    id INT IDENTITY(1,1) PRIMARY KEY,
    propertyid INT,
    personid INT,
    FOREIGN KEY (propertyid) REFERENCES Property(id) ON DELETE NO ACTION,
    FOREIGN KEY (personid) REFERENCES Person(id) ON DELETE NO ACTION,
);
CREATE TABLE Sales (
    id INT IDENTITY(1,1) PRIMARY KEY,
    sellerid INT,
    propertyid INT,
    buyerid INT,
    price DECIMAL(10,2) NOT NULL,
    saleDate DATE NOT NULL,
    FOREIGN KEY (sellerid) REFERENCES Employee(id) ON DELETE NO ACTION,
    FOREIGN KEY (propertyid) REFERENCES Property(id) ON DELETE NO ACTION,
    FOREIGN KEY (buyerid) REFERENCES Buyer(id) ON DELETE NO ACTION,
    CONSTRAINT UC_Sales_PropertySeller UNIQUE (propertyid, sellerid)
);
CREATE TABLE PriorityLevel (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    range INT NOT NULL CHECK (range >= 1 AND range <= 5)
);
CREATE TABLE Task (
    id INT IDENTITY(1,1) PRIMARY KEY,
    employeeid INT,
    title VARCHAR(50) NOT NULL,
    description TEXT,
    duedate DATE NOT NULL,
    priorityLevelid INT,
    assigneeId INT,
    FOREIGN KEY (employeeid) REFERENCES Employee(id) ON DELETE CASCADE,
    FOREIGN KEY (priorityLevelid) REFERENCES PriorityLevel(id) ON DELETE NO ACTION, -- Updated cascade action
    FOREIGN KEY (assigneeId) REFERENCES Employee(id) ON DELETE NO ACTION, -- Updated cascade action
    CHECK (assigneeId <> employeeid)
);
CREATE TABLE Reminder (
    id INT IDENTITY(1,1) PRIMARY KEY,
    typeid INT,
    statusid INT,
    taskid INT,
    lastSentDate DATE NOT NULL,
    FOREIGN KEY (typeid) REFERENCES Type(id) ON DELETE NO ACTION,
    FOREIGN KEY (statusid) REFERENCES Status(id) ON DELETE NO ACTION,
    FOREIGN KEY (taskid) REFERENCES Task(id) ON DELETE CASCADE
);
-- Create LeadInterestedIn table
CREATE TABLE LeadInterestedIn (
    id INT IDENTITY(1,1) PRIMARY KEY,
    leadid INT,
    propertyid INT,
    CONSTRAINT FK_LeadInterestedIn_Lead FOREIGN KEY (leadid) REFERENCES Lead(id), -- Removed ON DELETE CASCADE
    CONSTRAINT FK_LeadInterestedIn_Property FOREIGN KEY (propertyid) REFERENCES Property(id), -- Removed ON DELETE CASCADE
    CONSTRAINT UC_LeadInterestedIn UNIQUE (leadid, propertyid)
);
CREATE TABLE LeadResponse (
    id INT IDENTITY(1,1) PRIMARY KEY,
    leadid INT,
    responseid INT,
    propertyid INT,
    FOREIGN KEY (leadid) REFERENCES Lead(id) ON DELETE NO ACTION,
    FOREIGN KEY (responseid) REFERENCES Response(id) ON DELETE NO ACTION,
    FOREIGN KEY (propertyid) REFERENCES Property(id) ON DELETE NO ACTION,
    CONSTRAINT UC_LeadResponse UNIQUE (leadid, responseid),
);

-- Create an AFTER DELETE trigger to cascade deletion to related tables
CREATE TABLE PaymentMethod (
    id INT IDENTITY(1,1) PRIMARY KEY,
    bank VARCHAR(255),
    accountNo CHAR(13) CHECK (LEN(accountNo) = 16),
    method VARCHAR(255)
);
CREATE TABLE Payment (
    id INT IDENTITY(1,1) PRIMARY KEY,
    BuyerId INT,
    PaymentMethodId INT,
    CONSTRAINT FK_Payment_Buyer FOREIGN KEY (BuyerId) 
    REFERENCES Buyer(id) ON DELETE NO ACTION,
    
    CONSTRAINT FK_Payment_PaymentMethod FOREIGN KEY (PaymentMethodId)
    REFERENCES PaymentMethod(id) ON DELETE CASCADE
);

