
# Stock-Tracking-System

Bu projenin amacı, bir stok takip oluşturup, envanter üzerindeki stok durumlarını takip etmeyi sağlamaktır. Proje stok,tedarikçi gibi nesneleri ekleme, listeleme ve yönetme gibi işlemleri düzenlerken güvenli veri saklama ve kullanıcı rolleri bazında erişim kontrolü ve log takip etme gibi özellikleri destekler. 

## Kullanılan Teknolojiler

**Front-End:** React

**Back-End:** .NET Core, WebAPI, Serilog, EF


## Back-End

Projenin backend kısmı .NET Core WebAPI kullanılarak geliştirilmiştir. Backend, stok takip sisteminin iş mantığını ve veri yönetimini sağlamaktan sorumludur. Kullanıcı kısmı ise Identity kütüphanesi kullanılarak geliştirilmiştir.

API güvenliğinin sağlanması için JSON Web Token (JWT) kullanılmıştır. Kullanıcı giriş yaptıktan sonra, bir JWT token oluşturulur ve sonraki işlemlerde bu token aracılığıyla yetkilendirme kontrolü yapılır.

Controller-Service-Repository katmanları ile katmanlı bir mimari oluşturulmuştur.

- Controller Katmanı, istemciden gelen istekleri alır ve ilgili işlevleri çağırır.

- Service Katmanı, iş kurallarını uygular ve veritabanı işlemlerini koordine eder.

- Repository Katmanı, EF kullanarak veritabanı CRUD ve veri tabanı sorgu işlemlerini gerçekleştirir. Sorgular Generic bazlı sınıf yazrdımıyla soyutlanmıştır.

Sistemin güvenilirliğini artırmak için xUnit ile birim testler yazılmıştır. Servis katmanındaki metotlar test edilerek, sistemin hata durumlarına karşı nasıl tepki vereceği doğrulanmıştır.

Veri tabanı yönetim sistemi olarak ise MSSql kullanılmıştır. Aşağıdaki ER diyagramı, uygulamada kullanılan veri modelini detaylandırmaktadır.

![ER-Diagram](https://github.com/user-attachments/assets/b1f495d7-5af0-410b-9ffd-8bf240e376f4)

## DTO Yapıları

#### AuthResponseDto Yapısı

| Alan      | Tip     | 
| :-------- | :------ |
| `id` | `string`|
| `token` | `string`|
| `role` | `string`|
| `message` | `string`|

#### LoginDto Yapısı

| Alan       | Tip     | 
| :--------- | :------ |
| `Username` | `string`|
| `Password` | `string`|

#### ProductDtoUI Yapısı

| Alan           | Tip      | 
| :------------- | :------- |
| `Name` | `string` |
| `Description`  | `string` |
| `UnitPrice` | `decimal`|
| `UnitsInStock` | `int`|
| `CategoryId` | `int`|
| `SupplierId` | `int`|

#### RegisterDto Yapısı

| Alan         | Tip     | 
| :----------- | :------ |
| `Username` | `string`|
| `FullName` | `string`|
| `PhoneNumber`| `string`|
| `Email`| `string`|
| `Password`| `string`|

#### StockTransactionDtoIU Yapısı

| Alan       | Tip  | 
| :--------- | :--- |
| `Quantity`| `int`|
| `ProductId`| `int`|

#### SupplierDtoIU Yapısı

| Alan         | Tip     | 
| :----------- | :------ |
| `Name`| `string`|
| `ContactName`| `string`|
| `Phone` | `string`|
| `Email`| `string`|
| `AddressId`|`int`|



## Address API Referance

Bu kısım, Address API'sine ait endpointleri ve her birinin nasıl kullanılacağını açıklar. Aşağıdaki detaylar, ilgili API metodlarının istek yollarını, parametrelerini ve yanıtlarını açıklar.

### Tüm Adresleri Listeleme
```http
GET /api/adress/list
```
Tüm adresleri döner.

---

### ID ile Adres Getirme
```http
GET /api/adress/get
```
#### Header Parametreleri

| Parametre  | Tip | Açıklama                      |
| :--------- | :-- | :---------------------------- |
| Address-Id | int | **Zorunlu**. Adres ID bilgisi |

---

### Adres Oluşturma
```
POST /api/adress
```
#### İstek Gövdesi

| Parametre | Tip     | Açıklama                          |
| :-------- | :------ | :-------------------------------- |
| address   | Address | **Zorunlu**. Yeni adres bilgileri |

---

### Adres Güncelleme
```
PUT /api/adress/update
```
#### Header Parametreleri

| Parametre  | Tip | Açıklama                                    |
| :--------- | :-- | :------------------------------------------ |
| Address-Id | int | **Zorunlu**. Güncellenecek adres ID bilgisi |

#### İstek Gövdesi

| Parametre | Tip     | Açıklama                            |
| :-------- | :------ | :---------------------------------- |
| address   | Address | **Zorunlu**. Güncel adres bilgileri |

---

### Adres Silme
```
DELETE /api/adress/delete
```
#### Header Parametreleri

| Parametre  | Tip | Açıklama                                |
| :--------- | :-- | :-------------------------------------- |
| Address-Id | int | **Zorunlu**. Silinecek adres ID bilgisi |





## Auth API Referance

Bu kısım, Auth API'sine ait endpointleri ve her birinin nasıl kullanılacağını açıklar. Aşağıdaki detaylar, ilgili API metodlarının istek yollarını, parametrelerini ve yanıtlarını açıklar.

---

### Kullanıcı Kaydı
```
POST /api/auth/register
```
#### İstek Gövdesi

| Parametre   | Tip    | Açıklama                                   |
| :---------- | :----- | :----------------------------------------- |
| Username    | string | **Zorunlu**. Kullanıcı adı                 |
| FullName    | string | **Zorunlu**. Kullanıcının tam adı          |
| PhoneNumber | string | **Zorunlu**. Kullanıcının telefon numarası |
| Email       | string | **Zorunlu**. Kullanıcının e-posta adresi   |
| Password    | string | **Zorunlu**. Kullanıcı şifresi             |


Yanıt AuthResponseDto olarak döner. İçerisinde kullanıcı id'si, token, kullanıcı rolü ve mesaj bulunur.

---

### Kullanıcı Girişi
```
POST /api/auth/login
```
#### İstek Gövdesi

| Parametre | Tip    | Açıklama                       |
| :-------- | :----- | :----------------------------- |
| Username  | string | **Zorunlu**. Kullanıcı adı     |
| Password  | string | **Zorunlu**. Kullanıcı şifresi |

Yanıt AuthResponseDto olarak döner. İçerisinde kullanıcı id'si, token, kullanıcı rolü ve mesaj bulunur.



## Category API Referance

Bu kısım, Category API'sine ait endpointleri ve her birinin nasıl kullanılacağını açıklar. Aşağıdaki detaylar, ilgili API metodlarının istek yollarını, parametrelerini ve yanıtlarını açıklar.

---

### Tüm Kategorileri Listeleme
```
GET /api/category
```
Tüm kategorileri döner.

---

### ID ile Kategori Getirme
```
GET /api/category/{id}
```
#### Yol Parametreleri

| Parametre | Tip | Açıklama                         |
| :-------- | :-- | :------------------------------- |
| id        | int | **Zorunlu**. Kategori ID bilgisi |

---

### Kategori Oluşturma
```
POST /api/category
```
#### İstek Gövdesi

| Parametre | Tip      | Açıklama                             |
| :-------- | :------- | :----------------------------------- |
| category  | Category | **Zorunlu**. Yeni kategori bilgileri |

#### Yanıt

| Alan | Tip    | Açıklama                        |
| :--- | :----- | :------------------------------ |
| category  | Category | Kaydedilen kategorinin bilgileri |

---

### Kategori Güncelleme
```
PUT /api/category/{id}
```
#### Yol Parametreleri

| Parametre | Tip | Açıklama                                       |
| :-------- | :-- | :--------------------------------------------- |
| id        | int | **Zorunlu**. Güncellenecek kategori ID bilgisi |

#### İstek Gövdesi

| Parametre | Tip      | Açıklama                               |
| :-------- | :------- | :------------------------------------- |
| category  | Category | **Zorunlu**. Güncel kategori bilgileri |

---

### Kategori Silme
```
DELETE /api/category/{id}
```
#### Yol Parametreleri

| Parametre | Tip | Açıklama                                   |
| :-------- | :-- | :----------------------------------------- |
| id        | int | **Zorunlu**. Silinecek kategori ID bilgisi |




## Log API Referance

Bu kısım, Log API'sine ait endpointleri ve her birinin nasıl kullanılacağını açıklar. Aşağıdaki detaylar, ilgili API metodlarının istek yollarını, parametrelerini ve yanıtlarını açıklar. Bu API yalnızca Admin rolüne sahip kullanıcılar tarafından kullanılabilir.

---

### Log Dosyalarını Listeleme
```
GET /api/logs/list
```
Tüm log dosyalarının isimlerini döner.

#### Yanıt

| Tip          | Açıklama                      |
| :----------- | :---------------------------- |
| List<string> | Log dosyalarının isim listesi |

---

### Log Dosyası İndirme
```
GET /api/logs/{fileName}
```
#### Yol Parametreleri

| Parametre | Tip    | Açıklama                                 |
| :-------- | :----- | :--------------------------------------- |
| fileName  | string | **Zorunlu**. İndirilecek log dosyası adı |

#### Yanıt

| Tip        | Açıklama                      |
| :--------- | :---------------------------- |
| text/plain | İlgili log dosyasının içeriği |

Eğer dosya veya Logs klasörü bulunamazsa, 404 Not Found döner.



## Product API Referance

Bu kısım, Product API'sine ait endpointleri ve her birinin nasıl kullanılacağını açıklar. Aşağıdaki detaylar, ilgili API metodlarının istek yollarını, parametrelerini ve yanıtlarını açıklar.

---

### Tüm Ürünleri Listeleme
```
GET /api/product
```
Tüm ürünleri döner.

#### Yanıt

| Tip              | Açıklama                |
| :--------------- | :---------------------- |
| List<ProductDto> | Tüm ürünlerin bilgileri |

---

### ID ile Ürün Getirme
```
GET /api/product/{id}
```
#### Yol Parametreleri

| Parametre | Tip | Açıklama                     |
| :-------- | :-- | :--------------------------- |
| id        | int | **Zorunlu**. Ürün ID bilgisi |

#### Yanıt

| Tip        | Açıklama            |
| :--------- | :------------------ |
| ProductDto | İlgili ürün bilgisi |

---

### Ürün Oluşturma
```
POST /api/product
```
#### İstek Gövdesi

| Parametre | Tip          | Açıklama                         |
| :-------- | :----------- | :------------------------------- |
| dto       | ProductDtoUI | **Zorunlu**. Yeni ürün bilgileri |

#### Yanıt

| Tip        | Açıklama                 |
| :--------- | :----------------------- |
| ProductDto | Oluşturulan ürün bilgisi |

---

### Ürün Güncelleme
```
PUT /api/product/{id}
```
#### Yol Parametreleri

| Parametre | Tip | Açıklama                                   |
| :-------- | :-- | :----------------------------------------- |
| id        | int | **Zorunlu**. Güncellenecek ürün ID bilgisi |

#### İstek Gövdesi

| Parametre | Tip          | Açıklama                           |
| :-------- | :----------- | :--------------------------------- |
| dto       | ProductDtoUI | **Zorunlu**. Güncel ürün bilgileri |

---

### Ürün Silme
```
DELETE /api/product/{id}
```
#### Yol Parametreleri

| Parametre | Tip | Açıklama                               |
| :-------- | :-- | :------------------------------------- |
| id        | int | **Zorunlu**. Silinecek ürün ID bilgisi |





## StockTransaction API Referance

Bu kısım, StockTransaction API'sine ait endpointleri ve her birinin nasıl kullanılacağını açıklar. Aşağıdaki detaylar, ilgili API metodlarının istek yollarını, parametrelerini ve yanıtlarını açıklar.

---

### Tüm Stok İşlemlerini Listeleme
```
GET /api/transaction
```
**Yetki:** Admin veya Manager rolü gereklidir.

#### Yanıt

| Tip                    | Açıklama                     |
| :--------------------- | :--------------------------- |
| List<StockTransaction> | Tüm stok işlemleri bilgileri |

---

### ID ile Stok İşlemi Getirme
```
GET /api/transaction/{id}
```
#### Yol Parametreleri

| Parametre | Tip | Açıklama                      |
| :-------- | :-- | :---------------------------- |
| id        | int | **Zorunlu**. İşlem ID bilgisi |

#### Yanıt

| Tip              | Açıklama                   |
| :--------------- | :------------------------- |
| StockTransaction | İlgili stok işlemi bilgisi |

---

### Stok Ekleme
```
POST /api/transaction/add-stock
```
#### İstek Gövdesi

| Parametre | Tip                   | Açıklama                              |
| :-------- | :-------------------- | :------------------------------------ |
| dto       | StockTransactionDtoIU | **Zorunlu**. Eklenecek stok bilgileri |

#### Yanıt

| Tip              | Açıklama                        |
| :--------------- | :------------------------------ |
| StockTransaction | Oluşturulan stok işlemi bilgisi |

---

### Stok Çıkarma
```
POST /api/transaction/remove-stock
```
#### İstek Gövdesi

| Parametre | Tip                   | Açıklama                                |
| :-------- | :-------------------- | :-------------------------------------- |
| dto       | StockTransactionDtoIU | **Zorunlu**. Çıkarılacak stok bilgileri |

#### Yanıt

| Tip              | Açıklama                        |
| :--------------- | :------------------------------ |
| StockTransaction | Oluşturulan stok işlemi bilgisi |




## Supplier API Referance

Bu kısım, Supplier API'sine ait endpointleri ve her birinin nasıl kullanılacağını açıklar. Aşağıdaki detaylar, ilgili API metodlarının istek yollarını, parametrelerini ve yanıtlarını açıklar.

---

### Tüm Tedarikçileri Listeleme
```
GET /api/supplier
```
Tüm tedarikçileri döner.

#### Yanıt

| Tip               | Açıklama                     |
| :---------------- | :--------------------------- |
| List<SupplierDto> | Tüm tedarikçilerin bilgileri |

---

### ID ile Tedarikçi Getirme
```
GET /api/supplier/{id}
```
#### Yol Parametreleri

| Parametre | Tip | Açıklama                          |
| :-------- | :-- | :-------------------------------- |
| id        | int | **Zorunlu**. Tedarikçi ID bilgisi |

#### Yanıt

| Tip           | Açıklama                 |
| :------------ | :----------------------- |
| SupplierDtoIU | İlgili tedarikçi bilgisi |

---

### Tedarikçi Oluşturma
```
POST /api/supplier
```
#### İstek Gövdesi

| Parametre | Tip           | Açıklama                              |
| :-------- | :------------ | :------------------------------------ |
| dto       | SupplierDtoIU | **Zorunlu**. Yeni tedarikçi bilgileri |

#### Yanıt

| Tip           | Açıklama                      |
| :------------ | :---------------------------- |
| SupplierDtoIU | Oluşturulan tedarikçi bilgisi |

---

### Tedarikçi Güncelleme
```
PUT /api/supplier/{id}
```
#### Yol Parametreleri

| Parametre | Tip | Açıklama                                        |
| :-------- | :-- | :---------------------------------------------- |
| id        | int | **Zorunlu**. Güncellenecek tedarikçi ID bilgisi |

#### İstek Gövdesi

| Parametre | Tip           | Açıklama                                |
| :-------- | :------------ | :-------------------------------------- |
| dto       | SupplierDtoIU | **Zorunlu**. Güncel tedarikçi bilgileri |


---

### Tedarikçi Silme
```
DELETE /api/supplier/{id}
```
#### Yol Parametreleri

| Parametre | Tip | Açıklama                                    |
| :-------- | :-- | :------------------------------------------ |
| id        | int | **Zorunlu**. Silinecek tedarikçi ID bilgisi |





## User API Referance

Bu kısım, User API'sine ait endpointleri ve her birinin nasıl kullanılacağını açıklar. Aşağıdaki detaylar, ilgili API metodlarının istek yollarını, parametrelerini ve yanıtlarını açıklar. Bu API yalnızca Admin rolüne sahip kullanıcılar tarafından kullanılabilir.

---

### Tüm Kullanıcıları Listeleme
```
GET /api/user/list
```
#### Yanıt

| Alan     | Tip    | Açıklama                    |
| :------- | :----- | :-------------------------- |
| Id       | string | Kullanıcı ID bilgisi        |
| UserName | string | Kullanıcı adı               |
| FullName | string | Kullanıcının tam adı        |
| Email    | string | Kullanıcının e-posta adresi |

---

### Kullanıcı Rolünü Değiştirme
```
PUT /api/user/change-role/{id}
```
#### Yol Parametreleri

| Parametre | Tip    | Açıklama                          |
| :-------- | :----- | :-------------------------------- |
| id        | string | **Zorunlu**. Kullanıcı ID bilgisi |

#### İstek Gövdesi

| Parametre | Tip    | Açıklama                      |
| :-------- | :----- | :---------------------------- |
| Role      | string | **Zorunlu**. Yeni rol bilgisi |


---

### Kullanıcı Silme
```
DELETE /api/user/delete/{id}
```
#### Yol Parametreleri

| Parametre | Tip    | Açıklama                                    |
| :-------- | :----- | :------------------------------------------ |
| id        | string | **Zorunlu**. Silinecek kullanıcı ID bilgisi |

#### Yanıt

| Tip    | Açıklama                         |
| :----- | :------------------------------- |
| string | Başarı mesajı: Kullanıcı silindi |



## Front-End

Frontend kısmı, React.js kullanılarak geliştirilmiştir. Uygulama, kullanıcıların ürünleri görüntülemesi, stok durumlarını takip etmesi, stok ekleme ve çıkarma işlemleri yapması; yöneticilerin ise ürünleri, kategorileri ve tedarikçileri yönetmesini sağlar. Kullanıcı rollerine göre erişim yetkisi bulunmayan alanlar render edilmemektedir. Aşağıda, React ile geliştirilen frontend’in temel yapı taşları ve işleyişi açıklanmıştır.

- Backend ile iletişim, `fetch` komutu kullanılarak yapılmaktadır.

## Screenshots

![Login-Page](https://github.com/user-attachments/assets/2de16398-2c43-443c-8473-952d1a12817f)
---
![Product-Detail](https://github.com/user-attachments/assets/82dce46a-68c5-468c-8173-decc4bc13430)
---
![Stock-Transaction-List](https://github.com/user-attachments/assets/27354839-c365-4d96-abf6-cf9c48bb9cd4)
---
![Log-Manager](https://github.com/user-attachments/assets/cd45f60b-6fc2-48c2-9921-d278ec54a6d4)



---
## Run Locally

Projeyi klonlayın

```bash
  git clone https://github.com/hegekara/stock-tracking-system.git
```

Proje dizinine gidin

```bash
  cd stock-tracking-system
```

Backend'i başlatın.

```bash
  cd Backend/src/Backend.Api
  dotnet run
```

Front-End'i başlatın

```bash
  cd frontend
  npm run dev
```

Servis katmanını test edin

```bash
  cd Backend/tests/Backend.Api.Tests
  dotnet test
```
