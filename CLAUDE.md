# CLAUDE.md

## Project Overview

Dapr C#/.NET sample application demonstrating pub/sub messaging with Apache Kafka using Minimal APIs. Contains a publisher and subscriber service orchestrated via Docker Compose.

- **Language:** C# / .NET 10
- **Build:** `dotnet build` via Makefile
- **Runtime:** Dapr sidecar + Kafka (Docker Compose)
- **Repository:** [AndriyKalashnykov/dapr-csharp-kafka](https://github.com/AndriyKalashnykov/dapr-csharp-kafka)

## Quick Reference

```bash
make help          # Show all available targets
make build         # Restore and build publisher + subscriber
make build-images  # Build Docker images for both services
make runk          # Start Kafka via Docker Compose
make runs          # Build and run subscriber with Dapr
make runp          # Build and run publisher with Dapr
make runall        # Build images and run everything via Docker Compose
make stopall       # Stop all Docker Compose services
make clean         # Remove bin/ and obj/ directories
make upgrade       # Upgrade outdated NuGet packages
```

## Project Structure

```
publisher/          # Publisher service (Minimal API, Dapr SDK)
subscriber/         # Subscriber service (Minimal API, Dapr SDK)
components/         # Dapr component definitions
docker-compose.yml          # Full stack (Kafka + publisher + subscriber)
docker-compose-kafka.yml    # Kafka only
global.json         # .NET SDK version (10.0.x)
renovate.json       # Automated dependency updates
```

## Build & Test

```bash
make build          # Clean, restore NuGet packages, build both projects
make build-images   # Build Docker images via buildx
```

No test projects exist yet. When adding tests, follow TDD (write tests first, then implement).

## CI/CD

- **CI workflow:** `.github/workflows/ci.yml` -- builds on push/PR, builds Docker images on tags
- **Cleanup workflow:** `.github/workflows/cleanup-runs.yml` -- weekly old run cleanup

## Dependencies

Managed via Renovate (`renovate.json`). NuGet packages and GitHub Actions are auto-updated with automerge enabled.

## Conventions

- Makefile targets use `#target: @ Description` comment format for `make help`
- Immutable data patterns preferred
- Keep files under 800 lines
- Validate all inputs at system boundaries
