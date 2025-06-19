
# DirectoryApp Microservices

Bu proje, birbirleriyle haberleşen iki mikroservisten (`DirectoryApi` ve `ReportApi`) oluşur. Her mikroservis ayrı katmanlara ayrılmıştır (Application, Domain, Infrastructure, Presentation). Ayrıca PostgreSQL veritabanı ve mesajlaşma altyapısı ( RabbitMQ) kullanılarak mikroservisler arası iletişim sağlanır.

## İçindekiler

- [Teknolojiler](#teknolojiler)
- [Katmanlar](#katmanlar)
- [Kurulum](#kurulum)
- [Veritabanı Migrasyonları](#veritabanı-migrasyonları)
- [Servisleri Başlatma](#servisleri-başlatma)
- [Message Queue Entegrasyonu](#message-queue-entegrasyonu)
- [Test](#test)
- [Geliştirici Notları](#geliştirici-notları)

---

## Teknolojiler

- .NET Core
- PostgreSQL
- RabbitMQ (veya başka bir MQ)
- Entity Framework Core
- Git

---

## Katmanlar

Her mikroservis aşağıdaki katmanlara ayrılmıştır:

- `Application`: Use case'ler ve servisler.
- `Domain`: Entity ve domain servisleri.
- `Infrastructure`: Veritabanı erişimi, dış servisler ve MQ bağlantıları.
- `Presentation`: API endpoint’leri (Controllers).

---

## Kurulum

### Gereksinimler

- .NET 6 SDK+
- Docker (opsiyonel ama önerilir)
- PostgreSQL
- RabbitMQ

### Klonlama

```bash
git clone https://github.com/faruk-34/DirectoryApp.git
cd DirectoryApp
```

---

## Veritabanı Migrasyonları

EF Core kullanarak migrasyonları çalıştırmak için:

```bash
# DirectoryApi için
cd src/services/directory/DirectoryApi.Infrastructure
dotnet ef database update

# ReportApi için
cd src/services/report/ReportApi.Infrastructure
dotnet ef database update
```

---

## Servisleri Başlatma

Visual Studio üzerinden başlatmak için:

1. `DirectoryApi.Presentation` projesini başlangıç projesi olarak ayarlayın ve çalıştırın.
2. `ReportApi.Presentation` projesini de paralel olarak çalıştırın.

CLI üzerinden:

```bash
dotnet run --project src/services/directory/DirectoryApi.Presentation
dotnet run --project src/services/report/ReportApi.Presentation
```

---

## Message Queue Entegrasyonu

Bu proje mikroservisler arası iletişim için bir Message Queue (örneğin RabbitMQ) kullanır. Aşağıdaki şekilde bir RabbitMQ container'ı çalıştırabilirsiniz:

```bash
docker run -d --hostname rabbitmq --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

 

---

## Test

Test projesi `DirectoryApi.Test` altında yer alır. Testleri çalıştırmak için:

```bash
dotnet test src/services/directory/DirectoryApi.Test
```

---

## Geliştirici Notları

- Bağımlılık enjeksiyonu (DI) her katmanda uygun şekilde yapılandırılmıştır.
- API endpoint'leri `DirectoryApi.Presentation/Controllers` klasöründedir.
- Postman veya `DirectoryApi.Presentation.http` dosyasını kullanarak test istekleri gönderilebilir.
- Message Queue üzerinden bir olay tetiklendiğinde, diğer mikroservis mesajı alarak ilgili işlemleri yapar.

--- 
