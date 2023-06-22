# **Motorcycle Sales Website**
An ASP.NET Core MVC project for buying motorcycles online. <br>
Main functionalities<br>
- Viewing and searching available motorcycles.
- Purchasing motorcycles.
- Registration and Authentication for users.
- Price statistics depending on different criteria.
- Sales statistics for privileged users.
- CRUD operations on content for privileged users.
## **Developed with**
<br>
<p align="center">
    <img src="https://img.shields.io/badge/.NET-7.0-blueviolet">
    <img src="https://img.shields.io/badge/MS SQL Server-2019-red">
    <img src="https://img.shields.io/badge/Node.js-v18.15.0-253d85">
    <img src="https://img.shields.io/badge/npm-9.5.0-success">
    <img src="https://img.shields.io/badge/jQuery-3.6.0-yellow">
    <img src="https://img.shields.io/badge/Tailwind CSS-3.3.2-aqua">
</p>

## **Getting Started**
This paragraph provides step-by-step instructions on how to install the necessary software and configure the project to make it executable on your local machine.
### **Prerequisites**
This software is **required** to be installed on your device:
1. [Git](https://git-scm.com/book/en/v2/Getting-Started-Installing-Git)
2. [.NET](https://dotnet.microsoft.com/en-us/download)
3. [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
4. [Node.js](https://nodejs.org/)
### **Installation**
1. Open the _destination directory_ in Terminal.
2. Clone the Repository by running the command:
```
git clone https://github.com/tereshchuk055/moto-sales-website
```
3. Open the _project directory_ in Terminal.
4. Run the following commands:
```
npm init
npm install
```
5. Configure the database using script [Query.sql](https://github.com/tereshchuk055/moto-sales-website/blob/main/Query.sql) from the project. You can use any convenient software to do it, e.g.: [Microsoft SQL Server Management Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16#download-ssms)
6. Get your _**connection string**_ and fill in the corresponding field in the [appsettings.json](https://github.com/tereshchuk055/moto-sales-website/blob/main/appsettings.json):
```json
"ConnectionStrings": {
    "Default": "YOUR CONNECTION STRING"
  },
```
7. Get your [Cloudinary](https://cloudinary.com/documentation/cloudinary_get_started) requisites and fill in the corresponding fields in the [appsettings.json](https://github.com/tereshchuk055/moto-sales-website/blob/main/appsettings.json):
```json
 "CloudinarySettings": {
    "CloudName": "YOUR CLOUD NAME",
    "ApiKey": "YOUR API KEY",
    "ApiSecret": "YOUR API SECRET"
  },
```
### **Running the app**
Open the _project directory_ in the Terminal and execute the following command:
```
dotnet run
```
In case the website does not open automatically, navigate to the link mentioned in the console, such as **https://localhost:3000/**.

## **Authors**
* **Vladyslav Tereshchuk** - [tereshchuk055](https://github.com/tereshchuk055)
