# Local KeyVault
Dead-simple local Azure KeyVault-replacement

This is useful for running a fake KeyVault locally, to run against development settings.

## Usage

Keys & values are stored in `appSettings`, under a section named `KeyVaultValues`. Simply add items to `appSettings`:

```
  "KeyVaultValues": {
    "Key": "Value"  
  }
```

The key is part of the url: `http://localhost:PORT/secrets/Key`.

Run the project via Visual Studio, VS Code, or directly via `dotnet run`. Update your project's `KeyVaultConfig:KeyVaultUrl` value with this project's url, eg `http://localhost:52137`.

## Missing

This project does not attempt to validate KeyVault authorization.
