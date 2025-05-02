# FluentRestClient

A lightweight, fluent, and extensible REST client wrapper for .NET, designed to simplify API requests with support for features like MessagePack, configurable headers, and easy request customization.

## 🚀 Features

- Fluent request building syntax
- Supports JSON and MessagePack serialization
- Cancellation token support
- Custom headers, query parameters, and request options
- Strongly typed responses
- Lightweight and dependency-free

## 📦 Installation

Install via NuGet:

```bash
dotnet add package FluentRestClient
```
Or via the NuGet Package Manager:

```
Install-Package FluentRestClient
```

## 🛠️ Usage
```csharp
var response = await RequestBuilder
    .Create(HttpMethod.Get, "https://api.example.com/items")
    .WithHeader("Authorization", "Bearer YOUR_TOKEN")
    .WithQuery("page", "1")
    .SendAsync<ApiResponse<List<ItemDto>>>(httpClient, cancellationToken);
```
Or with MessagePack serialization:

```csharp
var response = await RequestBuilder
    .Create(HttpMethod.Get, "https://api.example.com/items")
    .WithMessagePackEnabled()
    .SendAsync<List<ItemDto>>(httpClient, cancellationToken);
```

## ✨ Example API

```csharp
public Task<ApiResponse<List<UserItem>>?> MarketMap(CancellationToken cancellationToken = default)
    => RequestBuilder.Create(HttpMethod.Get, Urls.GetUsersList)
        .SendAsync<ApiResponse<List<UserItem>>>(_restClient, cancellationToken);
```

## 📄 Documentation

-[x] Fluent API for building REST requests

-[x] Custom serialization options

-[ ] Optional retry policies (coming soon)

## 🧩 Extensibility
You can extend the RequestBuilder to add:

- Global headers

- Authentication middleware

- Logging

- Retry policies (e.g., Polly)

## 🧪 Testing
  Mock `IHttpClientFactory` for unit tests.

## 🤝 Contributing

Pull requests are welcome! Feel free to fork the repo and submit improvements.

## 📜 License
MIT License


