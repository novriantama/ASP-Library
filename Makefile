.PHONY: help up up-d up-db down down-v build restart logs logs-app logs-db run migrate db-shell

# Default environment file
ENV_FILE ?= .env

help: ## Show this help message
	@echo "Usage: make [target]"
	@echo ""
	@echo "Targets:"
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "  \033[36m%-15s\033[0m %s\n", $$1, $$2}'

up: ## Start all services (app + db) in foreground
	docker compose --env-file $(ENV_FILE) up

up-d: ## Start all services (app + db) in background
	docker compose --env-file $(ENV_FILE) up -d

up-db: ## Start only the PostgreSQL database container in background
	docker compose --env-file $(ENV_FILE) up -d db

down: ## Stop and remove all containers
	docker compose --env-file $(ENV_FILE) down

down-v: ## Stop and remove all containers and volumes (data wipe)
	docker compose --env-file $(ENV_FILE) down -v

build: ## Rebuild docker containers
	docker compose --env-file $(ENV_FILE) build

restart: ## Restart all containers
	docker compose --env-file $(ENV_FILE) restart

logs: ## Follow logs from all containers
	docker compose --env-file $(ENV_FILE) logs -f

logs-app: ## Follow logs from app container
	docker compose --env-file $(ENV_FILE) logs -f app

logs-db: ## Follow logs from database container
	docker compose --env-file $(ENV_FILE) logs -f db

run: ## Run the .NET application locally
	dotnet run

migrate: ## Apply database migrations to PostgreSQL
	dotnet dotnet-ef database update

migrate-add: ## Add a new migration (usage: make migrate-add name=MigrationName)
	@if [ -z "$(name)" ]; then echo "Error: 'name' is required. Example: make migrate-add name=MyMigration"; exit 1; fi
	dotnet dotnet-ef migrations add $(name)

db-shell: ## Open PostgreSQL interactive psql shell
	docker compose --env-file $(ENV_FILE) exec db psql -U postgres -d library_db
