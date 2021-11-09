# Azure Resource Scheduler

## Configuration

### Configuring OpenId Connect Client

The following configuration describes creating a new OpenID Connect client in [Azure Active Directory](https://portal.azure.com/).

Go to the [Azure Portal](https://portal.azure.com/) and open the resource `Azure Active Directory`. 

Go to `App registrations` and register a new application:

* Single tenant
* Redirect URI as `Single-page application` (multiple values can be added later in the clients `Authentication` section)
  * http(s)://\<hostname>/authentication/callback
  * http(s)://\<hostname>/authentication/silent_callback

Once created, make sure the following settings are updated / correct:

* Authentication - Allow public client flows
  * Yes
* Authentication - Implicit grant and hybrid flows
  * All unchecked
* Token configuration
  * Add group claim, with value `Group ID` and `Emit groups as role claims` as checked


## Development

### Adding database migrations

Ensure you have the `dotnet-ef` tool installed:

```
dotnet tool install --global dotnet-ef
```

Add a code first database migration:

```
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet ef migrations add "<migration_name>" --startup-project Host --project Repository --context DatabaseContext
```

### User secrets for development

```
# Database connection
dotnet user-secrets set --project Host "ConnectionStrings:Database" "Server=localhost;Database=az-resource-scheduler;User ID=sa;Password=yourStrong(!)Password;MultipleActiveResultSets=true"

# Authentication
dotnet user-secrets set --project Host "oidc:audience" "<audience>"
dotnet user-secrets set --project Host "oidc:authorityUri" "<authorityUri>"

dotnet user-secrets set --project Host "oidc:claim_types:name" "<name>" # (default: name)
dotnet user-secrets set --project Host "oidc:claim_types:role" "<role>" # (default: roles)

dotnet user-secrets set --project Host "oidc:roles:user" "<user>" # (default: user)
dotnet user-secrets set --project Host "oidc:roles:admin" "<admin>" # (default: admin)

# Feature flags
dotnet user-secrets set --project Host "unleash:apiUrl" "<apiUrl>"
dotnet user-secrets set --project Host "unleash:instanceTag" "<instanceTag>"
```

## Tooling and dependencies

* [Hangfire](https://www.hangfire.io/)
* [Azure SDK for .NET](https://github.com/Azure/azure-sdk-for-net/)