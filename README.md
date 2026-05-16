# AI Document Validator  
### Grupo Fácil Health Tech Challenge

An AI-powered CLI application for automated extraction and validation of health insurance onboarding documents using **C#**, **.NET 8**, and the **Google Gemini API**.

---

# 📑 Table of Contents

- [Overview](#-overview)
- [Core Features](#-core-features)
- [Architecture](#-architecture)
- [Validation Workflow](#-validation-workflow)
- [Technology Stack](#-technology-stack)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
  - [Prerequisites](#1-prerequisites)
  - [Clone the Repository](#2-clone-the-repository)
  - [Configure Environment Variables](#3-configure-environment-variables)
  - [Provide Test Documents](#4-provide-test-documents)
  - [Run the Application](#5-run-the-application)
- [Design Principles](#-design-principles)
- [Project Goals](#-project-goals)
- [License](#-license)

---

# 📖 Overview

The **AI Document Validator** is a command-line application designed to automate the extraction and validation of onboarding documents commonly used in health insurance workflows.

The system combines:

- **Multimodal AI extraction**
- **Deterministic business validation**
- **Scalable software architecture**

Its primary objective is to reduce manual verification effort while improving consistency, reliability, and maintainability.

---

# 🚀 Core Features

| Feature | Description |
|---|---|
| **Multi-Format Support** | Supports PDF, PNG, JPG, and JPEG files |
| **AI Document Classification** | Identifies document types such as RG, CNH, and Passports |
| **Structured Data Extraction** | Extracts fields like Full Name, Date of Birth, Document Number, and Expiration Date |
| **Deterministic Validation Rules** | Applies strict validation logic for required fields, expiration dates, and legibility |
| **Interactive CLI Experience** | Rich terminal interface powered by Spectre.Console |
| **Extensible Architecture** | Easily supports new document types without modifying the orchestration flow |

---

# 🏗️ Architecture

The project was designed following **Clean Code** and **SOLID** principles to ensure scalability and maintainability.

## Core Architectural Decisions

| Principle | Implementation |
|---|---|
| **Separation of Concerns** | UI, AI services, and validation logic are isolated into independent layers |
| **Open/Closed Principle (OCP)** | New document types can be added without changing existing orchestration logic |
| **Factory Method Pattern** | Dynamic document creation and extensibility |
| **Polymorphism** | Flexible handling of multiple document types |
| **Service-Oriented Design** | AI communication centralized through dedicated service classes |

---

## Main Components

| Component | Responsibility |
|---|---|
| `Program.cs` | Application entry point |
| `TerminalUI.cs` | Terminal rendering and user interaction |
| `GeminiService.cs` | Communication with the Gemini multimodal API |
| `Validators/` | Business validation rules |
| `Factories/` | Document instantiation logic |
| `Models/` | Shared domain entities |

---

# 🔄 Validation Workflow

```text
1. Load document files
        ↓
2. Detect file format
        ↓
3. Send file to Gemini API
        ↓
4. Classify document type
        ↓
5. Extract structured information
        ↓
6. Apply business validation rules
        ↓
7. Display validation results in CLI
```

---

# 🧠 Technology Stack

| Technology | Purpose |
|---|---|
| **C#** | Main programming language |
| **.NET 8** | Application framework |
| **Google Gemini API** | Multimodal AI extraction |
| **Spectre.Console** | Interactive terminal UI |
| **dotenv** | Environment variable management |

---

# 📂 Project Structure

```text
├── DocsTeste/
├── Factories/
├── Models/
├── Services/
│   └── GeminiService.cs
├── UI/
│   └── TerminalUI.cs
├── Validators/
├── Program.cs
├── .env
└── README.md
```

---

# ⚙️ Getting Started

# 1. Prerequisites

Before running the application, make sure the following dependencies are installed:

- .NET 8.0 SDK
- Google AI Studio API Key

---

# 2. Clone the Repository

```bash
git clone https://github.com/AmandaFernandes0701/grupo-facil-challenge-hackathon-ai-document-validator.git

cd grupo-facil-challenge-hackathon-ai-document-validator
```

---

# 3. Configure Environment Variables

Create a `.env` file in the project root directory:

```env
GEMINI_API_KEY=your_secret_api_key_here
```

---

# 4. Provide Test Documents

Create a folder named `DocsTeste` in the project root and add your test documents.

## Supported Formats

- PDF
- PNG
- JPG
- JPEG

## Example

```text
/DocsTeste
 ├── rg_frente.jpg
 ├── cnh.pdf
 └── passport.png
```

---

# 5. Run the Application

```bash
dotnet run
```

Navigate through the interface using:

| Key | Action |
|---|---|
| `↑ ↓` | Navigate menus |
| `Enter` | Select option |

---

# 📐 Design Principles

The application prioritizes:

- Readability
- Extensibility
- Maintainability
- Scalability
- Low coupling
- High cohesion

The architecture was intentionally designed to simplify future integrations and support additional document types with minimal implementation effort.

---

# 🎯 Project Goals

This project was developed as part of the **Grupo Fácil Health Tech Challenge** with the following objectives:

- Automate onboarding document validation
- Reduce operational overhead
- Improve validation consistency
- Demonstrate scalable AI-assisted architecture
- Apply modern software engineering practices

---

# 📄 License

This project was developed for educational and technical challenge purposes.
