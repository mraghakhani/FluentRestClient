# FluentRestClient

A lightweight, fluent, and extensible REST client wrapper for .NET, designed to simplify API requests with support for features like MessagePack, configurable headers, and easy request customization.

## 🚀 Features

- Fluent request building syntax
- Supports JSON and MessagePack serialization
- **Multipart/form-data support for file uploads**
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
### File Upload with Multipart/Form-Data

Upload files using multipart/form-data:

```csharp
// Upload a single file
byte[] fileBytes = File.ReadAllBytes("document.pdf");
var response = await RequestBuilder
    .Create(HttpMethod.Post, "https://api.example.com/upload")
    .WithFile("file", fileBytes, "document.pdf", "application/pdf")
    .WithFormField("description", "My document")
    .SendAsync<UploadResponse>(httpClient, cancellationToken);
```

Or upload using a file stream:

```csharp
using var fileStream = File.OpenRead("image.jpg");
var response = await RequestBuilder
    .Create(HttpMethod.Post, "https://api.example.com/upload")
    .WithFileStream("image", fileStream, "image.jpg", "image/jpeg")
    .WithFormField("title", "Profile Picture")
    .SendAsync<UploadResponse>(httpClient, cancellationToken);
```

Upload multiple files:

```csharp
var response = await RequestBuilder
    .Create(HttpMethod.Post, "https://api.example.com/upload-multiple")
    .WithFile("file1", file1Bytes, "doc1.pdf", "application/pdf")
    .WithFile("file2", file2Bytes, "doc2.pdf", "application/pdf")
    .WithFormField("category", "documents")
    .SendAsync<UploadResponse>(httpClient, cancellationToken);
```

## ✨ Example API

```csharp
public Task<ApiResponse<List<UserItem>>?> UsersList(CancellationToken cancellationToken = default)
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


