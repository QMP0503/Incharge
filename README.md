# Incharge: Gym Management Web Application

- ### Description
  Incharge is an online gym management application for small business. The project is made using ASP.NET 8 MVC (C#, Html, CSS and Javascript are the languages used) with
  MySQL 8.0 as the database. I made the project on Visual Studios 2022 and MySQL Workbench 8.0.
  
  The application allows users to manage imporant information in the gym such as:
    - Client information
    - Employee information
    - Gym class schedule
    - Equipment information
    - Location status (spaces within the gym)
    - Business information (sales and expenses)

  Application pages:
  - Home Dashboard:
  ![Alt text](https://res.cloudinary.com/dmmlhlebe/image/upload/v1724133083/home-dash_mk5rxc.png)

  - Business Analytics Dashboard:
  ![Alt text](https://res.cloudinary.com/dmmlhlebe/image/upload/v1724133082/analytics_juijbm.png)

  - Gym schedule (location):
  ![Alt text](https://res.cloudinary.com/dmmlhlebe/image/upload/v1724133028/gymschedule-location_jrhooa.png)

  Packages and Libraries Used:
  - Cloudinary : Help convert and host image files as URL
  - ChartJS 4.4.3 : Javascript library to create line and pic charts within my application (seen in Business Analytics page)
  - Boostrap 5.3 : Frontend package for most icons and styles on my website
  - Fontawesome : Icon package
  - ASP.NET CORE indentity: For user login and registration
  - ASP.NET Entity Framework: Data migration
  - Automapper : Help map data from Viewmodels to Models
  - DotNetEnv : File type used to store connection string and cloudinary API key
  
- ### Installation and Setup
  Having Visual Studio 2022 would help with the installation and running process as all the package I used were installed on Visual Studio and have not
  tested using another IDE to run my application. I also used MySQL workbench which helps with database managament.

  #### Steps (After cloning the project and assuming you have a Cloudinary account and MySQL installed):
  1. Create and .env file to store your personal Cloudinary API key and connection string and place inside application folder
       - The program.cs file was configured to read the .env file using this format:
         
                CLOUDINARY_CLOUD_NAME= ... 
                CLOUDINARY_API_KEY= ...
                CLOUDINARY_API_SECRET= ... 
                CONNECTION_STRING="Server= ... ;Database= ... ;User= ... ;Password= ...;"
         
      - Replace all the ... with your own information

  2. Run the application and login using admin@admin.com account
      - I have hard coded an admin account into "program.cs" as the application required you to sign in before doing anything
      - The admin account information:
          - Username/Email : admin@admin.com
          - Password : 1234

  Thanks for reading :))
  
