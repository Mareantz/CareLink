# CareLink: Predictive Healthcare Management System

[![Clean Architecture](https://img.shields.io/badge/Clean%20Architecture-.NET-5C2D91.svg)](#)
[![Angular](https://img.shields.io/badge/Angular-v19-DD0031.svg)](#)
[![Tests](https://img.shields.io/badge/FE%20Coverage-82%25-brightgreen.svg)](#)
[![Tests](https://img.shields.io/badge/BE%20Coverage-25%25-orange.svg)](#)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](#)

CareLink is a predictive healthcare management system designed to simplify patient data management, appointment bookings, and medical history tracking. Powered by a machine learning module, CareLink also provides health risk predictions based on historical data.

> **Frontend (Angular)**: [Live App Deployed on Vercel](https://healthcaremanagement-fe.vercel.app/)  
> **Backend (.NET)**: Deployed on Railway

---

## Table of Contents

1. [Features](#features)  
2. [Architecture](#architecture)  
3. [Technologies Used](#technologies-used)  
4. [Getting Started](#getting-started)  
5. [Usage](#usage)  
6. [Testing](#testing)  
7. [Deployment](#deployment)  
8. [Contributors](#contributors)  

---

## Features

- **Separate Dashboards**:  
  - **Patient Dashboard**:  
    - Book appointments with doctors.  
    - Cancel existing appointments.  
    - View medical histories (including any attachments).  
  - **Doctor Dashboard**:  
    - View/cancel appointments.  
    - Complete appointments to generate medical history for patients (attachments included).  
    - Advanced patient filtering (by gender, first name, last name, etc.).

- **Predictive Analytics**:  
  - A machine learning module provides health risk predictions based on historical data.

- **Authentication & Authorization**:  
  - JSON Web Token (JWT) is generated upon login, stored in local storage.  
  - Secure backend endpoints validate the token before performing sensitive operations.

- **Clean Architecture & CQRS**:  
  - Ensures modular, maintainable code, separating read and write operations.

- **Repository & Result Patterns**:  
  - Simplifies data access and ensures consistent handling of API responses.

- **High-Level Testing**:  
  - **Frontend**: ~82% code coverage.  
  - **Backend**: ~25% code coverage (ongoing improvement).  
  - **Unit Tests**: Mocking with NSubstitute.  
  - **Integration Tests**: Ensures end-to-end functionality.

- **SonarQube Integration**:  
  - Automatic code quality checks.

- **Continuous Deployment**:  
  - **Backend** on Railway  
  - **Frontend** on Vercel

---

## Architecture


- **Clean Architecture** layers:
  - **Domain**: Business models and core logic.
  - **Application**: Use cases (commands and queries via CQRS & MediatR).
  - **Infrastructure**: Database and external services (PostgreSQL, ML module).
  - **WebAPI**: Controllers/Endpoints, Authentication, and Authorization.

---

## Technologies Used

### Backend (.NET)

- **.NET 9**  
- **CQRS + MediatR**  
- **PostgreSQL**  
- **Repository Pattern**  
- **JWT Authentication**  
- **NSubstitute** (Unit Testing)

### Frontend (Angular)

- **Angular 19+**  
- **Angular Material**  
- **RxJS**  
- **TypeScript**  
- **Jasmine/Karma** (Testing)

### Others

- **SonarQube** (Code Quality)  
- **Railway** (Backend Deployment)  
- **Vercel** (Frontend Deployment)

---

## Getting Started

### Prerequisites

- **.NET 9 SDK**  
- **Node.js** (version 14+) & npm/yarn  
- **PostgreSQL** database  
- **SonarQube** (optional, for code quality analysis)

### Installation

1. **Clone the repository**:

   ```bash
   git clone https://github.com/YourUsername/CareLink.git
   cd CareLink/HealthcareManagementSystem
   ```
2. **Set up the Backend**:
   ```bash
   cd HealthcareManagementSystem
   dotnet restore
   dotnet build
   ```
    - Configure your PostgreSQL connection string in appsettings.json or use environment variables:
    ```json
    {
    "ConnectionStrings": {
        "DefaultConnection": "host=localhost; port=5432; database=HealthcareManagement; userid=postgres; password=root"
        }
    }
    ```
    - (Optional) Run migrations if any:
    ```bash
    dotnet ef database update
    ```
3. **Set up the Frontend**:
    ```bash
    cd healthcare-management
    npm install
    npm run build
    ```
    - Configure environment variables in src/environments/environment.ts if necessary, especially API endpoints.

---

## Usage

1. **Start the Backend**:
    ```bash
    cd HealthcareManagementSystem
    dotnet run
    ```
    The API should be available at `https://localhost:7263` (or a specified port)
2. **Start the Frontend**:
    ```bash
    cd healthcare-management
    npm start
    ```
    The Angular app should be running at `http://localhost:4200`
3. **Login & Roles**:
    - **Patient**: Access patient dashboard to view your medical history, available doctors for booking appointments, the health risk prediction calculator, and your medical history based on the completed appointments.
    - **Doctor**: Access doctor dashboard to modify your profile, manage appointments, view patient data, and generate medical records based on completed appointments.
4. **Token Management**:
    - A JWT token is generated on successful login.
    - The token is stored in `localStorage`.
    - Most sensitive operations require a valid token.

---

## Testing
- Frontend(Angular):
    ```bash
    cd healthcare-management
    npm test --code-coverage
    ```
    - Code coverage ~82% is generated in the `coverage` folder.
- Backend(ASP.NET):
    ```bash
    cd HealthcareManagementSystem
    dotnet test
    ```
    - Code coverage ~25%
    - Mocks created using NSubstitute

    - Includes only Unit Tests
- Sonarqube Analysis (Optional):
    1. Start your local SonarQube server
    2. Create a local project and configure it
    3. Follow the steps from SonarQube and run the analysis
    4. Check the SonarQube dashboard for code quality insights.

---

## Deployment
- **Backend**:
    - Deployed on [Railway](https://railway.com/)
    - Ensure environment variables and secrets are properly configured in Railway for seamless deployment
- **Frontend**:
    - Deployed on [Vercel](https://vercel.com/)
- **Continous Integration and Continous Deployment(CI/CD)**
    - Integrated with SonarQube for automated code quality checks. (yaml file located in .github/workflows)
    - Automated deployment pipelines set up at every push on the main branch for both backend and frontend to ensure continous delivery.



---

## Contributors
- This project was developed by a team of 4. The **main contributions** were made by:
  - [Ciotir Marian](https://github.com/Mareantz)
  - [Rogojină Mihai](https://github.com/RogoMY)
