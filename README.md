# .NET Web API Rezervasyon Uygulaması

Bu proje, bir **.NET Web API** rezervasyon uygulamasıdır. **PostgreSQL** veritabanı kullanılarak geliştirilmiş ve **JWT (JSON Web Token)** ile kimlik doğrulama yapılmaktadır. Veritabanı kurulumu için **EF Core Migration** kullanılmıştır.

## Gereksinimler

- **.NET 8.0** veya daha yeni bir sürüm
- **PostgreSQL** veritabanı

## Proje Kurulumu

Aşağıdaki adımları takip ederek projeyi kurup çalıştırabilirsiniz.

### 1. Projeyi Klonlayın

GitHub reposunu yerel bilgisayarınıza klonlamak için terminal veya komut istemcisinde şu komutu kullanın:

git clone https://github.com/safatosun/ReservationApp.git

### 2. Veritabanı Bağlantı Dizesini ve JWT Token Yapılandırın

Projede PostgreSQL veritabanı kullanıldığı için, appsettings.json dosyasındaki ConnectionStrings bölümünü ve JWT token bölümünü aşağıdaki gibi yapılandırmanız gerekmektedir:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=veritabani_adi;Username=postgres;Password=sifre"
  },
  "JwtSettings": {
    "Issuer": "your-issuer",
    "Audience": "your-audience",
    "SecretKey": "your-secret-key",
    "ExpirationInMinutes": your-minutes
  }
}
```
### 3. Veritabanı Migration'larını Uygulayın

Veritabanı migration'ları oluşturulmuş ve ilk kurulum için hazırdır. Veritabanını güncellemek için şu komutu çalıştırın:

dotnet ef database update

### 4. Projeyi çalıştırın
