# Browser-based file system

Welcome to a large-scale browser-based file system, functionally similar to Dropbox’s web interface, or to a folder browsing structure! This project is a built using a modern tech stack, including **ASP.NET Core WebAPI** for the backend. The application allows users to browse, search and create folders and files.

---

## Features

### Backend (ASP.NET WebAPI)
- **Create folders and subfolders**: Implemented creating folders and subfolders in one endpoint(based on parentFolderId -> if parentFolderId is null that means folder is root folder but if parentFolder is some id then folder is subfolder)
- **Create new files in the folders**: Implemented creating files with parentfolder Id.
- **Search files by its exact name within a parent folder and across all files**: User can search files in some specific folder and can search without specifing a folder.
- **Delete folders and files**: Deletes folder and files by Id. If user deletes folder with some files and subfolders, everything inside that folder is deleted.

---

## Technologies Used

### Backend
- **ASP.NET Core WebAPI**
- **Entity Framework Core**

## Getting Started

### Prerequisites
- **.NET SDK** (version 9.0 or later)
- **SQL Server** (or another database supported by Entity Framework Core)
- **Docker Desktop**
  
### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/MatePaic/browse_file_system.git
2. Restore dependencies:
   ```bash
   dotnet restore
   dotnet build
3. Update the database connection string in appsettings.json
4. Start the app server and database server:
   ```bash
   docker compose up -d -> database
   cd ../API
   dotnet run

   or all with docker 
   docker compose build browse-svc
   docker compose up -d
5. Attach postman json collection to postman
