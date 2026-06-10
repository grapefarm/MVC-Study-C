## 專案架構概覽

專案版本管理採用 Feature Branch Workflow，各階段記錄如下：

- `v1-search`: 實作搜尋功能。
- `v2-sort`: 實作列表排序。
- `v3-viewmodel-create`: 實作 ViewModel 綁定以強化安全性。
- `v4-refactor-edit`: 優化編輯頁面邏輯。
- `v5-safe-delete-api`: 實作後端防禦性檢查 API。
- `v6-refactor-di`: 完成三層式架構重構與 DI 注入。
- `v7-sampledb`：針對 Northwind 原始資料結構進行反向工程重構，優化查詢排序邏輯；並實作業務邏輯層的權限控制（禁止刪除職稱為 'Sales Representative' 的員工）。
- `v7.5-controller-pagination`：在 Controller 層實作分頁邏輯，處理資料的切分（Skip/Take）與頁碼呈現。
- `v8-pagination`：進行架構解耦，將分頁業務邏輯遷移至 Service 層，提升 Controller 的職責單一性 (SRP)。
- `v9-soft-delete`：導入軟刪除機制 (Soft Delete)，結合 EF Core 全域查詢篩選 (Global Query Filter)，徹底解決外鍵約束衝突並確保資料的可恢復性。

解決方案(更新至2026/06/09)：

```powershell
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 8.0.10
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.10
Install-Package Microsoft.VisualStudio.Web.CodeGeneration.Design -Version 8.0.7 
```

```powershell
Scaffold-DbContext "Server=.;Database=Db;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutDir Models -Context NorthwindContext -Force
```

```json
  "ConnectionStrings": {
    "DbContext": "Server=localhost;Database=Db;Trusted_Connection=True;TrustServerCertificate=True;"
  }
```

```csharp
			builder.Services.AddDbContext<DbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DbContext")));	
```

```csharp
builder.Services.AddScoped<ISampleService, SampleService>();
```

```csharp
//外鍵衝突解法 2026/6/9

//1.新增欄位給資料表
//ALTER TABLE Employees ADD IsDeleted bit NOT NULL DEFAULT 0;

//2.新增欄位給資料表對應的Model
public bool IsDeleted { get; set; }

//3.新增篩選條件給Index() Action
.Where(e => !e.IsDeleted)

//或者去DbContext的OnModelCreating裡面加上全域過濾器
entity.HasQueryFilter(e => !e.IsDeleted);
```
