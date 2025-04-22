# PostgreSQL Docker Setup for PotPal

This setup provides a lightweight PostgreSQL 17 database container using the official `postgres:17-alpine` image. It is configured to persist data and is suitable for local development for the PotPal project.

## 🚀 How to Use

1. Make sure you have [Docker](https://www.docker.com/products/docker-desktop) installed.
2. Create a file named `docker-compose.yml` and paste the following content:

```yaml
services:

  db:
    image: postgres:17-alpine
    restart: always
    environment:
      POSTGRES_USER: user
      POSTGRES_PASSWORD: password
    ports:
      - "5432:5432"
    volumes:
      - "./data:/var/lib/postgresql/data/"