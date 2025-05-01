# PotPal – Docker Project Setup Guide

## 🔧 Docker Services

This project includes two main services:

- **PostgreSQL** — A database service using the `postgres:17-alpine` image.
- **Backend API** — An ASP.NET Core Web API that communicates with the database and receives data from Arduino devices.

The project is hosted at:  
🔗 **http://10.133.51.109:6002/**

---

## 📦 Container Structure

### 1. Database (`db`)
- Image: `postgres:17-alpine`
- Ports: `5432:5432`
- Data is stored locally in `./data`
- Environment variables (taken from `.env`):
  - `PG_USER`
  - `PG_PASSWORD`
  - `PG_DATABASE`

### 2. Backend (`backend`)
- Built from the `./Backend` directory
- Ports: external `6002` → internal `5000`
- Uses the `DB_CONNECTION_STRING` environment variable to connect to the database

---

## ▶️ Running the Project

1. **Clone or update the repository:**

```bash
cd ~/projects/iot-project-potpal
git pull
```

2. **Start/update containers:**

Use the provided update script:

```bash
sh ~/update.sh
```

This executes the following:

```bash
cd ~/projects/iot-project-potpal
git pull
docker compose up --build -d
```

> The `--build` flag ensures that any code changes are included during startup.

---

## ⛔ Stopping the Project

```bash
docker compose down
```

This stops the containers without deleting the stored database data.

---

## 🗂️ Important Files

- `docker-compose.yml` — Service configuration
- `Dockerfile` in `./Backend` — Describes how to build the API
- `.env` — Environment variable settings. Make sure to create this file: 

`PG_USER = "user"`
`PG_PASSWORD = "Password123!"`
`PG_DATABASE = "PotPalDb"`

---

## ❗ Notes

- The project is intended to run in a local network or internal server.
- If the API doesn't work after code changes, check the `Dockerfile` and ensure the `update.sh` script uses `--build`.
- If `.env` is missing, you must set environment variables manually or create the file.

